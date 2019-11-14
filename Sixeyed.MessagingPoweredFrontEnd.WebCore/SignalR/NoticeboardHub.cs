using Microsoft.AspNetCore.SignalR;
using Sixeyed.MessagingPoweredFrontEnd.ContractsCore;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.Handlers;
using Sixeyed.MessagingPoweredFrontEnd.WebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sixeyed.MessagingPoweredFrontEnd.WebCore.SignalR
{    
    public class NoticeboardHub : Hub
    {
        public IEnumerable<MailModel> GetMail()
        {
            return NewMailHandler.MailBag.OrderByDescending(x => x.SentDate);
        }

        public void RegisterUser(string userId)
        {
            ReplyHandler.RegisterUser(userId, Context.ConnectionId);
        }

        public void Send(string sender, string content)
        {
            ReplyHandler.Queue.Publish<SendMailRequest>(new SendMailRequest
            {
                Content = content,
                Sender = sender,
                SentAt = DateTime.Now
            });
        }
    }
}