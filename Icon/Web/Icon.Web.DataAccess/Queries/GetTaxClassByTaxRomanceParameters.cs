using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxClassByTaxRomanceParameters : IQuery<HierarchyClass>
    {
        public string TaxRomance { get; set; }
    }
}
