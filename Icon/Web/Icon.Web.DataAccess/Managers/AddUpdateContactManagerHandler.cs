using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Data;
using System.Data.SqlClient;


namespace Icon.Web.DataAccess.Managers
{
    public class AddUpdateContactManagerHandler : IManagerHandler<AddUpdateContactManager>
    {
        private ICommandHandler<AddUpdateContactCommand> contactCommandHandler;

        public AddUpdateContactManagerHandler(ICommandHandler<AddUpdateContactCommand> contactCommandHandler)
        {
           this.contactCommandHandler = contactCommandHandler;
        }

        public void Execute(AddUpdateContactManager data)
        {
            try
            {
                contactCommandHandler.Execute(new AddUpdateContactCommand() { Contacts = data.Contacts });
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (DataException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (SqlException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException($"An error occurred in creating or updating contact.", ex);
            }
        }
    }
}
