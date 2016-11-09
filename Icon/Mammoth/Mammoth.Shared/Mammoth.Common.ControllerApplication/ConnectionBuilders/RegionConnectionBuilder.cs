using Mammoth.Common.DataAccess.ConnectionBuilders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication.ConnectionBuilders
{
    public class RegionConnectionBuilder : IConnectionBuilder
    {
        private IRegionalControllerApplicationSettings settings;
        private Dictionary<string, string> connectionStrings;

        public RegionConnectionBuilder(IRegionalControllerApplicationSettings settings)
        {
            this.settings = settings;
            this.connectionStrings = new Dictionary<string, string>();
            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                if (connectionString.Name.StartsWith("ItemCatalog"))
                {
                    var regionCode = connectionString.Name.Split('_')[1];
                    connectionStrings.Add(regionCode, connectionString.ConnectionString);
                }
            }
        }

        public string BuildConnectionString()
        {
            return connectionStrings[settings.CurrentRegion];
        }
    }
}
