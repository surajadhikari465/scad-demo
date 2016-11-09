using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Contracts = Icon.Esb.Schemas.Ewic.ContractTypes;

namespace Icon.Esb.EwicAplListener.MessageParsers
{
    public class AplMessageParser : IMessageParser<AuthorizedProductListModel>
    {
        private ILogger<AplMessageParser> logger;
        private XmlSerializer serializer;
        private TextReader textReader;

        public AplMessageParser(ILogger<AplMessageParser> logger)
        {
            this.logger = logger;
            this.serializer = new XmlSerializer(typeof(Contracts.eWICExport));
        }

        public AuthorizedProductListModel ParseMessage(IEsbMessage message)
        {
            Contracts.eWICExport deserializedMessage;

            using (textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(message.MessageText)))
            {
                deserializedMessage = serializer.Deserialize(textReader) as Contracts.eWICExport;
            }

            if (deserializedMessage == null)
            {
                throw new InvalidOperationException("The message deserialized to a null object.  The AuthorizedProductListModel cannot be built.");
            }
            else
            {
                var aplModel = new AuthorizedProductListModel
                {
                    MessageXml = XDocument.Parse(message.MessageText).ToString(),
                    Items = new List<EwicItemModel>()
                };

                EwicItemModel ewicItemModel;
                decimal? benefitQuantity;
                decimal? packageSize;
                decimal? itemPrice;

                foreach (var item in deserializedMessage.Items)
                {
                    // BenefitQuantity, PackageSize, and ItemPrice look like an integers in the xml, but they need to be parsed like a decimal(5,2).
                    benefitQuantity = item.BenefitQuantitySpecified ? (decimal?)item.BenefitQuantity / 100 : null;
                    itemPrice = item.ItemPriceSpecified ? (decimal?)item.ItemPrice / 100 : null;
                    packageSize = item.PackageSizeSpecified ? (decimal?)item.PackageSize / 100 : null;

                    ewicItemModel = new EwicItemModel
                    {
                        AgencyId = item.StateID,
                        ScanCode = item.UPC,
                        ItemDescription = item.ItemDescription,
                        UnitOfMeasure = item.UOM,
                        PackageSize = packageSize,
                        BenefitQuantity = benefitQuantity,
                        BenefitUnitDescription = item.BenefitUnitDescription,
                        ItemPrice = itemPrice,
                        PriceType = item.PriceType
                    };

                    aplModel.Items.Add(ewicItemModel);
                }

                logger.Info(String.Format("Message has been parsed successfully.  APL file contains {0} entries.", aplModel.Items.Count));

                return aplModel;
            }
        }
    }
}
