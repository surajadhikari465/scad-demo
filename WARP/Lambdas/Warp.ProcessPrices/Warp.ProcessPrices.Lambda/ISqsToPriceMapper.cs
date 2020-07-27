using Amazon.Lambda.SQSEvents;
using Warp.ProcessPrices.Common;

namespace Warp.ProcessPrices.Lambda
{
    public interface ISqsToPriceMapper
    {
        MappedPriceModelResponse Map(SQSEvent.SQSMessage sqsRecord);
    }
}