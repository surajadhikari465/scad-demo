using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;

namespace Icon.Web.DataAccess.Queries
{
    public class GetTaxClassByTaxRomanceParameters : IQuery<HierarchyClass>
    {
        public string TaxRomance { get; set; }
    }
}
