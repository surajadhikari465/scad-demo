using Icon.Common.DataAccess;

namespace WebSupport.DataAccess.Queries
{
    public class IsRegionOnGpmParameters:IQuery<bool>
    {
        public string Region { get; set; }
    }
}