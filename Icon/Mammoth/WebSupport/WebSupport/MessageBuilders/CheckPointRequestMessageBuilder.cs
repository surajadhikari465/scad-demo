using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using System;
using WebSupport.Models;
using Contracts = Icon.Esb.Schemas.Infor.ContractTypes;

namespace WebSupport.MessageBuilders
{
    public class CheckPointRequestMessageBuilder : IMessageBuilder<CheckPointRequestBuilderModel>
    {
        private const string DateTimeFormat = "yyyy-MM-ddTHH:mm:ss";
        private ISerializer<Contracts.PriceChangeMasterType> serializer;

        public CheckPointRequestMessageBuilder(ISerializer<Contracts.PriceChangeMasterType> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(CheckPointRequestBuilderModel request)
        {
            Contracts.PriceChangeMasterType priceChangeMasterType = new Contracts.PriceChangeMasterType
            {
                BusinessKey = new Contracts.PriceChangeMasterTypeBusinessKey
                {
                    Value = "00000000 - 0000 - 4000 - 8000 - 000000000000",
                    variationID = "0"
                },
                isCheckPoint = true,
                isCheckPointSpecified = true,
                PriceChangeHeader = new Contracts.PriceChangeType[]
                {
                   new Contracts.PriceChangeType
                     {
                       isCheckPointSpecified = true,
                       isCheckPoint = true,
                       PatchFamilyID = request.getCurrentPriceInfo.PatchFamilyId,
                       PatchNum = request.getCurrentPriceInfo.SequenceId.ToString(),
                       TimeStampSpecified = true,
                       TimeStamp = Convert.ToDateTime(DateTime.Now.ToString(DateTimeFormat))
                     }
              }
            };

            return serializer.Serialize(priceChangeMasterType, new Utf8StringWriter());
        }
    }
}