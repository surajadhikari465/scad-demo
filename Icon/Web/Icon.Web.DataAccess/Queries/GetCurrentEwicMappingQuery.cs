using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCurrentEwicMappingQuery : IQueryHandler<GetCurrentEwicMappingParameters, string>
    {
        private readonly IconContext context;

        public GetCurrentEwicMappingQuery(IconContext context)
        {
            this.context = context;
        }

        public string Search(GetCurrentEwicMappingParameters parameters)
        {
            var currentMapping = context.Mapping.FirstOrDefault(m =>
                m.ScanCode.scanCode == parameters.WfmScanCode &&
                m.AplScanCode == parameters.AplScanCode);
            
            if (currentMapping == null)
            {
                return String.Empty;
            }
            else
            {
                return currentMapping.AplScanCode;
            }
        }
    }
}
