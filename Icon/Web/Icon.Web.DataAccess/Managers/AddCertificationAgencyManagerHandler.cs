using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class AddCertificationAgencyManagerHandler : IManagerHandler<AddCertificationAgencyManager>
    {
        private IconContext context;
        private ICommandHandler<AddCertificationAgencyCommand> AddCertificationAgencyCommandHandler;

        public AddCertificationAgencyManagerHandler(
            IconContext context,
            ICommandHandler<AddCertificationAgencyCommand> AddCertificationAgencyCommandHandler)
        {
            this.context = context;
            this.AddCertificationAgencyCommandHandler = AddCertificationAgencyCommandHandler;
        }

        public void Execute(AddCertificationAgencyManager data)
        {            
            AddCertificationAgencyCommand addCertificationAgencyCommand = Mapper.Map<AddCertificationAgencyCommand>(data);
            
            try
            {
                AddCertificationAgencyCommandHandler.Execute(addCertificationAgencyCommand);
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred when adding Certification Agency {0}.", data.Agency.hierarchyClassName), ex);
            }
        }
    }
}
