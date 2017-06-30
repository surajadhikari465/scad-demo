using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Logging;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;

namespace GlobalEventController.Controller.EventServices
{
    public class BulkItemEventService : EventServiceBase, IBulkEventService
    {
        private ILogger<BulkItemEventService> logger;
        private ICommandHandler<BulkAddBrandCommand> bulkAddBrandCommandHandler;
        private ICommandHandler<BulkAddUpdateLastChangeCommand> bulkAddUpdateLastChangeCommandHandler;
        private ICommandHandler<BulkUpdateItemCommand> bulkUpdateItemCommandHandler;
        private ICommandHandler<BulkAddValidatedScanCodeCommand> bulkScanCodeCommandHandler;
        private IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithTaxQueryHandler;
        private ICommandHandler<BulkUpdateNutriFactsCommand> bulkUpdateNutriFactsCommandHandler;
        private ICommandHandler<BulkUpdateItemSignAttributesCommand> bulkUpdateItemSignAttributesCommandHandler;
        private IQueryHandler<BulkGetItemsWithNoNatlClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithNoNatlClassQueryHandler;
        private IQueryHandler<BulkGetItemsWithNoRetailUomQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithNoRetailUomQueryHandler;

        public List<ValidatedItemModel> ValidatedItemList { get; set; }
        public List<string> ScanCodesWithNoTaxList { get; set; }
        public List<NutriFactsModel> ItemNutriFacts { get; set; }

        public BulkItemEventService(
            ILogger<BulkItemEventService> logger,
            ICommandHandler<BulkAddBrandCommand> bulkBrandHandler,
            ICommandHandler<BulkAddUpdateLastChangeCommand> bulkLastChangeHandler,
            ICommandHandler<BulkUpdateItemCommand> bulkItemHandler,
            ICommandHandler<BulkAddValidatedScanCodeCommand> bulkScanCodeHandler,
            IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithTax,
            ICommandHandler<BulkUpdateNutriFactsCommand> bulkUpdateNutriFactsHandler,
            ICommandHandler<BulkUpdateItemSignAttributesCommand> bulkUpdateItemSignAttributesCommandHandler,
            IQueryHandler<BulkGetItemsWithNoNatlClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithNoNatlClassQueryHandler,
            IQueryHandler<BulkGetItemsWithNoRetailUomQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithNoRetailUomQueryHandler)
        {
            this.logger = logger;
            this.bulkAddBrandCommandHandler = bulkBrandHandler;
            this.bulkAddUpdateLastChangeCommandHandler = bulkLastChangeHandler;
            this.bulkUpdateItemCommandHandler = bulkItemHandler;
            this.bulkScanCodeCommandHandler = bulkScanCodeHandler;
            this.bulkGetValidatedItemsWithTaxQueryHandler = bulkGetValidatedItemsWithTax;
            this.bulkUpdateNutriFactsCommandHandler = bulkUpdateNutriFactsHandler;
            this.bulkUpdateItemSignAttributesCommandHandler = bulkUpdateItemSignAttributesCommandHandler;
            this.bulkGetValidatedItemsWithNoNatlClassQueryHandler = bulkGetValidatedItemsWithNoNatlClassQueryHandler;
            this.bulkGetValidatedItemsWithNoRetailUomQueryHandler = bulkGetValidatedItemsWithNoRetailUomQueryHandler;
        }

        public override void Run()
        {
            RegionalItemMessage = new List<RegionalItemMessageModel>();
            this.ValidatedItemList = this.ValidatedItemList.DistinctBy(v => v.ScanCode).ToList();

            List<ValidatedItemModel> ValidatedItemWithTaxList = bulkGetValidatedItemsWithTaxQueryHandler.Handle(new BulkGetItemsWithTaxClassQuery() { ValidatedItems = ValidatedItemList });
            ScanCodesWithNoTaxList = ValidatedItemList.Where(vi => !ValidatedItemWithTaxList.Any(pv => pv.ScanCode == vi.ScanCode)).Select(vi => vi.ScanCode).ToList();
            var itemWithNoTaxMatchedList = ValidatedItemList
                .Where(vi => !ValidatedItemWithTaxList
                .Any(pv => pv.ScanCode == vi.ScanCode))
                .Select(vi => new RegionalItemMessageModel
                {
                    RegionCode = this.Region,
                    Identifier = vi.ScanCode,
                    Message = "Tax Class doesn’t exist with tax name - " + vi.TaxClassName
                })
                .ToList();

            var itemWithNoNatlClassMatchedList = bulkGetValidatedItemsWithNoNatlClassQueryHandler
                .Handle(new BulkGetItemsWithNoNatlClassQuery() { ValidatedItems = ValidatedItemList })
                    .Select(vi => new RegionalItemMessageModel
                    {
                        RegionCode = this.Region,
                        Identifier = vi.ScanCode,
                        Message = "National Class doesn't exist with ClassId - " + vi.NationalClassCode
                    }).ToList();

            var itemWithNoRetailUomMatchedList = bulkGetValidatedItemsWithNoRetailUomQueryHandler
                .Handle(new BulkGetItemsWithNoRetailUomQuery() { ValidatedItems = ValidatedItemList })
                    .Select(vi => new RegionalItemMessageModel
                    {
                        RegionCode = this.Region,
                        Identifier = vi.ScanCode,
                        Message = "Retail UOM doesn't exist with UOM abbreviation - " + vi.RetailUom
                    }).ToList();

            RegionalItemMessage.AddRange(itemWithNoTaxMatchedList);
            RegionalItemMessage.AddRange(itemWithNoNatlClassMatchedList);
            RegionalItemMessage.AddRange(itemWithNoRetailUomMatchedList);

            // Run Commands
            BulkAddBrandCommand bulkBrandCommand = new BulkAddBrandCommand { ValidatedItems = ValidatedItemList };
            bulkAddBrandCommandHandler.Handle(bulkBrandCommand);

            List<IconItemLastChangeModel> iconItemLastChangedItems = ValidatedItemList.Select(vi => new IconItemLastChangeModel(vi)
            {
                AreNutriFactsUpdated = ItemNutriFacts != null && ItemNutriFacts.Count > 0 && ItemNutriFacts.Any(ni => ni.Plu == vi.ScanCode)
            }).ToList();

            BulkUpdateItemCommand bulkItemCommand = new BulkUpdateItemCommand { ValidatedItems = ValidatedItemList };
            bulkUpdateItemCommandHandler.Handle(bulkItemCommand);

            BulkAddValidatedScanCodeCommand bulkScanCodeCommand = new BulkAddValidatedScanCodeCommand { ValidatedItems = ValidatedItemList };
            bulkScanCodeCommandHandler.Handle(bulkScanCodeCommand);

            BulkAddUpdateLastChangeCommand bulkLastChangeCommand = new BulkAddUpdateLastChangeCommand { IconLastChangedItems = iconItemLastChangedItems };
            bulkAddUpdateLastChangeCommandHandler.Handle(bulkLastChangeCommand);

            bulkUpdateItemSignAttributesCommandHandler.Handle(new BulkUpdateItemSignAttributesCommand { ValidatedItems = ValidatedItemList });

            if (ItemNutriFacts != null && ItemNutriFacts.Count > 0)
            {
                bulkUpdateNutriFactsCommandHandler.Handle(new BulkUpdateNutriFactsCommand() { ItemNutriFacts = ItemNutriFacts });
            }
        }
    }
}
