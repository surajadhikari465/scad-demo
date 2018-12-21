using Icon.Dashboard.DataFileGenerator.Models;

namespace Icon.Dashboard.DataFileGenerator.Services
{
    public interface IConfigReader
    {
        ConfigDataModel ReadAppConfig();
    }
}