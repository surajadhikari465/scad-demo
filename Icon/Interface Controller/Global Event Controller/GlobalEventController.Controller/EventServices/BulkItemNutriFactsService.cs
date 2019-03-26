using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System;
using Icon.Framework;

namespace GlobalEventController.Controller.EventServices
{
	public class BulkItemNutriFactsService : EventServiceBase, IBulkEventService
	{
		private ICommandHandler<BulkUpdateNutriFactsCommand> bulkUpdateNutriFactsHandler;
		private ICommandHandler<BulkAddUpdateLastChangeCommand> bulkLastChangeHandler;
		private ICommandHandler<BulkDeleteNutriFactsCommand> bulkDeleteHandler;
		private IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithTax;

		public List<ValidatedItemModel> ValidatedItemList { get; set; }
		public List<string> ScanCodesWithNoTaxList { get; set; }
		public List<NutriFactsModel> ItemNutriFacts { get; set; }

		public BulkItemNutriFactsService(
				ICommandHandler<BulkUpdateNutriFactsCommand> bulkUpdateNutriFactsHandler,
				ICommandHandler<BulkAddUpdateLastChangeCommand> bulkLastChangeHandler,
				ICommandHandler<BulkDeleteNutriFactsCommand> bulkDeleteHandler,
				IQueryHandler<BulkGetItemsWithTaxClassQuery, List<ValidatedItemModel>> bulkGetValidatedItemsWithTax)
		{
			this.bulkLastChangeHandler = bulkLastChangeHandler;
			this.bulkGetValidatedItemsWithTax = bulkGetValidatedItemsWithTax;
			this.bulkUpdateNutriFactsHandler = bulkUpdateNutriFactsHandler;
			this.bulkDeleteHandler = bulkDeleteHandler;
		}

		public override void Run()
		{
			if(this.ValidatedItemList == null || this.ValidatedItemList.Count == 0)
			{
				return;
			}

			this.ValidatedItemList = this.ValidatedItemList.DistinctBy(v => v.ScanCode).ToList();

			if(this.ValidatedItemList.Any(x => x.EventTypeId == EventTypes.NutritionDelete))
			{
				this.bulkDeleteHandler.Handle(new BulkDeleteNutriFactsCommand { ScanCodes = this.ValidatedItemList.Where(x => x.EventTypeId == EventTypes.NutritionDelete).Select(x => x.ScanCode).Distinct(StringComparer.InvariantCultureIgnoreCase).ToList() });
			}

			if(this.ValidatedItemList.Any(x => x.EventTypeId != EventTypes.NutritionDelete))
			{
				List<ValidatedItemModel> ValidatedItemWithTaxList = bulkGetValidatedItemsWithTax.Handle(new BulkGetItemsWithTaxClassQuery() { ValidatedItems = this.ValidatedItemList.Where(x => x.EventTypeId != EventTypes.NutritionDelete).ToList() });

				List<IconItemLastChangeModel> iconItemLastChangedItems = ValidatedItemWithTaxList.Select(vi => new IconItemLastChangeModel(vi)
				{
					AreNutriFactsUpdated = ItemNutriFacts != null && ItemNutriFacts.Count > 0 && ItemNutriFacts.Any(ni => ni.Plu == vi.ScanCode)
				}).ToList();

				bulkUpdateNutriFactsHandler.Handle(new BulkUpdateNutriFactsCommand() { ItemNutriFacts = ItemNutriFacts });
				bulkLastChangeHandler.Handle(new BulkAddUpdateLastChangeCommand { IconLastChangedItems = iconItemLastChangedItems });
			}
		}
	}
}