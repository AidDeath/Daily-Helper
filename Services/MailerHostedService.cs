using Daily_Helper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Daily_Helper.Services
{
    public class MailerHostedService : BackgroundService
    {
        private DailyHelperDbContext _db;
        private MailQueueProvider _mailQueue;
        public MailerHostedService(DailyHelperDbContext db, MailQueueProvider mailQueue)
        {
            _db = db;
            _mailQueue = mailQueue;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    if (_mailQueue.MailQueue.Count == 0) return;

                    var currentFailureId = _mailQueue.MailQueue.Dequeue();



                    if (currentFailureId is not null)
                    {
                        var a = _db.FailureEvents
                            .Include(fe => fe.RoutineIdentifer)
                            .ThenInclude(ri => ri.MailRecievers)
                            .FirstOrDefault(fe => fe.RoutineId == currentFailureId && fe.IsStillActive);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        private async Task Payload()
        {
            
        }
    }
}
