using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class ArchiveEventModelWrapper<TEventModel>
        where TEventModel : class
    {
        public TEventModel EventModel { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDetails { get; set; }

        public ArchiveEventModelWrapper() { }

        public ArchiveEventModelWrapper(TEventModel eventModel)
        {
            EventModel = eventModel;
        }

        public ArchiveEventModelWrapper(TEventModel eventModel, string errorCode, string errorDetails)
        {
            EventModel = eventModel;
            ErrorCode = errorCode;
            ErrorDetails = errorDetails;
        }
    }
}
