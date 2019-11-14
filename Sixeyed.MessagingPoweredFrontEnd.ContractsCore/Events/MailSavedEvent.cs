using System;

namespace Sixeyed.MessagingPoweredFrontEnd.ContractsCore
{
    public class MailSavedEvent
    {
        public string Sender { get; set; }

        public DateTime SentAt { get; set; }

        public DateTime SavedAt { get; set; }
    }
}
