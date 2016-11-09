using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAplScanCodesQuery : IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>>
    {
        private readonly IconContext context;

        public GetAplScanCodesQuery(IconContext context)
        {
            this.context = context;
        }

        public List<EwicAplScanCodeModel> Search(GetAplScanCodesParameters parameters)
        {
            var distinctAplScanCodes = context.AuthorizedProductList
                .GroupBy(apl => apl.ScanCode)
                .Select(g => new EwicAplScanCodeModel
                    {
                        ScanCode = g.Key,
                        ItemDescription = g.FirstOrDefault().ItemDescription
                    })
                .ToList();

            return distinctAplScanCodes;
        }
    }
}
