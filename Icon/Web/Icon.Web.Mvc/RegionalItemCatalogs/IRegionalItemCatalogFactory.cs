using System.Collections.Generic;

namespace Icon.Web.Mvc.RegionalItemCatalogs
{
    public interface IRegionalItemCatalogFactory
    {
        List<RegionalItemCatalog> GetRegionalItemCatalogs();
    }
}