using Icon.Common.Email;
using Icon.Esb.CchTax.Commands;
using Icon.Esb.CchTax.Decorator;
using Icon.Esb.CchTax.Infrastructure;
using Icon.Esb.CchTax.MessageParsers;
using Icon.Esb.CchTax.Models;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System.Collections.Generic;
using System.Configuration;

namespace Icon.Esb.CchTax
{
    public static class CchTaxListenerBuilder
    {
        public static CchTaxListener Build()
        {
            var applicationSettings = CchTaxListenerApplicationSettings.CreateDefaultSettings<CchTaxListenerApplicationSettings>("CCH Tax Listener");
            var connectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
            
            IMessageParser<List<TaxHierarchyClassModel>> messageParser = new CchTaxMessageParser();
            DataConnectionManager manager = new DataConnectionManager();
            string mammothConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

            CchTaxListener listener = new CchTaxListener(
                applicationSettings,
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                messageParser,
                EmailClient.CreateFromConfig(),
                new NLogLogger<CchTaxListener>(),
                new SaveTaxHierarchyClassesCommandHandler(applicationSettings),
                new DataConnectionCommandHandlerDecorator<SaveTaxToMammothCommand>(
                    manager, 
                    mammothConnectionString,
                    new SaveTaxToMammothCommandHandler(manager, applicationSettings)),
                RegionModel.CreateRegionModelsFromConfig());

            return listener;
        }
    }
}
