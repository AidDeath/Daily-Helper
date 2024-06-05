using Daily_Helper.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
        ILogger<MailerHostedService> _logger;
        SettingsSingleton _settings;
        public MailerHostedService(DailyHelperDbContext db, MailQueueProvider mailQueue, ILogger<MailerHostedService> logger, SettingsSingleton settings)
        {
            _db = db;
            _mailQueue = mailQueue;
            _logger = logger;
            _settings = settings;

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    _logger.LogInformation("Mailer service call");

                    if (_mailQueue.MailQueue.Count != 0)
                    {
                        var currentFailureId = _mailQueue.MailQueue.Dequeue();

                        var failureEvent = _db.FailureEvents
                            .Include(fe => fe.RoutineIdentifer)
                            .ThenInclude(ri => ri.MailRecievers)
                            .ThenInclude(mr => mr.Email)
                            .FirstOrDefault(fe => fe.RoutineId == currentFailureId && fe.IsStillActive);

                        foreach(var mailReceicer in failureEvent.RoutineIdentifer.MailRecievers)
                        {


                           var sendResult =  SendMessage
                                (
                                    mailReceicer.Email.EmailAddress,
                                    $"Обнаружен сбой: {failureEvent.RoutineIdentifer.Description}",
                                    failureEvent.FailureDescription
                                );
                            
                            MailLog mailLog = new()
                            {
                                SendAt = DateTime.Now,
                                FailureEventId = failureEvent.Id,
                                EmailId = mailReceicer.Email.Id,
                                SendResult = sendResult,
                                Subject = $"Обнаружен сбой: {failureEvent.RoutineIdentifer.Description}"

                            };

                            _db.MailLogs.Add(mailLog);


                        }

                        await _db.SaveChangesAsync();

                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                }
            }
        }

        string SendMessage(string addressTo, string messageSubject, string messageText)
        {
            try
            {
                string from = _settings.SenderLogin; // Адрес отправителя
                string pass = _settings.SenderPassword; // Пароль отправителя

                MailMessage mess = new MailMessage();
                mess.To.Add(addressTo); // Адрес получателя
                mess.From = new MailAddress(from);
                mess.Subject = messageSubject; // Тема
                mess.Body = messageText; // Текст сообщения

                SmtpClient client = new SmtpClient();
                client.Host = _settings.SmtpServer; // SMTP-сервер отправителя
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], pass);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(mess); // Отправка пользователю

                mess.Dispose();

                return "Сообщение отправлено успешно";
            }
            catch (Exception e)
            {
                return $"Ошибка при отправке: \n{e.GetBaseException().Message}";
            }
        }
    }



}
