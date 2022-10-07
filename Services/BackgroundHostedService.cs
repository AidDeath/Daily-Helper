﻿using Daily_Helper.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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


        public BackgroundHostedService(ILogger<BackgroundHostedService> logger, RoutineTestsProvider routineTests )
        {
            _logger = logger;

            _logger.LogInformation("Background service started...");

            _routineTests = routineTests;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //_valuesProvider.StringValue = await Payload();
                    await Payload();
                    _logger.LogInformation("Background service call");

                    
                    foreach (var routine in _routineTests.Routines)
                    {
                        _logger.LogInformation($"{routine.Description} - {routine.Result}");
                    }
                    
                }
                catch (Exception ex)
                {
                    _logger.LogError($"There was an error: {ex.GetBaseException().Message}");
                }
            }
        }


        private async Task Payload()
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            if (_routineTests != null && _routineTests.Routines.Count > 0)
            {
                foreach (var routine in _routineTests.Routines)
                {
                    if (routine.IsActivated)
                    {
                        await routine.ExecuteRoutineTest();
                        routine.LastExecutted = DateTime.Now;
                    }
                        
                }
            }

            //return _randomStringService.GetRandomString();
            return;
        }
    }
}
