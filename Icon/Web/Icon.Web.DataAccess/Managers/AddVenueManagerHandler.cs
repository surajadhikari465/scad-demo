using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Icon.Web.DataAccess.Managers
{
    public class AddVenueManagerHandler : IManagerHandler<AddVenueManager>
    {
        private readonly IconContext context;
        private ICommandHandler<AddVenueCommand> addVenueHandler;
        private ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler;
        private IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery;

        public AddVenueManagerHandler(
            IconContext context,
            ICommandHandler<AddVenueCommand> addVenueHandler,
            ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler,
            IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery)
        {
            this.context = context;
            this.addVenueHandler = addVenueHandler;
            this.addLocaleMessageHandler = addLocaleMessageHandler;
            this.getLocaleQuery = getLocaleQuery;
        }

        public void Execute(AddVenueManager data)
        {
            AddVenueCommand addVenueCommand = Mapper.Map<AddVenueCommand>(data);
            Boolean enableLocaleEventGenerationForVenue;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    addVenueHandler.Execute(addVenueCommand);

                    Locale locale = getLocaleQuery.Search(new GetLocaleParameters { LocaleId = addVenueCommand.LocaleId }).First();

                    if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableLocaleEventGenerationForVenue"].ToString(), out enableLocaleEventGenerationForVenue))
                    {
                        enableLocaleEventGenerationForVenue = false;
                    }

                    if (enableLocaleEventGenerationForVenue)
                    {
                        addLocaleMessageHandler.Execute(new AddLocaleMessageCommand { Locale = locale });
                    }
                  
                    transaction.Commit();
                }
                catch (ArgumentException exception)
                {
                    transaction.Rollback();
                    throw new CommandException(exception.Message, exception);
                }

                catch (Exception exception)
                {
                    transaction.Rollback();
                    throw new CommandException(String.Format("Error updating Locale {0}: {1}", data.LocaleName, exception.Message), exception);
                }
            }
        }
    }
}