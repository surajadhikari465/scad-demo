using Icon.Common.DataAccess;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Managers
{
    public class ValidateItemManagerHandler : IManagerHandler<ValidateItemManager>
    {
        private ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportItemCommandHandler;

        public ValidateItemManagerHandler(ICommandHandler<BulkImportCommand<BulkImportItemModel>> bulkImportItemCommandHandler)
        {
            this.bulkImportItemCommandHandler = bulkImportItemCommandHandler;
        }

        public void Execute(ValidateItemManager data)
        {
            try
            {
                var item = CreateImportModelForItemValidation(data);

                BulkImportCommand<BulkImportItemModel> bulkImportCommand = new BulkImportCommand<BulkImportItemModel>
                {
                    UserName = data.UserName,
                    UpdateAllItemFields = false,
                    BulkImportData = new List<BulkImportItemModel>
                    {
                        item
                    }
                };

                bulkImportItemCommandHandler.Execute(bulkImportCommand);
            }
            catch (InvalidOperationException ex)
            {
                throw new CommandException(ex.Message, ex);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("There was an error validating scan code {0}. Error: {1}", data.ScanCode, ex.Message), ex);
            }
        }

        /// <summary>
        /// Except for ScanCode and IsValidated, sets all properties to empty string so that the import stored procedure will ignore updating 
        /// and possibly deleting those properties.
        /// </summary>
        /// <param name="data">The data to assign to the BulkImportItemModel.</param>
        /// <returns>A BulkImportItemModel with the ScanCode and IsValidated set.</returns>
        private static BulkImportItemModel CreateImportModelForItemValidation(ValidateItemManager data)
        {
            var item = new BulkImportItemModel();
            foreach (var property in typeof(BulkImportItemModel).GetProperties().Where(p => p.PropertyType == typeof(string)))
            {
                property.SetValue(item, string.Empty);
            }
            item.ScanCode = data.ScanCode;
            item.IsValidated = "1";

            return item;
        }
    }
}
