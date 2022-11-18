using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using System.Collections.Generic;

namespace GPMService.Producer.DataAccess
{
    internal interface INearRealTimeProcessorDAL
    {
        IList<MessageSequenceModel> GetLastSequence(string patchFamilyID);
        void ArchiveMessage(ReceivedMessage receivedMessage, string errorCode, string errorDetails);
        IList<GetRegionCodeQueryModel> GetRegionCodeQuery(string businessUnitID);
        void ArchiveErrorResponseMessage(string messageID, string messageTypeName, string xmlMessagePayload, Dictionary<string, string> messageProeprties);
        void ProcessPriceMessage(ReceivedMessage receivedMessage, MammothPricesType mammothPrices);
        void InsertEmergencyPrices(MammothPricesType mammothPrices);
    }
}
