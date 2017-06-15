﻿using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using Irma.Framework;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;


namespace GlobalEventController.Controller.EventServices
{
    public class BulkItemNutriFactsService : EventServiceBase, IBulkEventService
    {
        private ICommandHandler<BulkUpdateNutriFactsCommand> bulkUpdateNutriFactsHandler;
        private ICommandHandler<BulkAddUpdateLastChangeCommand> bulkLastChangeHandler;
        private IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithTax;

        public List<ValidatedItemModel> ValidatedItemList { get; set; }
        public List<string> ScanCodesWithNoTaxList { get; set; }
        public List<NutriFactsModel> ItemNutriFacts { get; set; }

        public BulkItemNutriFactsService(IrmaContext context,
            ICommandHandler<BulkUpdateNutriFactsCommand> bulkUpdateNutriFactsHandler,
            ICommandHandler<BulkAddUpdateLastChangeCommand> bulkLastChangeHandler,
            IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithTax)
            : base (context)
        {
            this.bulkLastChangeHandler = bulkLastChangeHandler;
            this.bulkGetValidatedItemsWithTax = bulkGetValidatedItemsWithTax;
            this.bulkUpdateNutriFactsHandler = bulkUpdateNutriFactsHandler;
        }

        public override void Run()
        {
            this.ValidatedItemList = this.ValidatedItemList.DistinctBy(v => v.ScanCode).ToList();
            List<ValidatedItemModel> ValidatedItemWithTaxList = bulkGetValidatedItemsWithTax.Handle(new BulkGetItemsWithTaxClassQuery() { ValidatedItems = this.ValidatedItemList });

            List<IconItemLastChangeModel> iconItemLastChangedItems = ValidatedItemWithTaxList.Select(vi => new IconItemLastChangeModel(vi)
            {
                AreNutriFactsUpdated = ItemNutriFacts != null && ItemNutriFacts.Count > 0 && ItemNutriFacts.Any(ni => ni.Plu == vi.ScanCode)
            }).ToList();

            bulkUpdateNutriFactsHandler.Handle(new BulkUpdateNutriFactsCommand() { ItemNutriFacts = ItemNutriFacts });

            BulkAddUpdateLastChangeCommand bulkLastChangeCommand = new BulkAddUpdateLastChangeCommand { IconLastChangedItems = iconItemLastChangedItems };
            bulkLastChangeHandler.Handle(bulkLastChangeCommand);
        }
    }
}
