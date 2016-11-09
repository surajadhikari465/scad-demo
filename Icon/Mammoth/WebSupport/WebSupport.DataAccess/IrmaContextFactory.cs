namespace WebSupport.DataAccess
{
    using System;
    using System.Configuration;

    using Irma.Framework;

    public class IrmaContextFactory : IIrmaContextFactory
    {
        private const string DefaultDbPrefix = "ItemCatalog";

        public IrmaContext CreateContext(string regionAbbreviation)
        {
            var regionalConnectionStringKey = String.Format("{0}_{1}", DefaultDbPrefix, regionAbbreviation);
            var regionalConnectionString = ConfigurationManager.ConnectionStrings[regionalConnectionStringKey].ConnectionString;

            return new IrmaContext(regionalConnectionString);
        }
    }
}
