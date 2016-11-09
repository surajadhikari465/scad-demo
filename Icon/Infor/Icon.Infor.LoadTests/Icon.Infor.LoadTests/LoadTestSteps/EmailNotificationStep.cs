using Icon.Common.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.LoadTests.LoadTestSteps
{
    public class EmailNotificationStep : ILoadTestStep
    {
        public ILoadTestStatus Results { get; set; }
        public string[] Recipients { get; set; }
        public string EmailSubject { get; set; }

        public EmailNotificationStep(ILoadTestStatus results, string[] recipients, string emailSubject)
        {
            Results = results;
            Recipients = recipients;
            EmailSubject = emailSubject;
        }

        public LoadTestStepResult Execute()
        {
            EmailClient client = new EmailClient(new EmailClientSettings
            {
                Host = "smtp.wholefoods.com",
                Port = 25,
                Sender = "Icon-Load-Tests@wholefoods.com",
                Recipients = Recipients,
                SendEmails = true
            });

            client.Send(Results.CreateTableForEmail(), EmailSubject);

            return true;
        }
    }
}
