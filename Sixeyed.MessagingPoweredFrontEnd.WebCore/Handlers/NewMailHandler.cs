using Microsoft.AspNetCore.SignalR;
using Sixeyed.MessagingPoweredFrontEnd.ContractsCore;
using Sixeyed.MessagingPoweredFrontEnd.QueueCore;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.Models;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Sixeyed.MessagingPoweredFrontEnd.WebCore.Handlers
{
    public static class NewMailHandler
    {
        public static List<MailModel> MailBag { get; private set; }
        private static Queue _Queue;
        private static IHubClients _Clients;

        static NewMailHandler()
        {
            MailBag = new List<MailModel>();
            string queueHost = "localhost";
            _Queue = new Queue(queueHost);
        }

        public static void Init(IHubContext<NoticeboardHub> hubContext)
        {
            if (hubContext is null)
            {
                throw new ArgumentNullException(nameof(hubContext));
            }

            _Queue.Listen<SendMailRequest>("noticeboard-broadcast", x => NewMail(x));
            _Clients = hubContext.Clients;
        }
        
        private static void NewMail(SendMailRequest request)
        {
            try
            {
                var model = new MailModel
                {
                    Content = request.Content,
                    Sender = request.Sender,
                    SentAt = request.SentAt.ToString("HH:mm.ss"),
                    SentDate = request.SentAt
                };

                MailBag.Add(model);
                _Clients.All.SendCoreAsync("newMail", new object[] { model });

                _Queue.Reply<MailBroadcastEvent>(new MailBroadcastEvent
                {
                    Sender = request.Sender,
                    SentAt = request.SentAt,                    
                    BroadcastAt = DateTime.Now
                });
            }
            catch
            {
                _Queue.Reply<HandlerFailedEvent>(new HandlerFailedEvent
                {
                    Sender = request.Sender,
                    SentAt = request.SentAt,
                    Message = "Broadcast failed!"
                });
            }
        }
    }
}