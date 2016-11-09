using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateCertificationAgencyManagerHandler : IManagerHandler<UpdateCertificationAgencyManager>
    {
        private IconContext context;
        private ICommandHandler<UpdateCertificationAgencyCommand> updateCertificationAgencyCommandHandler;

        public UpdateCertificationAgencyManagerHandler(
            IconContext context,
            ICommandHandler<UpdateCertificationAgencyCommand> updateCertificationAgencyCommandHandler)
        {
            this.context = context;
            this.updateCertificationAgencyCommandHandler = updateCertificationAgencyCommandHandler;
        }

        public void Execute(UpdateCertificationAgencyManager data)
        {
            UpdateCertificationAgencyCommand updateAgencyCommand = Mapper.Map<UpdateCertificationAgencyCommand>(data);

            try
            {
                updateCertificationAgencyCommandHandler.Execute(updateAgencyCommand);
            }
            catch (DuplicateValueException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("An error occurred when updating Agency ID {0}. {1}", data.Agency.hierarchyClassID, ex.Message), ex);
            }
        }
    }
}
