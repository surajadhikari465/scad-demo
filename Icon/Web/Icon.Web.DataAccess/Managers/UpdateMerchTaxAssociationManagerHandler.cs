using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateMerchTaxAssociationManagerHandler : IManagerHandler<UpdateMerchTaxAssociationManager>
    {
        private ICommandHandler<UpdateMerchTaxMappingCommand> updateMerchTaxMappingHandler;
        private ICommandHandler<ApplyMerchTaxAssociationToItemsCommand> applyMerchTaxAssociationToItemsCommandHandler;

        public UpdateMerchTaxAssociationManagerHandler(
            ICommandHandler<UpdateMerchTaxMappingCommand> updateMerchTaxMappingHandler,
            ICommandHandler<ApplyMerchTaxAssociationToItemsCommand> applyMerchTaxAssociationToItemsCommandHandler)
        {
            this.updateMerchTaxMappingHandler = updateMerchTaxMappingHandler;
            this.applyMerchTaxAssociationToItemsCommandHandler = applyMerchTaxAssociationToItemsCommandHandler;
        }

        public void Execute(UpdateMerchTaxAssociationManager data)
        {
            ApplyMerchTaxAssociationToItemsCommand applyMerchTaxAssociationToItemsCommand = Mapper.Map<ApplyMerchTaxAssociationToItemsCommand>(data);

            try
            {
                applyMerchTaxAssociationToItemsCommandHandler.Execute(applyMerchTaxAssociationToItemsCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while updating items to the new tax class.", ex);
            }

            UpdateMerchTaxMappingCommand updateMerchTaxMappingCommand = Mapper.Map<UpdateMerchTaxMappingCommand>(data);

            try
            {
                updateMerchTaxMappingHandler.Execute(updateMerchTaxMappingCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while updating the merchandise/tax mapping.", ex);
            }
        }
    }
}
