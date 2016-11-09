using RegionalEventController.DataAccess.Interfaces;

namespace RegionalEventController.DataAccess.Queries
{
    public class GetAppConfigValueQuery : IQuery<string>
    {
        public string applicationName { get; set; }
        public string configurationKey { get; set; }
    }
}
