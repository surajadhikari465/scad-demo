using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb.Schemas.Wfm.Contracts;
using Mammoth.Common.DataAccess;
using System;
using System.Globalization;
using System.Linq;
using WebSupport.DataAccess.Models;
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
                    Process =  new Contracts.ProcessType
                    {

                    },
                    PriceChangePatchMaster =  new Contracts.PriceChangePatchMaster[]
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
                                      TimeStamp = DateTime.Now
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