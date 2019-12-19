using System.Linq;
using OOS.Model;

namespace OutOfStock
{
    public class ApplcationConfig :IApplicationConfig
    {
        private readonly IOOSEntitiesFactory _oosFactory; 
        public ApplcationConfig(IOOSEntitiesFactory oosFactory)
        {
            _oosFactory = oosFactory;
        }

        public string GetValue(string key)
        {
            string value;
            using (var oos = _oosFactory.New())
            {
                value = oos.ApplicationConfig.FirstOrDefault(ac => ac.Key.Equals(key))?.ToString();
            }

            return value;

        }

        public void SetValue(string key, string value)
        {
            using (var oos = _oosFactory.New())
            {
                var config = oos.ApplicationConfig.FirstOrDefault(ac => ac.Key.Equals(key));
                if (config == null) return;
                config.Value = value;
                oos.SaveChanges();


            }
        }
    }
}