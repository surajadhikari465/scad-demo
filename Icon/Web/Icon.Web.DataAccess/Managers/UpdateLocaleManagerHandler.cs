using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateLocaleManagerHandler : IManagerHandler<UpdateLocaleManager>
    {
        private readonly IconContext context;
        private ICommandHandler<UpdateLocaleCommand> updateLocaleHandler;
        private ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler;
        private ICommandHandler<AddVimEventCommand> addVimLocaleEventCommandHandler;
        private IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery;

        public UpdateLocaleManagerHandler(
            IconContext context,
            ICommandHandler<UpdateLocaleCommand> updateLocaleHandler,
            ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler,
            ICommandHandler<AddVimEventCommand> addVimLocaleEventCommandHandler,
            IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery)
        {
            this.context = context;
            this.updateLocaleHandler = updateLocaleHandler;
            this.addLocaleMessageHandler = addLocaleMessageHandler;
            this.addVimLocaleEventCommandHandler = addVimLocaleEventCommandHandler;
            this.getLocaleQuery = getLocaleQuery;
        }

        public void Execute(UpdateLocaleManager data)
        {
            UpdateLocaleCommand updateLocaleCommand = Mapper.Map<UpdateLocaleCommand>(data);

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    updateLocaleHandler.Execute(updateLocaleCommand);

                    Locale locale = getLocaleQuery.Search(new GetLocaleParameters { LocaleId = data.LocaleId }).First();

                    addLocaleMessageHandler.Execute(new AddLocaleMessageCommand { Locale = locale });
                    addVimLocaleEventCommandHandler.Execute(new AddVimEventCommand { EventReferenceId = locale.localeID, EventTypeId = VimEventTypes.LocaleUpdate });

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