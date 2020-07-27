using System;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Warp.ProcessPrices.DataAccess.Commands;

namespace Warp.ProcessPrices.Lambda
{
    public class SqsMessageProcessor : ISqsMessageProcessor
    {
        private readonly ISqsToPriceMapper mapper;
        private readonly ICommandHandler<AddPriceCommand> addPriceCommandHandler;

        public SqsMessageProcessor(ISqsToPriceMapper mapper, ICommandHandler<AddPriceCommand> addPriceCommandHandler)
        {
            this.mapper = mapper;
            this.addPriceCommandHandler = addPriceCommandHandler;
        }

        public void Process(SQSEvent.SQSMessage sqsMessage, ILambdaContext context)
        {
            try
            {
                var priceResponse = mapper.Map(sqsMessage);
                if (priceResponse.IsValid)
                {
                    // process
                    addPriceCommandHandler.Execute(new AddPriceCommand { Price = priceResponse.PriceModel });
                }
                else
                {
                    //log, dlq,etc
                }
            }
            catch (Exception ex)
            {
                LambdaLogger.Log($"[Error] {ex.Message}\r\n");
                if (ex.InnerException != null)
                    LambdaLogger.Log($"[Inner] {ex.InnerException.Message}\r\n");
                LambdaLogger.Log($"[Stack] {ex.StackTrace}\r\n");
            }
        }
    }
}
