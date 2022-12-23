using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.Helpers;
using GPMService.Producer.Serializer;
using Icon.Common.Xml;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using System;
using System.Collections.Generic;

namespace GPMService.Producer.Archive
{
    internal class JustInTimePriceArchiver
    {
        private readonly ICommonDAL commonDAL;
        private readonly ErrorEventPublisher errorEventPublisher;
        private readonly ISerializer<MammothPricesType> mammothPricesSerializer;
        private readonly ILogger<JustInTimePriceArchiver> logger;

        public JustInTimePriceArchiver(
            ICommonDAL commonDAL, 
            ErrorEventPublisher errorEventPublisher, 
            ISerializer<MammothPricesType> mammothPricesSerializer, 
            ILogger<JustInTimePriceArchiver> logger
            )
        {
            this.commonDAL = commonDAL;
            this.errorEventPublisher = errorEventPublisher;
            this.mammothPricesSerializer = mammothPricesSerializer;
            this.logger = logger;
        }

        public void ArchivePrice(MammothPricesType pricesToArchive, Dictionary<string, string> priceProperties)
        {
            try
            {
                IEnumerable<MammothPriceType> archivedPrices = commonDAL.ArchiveActivePrice(pricesToArchive);
                List<string> logList = new List<string>();
                foreach (MammothPriceType archivedPrice in archivedPrices)
                {
                    logList.Add($@"{{Region:{archivedPrice.Region}, PriceID:{archivedPrice.PriceID}}}");
                }
                logger.Info($@"{{Archived Prices:{{TransactionID:{priceProperties[Constants.MessageHeaders.TransactionID]}, Prices: {string.Join(",", logList)}}}");
            }
            catch (Exception e)
            {
                logger.Error($@"ArchivePriceError: MessageID:'{priceProperties[Constants.MessageHeaders.TransactionID]}', Msg:'{e.Message}', StackTrace: {e.StackTrace}");
                errorEventPublisher.PublishErrorEvent(
                    "MammothActivePriceArchiver",
                    priceProperties[Constants.MessageHeaders.TransactionID],
                    priceProperties,
                    mammothPricesSerializer.Serialize(pricesToArchive, new Utf8StringWriter()),
                    e.GetType().ToString(),
                    e.Message,
                    "Fatal"
                    );
            }
        }
    }
}
