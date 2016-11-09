using Icon.Web.DataAccess.Infrastructure;
using Icon.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluMappingQuery : IQueryHandler<GetPluMappingParameters, List<Item>>
    {
        private readonly IconContext context;

        public GetPluMappingQuery(IconContext context)
        {
            this.context = context;
        }

        public List<Item> Search(GetPluMappingParameters parameters)
        {
            IQueryable<Item> query = context.Item
                .Include(item => item.ScanCode.Select(s => s.ScanCodeType))
                .Include(item => item.ItemTrait.Select(itemTrait => itemTrait.Trait))
                .Include(item => item.ItemHierarchyClass.Select(hc => hc.HierarchyClass.Hierarchy))
                .Include(item => item.PLUMap);

            // For all searches, only consider types 2 or 3 scan codes (POS PLU and Scale PLU).
            query = query.Where(item => item.ScanCode.Any(s => s.scanCodeTypeID == 2 || s.scanCodeTypeID == 3));

            if (parameters.ItemId != null)
            {
                return query.Where(i => i.itemID == parameters.ItemId).ToList();
            }

            if (!String.IsNullOrEmpty(parameters.NationalPlu))
            {
                query = query
                    .Where(item => item.ScanCode
                        .Any(s => s.scanCode.StartsWith(parameters.NationalPlu)));
            }

            if (!String.IsNullOrEmpty(parameters.RegionalPlu))
            {
                query = query.Where(item =>
                    item.PLUMap.flPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.maPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.mwPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.naPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.ncPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.nePLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.pnPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.rmPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.soPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.spPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.swPLU.StartsWith(parameters.RegionalPlu)
                    || item.PLUMap.ukPLU.StartsWith(parameters.RegionalPlu));
            }

            if (!String.IsNullOrEmpty(parameters.BrandName))
            {
                query = query.Where(item => item.ItemHierarchyClass
                    .Any(ihc => ihc.HierarchyClass.Hierarchy.hierarchyName == HierarchyNames.Brands
                        && ihc.HierarchyClass.hierarchyClassName.ToLower().Contains(parameters.BrandName.ToLower())));
            }

            if (!String.IsNullOrEmpty(parameters.PluDescription))
            {
                query = query
                    .Where(item => item.ItemTrait
                        .Any(itemTrait =>
                            itemTrait.Trait.traitCode == TraitCodes.ProductDescription
                            && itemTrait.traitValue.ToLower().Contains(parameters.PluDescription.ToLower())));
            }

            return query.ToList();
        }
    }
}
