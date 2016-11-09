using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.Common.Validators;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Managers
{
    public class AddItemManagerHandler : IManagerHandler<AddItemManager>
    {
        private IObjectValidator<AddItemManager> addItemManagerValidator;
        private ICommandHandler<BulkImportCommand<BulkImportNewItemModel>> bulkImportNewItemCommandHandler;
        private IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass> getHierarchyClassByNameQuery;
        private IQueryHandler<GetTaxClassByTaxRomanceParameters, HierarchyClass> getTaxClassByTaxRomanceQuery;


        public AddItemManagerHandler(IObjectValidator<AddItemManager> addItemManagerValidator,
            ICommandHandler<BulkImportCommand<BulkImportNewItemModel>> bulkImportNewItemCommandHandler,
            IQueryHandler<GetHierarchyClassByNameParameters, HierarchyClass> getHierarchyClassByNameQuery,
            IQueryHandler<GetTaxClassByTaxRomanceParameters, HierarchyClass> getTaxClassByTaxRomanceQuery)
        {
            this.addItemManagerValidator = addItemManagerValidator;
            this.bulkImportNewItemCommandHandler = bulkImportNewItemCommandHandler;
            this.getHierarchyClassByNameQuery = getHierarchyClassByNameQuery;
            this.getTaxClassByTaxRomanceQuery = getTaxClassByTaxRomanceQuery;
        }

        public void Execute(AddItemManager data)
        {
            SetMerchandiseAndTaxId(data);

            ObjectValidationResult validationResult = addItemManagerValidator.Validate(data);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(String.Format("Cannot add item. Error details: {0}", validationResult.Error));
            }

            try
            {
                BulkImportCommand<BulkImportNewItemModel> bulkImportCommand = new BulkImportCommand<BulkImportNewItemModel>
                {
                    UserName = data.UserName,
                    UpdateAllItemFields = true,
                    BulkImportData = new List<BulkImportNewItemModel>
                    {
                        data.Item   
                    }
                };

                bulkImportNewItemCommandHandler.Execute(bulkImportCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException(String.Format("There was an error adding Scan Code {0}: {1}", data.Item.ScanCode, ex.Message), ex);
            }
        }

        private void SetMerchandiseAndTaxId(AddItemManager data)
        {
            if (String.IsNullOrWhiteSpace(data.Item.MerchandiseId) && !String.IsNullOrWhiteSpace(data.Item.MerchandiseLineage))
            {
                var merchandiseHierarchyClass = getHierarchyClassByNameQuery.Search(new GetHierarchyClassByNameParameters
                {
                    HierarchyClassName = data.Item.MerchandiseLineage,
                    HierarchyName = HierarchyNames.Merchandise,
                    HierarchyLevel = HierarchyLevels.SubBrick
                });

                if (merchandiseHierarchyClass != null)
                {
                    data.Item.MerchandiseId = merchandiseHierarchyClass.hierarchyClassID.ToString();
                }
                else
                {
                    throw new ArgumentException((String.Format("Merchandise Hierarchy Class {0} does not exist.", data.Item.MerchandiseLineage)));
                }
            }

            if (String.IsNullOrWhiteSpace(data.Item.TaxId) && !String.IsNullOrWhiteSpace(data.Item.TaxLineage))
            {
                var tax = getTaxClassByTaxRomanceQuery.Search(new GetTaxClassByTaxRomanceParameters
                {
                    TaxRomance = data.Item.TaxLineage,
                });

                if (tax != null)
                {
                    data.Item.TaxId = tax.hierarchyClassID.ToString();
                }
                else
                {
                    throw new ArgumentException((String.Format("Tax Hierarchy Class {0} does not exist.", data.Item.TaxLineage)));
                }
            }
        }
    }
}
