using GPMService.Producer.Model.DBModel;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface INearRealTimeProcessorDAL
    {
        IList<MessageSequenceModel> GetLastSequence(string correlationID);
    }
}
