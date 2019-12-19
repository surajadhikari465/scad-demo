using Icon.Services.ItemPublisher.Infrastructure;
using System.Configuration;

namespace Icon.Services.Newitem.Test.Common
{
    /// <summary>
    /// This class is a connection and transaction manager for integration testing.
    /// </summary>
    public class ConnectionHelper
    {
        public IProviderFactory ProviderFactory { get; private set; }

        public ConnectionHelper()
        {
            this.ProviderFactory = new ProviderFactory(this.GetTestConnectionString());
        }

        /// <summary>
        /// Returns the Icon connection string from the app.config
        /// </summary>
        /// <returns></returns>
        private string GetTestConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Icon"].ConnectionString;
        }
    }
}