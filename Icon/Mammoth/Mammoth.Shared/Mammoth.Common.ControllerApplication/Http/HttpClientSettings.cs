using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication.Http
{
    public class HttpClientSettings
    {
        public string BaseAddress { get; set; }

        public static HttpClientSettings CreateFromConfig()
        {
            return new HttpClientSettings 
                {
                    BaseAddress = AppSettingsAccessor.GetStringSetting("BaseAddress"),
                };
        }
    }
}
