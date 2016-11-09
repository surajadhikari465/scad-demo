namespace Icon.Monitoring.DataAccess
{
    using System;
    using System.Configuration;

    public class ConnectionBuilder : IConnectionBuilder
    {
        private const string Icon = "Icon";
        private const string IrmaItemCatalog = "ItemCatalog";
        private const string Mammoth = "Mammoth";

        public string GetIconConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Icon].ConnectionString;
        }

        public string GetIrmaConnectionStringForRegion(string region)
        {
            var regionalKey = string.Format("{0}_{1}", IrmaItemCatalog, region);
            return ConfigurationManager.ConnectionStrings[regionalKey].ConnectionString;
        }

        public string GetMammothConnectionString()
        {
            return ConfigurationManager.ConnectionStrings[Mammoth].ConnectionString;
        }
    }
}
