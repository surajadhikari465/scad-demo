using System;
using System.Linq;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.Model;
using Icon.Dvs;
using Icon.Dvs.Subscriber;
using Icon.Common.Email;
using Icon.Logging;
using IrmaPriceListenerService.DataAccess;
using IrmaPriceListenerService.Archive;
using Icon.Dvs.MessageParser;
using Icon.Esb.Schemas.Mammoth;
using Polly;
using Polly.Retry;
using System.Collections.Generic;
using IrmaPriceListenerService.Model;

namespace IrmaPriceListenerService.Listener
{
    public class IrmaPriceListener : ListenerApplication<IrmaPriceListener>
    {
        private IIrmaPriceDAL irmaPriceDAL;
        private IMessageArchiver messageArchiver;
        private IErrorEventPublisher errorEventPublisher;
        private IMessageParser<MammothPricesType> messageParser;
        private RetryPolicy retryPolicy;

        private const int DB_TIMEOUT_RETRY_COUNT = 3;
        private const int RETRY_INTERVAL_MILLISECONDS = 0;

        public IrmaPriceListener(
            DvsListenerSettings settings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<IrmaPriceListener> logger,
            IIrmaPriceDAL irmaPriceDAL,
            IMessageArchiver messageArchiver,
            IErrorEventPublisher errorEventPublisher
        ): base(settings, subscriber, emailClient, logger)
        {
            this.irmaPriceDAL = irmaPriceDAL;
            this.messageArchiver = messageArchiver;
            this.errorEventPublisher = errorEventPublisher;
            this.retryPolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                    DB_TIMEOUT_RETRY_COUNT,
                    retryAttempt => TimeSpan.FromMilliseconds(RETRY_INTERVAL_MILLISECONDS)
                );
        }

        public override void HandleMessage(DvsMessage message)
        {
            string guid = Guid.NewGuid().ToString();
            MammothPriceType[] mammothPrices = null;
            IList<MammothPriceWithErrorType> mammothPricesWithErrors = null;

            try
            {
                logger.Info($"Received Message: {message.SqsMessage.MessageAttributes[Constants.MessageAttribute.TransactionId]}");
                mammothPrices = messageParser.ParseMessage(message).MammothPrice;

                var mammothPricesWithoutRwd = mammothPrices.Where<MammothPriceType>(
                    x => x.PriceType != Constants.PriceType.Rwd
                ).ToList();

                logger.Info(
                    $@"{{""TransactionID"": ""{message.SqsMessage.MessageAttributes[Constants.MessageAttribute.TransactionId]}"",
                ""Message"": ""Filtering RWD prices from message."",
                ""NumberOfPrices"": ""{mammothPrices.Length}"",
                ""NumberOfRwdPrices"": ""{mammothPrices.Length - mammothPricesWithoutRwd.Count}""}}"
                );

                if (mammothPricesWithoutRwd.Count > 0)
                {
                    var orderedPrices = OrderAndGroupMammothPrices(mammothPricesWithoutRwd);
                    mammothPricesWithErrors = LoadMammothPricesToIrma(orderedPrices, guid);
                    UpdateIrmaPrice(guid);
                    DeleteStagedMammothPrice(guid);
                }
                else
                {
                    logger.Info(
                        $@"{{""TransactionID"": ""{message.SqsMessage.MessageAttributes[Constants.MessageAttribute.TransactionId]}"",
                    ""Message"": ""Filtered all prices out of IRMA update because they were all RWD prices.""}}"
                    );
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Error occurred on Message: {message.SqsMessage.MessageAttributes[Constants.MessageAttribute.TransactionId]}, Error Details: {ex}");
                this.errorEventPublisher.PublishErrorMessage(message, ex);
                DeleteStagedMammothPrice(guid);
            }
            finally
            {
                messageArchiver.ArchivePriceMessage(message, mammothPrices, mammothPricesWithErrors);
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
