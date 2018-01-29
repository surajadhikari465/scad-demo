using Icon.Esb;
using Icon.Esb.Producer;
using System;
using WebSupport.DataAccess;

namespace WebSupport.EsbProducerFactory
{
    public class PriceRefreshEsbProducerFactory : IPriceRefreshEsbProducerFactory
    {
        public IEsbProducer CreateEsbProducer(string system, string region)
        {
            if(system == PriceRefreshConstants.R10)
            {
                return new NonJndiEsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("R10"));
            }
            else if(system == PriceRefreshConstants.IRMA)
            {
                var settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("IRMA");

               // all the 365 stores are stores in RM region in IRMA.
                if (region == RegionNameConstants.TS)
                {
                    region = RegionNameConstants.RM;
                }

                settings.QueueName = settings.QueueName.Replace("XX", region);
                return new NonJndiEsbProducer(settings);
            }
            else
            {
                throw new ArgumentException($"{system} is not a valid downstream system for price refreshes.", nameof(system));
            }
        }
    }
}