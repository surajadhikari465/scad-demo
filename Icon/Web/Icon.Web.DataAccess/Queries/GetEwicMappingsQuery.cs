using System.Data.Entity;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicMappingsQuery : IQueryHandler<GetEwicMappingsParameters, List<EwicMappingModel>>
    {
        private readonly IconContext context;

        public GetEwicMappingsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<EwicMappingModel> Search(GetEwicMappingsParameters parameters)
        {
            var distinctAplMappings = context.Mapping
                .Include(m => m.ScanCode.Item.ItemTrait.Select(it => it.Trait))
                .Where(m => m.AplScanCode == parameters.AplScanCode)
                .Select(m => new EwicMappingModel
                    {
                        ScanCode = m.ScanCode.scanCode
                    })
                .Distinct()
                .ToList();

            foreach (var mapping in distinctAplMappings)
            {
                mapping.ProductDescription = context.ScanCode.Single(sc => sc.scanCode == mapping.ScanCode).Item.ItemTrait
                    .Single(it => it.localeID == Locales.WholeFoods && it.Trait.traitCode == TraitCodes.ProductDescription).traitValue;
            }

            return distinctAplMappings;
        }
    }
}
