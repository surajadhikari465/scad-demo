using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemTraitByTraitCodeParameters : IQuery<ItemTrait>
    {
        public string ScanCode { get; set; }
        public string TraitCode { get; set; }
    }
}
