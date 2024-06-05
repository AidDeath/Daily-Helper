using Daily_Helper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Daily_Helper.Services
{
    /// <summary>
    /// Way how to implement background working service. Works with arranging DI With IHost in App class
    /// </summary>
    public class BackgroundHostedService : BackgroundService
    {
        private readonly ILogger<BackgroundHostedService> _logger;
        private RoutineTestsProvider _routineTests;
        private MailQueueProvider _mailQueueProvider;
        private readonly SettingsSingleton _settings;
        private readonly DailyHelperDbContext _db;


        public BackgroundHostedService(ILogger<BackgroundHostedService> logger, RoutineTestsProvider routineTests, SettingsSingleton settings, DailyHelperDbContext db, MailQueueProvider mailQueueProvider)
        {
            _logger = logger;

            _logger.LogInformation("Background service started...");

            _routineTests = routineTests;
            _mailQueueProvider = mailQueueProvider;
            _settings = settings;
            _db = db;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background service call");
                try
                {
                    await Payload();
                    
                    foreach (var routine in _routineTests.Routines)
                    {
                        _logger.LogInformation($"{routine.Description} - {routine.Result}");

                        if (!_db.RoutineIdentifers.Any(rec => rec.RoutineId == routine.RoutineId))
                            _db.RoutineIdentifers.Add(new RoutineIdentifer { RoutineId = routine.RoutineId, Description = routine.Description, IsCurrentlyInList = true });

                        //If routine failed and there is no record with "IsStillActive" checked in database - we add record
                        if (routine.Success == false && _db.FailureEvents.Where((record) => record.RoutineId == routine.RoutineId).All((rec) => !rec.IsStillActive))
                        {
                            _db.FailureEvents.Add(new FailureEvent
                            {
                                FailureDescription = routine.Result,
                                Occured = routine.LastExecutted,
                                RoutineId = routine.RoutineId,
                                //RoutineIdentifer = new RoutineIdentifer { IsCurrentlyInList = true, Description = routine.Description, RoutineId = routine.RoutineId}
                                
                                
                            });
                            //Here we have new fail, that havent caught yet. So, This is a place for sending Emails
                            _mailQueueProvider.MailQueue.Enqueue(routine.RoutineId);
                        }


                        // Need to replace. Repeadedly db query
                        if (routine.Success == true && _db.FailureEvents.Any((record) => record.RoutineId == routine.RoutineId && record.IsStillActive))
                        {
                            _db.FailureEvents.FirstOrDefault(rec => rec.RoutineId == routine.RoutineId && rec.IsStillActive).IsStillActive = false;
                            //await _db.SaveChangesAsync(stoppingToken);
                        }



                        
                    }

                    //Every time we will check for routines deleted from list
                    await _db.RoutineIdentifers.ForEachAsync(rec => rec.IsCurrentlyInList = _routineTests.Routines.Any(routine => routine.RoutineId == rec.RoutineId));
                    await _db.SaveChangesAsync(stoppingToken);
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError($"There was an error: {ex.GetBaseException().Message}");
                }
            }
        }


        private async Task Payload()
        {
            await Task.Delay(TimeSpan.FromSeconds(_settings.CheckInterval));

            if (_routineTests != null && _routineTests.Routines.Count > 0)
            {
                foreach (var routine in _routineTests.Routines)
                {
                    if (routine.IsActivated)
                    {
                        routine.LastExecutted = DateTime.Now;
                        await Task.Run(routine.ExecuteRoutineTest);
                    }
                        
                }
            }

            return;
        }
    }
}
