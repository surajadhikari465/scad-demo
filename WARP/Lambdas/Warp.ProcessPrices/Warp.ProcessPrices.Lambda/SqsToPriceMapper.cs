using System;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Newtonsoft.Json;
using Warp.ProcessPrices.Common;

namespace Warp.ProcessPrices.Lambda
{
    public class SqsToPriceMapper : ISqsToPriceMapper
    {
        public MappedPriceModelResponse Map(SQSEvent.SQSMessage sqsRecord)
        {
            LambdaLogger.Log("MAP: " + sqsRecord.MessageId);
            try
            {
                PriceModel price = JsonConvert.DeserializeObject<PriceModel>(sqsRecord.Body);
                return new MappedPriceModelResponse { IsValid = true, PriceModel = price };
            }
            catch (Exception ex)
            {
                return new MappedPriceModelResponse { IsValid = false, Message = ex.Message };
            }
        }
    }
}