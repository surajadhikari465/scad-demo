using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.RegionalItemCatalogs
{
    public interface IRegionalItemCatalogFactory
    {
        List<RegionalItemCatalog> GetRegionalItemCatalogs();
    }
}