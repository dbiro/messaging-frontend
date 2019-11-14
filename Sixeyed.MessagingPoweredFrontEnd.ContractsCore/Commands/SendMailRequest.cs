using System;

namespace Sixeyed.MessagingPoweredFrontEnd.ContractsCore
{
    public class SendMailRequest
    {
        public string Sender { get; set; }

        public string Content { get; set; }

        public DateTime SentAt { get; set; }
    }
}
