using System;
using System.Linq;
using Icon.Common.Email;
using Icon.Logging;
using IrmaPriceListenerService.DataAccess;
using IrmaPriceListenerService.Archive;
using Icon.Esb.Schemas.Mammoth;
using Polly;
using Polly.Retry;
using System.Collections.Generic;
using IrmaPriceListenerService.Model;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using IrmaPriceListenerService.Service.Parser;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace IrmaPriceListenerService.Listener
{
    public class IrmaPriceListener : SQSExtendedClientListener<IrmaPriceListener>
    {
        private readonly IIrmaPriceDAL irmaPriceDAL;
        private readonly IMessageArchiver messageArchiver;
        private readonly IErrorEventPublisher errorEventPublisher;
        private readonly IMessageParser<MammothPricesType> messageParser;
        private readonly RetryPolicy retryPolicy;

        private const int DB_TIMEOUT_RETRY_COUNT = 3;
        private const int RETRY_INTERVAL_MILLISECONDS = 5000;

        public IrmaPriceListener(
            SQSExtendedClientListenerSettings settings,
            ISQSExtendedClient sqsExtendedClient,
            IEmailClient emailClient,
            ILogger<IrmaPriceListener> logger,
            IIrmaPriceDAL irmaPriceDAL,
            IMessageParser<MammothPricesType> messageParser,
            IMessageArchiver messageArchiver,
            IErrorEventPublisher errorEventPublisher
        ): base(settings, emailClient, sqsExtendedClient, logger)
        {
            this.irmaPriceDAL = irmaPriceDAL;
            this.messageParser = messageParser;
            this.messageArchiver = messageArchiver;
            this.errorEventPublisher = errorEventPublisher;
            this.retryPolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                    DB_TIMEOUT_RETRY_COUNT,
                    retryAttempt => TimeSpan.FromMilliseconds(RETRY_INTERVAL_MILLISECONDS)
                );
        }

        public override void HandleMessage(SQSExtendedClientReceiveModel message)
        {
            string guid = Guid.NewGuid().ToString();
            MammothPriceType[] mammothPrices = null;
            IList<MammothPriceWithErrorType> mammothPricesWithErrors = null;

            try
            {
                logger.Info($"Received Message: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}");
                mammothPrices = messageParser.ParseMessage(message).MammothPrice;

                var mammothPricesWithoutRwd = mammothPrices.Where<MammothPriceType>(
                    x => x.PriceType != Constants.PriceType.Rwd
                ).ToList();

                logger.Info(
                    $@"{{""TransactionID"": ""{message.MessageAttributes[Constants.MessageAttribute.TransactionId]}"",
                ""Message"": ""Filtering RWD prices from message."",
                ""NumberOfPrices"": ""{mammothPrices.Length}"",
                ""NumberOfRwdPrices"": ""{mammothPrices.Length - mammothPricesWithoutRwd.Count}""}}"
                );

                if (mammothPricesWithoutRwd.Count > 0)
                {
                    long groupStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    var orderedPrices = OrderAndGroupMammothPrices(mammothPricesWithoutRwd);
                    logger.Info($@"TransactionID: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}, 
Grouping Time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - groupStartTime} ms");

                    long irmaLoadStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    mammothPricesWithErrors = LoadMammothPricesToIrma(orderedPrices, guid);
                    logger.Info($@"TransactionID: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}, 
Irma Load Time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - irmaLoadStartTime} ms");

                    long irmaUpdateStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    UpdateIrmaPrice(guid);
                    logger.Info($@"TransactionID: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}, 
Irma Update Time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - irmaUpdateStartTime} ms");

                    long deleteStagedStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                    DeleteStagedMammothPrice(guid);
                    logger.Info($@"TransactionID: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}, 
Irma Delete Time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - deleteStagedStartTime} ms");

                    logger.Info(
                        $@"{{""TransactionID"": ""{message.MessageAttributes[Constants.MessageAttribute.TransactionId]}"",
                    ""Message"": ""Successfully processed IRMA Price update.""}}"
                    );
                }
                else
                {
                    logger.Info(
                        $@"{{""TransactionID"": ""{message.MessageAttributes[Constants.MessageAttribute.TransactionId]}"",
                    ""Message"": ""Filtered all prices out of IRMA update because they were all RWD prices.""}}"
                    );
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Error occurred on Message: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}, Error Details: {ex}");
                this.errorEventPublisher.PublishErrorMessage(message, ex);
                DeleteStagedMammothPrice(guid);
            }
            finally
            {
                Acknowledge(message);

                long archiveStartTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                messageArchiver.ArchivePriceMessage(message, mammothPrices, mammothPricesWithErrors);
                logger.Info($@"TransactionID: {message.MessageAttributes[Constants.MessageAttribute.TransactionId]}, 
Archive Time: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() - archiveStartTime} ms");
            }
        }

        // Orders By StartDate, Groups By ItemId, Groups By PriceType, Returns Flattened Result
        private IList<MammothPriceType> OrderAndGroupMammothPrices(IList<MammothPriceType> mammothPrices)
        {
            var pricesOrderedByStartDate = mammothPrices.OrderBy(x => x.StartDate).ToList();
            var pricesGroupedByItemId = pricesOrderedByStartDate.GroupBy(x => x.ItemId);
            IList<MammothPriceType> flattenedList = new List<MammothPriceType>();

            foreach(var itemGroup in pricesGroupedByItemId)
            {
                var itemPriceList = itemGroup.ToList();
                var itemPricesGroupedByType = itemPriceList.GroupBy(x => x.PriceType);

                foreach(var priceTypeGroup in itemPricesGroupedByType)
                {
                    var priceTypeList = priceTypeGroup.ToList();
                    foreach(var price in priceTypeList)
                    {
                        flattenedList.Add(price);
                    }
                }
            }
            return flattenedList;
        }

        // Tries batch update & falls back to single updates if batch update didn't work
        private IList<MammothPriceWithErrorType> LoadMammothPricesToIrma(IList<MammothPriceType> mammothPrices, string guid)
        {
            try
            {
                this.retryPolicy.Execute(() =>
                {
                    irmaPriceDAL.LoadMammothPricesBatch(mammothPrices, guid);
                });
                return new List<MammothPriceWithErrorType>();
            }
            catch (Exception e)
            {
                logger.Error(e.ToString());
                return this.LoadPricesToIrmaOneByOne(mammothPrices, guid);
            }
        }

        private IList<MammothPriceWithErrorType> LoadPricesToIrmaOneByOne(IList<MammothPriceType> mammothPrices, string guid)
        {
            IList<MammothPriceWithErrorType> pricesErrorDetails = new List<MammothPriceWithErrorType>();

            foreach (var mammothPrice in mammothPrices)
            {
                try
                {
                    irmaPriceDAL.LoadMammothPricesSingle(mammothPrice, guid);
                }
                catch (Exception ex)
                {
                    pricesErrorDetails.Add(new MammothPriceWithErrorType()
                    {
                        MammothPrice = mammothPrice,
                        ErrorCode = ex.GetType().ToString(),
                        ErrorDetails = ex.ToString()
                    });
                }
            }
            return pricesErrorDetails;
        }

        private void UpdateIrmaPrice(string guid)
        {
            this.retryPolicy.Execute(() =>
            {
                irmaPriceDAL.UpdateIrmaPrice(guid);
            });
        }

        private void DeleteStagedMammothPrice(string guid)
        {
            this.retryPolicy.Execute(() =>
            {
                irmaPriceDAL.DeleteStagedMammothPrices(guid);
            });
        }
    }
}
