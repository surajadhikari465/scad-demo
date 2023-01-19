using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using ErrorMessagesMonitor.DataAccess;
using ErrorMessagesMonitor.Model;
using ErrorMessagesMonitor.Serializer;
using Icon.Common.Xml;
using Topshelf.Options;

namespace ErrorMessagesMonitor.Message.Processor
{
    internal class ErrorMessagesProcessor : IErrorMessagesProcessor
    {
        private readonly IErrorMessagesMonitorDAL errorMessagesMonitorDAL;
        private readonly ISerializer<ErrorDetailsCanonicalModel> serializer;

        public ErrorMessagesProcessor(
            IErrorMessagesMonitorDAL errorMessagesMonitorDAL,
            ISerializer<ErrorDetailsCanonicalModel> serializer
        )
        {
            this.errorMessagesMonitorDAL = errorMessagesMonitorDAL;
            this.serializer = serializer;
        }

        public void Process()
        {
            string instanceID = Guid.NewGuid().ToString();
            errorMessagesMonitorDAL.MarkErrorMessageRecordsAsInProcess(instanceID);
            IList<ErrorMessageModel> errorMessageList = errorMessagesMonitorDAL.GetErrorMessages(instanceID);
            foreach (var errorMessage in errorMessageList)
            {
                IList<ErrorDetailsModel> errorDetailsList = 
                    errorMessagesMonitorDAL.GetErrorDetails(instanceID, errorMessage);

                ErrorDetailsCanonicalModel errorDetailsCanonicalModel = new ErrorDetailsCanonicalModel
                {
                    ErrorDetailsList = errorDetailsList,
                    Application = errorMessage.Application,
                    ErrorCode = errorMessage.ErrorCode,
                    ErrorSeverity = errorMessage.ErrorSeverity
                };

                string errorDetailsCanonicalXml = serializer.Serialize(errorDetailsCanonicalModel, new Utf8StringWriter());

                // TODO - Transform xml to html, if fatal -> mapContent -> renderJSON -> Send Opsgenie else if send emails -> send mail else end

            }
        }
    }
}
