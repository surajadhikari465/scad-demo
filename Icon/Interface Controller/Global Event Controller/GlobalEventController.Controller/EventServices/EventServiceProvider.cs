using GlobalEventController.Common;
using GlobalEventController.Controller.Decorators;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using System;
using System.Configuration;

namespace GlobalEventController.Controller.EventServices
{
    public class EventServiceProvider : IEventServiceProvider
    {
        private static EmailClient emailClient = new EmailClient(EmailClientSettings.CreateFromConfig());

        private int commandTimeout;
        private IDbContextFactory<IconContext> iconContextFactory;
        private IRegionalIrmaDbContextFactory irmaContextFactory;

        public EventServiceProvider(IDbContextFactory<IconContext> iconContextFactory, IRegionalIrmaDbContextFactory irmaContextFactory)
        {
            this.iconContextFactory = iconContextFactory;
            this.irmaContextFactory = irmaContextFactory;
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

            SetIrmaDbContextFactorySettings(region);

            return new UpdateBrandEventService(
                new AddOrUpdateBrandCommandHandler(irmaContextFactory, new NLogLoggerInstance<AddOrUpdateBrandCommandHandler>(StartupOptions.Instance.ToString())),
                new AddUpdateLastChangeByIdentifiersCommandHandler(irmaContextFactory),
                new GetItemIdentifiersQueryHandler(irmaContextFactory));
        }

        public IEventService GetBrandDeleteEventService(Enums.EventNames eventName, string region, IEmailClient emailClient)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            if (eventName != Enums.EventNames.IconToIrmaBrandDelete)
            {
                return null;
            }

            SetIrmaDbContextFactorySettings(region);

            return new BrandDeleteEventService(
                new BrandDeleteCommandHandler(irmaContextFactory),
                new GetIrmaBrandQueryHandler(irmaContextFactory),
                new NLogLoggerInstance<BrandDeleteEventService>(StartupOptions.Instance.ToString()),
                emailClient,
                GlobalControllerSettings.CreateFromConfig());
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

            SetIrmaDbContextFactorySettings(region);

            switch (eventName)
            {
                case Enums.EventNames.IconToIrmaTaxClassUpdate:
                    return new UpdateTaxClassEventService(
                       new UpdateTaxClassCommandHandler(irmaContextFactory),
                       new GetTaxAbbreviationQueryHandler(iconContextFactory));
                case Enums.EventNames.IconToIrmaNewTaxClass:
                    return new AddTaxClassEventService(
                        new AddTaxClassCommandHandler(irmaContextFactory),
                        new GetTaxAbbreviationQueryHandler(iconContextFactory));
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

            SetIrmaDbContextFactorySettings(region);

            return new BulkItemEventServiceUomChangeEmailDecorator(
                new EmailUomChangeService(EmailClient.CreateFromConfigForRegion(region)),
                new GetItemsByScanCodeQueryHandler(irmaContextFactory),
                GlobalControllerSettings.CreateFromConfig(region),
                    new BulkItemEventService(
                        new NLogLoggerInstance<BulkItemEventService>(StartupOptions.Instance.ToString()),
                        new BulkAddBrandCommandHandler(irmaContextFactory),
                        new BulkAddUpdateLastChangeCommandHandler(irmaContextFactory),
                        new BulkUpdateItemCommandHandler(irmaContextFactory),
                        new BulkAddValidatedScanCodeCommandHandler(irmaContextFactory),
                        new BulkGetItemsWithTaxClassQueryHandler(irmaContextFactory),
                        new BulkUpdateNutrifactsCommandHandler(irmaContextFactory),
                        new BulkUpdateItemSignAttributesCommandHandler(irmaContextFactory),
                        new BulkGetItemsWithNoNatlClassQueryHandler(irmaContextFactory),
                        new BulkGetItemsWithNoRetailUomQueryHandler(irmaContextFactory)));
        }

        public IBulkEventService GetBulkItemNutriFactsEventService(string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            SetIrmaDbContextFactorySettings(region);

            return new BulkItemNutriFactsService(
                new BulkUpdateNutrifactsCommandHandler(irmaContextFactory),
                new BulkAddUpdateLastChangeCommandHandler(irmaContextFactory),
                new BulkGetItemsWithTaxClassQueryHandler(irmaContextFactory));
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

            SetIrmaDbContextFactorySettings(region);

            return new DeleteNationalHierarchyEventService(
                new DeleteNationalHierarchyCommandHandler(
                    irmaContextFactory, 
                    new NLogLoggerInstance<DeleteNationalHierarchyCommandHandler>(StartupOptions.Instance.ToString())));
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

            SetIrmaDbContextFactorySettings(region);

            return new AddOrUpdateNationalHierarchyEventService(
                new AddOrUpdateNationalHierarchyCommandHandler(
                    irmaContextFactory,
                    new NLogLoggerInstance<AddOrUpdateNationalHierarchyCommandHandler>(StartupOptions.Instance.ToString())),
                new GetHierarchyClassQueryHandler(iconContextFactory));
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
                    return GetBrandDeleteEventService(eventName, region, emailClient);
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

        private void SetIrmaDbContextFactorySettings(string region)
        {
            irmaContextFactory.Region = region;
            string timeoutConfiguration = ConfigurationManager.AppSettings["DbContextConnectionTimeout"];

            if (int.TryParse(timeoutConfiguration, out commandTimeout))
            {
                irmaContextFactory.CommandTimeout = commandTimeout;
            }
        }
    }
}
