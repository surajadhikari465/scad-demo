using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using System;

namespace Icon.Web.DataAccess.Managers
{
    public class AddMerchTaxAssociationManagerHandler : IManagerHandler<AddMerchTaxAssociationManager>
    {
        private ICommandHandler<AddMerchTaxMappingCommand> addMerchTaxMappingHandler;
        private ICommandHandler<ApplyMerchTaxAssociationToItemsCommand> applyMerchTaxAssociationToItemsCommandHandler;

        public AddMerchTaxAssociationManagerHandler(
            ICommandHandler<AddMerchTaxMappingCommand> addMerchTaxMappingHandler,
            ICommandHandler<ApplyMerchTaxAssociationToItemsCommand> applyMerchTaxAssociationToItemsCommandHandler)
        {
            this.addMerchTaxMappingHandler = addMerchTaxMappingHandler;
            this.applyMerchTaxAssociationToItemsCommandHandler = applyMerchTaxAssociationToItemsCommandHandler;
        }

        public void Execute(AddMerchTaxAssociationManager data)
        {
            ApplyMerchTaxAssociationToItemsCommand applyMerchTaxAssociationToItemsCommand = Mapper.Map<ApplyMerchTaxAssociationToItemsCommand>(data);

            try
            {
                applyMerchTaxAssociationToItemsCommandHandler.Execute(applyMerchTaxAssociationToItemsCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while associating items to the new tax class.", ex);
            }

            AddMerchTaxMappingCommand addMerchTaxMappingCommand = Mapper.Map<AddMerchTaxMappingCommand>(data);

            try
            {
                addMerchTaxMappingHandler.Execute(addMerchTaxMappingCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while adding the merchandise/tax mapping.", ex);
            }
        }
    }
}
