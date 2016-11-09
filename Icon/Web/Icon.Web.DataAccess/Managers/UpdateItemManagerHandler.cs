using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Utility;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;

namespace Icon.Web.DataAccess.Managers
{
    public class UpdateItemManagerHandler : IManagerHandler<UpdateItemManager>
    {
        private IObjectValidator<UpdateItemManager> updateItemManagerValidator;
        private ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportItemCommandHandler;
        private IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass> getHierarchyClassByNameQuery;

        public UpdateItemManagerHandler(
            IObjectValidator<UpdateItemManager> updateItemManagerValidator,
            ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportItemCommandHandler,
            IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass> getHierarchyClassByNameQuery)
        {
            this.updateItemManagerValidator = updateItemManagerValidator;
            this.bulkImportItemCommandHandler = bulkImportItemCommandHandler;
            this.getHierarchyClassByNameQuery = getHierarchyClassByNameQuery;
        }

        public void Execute(UpdateItemManager data)
        {
            ObjectValidationResult validationResult = updateItemManagerValidator.Validate(data);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(String.Format("Cannot update item.  Error details: {0}", validationResult.Error));
            }

            try
            {
                BulkImportCommand<BulkImportItemModel> bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
                {
                    UserName = data.UserName,
                    UpdateAllItemFields = true,
                    BulkImportData = new List<BulkImportItemModel>
                    {
                        data.Item
                    }
                };

                bulkImportItemCommandHandler.Execute(bulkImportCommand);
            }
            catch (DbEntityValidationException exception)
            {
                throw new CommandException(String.Format("There was a database validation error when updating Scan Code {0}.", data.Item.ScanCode), exception);
            }
            catch (InvalidOperationException exception)
            {
                throw new CommandException(exception.Message, exception);
            }
            catch (Exception exception)
            {
                throw new CommandException(String.Format("There was an error while updating Scan Code {0}: {1}", data.Item.ScanCode, exception.Message), exception);
            }
        }
    }
}
