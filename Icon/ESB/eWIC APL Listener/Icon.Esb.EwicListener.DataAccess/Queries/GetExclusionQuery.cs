using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Framework;
using System.Linq;

namespace Icon.Esb.EwicAplListener.DataAccess.Queries
{
    public class GetExclusionQuery : IQueryHandler<GetExclusionParameters, ScanCodeModel>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public GetExclusionQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public ScanCodeModel Search(GetExclusionParameters parameters)
        {
            string trimmedScanCode = parameters.ExcludedScanCode.TrimStart('0');

            var exclusion = globalContext.Context.Agency
                .Where(a => a.ScanCode.Any(sc => sc.scanCode == trimmedScanCode))
                .SelectMany(a => a.ScanCode.Select(sc => sc.scanCode))
                .FirstOrDefault(sc => sc == trimmedScanCode);

            if (exclusion == null)
            {
                return new ScanCodeModel();
            }
            else
            {
                var scanCodeModel = new ScanCodeModel
                {
                    ScanCode = exclusion,
                    ProductDescription = globalContext.Context.ScanCode.Single(sc => sc.scanCode == exclusion)
                                            .Item.ItemTrait.Single(it => it.traitID == Traits.ProductDescription).traitValue
                };

                return scanCodeModel;
            }
        }
    }
}
