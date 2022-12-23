using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.MessageParsers;
using Icon.Esb.CchTax.Models;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace Icon.Esb.CchTax
{
    public class CchTaxListener : ListenerApplication<CchTaxListener>
    {
        private ICommandHandler<SaveTaxHierarchyClassesCommand> saveTaxHierarchyClassCommandHandler;
        private ICommandHandler<SaveTaxToMammothCommand> saveTaxToMammothCommandHandler;
        private IEnumerable<RegionModel> regions;
        private IMessageParser<List<TaxHierarchyClassModel>> messageParser;

        public CchTaxListener(
            DvsListenerSettings listenerSettings,
            IDvsSubscriber subscriber,
            IMessageParser<List<TaxHierarchyClassModel>> messageParser,
            IEmailClient emailClient,
            ILogger<CchTaxListener> logger,
            ICommandHandler<SaveTaxHierarchyClassesCommand> saveTaxHierarchyClassCommandHandler,
            ICommandHandler<SaveTaxToMammothCommand> saveTaxToMammothCommandHandler,
            IEnumerable<RegionModel> regions)
                : base(listenerSettings, subscriber , emailClient, logger)
        {
            this.saveTaxHierarchyClassCommandHandler = saveTaxHierarchyClassCommandHandler;
            this.saveTaxToMammothCommandHandler = saveTaxToMammothCommandHandler;
            this.regions = regions;
            this.messageParser = messageParser;
        }

        public override void HandleMessage(DvsMessage message)
        {
           
                var parameters = new SaveTaxHierarchyClassesCommand
                {
                    TaxHierarchyClasses = messageParser.ParseMessage(message),
                    Regions = regions,
                    CchTaxMessage = XDocument.Parse(message.MessageContent).ToString() //Parsing the message to remove any possible unwanted characters
                };

                saveTaxHierarchyClassCommandHandler.Execute(parameters);

                saveTaxToMammothCommandHandler.Execute(new SaveTaxToMammothCommand
                {
                    TaxHierarchyClasses = parameters.TaxHierarchyClasses
                });

                if (regions.Any())
                {
                    logger.Info(String.Format("Processed {0} tax messages and generated events for regions {1}.", parameters.TaxHierarchyClasses.Count, String.Join(", ", regions.Select(r => r.RegionAbbr))));
                }
                else
                {
                    logger.Info(String.Format("Processed {0} tax messages.", parameters.TaxHierarchyClasses.Count));
                }
        }
    }
}
