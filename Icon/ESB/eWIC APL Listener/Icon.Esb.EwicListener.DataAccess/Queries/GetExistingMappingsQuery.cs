using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Esb.EwicAplListener.DataAccess.Queries
{
    public class GetExistingMappingsQuery : IQueryHandler<GetExistingMappingsParameters, List<ScanCodeModel>>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public GetExistingMappingsQuery(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public List<ScanCodeModel> Search(GetExistingMappingsParameters parameters)
        {
            var mappings = new List<ScanCodeModel>();

            bool mappingAlreadyExists = globalContext.Context.Mapping.Any(m => m.AgencyId == parameters.AgencyId && m.AplScanCode == parameters.AplScanCode);

            if (mappingAlreadyExists)
            {
                return mappings;
            }
            else
            {
                mappings = globalContext.Context.Mapping
                    .Where(m => m.AplScanCode == parameters.AplScanCode)
                    .Select(m => new ScanCodeModel { ScanCode = m.ScanCode.scanCode, ScanCodeId = m.ScanCode.scanCodeID })
                    .Distinct()
                    .ToList();

                return mappings;
            }
        }
    }
}
