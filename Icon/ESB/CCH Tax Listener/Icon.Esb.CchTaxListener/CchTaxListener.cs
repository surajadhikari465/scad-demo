using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.MessageParsers;
using Icon.Esb.CchTax.Models;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using TIBCO.EMS;

namespace Icon.Esb.CchTax
{
    public class CchTaxListener : ListenerApplication<CchTaxListener, CchTaxListenerApplicationSettings>
    {
        private ICommandHandler<SaveTaxHierarchyClassesCommand> saveTaxHierarchyClassCommandHandler;
        private ICommandHandler<SaveTaxToMammothCommand> saveTaxToMammothCommandHandler;
        private IEnumerable<RegionModel> regions;
        private IMessageParser<List<TaxHierarchyClassModel>> messageParser;

        public CchTaxListener(
            CchTaxListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IMessageParser<List<TaxHierarchyClassModel>> messageParser,
            IEmailClient emailClient,
            ILogger<CchTaxListener> logger,
            ICommandHandler<SaveTaxHierarchyClassesCommand> saveTaxHierarchyClassCommandHandler,
            ICommandHandler<SaveTaxToMammothCommand> saveTaxToMammothCommandHandler,
            IEnumerable<RegionModel> regions)
                : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.saveTaxHierarchyClassCommandHandler = saveTaxHierarchyClassCommandHandler;
            this.saveTaxToMammothCommandHandler = saveTaxToMammothCommandHandler;
            this.regions = regions;
            this.messageParser = messageParser;
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            try
            {
                var parameters = new SaveTaxHierarchyClassesCommand
                {
                    TaxHierarchyClasses = messageParser.ParseMessage(args.Message),
                    Regions = regions,
                    CchTaxMessage = XDocument.Parse(args.Message.MessageText).ToString() //Parsing the message to remove any possible unwanted characters
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
            catch (Exception ex)
            {
                LogAndNotifyErrorWithMessage(ex, args);
            }

            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }
    }
}
