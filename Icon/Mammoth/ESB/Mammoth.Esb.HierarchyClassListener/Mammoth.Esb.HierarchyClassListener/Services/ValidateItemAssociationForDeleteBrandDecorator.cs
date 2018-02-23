using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Schemas.Wfm.Contracts;using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mammoth.Esb.HierarchyClassListener.Services
{
    public class ValidateItemAssociationForDeleteBrandDecorator : DeleteHierarchyClassValidationDecoratorBase
    {
        private const int brandHierarchyId = Hierarchies.Brands;
        private const string brandHierarchyName = "Brand";
        private const string brandHierarchyNamePluralized = "Brands";
        protected override int HierarchyId { get => brandHierarchyId; }
        protected override string HierarchyName { get => brandHierarchyName; }
        protected override string HierarchyNamePluralized { get => brandHierarchyNamePluralized; }

        public ValidateItemAssociationForDeleteBrandDecorator(
            IHierarchyClassService<IHierarchyClassRequest> deleteBrandService,
            IQueryHandler<IGetAssociatedItemsParameter, IEnumerable<Item>> getAssociatedItemsQuery,
            ListenerApplicationSettings settings,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger)
            : base(deleteBrandService, getAssociatedItemsQuery, settings, emailClient, logger) { }

        protected override IGetAssociatedItemsParameter BuildQueryParameter(IList<int> hierarchyClassIDs)
        {
            var queryParameters = new GetItemsByBrandIdParameter { BrandIds = hierarchyClassIDs };
            return queryParameters;
        }

        protected override Dictionary<string, List<string>> CompileHumanReadableErrorData(IEnumerable<HierarchyClassModel> brands, IEnumerable<Item> associatedItems)
        {
            var brandNamesToItems = associatedItems
                  .Join(brands, ia => ia.BrandHCID, b => b.HierarchyClassId, (ia, b) => new
                  {
                      BrandId = ia.BrandHCID.GetValueOrDefault(defaultValue: 0),
                      BrandName = b.HierarchyClassName,
                      ScanCode = ia.ScanCode
                  })
                  .GroupBy(iab => iab.BrandName, iab => iab.ScanCode)
                  .ToDictionary(g => g.Key, g => g.ToList());
            return brandNamesToItems;
        }

        protected override void RemoveInvalidDataFromRequest(IHierarchyClassRequest request, IEnumerable<Item> associatedItems)
        {
            // remove invalid brands from original DeleteBrandRequest object
            request.HierarchyClasses.RemoveAll(hc => associatedItems.Select(iab => iab.BrandHCID).Contains(hc.HierarchyClassId));
        }
    }
}
