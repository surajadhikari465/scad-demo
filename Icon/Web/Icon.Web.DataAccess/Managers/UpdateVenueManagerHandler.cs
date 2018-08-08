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
    public class UpdateVenueManagerHandler : IManagerHandler<UpdateVenueManager>
    {
        private readonly IconContext context;
        private ICommandHandler<UpdateVenueCommand> updateVenueHandler;
        private ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler;
        private IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery;

        public UpdateVenueManagerHandler(
            IconContext context,
            ICommandHandler<UpdateVenueCommand> updateVenueHandler,
            ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler,
            IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery)
        {
            this.context = context;
            this.updateVenueHandler = updateVenueHandler;
            this.addLocaleMessageHandler = addLocaleMessageHandler;
            this.getLocaleQuery = getLocaleQuery;
        }

        public void Execute(UpdateVenueManager data)
        {
            UpdateVenueCommand updateVenueCommand = Mapper.Map<UpdateVenueCommand>(data);
            Boolean enableLocaleEventGenerationForVenue;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    updateVenueHandler.Execute(updateVenueCommand);

                    Locale locale = getLocaleQuery.Search(new GetLocaleParameters { LocaleId = data.LocaleId }).First();

                    if (!Boolean.TryParse(ConfigurationManager.AppSettings["EnableLocaleEventGenerationForVenue"].ToString(), out enableLocaleEventGenerationForVenue))
                    {
                        enableLocaleEventGenerationForVenue = false;
                    }

                    if(enableLocaleEventGenerationForVenue)
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