using Newtonsoft.Json;
using Services.Extract.Models;

namespace Services.Extract
{
    public class ExtractJobConfigurationParser : IExtractJobConfigurationParser
    {
        public ExtractJobConfiguration Parse(string configurationJson)
        {
            return JsonConvert.DeserializeObject<ExtractJobConfiguration>(configurationJson);
        }
    }
}