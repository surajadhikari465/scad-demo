using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb;
using Icon.Infor.Listeners.Price.Services;

namespace Icon.Infor.Listeners.Price.EsbFactory
{
    public class EsbConnectionSettingsFactory : IEsbConnectionSettingsFactory
    {
        public EsbConnectionSettings CreateConnectionSettings(Type t)
        {
            if(t == typeof(SendPricesToEsbService))
            {
                return EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("successfulPrices");
            }
            else if (t == typeof(SendFailedPricesToEsbService))
            {
                return EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("failedPrices");
            }
            else
            {
                throw new ArgumentException($"No ESB connection settings are registered for given type {t}", nameof(t));
            }
        }
    }
}
