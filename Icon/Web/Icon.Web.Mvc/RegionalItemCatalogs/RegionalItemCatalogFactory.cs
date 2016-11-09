using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.RegionalItemCatalogs
{
    public class RegionalItemCatalogFactory : IRegionalItemCatalogFactory
    {
        public List<RegionalItemCatalog> GetRegionalItemCatalogs()
        {
            List<RegionalItemCatalog> regionalItemCatalogs = new List<RegionalItemCatalog>();
            ConnectionStringSettingsCollection connectionStringSettings = ConfigurationManager.ConnectionStrings;

            foreach (ConnectionStringSettings settings in connectionStringSettings)
            {
                if (settings.Name.StartsWith("ItemCatalog"))
                {
                    regionalItemCatalogs.Add(GetRegionalItemCatalog(settings));
                }
            }

            return regionalItemCatalogs;
        }

        private RegionalItemCatalog GetRegionalItemCatalog(ConnectionStringSettings connectionStringSetting)
        {
            return new RegionalItemCatalog
            {
                Abbreviation = connectionStringSetting.Name.Split('_')[1],
                ConnectionString = connectionStringSetting.ConnectionString,
                ConnectionStringName = connectionStringSetting.Name
            };
        }
    }
}