using Services.Extract.Models;

namespace Services.Extract
{
    public interface IExtractJobConfigurationParser
    {
        ExtractJobConfiguration Parse(string configurationJson);
    }
}