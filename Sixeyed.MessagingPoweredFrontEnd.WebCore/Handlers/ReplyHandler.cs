using Microsoft.AspNetCore.SignalR;
using Sixeyed.MessagingPoweredFrontEnd.ContractsCore;
using Sixeyed.MessagingPoweredFrontEnd.QueueCore;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.Models;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.SignalR;
using System;
using System.Collections.Concurrent;

namespace Sixeyed.MessagingPoweredFrontEnd.WebCore.Handlers
{
    public static class ReplyHandler
    {
        private static ConcurrentDictionary<string, string> _UserConnectionIds = new ConcurrentDictionary<string, string>();
        public static Queue Queue;
        private static IHubClients _Clients;       
        
        public static void Init(IHubContext<NoticeboardHub> hubContext)
        {
            string queueHost = "localhost";
            Queue = new Queue(queueHost, createReplyQueue: true);
            Queue.Listen<MailBroadcastEvent>(Queue.ReplyQueueName, x => MailBroadcast(x));
            Queue.Listen<MailSavedEvent>(Queue.ReplyQueueName, x => MailSaved(x));
            _Clients = hubContext.Clients;
        }

        private static void MailSaved(MailSavedEvent mailSaved)
        {
            Send(mailSaved.Sender, () => new ResponseModel
            {
                Sender = mailSaved.Sender,
                SentAt = mailSaved.SentAt.ToString("HH:mm:ss.fff"),
                EventAt = mailSaved.SavedAt.ToString("HH:mm:ss.fff"),
                Event = "saved"
            });
        }

        private static void MailBroadcast(MailBroadcastEvent mailBroadcast)
        {
            Send(mailBroadcast.Sender, () => new ResponseModel
            {
                Sender = mailBroadcast.Sender,
                SentAt = mailBroadcast.SentAt.ToString("HH:mm:ss.fff"), 
                EventAt = mailBroadcast.BroadcastAt.ToString("HH:mm:ss.fff"),
                Event = "broadcast"
            });
        }

        private static void Send(string userId, Func<ResponseModel> modelBuilder)
        {
            if (!_UserConnectionIds.ContainsKey(userId))
            {
                return;
            }

            var connectionId = _UserConnectionIds[userId];
            var model = modelBuilder();

            _Clients.Client(connectionId).SendCoreAsync("showResponse", new object[] { model });
        }

        public static void RegisterUser(string userId, string connectionId)
        {
            _UserConnectionIds[userId] = connectionId;
        }
    }
}