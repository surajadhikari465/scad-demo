using SubteamEventController.DataAccess.BulkCommands;
using SubteamEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Commands;
using SubteamEventController.DataAccess.DataServices;
using GlobalEventController.DataAccess.Queries;
using SubteamEventController.DataAccess.Queries;
using GlobalEventController.Controller.EventServices;
using Icon.Framework;
using Icon.Logging;
using InterfaceController.Common;
using Irma.Framework;
using System;
using System.Configuration;

namespace SubteamEventController.Controller.EventServices
{
    public class ServiceProvider : IEventServiceProvider
    {
        private IrmaContext irmaContext;
        private IconContext iconContext;
        private int commandTimeout;

        public IEventService GetItemSubTeamEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            this.iconContext = new IconContext();
            this.irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(region));
            SetIrmaDbContextConnectionTimeout();

            if (eventName != Enums.EventNames.IconToIrmaItemSubTeamUpdates)
            {
                return null;
            }

            return new ItemSubTeamEventService(
                this.irmaContext,
                new GetScanCodeQueryHandler(this.iconContext),
                new GetItemIdentifiersQueryHandler(this.irmaContext),
                new UpdateItemSubTeamDataServiceHandler(
                    new AddUpdateItemSubTeamLastChangeCommandHandler(this.irmaContext),
                    new UpdateItemSubTeamCommandHandler(this.irmaContext),
                    new GetUserQueryHandler(this.irmaContext)),
                    new GetSubTeamHierarchyQueryHandlercs(this.iconContext),
                    new AddItemCategoryCommandHandler(this.irmaContext),
                    new GetUserQueryHandler(this.irmaContext));
        }


        public IEventService GetSubTeamEventService(Enums.EventNames eventName, string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            this.iconContext = new IconContext();
            this.irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(region));

            if (eventName != Enums.EventNames.IconToIrmaSubTeamUpdate)
            {
                return null;
            }

            return new SubTeamEventService(
                irmaContext,
                new GetSubTeamQuery(this.iconContext),
                new UpdateSubTeamCommandHandler(this.irmaContext));
        }


        public IBulkItemSubTeamEventService GetBulkItemSubTeamEventService(string region)
        {
            if (String.IsNullOrEmpty(region))
            {
                throw new ArgumentException("No region name specified to build database connection.");
            }

            this.iconContext = new IconContext();
            this.irmaContext = new IrmaContext(ConnectionBuilder.GetConnection(region));
            SetIrmaDbContextConnectionTimeout();

            return new BulkItemSubTeamEventService(this.irmaContext,
                new BulkUpdateItemSubTeamCommandHandler(this.irmaContext),
                new BulkAddUpdateLastChangeSubTeamCommandHandler(this.irmaContext));
        }

        public IEventService GetEventService(Enums.EventNames eventName, string region)
        {
            switch (eventName)
            {
                case Enums.EventNames.IconToIrmaItemSubTeamUpdates:
                    return GetItemSubTeamEventService(eventName, region);
                case Enums.EventNames.IconToIrmaSubTeamUpdate:
                    return GetSubTeamEventService(eventName, region);

                default:
                    return null;
            }
        }


        public IBulkEventService GetBulkItemEventService(string region)
        {
            throw new NotImplementedException();
        }
        public IBulkEventService GetBulkItemNutriFactsEventService(string region)
        {
            throw new NotImplementedException();
        }
        public IEventService GetItemEventService(InterfaceController.Common.Enums.EventNames eventName, string region)
        {
            throw new NotImplementedException();
        }

        public IEventService GetBrandEventService(InterfaceController.Common.Enums.EventNames eventName, string region)
        {
            throw new NotImplementedException();
        }

        public IEventService GetTaxEventService(InterfaceController.Common.Enums.EventNames eventName, string region)
        {
            throw new NotImplementedException();
        }

        private void SetIrmaDbContextConnectionTimeout()
        {
            string timeoutConfiguration = ConfigurationManager.AppSettings["DbContextConnectionTimeout"];

            if (Int32.TryParse(timeoutConfiguration, out this.commandTimeout))
            {
                this.irmaContext.Database.CommandTimeout = this.commandTimeout;
            }
        }
    }
}
