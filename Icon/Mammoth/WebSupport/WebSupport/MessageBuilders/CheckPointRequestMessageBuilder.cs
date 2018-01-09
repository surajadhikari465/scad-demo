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
        private ISerializer<Contracts.PriceChangeMaster> serializer;

        public CheckPointRequestMessageBuilder(ISerializer<Contracts.PriceChangeMaster> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(CheckPointRequestBuilderModel request)
        {
            Contracts.PriceChangeMaster priceChangeMaster = new Contracts.PriceChangeMaster
            {
                BusinessKey = new Contracts.PriceChangeMasterTypeBusinessKey
                {
                    Value = "00000000-0000-4000-8000-000000000000",
                    variationID = "0"
                },
                isCheckPointSpecified = true,
                isCheckPoint = true,
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

            return serializer.Serialize(priceChangeMaster, new Utf8StringWriter());
        }
    }
}