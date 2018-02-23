using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
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
    public class ValidateItemAssociationForDeleteMerchandiseHierarchyDecorator : DeleteHierarchyClassValidationDecoratorBase
    {
        private const int merchandiseHierarchyId = Hierarchies.Merchandise;
        private const string merchandiseHierarchyName = "Merchandise Hierarchy";
        private const string merchandiseHierarchyNamePluralized = "Merchandise Hierarchies";
        protected override int HierarchyId { get => merchandiseHierarchyId; }
        protected override string HierarchyName { get => merchandiseHierarchyName; }
        protected override string HierarchyNamePluralized { get => merchandiseHierarchyNamePluralized; }

        public ValidateItemAssociationForDeleteMerchandiseHierarchyDecorator(
            IHierarchyClassService<IHierarchyClassRequest> deleteMerchandiseClassService,
            IQueryHandler<IGetAssociatedItemsParameter, IEnumerable<Item>> getAssociatedItemsQuery,
            ListenerApplicationSettings settings,
            IEmailClient emailClient,
            ILogger<MammothHierarchyClassListener> logger)
            : base(deleteMerchandiseClassService, getAssociatedItemsQuery, settings, emailClient, logger) { }

        protected override IGetAssociatedItemsParameter BuildQueryParameter(IList<int> hierarchyClassIDs)
        {
            var queryParameters = new GetItemsByMerchandiseHierarchyIdParameter { MerchandiseHierarchyIDs = hierarchyClassIDs };
            return queryParameters;
        }

        protected override Dictionary<string, List<string>> CompileHumanReadableErrorData(IEnumerable<HierarchyClassModel> merchandiseClasses, IEnumerable<Item> associatedItems)
        {
            var merchandiseClassNamesToItems = associatedItems
                    .Join(merchandiseClasses, ia => ia.HierarchyMerchandiseID, b => b.HierarchyClassId, (ia, b) => new
                    {
                        MerchandiseClassId = ia.HierarchyMerchandiseID.GetValueOrDefault(defaultValue: 0),
                        MerchandiseClassName = b.HierarchyClassName,
                        ScanCode = ia.ScanCode
                    })
                    .GroupBy(iab => iab.MerchandiseClassName, iab => iab.ScanCode)
                    .ToDictionary(g => g.Key, g => g.ToList());
            return merchandiseClassNamesToItems;
        }

        protected override void RemoveInvalidDataFromRequest(IHierarchyClassRequest request, IEnumerable<Item> associatedItems)
        {
            // remove invalid merchandise hierarchies from original delete request object
            request.HierarchyClasses.RemoveAll(hc => associatedItems.Select(i => i.HierarchyMerchandiseID).Contains(hc.HierarchyClassId));
        }
    }
}
