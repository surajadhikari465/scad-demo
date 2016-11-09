using Icon.ApiController.Common;
using Icon.Common.DataAccess;

namespace Icon.ApiController.DataAccess.Queries
{
    public class GetLocaleLineageParameters : IQuery<LocaleLineageModel>
    {
        public int LocaleId { get; set; }
        public int LocaleTypeId { get; set; }
    }
}
