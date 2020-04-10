using Icon.Services.ItemPublisher.Services;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Infrastructure.Esb
{
    /// <summary>
    /// This class is responsible for creating the NonReceivingSystme list of systems
    /// that will not recieve an ESB message
    /// </summary>
    public class SystemListBuilder : ISystemListBuilder
    {
        private readonly ServiceSettings serviceSettings;

        public SystemListBuilder(ServiceSettings serviceSettings)
        {
            this.serviceSettings = serviceSettings;
        }

        /// <summary>
        /// Returns a list of systems that are set in the config that should not receive product messages
        /// </summary>
        /// <returns></returns>
        public List<string> BuildRetailNonReceivingSystemsList()
        {
            List<string> nonReceivingSystemsProduct = new List<string>();
            nonReceivingSystemsProduct.AddRange(this.serviceSettings.NonReceivingSystemsProduct);
            return nonReceivingSystemsProduct;
        }

        /// <summary>
        /// Returns a list of systems that should not receive product messages along with R10 because R10 should
        /// not receive non retail messages.
        /// </summary>
        /// <returns></returns>
        public List<string> BuildNonRetailReceivingSystemsList()
        {
            List<string> nonReceivingSystemsProduct = new List<string>();
            nonReceivingSystemsProduct.AddRange(this.serviceSettings.NonReceivingSystemsProduct);
            if (!nonReceivingSystemsProduct.Contains("R10"))
            {
                nonReceivingSystemsProduct.Add("R10");
            }

            return nonReceivingSystemsProduct;
        }
    }
}