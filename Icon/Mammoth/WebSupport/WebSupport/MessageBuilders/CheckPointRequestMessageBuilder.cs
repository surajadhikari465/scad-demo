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
        private ISerializer<Contracts.ProcessPriceChangePatchType> serializer;

        public CheckPointRequestMessageBuilder(ISerializer<Contracts.ProcessPriceChangePatchType> serializer)
        {
            this.serializer = serializer;
        }

        public string BuildMessage(CheckPointRequestBuilderModel request)
        {
            Contracts.ProcessPriceChangePatchType processPriceChangePatchType = new Contracts.ProcessPriceChangePatchType
            {
                DataArea = new Contracts.ProcessPriceChangePatchDataAreaType
                {
                    Process = new Contracts.ProcessType
                    {
                        TenantID = new Contracts.MetaIdentifierType
                        {
                            Value = "WFM"
                        }
                    },
                    PriceChangePatchMaster = new Contracts.PriceChangePatchMaster[]
                    {
                        new Contracts.PriceChangePatchMaster
                        {
                            isCheckPoint = true,
                            PriceChangePatchHeader = new Contracts.PriceChangePatchType[]
                            {
                                  new Contracts.PriceChangePatchType
                                    {
                                      PatchFamilyID = request.getCurrentPriceInfo.PatchFamilyId,
                                      PatchNum = request.getCurrentPriceInfo.SequenceId.ToString(),
                                      TimeStamp = Convert.ToDateTime(DateTime.Now.ToString(DateTimeFormat))
                                    }
                            }
                        }
                    }
                }
            };

            return serializer.Serialize(processPriceChangePatchType, new Utf8StringWriter());
        }
    }
}