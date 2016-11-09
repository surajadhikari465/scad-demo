using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetDefaultTaxClassMismatchesQuery : IQueryHandler<GetDefaultTaxClassMismatchesParameters, List<DefaultTaxClassMismatchModel>>
    {
        private IconContext context;

        public GetDefaultTaxClassMismatchesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<DefaultTaxClassMismatchModel> Search(GetDefaultTaxClassMismatchesParameters parameters)
        {
            List<DefaultTaxClassMismatchModel> queryResults = context.GetDefaultTaxClassMismatches().Select(q => new DefaultTaxClassMismatchModel
                {
                    ScanCode = q.ScanCode,
                    Brand = q.Brand,
                    ProductDescription = q.ProductDescription,
                    MerchandiseLineage = q.MerchandiseLineage,
                    DefaultTaxClass = q.DefaultTaxClass,
                    TaxClassOverride = q.TaxClassOverride
                }).ToList();

            return queryResults;
        }
    }
}
