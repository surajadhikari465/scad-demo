using System.Collections.Generic;
using ErrorMessagesMonitor.Model;

namespace ErrorMessagesMonitor.DataAccess
{
    internal interface IErrorMessagesMonitorDAL
    {
        void MarkErrorMessageRecordsAsInProcess(string instanceID);

        IList<ErrorMessageModel> GetErrorMessages(string instanceID);

        IList<ErrorDetailsModel> GetErrorDetails(string instanceID, ErrorMessageModel errorMessage);

        void MarkErrorMessagesAsProcessed(string instanceID);
    }
}
