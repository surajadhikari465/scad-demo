using GlobalEventController.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Common.Email;

namespace GlobalEventController.Controller.EventOperations
{
    public class DataIssueMessageCollector : IDataIssueMessageCollector
    {
        public List<RegionalItemMessageModel> Message { get; set; }
        private IEmailClient emailClient;

        public DataIssueMessageCollector (IEmailClient emailClient)
        {
            Message = new List<RegionalItemMessageModel>();
            this.emailClient = emailClient;
        }

        public void SendDataIssueMessage()
        {
            if (Message.Any())
            {
                var messageToSent = EmailHelper.BuildRegionalItemMessageTable(Message);
                emailClient.Send(String.Format(Resources.ErrorDataIssues, messageToSent), Resources.EmailSubjectItemUpdateDataIssue);
            }
        }
    }
}
