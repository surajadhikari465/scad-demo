using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetEwicExclusionsQuery : IQueryHandler<GetEwicExclusionsParameters, List<EwicExclusionModel>>
    {
        private readonly IconContext context;

        public GetEwicExclusionsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<EwicExclusionModel> Search(GetEwicExclusionsParameters parameters)
        {
            var agenciesWithExclusions = context.Agency
                .Include(a => a.ScanCode.Select(sc => sc.Item).Select(i => i.ItemTrait.Select(it => it.Trait)))
                .Where(a => a.ScanCode.Count > 0)
                .ToList();

            List<EwicExclusionModel> excludedScanCodes = new List<EwicExclusionModel>();
            foreach (var agency in agenciesWithExclusions)
            {
                foreach (var scanCode in agency.ScanCode)
                {
                    if (!excludedScanCodes.Any(sc => sc.ScanCode == scanCode.scanCode))
                    {
                        excludedScanCodes.Add(new EwicExclusionModel
                        {
                            ScanCode = scanCode.scanCode,
                            ProductDescription = scanCode.Item.ItemTrait.Single(it => it.Trait.traitCode == TraitCodes.ProductDescription).traitValue
                        });
                    }
                }
            }

            return excludedScanCodes;
        }
    }
}
