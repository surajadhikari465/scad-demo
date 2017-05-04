using GlobalEventController.Common;
using GlobalEventController.Controller.Decorators;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Common.Email;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using Irma.Framework;
using System;
using System.Configuration;

namespace GlobalEventController.Controller.EventServices
{
    public class EventServiceProvider : IEventServiceProvider
    {
        private ContextManager contextManager;
        private int commandTimeout;

        public EventServiceProvider(ContextManager contextManager)
        {
            this.contextManager = contextManager;
        }
        public IEventService GetBrandNameUpdateEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            if (eventName != Enums.EventNames.IconToIrmaBrandNameUpdate)
            {
                return null;
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            return new UpdateBrandEventService(
                irmaContext,
                new AddOrUpdateBrandCommandHandler(irmaContext, new NLogLoggerInstance<AddOrUpdateBrandCommandHandler>(StartupOptions.Instance.ToString())),
                new AddUpdateLastChangeByIdentifiersCommandHandler(irmaContext),
                new GetItemIdentifiersQueryHandler(irmaContext));
        }
        public IEventService GetBrandDeleteEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            if (eventName != Enums.EventNames.IconToIrmaBrandDelete)
            {
                return null;
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            return new BrandDeleteEventService(
                irmaContext,
                new BrandDeleteCommandHandler(irmaContext, new NLogLoggerInstance<BrandDeleteCommandHandler>(StartupOptions.Instance.ToString())));
        }
        public IEventService GetTaxEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            string connectionString = ConnectionBuilder.GetConnection(region);
            if (String.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException("There is no connection string found for the region associated to the event.");
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            switch (eventName)
            {
                case Enums.EventNames.IconToIrmaTaxClassUpdate:
                    return new UpdateTaxClassEventService(
                       irmaContext,
                       new UpdateTaxClassCommandHandler(irmaContext),
                       new GetHierarchyClassQueryHandler(iconContext));
                case Enums.EventNames.IconToIrmaNewTaxClass:
                    return new AddTaxClassEventService(
                        irmaContext,
                        new AddTaxClassCommandHandler(irmaContext),
                        new GetHierarchyClassQueryHandler(iconContext));
                default:
                    return null;
            }
        }
        public IBulkEventService GetBulkItemEventService(string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            return new BulkItemEventServiceUomChangeEmailDecorator(
                new EmailUomChangeService(EmailClient.CreateFromConfigForRegion(region)),
                new GetItemsByScanCodeQueryHandler(irmaContext),
                GlobalControllerSettings.CreateFromConfig(region),
                    new BulkItemEventService(irmaContext,
                        new NLogLoggerInstance<BulkItemEventService>(StartupOptions.Instance.ToString()),
                        new BulkAddBrandCommandHandler(irmaContext),
                        new BulkAddUpdateLastChangeCommandHandler(irmaContext),
                        new BulkUpdateItemCommandHandler(irmaContext),
                        new BulkAddValidatedScanCodeCommandHandler(irmaContext),
                        new BulkGetItemsWithTaxClassQueryHandler(irmaContext),
                        new BulkUpdateNutrifactsCommandHandler(irmaContext),
                        new BulkUpdateItemSignAttributesCommandHandler(irmaContext),
                        new BulkGetItemsWithNoNatlClassQueryHandler(irmaContext),
                        new BulkGetItemsWithNoRetailUomQueryHandler(irmaContext)));
        }
        public IBulkEventService GetBulkItemNutriFactsEventService(string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            return new BulkItemNutriFactsService(irmaContext,
                new BulkUpdateNutrifactsCommandHandler(irmaContext),
                new BulkAddUpdateLastChangeCommandHandler(irmaContext),
                new BulkGetItemsWithTaxClassQueryHandler(irmaContext));
        }
        public IEventService GetDeleteNationalHierarchyEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            if (eventName != Enums.EventNames.IconToIrmaNationalHierarchyDelete)
            {
                return null;
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            return new DeleteNationalHierarchyEventService(
                irmaContext,
                new DeleteNationalHierarchyCommandHandler(irmaContext, new NLogLoggerInstance<DeleteNationalHierarchyCommandHandler>(StartupOptions.Instance.ToString())));
        }
        public IEventService GetAddOrUpdateNationalHierarchyEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            if (eventName != Enums.EventNames.IconToIrmaNationalHierarchyUpdate)
            {
                return null;
            }

            var iconContext = contextManager.IconContext;
            var irmaContext = contextManager.IrmaContexts[region];
            SetIrmaDbContextConnectionTimeout(irmaContext);

            return new AddOrUpdateNationalHierarchyEventService(
                irmaContext, iconContext,
                new AddOrUpdateNationalHierarchyCommandHandler(irmaContext,
                                                                new NLogLoggerInstance<AddOrUpdateNationalHierarchyCommandHandler>(StartupOptions.Instance.ToString())));
        }
        public IEventService GetEventService(Enums.EventNames eventName, string region)
        {
            switch (eventName)
            {
                case Enums.EventNames.IconToIrmaBrandNameUpdate:
                    return GetBrandNameUpdateEventService(eventName, region);

                case Enums.EventNames.IconToIrmaTaxClassUpdate:
                case Enums.EventNames.IconToIrmaNewTaxClass:
                    return GetTaxEventService(eventName, region);
                case Enums.EventNames.IconToIrmaNutritionAdd:
                case Enums.EventNames.IconToIrmaBrandDelete:
                    return GetBrandDeleteEventService(eventName, region);
                case Enums.EventNames.IconToIrmaNationalHierarchyUpdate:
                    return GetAddOrUpdateNationalHierarchyEventService(eventName, region);
                case Enums.EventNames.IconToIrmaNationalHierarchyDelete:
                    return GetDeleteNationalHierarchyEventService(eventName, region);
                default:
                    return null;
            }
        }
        public IBulkItemSubTeamEventService GetBulkItemSubTeamEventService(string region)
        {
            throw new NotImplementedException();
        }
        public IEventService GetSubTeamEventService(Enums.EventNames eventNamestring, string region)
        {
            throw new NotImplementedException();
        }
        public IEventService GetItemSubTeamEventService(Enums.EventNames eventName, string region)
        {
            throw new NotImplementedException();
        }
        public void RefreshContexts()
        {
            contextManager.RefreshContexts();
        }
        private void SetIrmaDbContextConnectionTimeout(IrmaContext irmaContext)
        {
            string timeoutConfiguration = ConfigurationManager.AppSettings["DbContextConnectionTimeout"];

            if (int.TryParse(timeoutConfiguration, out commandTimeout))
            {
                irmaContext.Database.CommandTimeout = commandTimeout;
            }
        }
    }
}
