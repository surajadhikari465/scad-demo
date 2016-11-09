using Icon.Common.DataAccess;
using Infor.Services.NewItem.Models;
using Infor.Services.NewItem.Queries;
using System.Collections.Generic;
using System.Linq;

namespace Infor.Services.NewItem.Validators
{
    public class PluCollectionValidator : ICollectionValidator<NewItemModel>
    {
        private IQueryHandler<GetItemIdsQuery, Dictionary<string, int>> getItemIdsQueryHandler;

        public PluCollectionValidator(IQueryHandler<GetItemIdsQuery, Dictionary<string, int>> getItemIdsQueryHandler)
        {
            this.getItemIdsQueryHandler = getItemIdsQueryHandler;
        }

        public CollectionValidatorResult<NewItemModel> Validate(IEnumerable<NewItemModel> collection)
        {
            CollectionValidatorResult<NewItemModel> result = new CollectionValidatorResult<NewItemModel>()
            {
                ValidEntities = collection
            };

            var newPluItems = collection
                .Where(i => IsPluItem(i))
                .ToList();

            if (newPluItems.Any())
            {
                var itemsExistingInIcon = getItemIdsQueryHandler.Search(new GetItemIdsQuery
                {
                    ScanCodes = newPluItems.Select(ii => ii.ScanCode).ToList()
                });

                result.InvalidEntities = newPluItems.Where(i => !itemsExistingInIcon.ContainsKey(i.ScanCode)).ToList();
                result.ValidEntities = result.ValidEntities.Except(result.InvalidEntities);

                if (result.InvalidEntities.Any())
                {
                    result.Error = "Unable to send all queued items to Infor because they are PLUs that don't exist in Icon. Events will be removed from queue.";
                }
            }

            return result;
        }

        private bool IsPluItem(NewItemModel item)
        {
            if (item.IsRetailSale)
            {
                if (item.ScanCode.Length < 7)
                {
                    return true;
                }
                else if(item.ScanCode.Length == 11 && item.ScanCode.StartsWith("2") && item.ScanCode.EndsWith("00000"))
                {
                    return true;
                }
            }
            else if (item.ScanCode.Length == 11 && (item.ScanCode.StartsWith("46") || item.ScanCode.StartsWith("48")))
            {
                return true;
            }

            return false;
        }
    }
}
