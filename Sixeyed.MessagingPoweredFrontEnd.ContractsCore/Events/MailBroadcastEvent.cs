using System;

namespace Sixeyed.MessagingPoweredFrontEnd.ContractsCore
{
    public class MailBroadcastEvent
    {
        public string Sender { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime BroadcastAt { get; set; }
    }
}
