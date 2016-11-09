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
    public class AddLocaleManagerHandler : IManagerHandler<AddLocaleManager>
    {
        private IconContext context;
        private ICommandHandler<AddLocaleCommand> addLocaleHandler;
        private ICommandHandler<AddAddressCommand> addAddressHandler;
        private ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler;
        private ICommandHandler<AddVimEventCommand> addVimLocaleEventCommandHandler;
        private IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery;

        public AddLocaleManagerHandler(
            IconContext context,
            ICommandHandler<AddLocaleCommand> addLocaleHandler,
            ICommandHandler<AddAddressCommand> addAddressHandler,
            ICommandHandler<AddLocaleMessageCommand> addLocaleMessageHandler,
            ICommandHandler<AddVimEventCommand> addVimLocaleEventCommandHandler,
            IQueryHandler<GetLocaleParameters, List<Locale>> getLocaleQuery)
        {
            this.context = context;
            this.addLocaleHandler = addLocaleHandler;
            this.addAddressHandler = addAddressHandler;
            this.addLocaleMessageHandler = addLocaleMessageHandler;
            this.addVimLocaleEventCommandHandler = addVimLocaleEventCommandHandler;
            this.getLocaleQuery = getLocaleQuery;
        }

        public void Execute(AddLocaleManager data)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    AddLocaleCommand addLocaleCommand = Mapper.Map<AddLocaleCommand>(data);
                    addLocaleHandler.Execute(addLocaleCommand);

                    AddAddressCommand addAddressCommand = Mapper.Map<AddAddressCommand>(data);
                    addAddressCommand.LocaleId = addLocaleCommand.LocaleId;
                    addAddressHandler.Execute(addAddressCommand);

                    Locale locale = getLocaleQuery.Search(new GetLocaleParameters
                    {
                        LocaleId = addLocaleCommand.LocaleId
                    })
                        .Single();

                    addLocaleMessageHandler.Execute(new AddLocaleMessageCommand { Locale = locale });
                    addVimLocaleEventCommandHandler.Execute(new AddVimEventCommand { EventReferenceId = locale.localeID, EventTypeId = VimEventTypes.LocaleAdd });

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
                    throw new CommandException(String.Format("Error adding Locale {0}:  {1}", data.LocaleName, exception.Message), exception);
                }
            }
        }
    }
}