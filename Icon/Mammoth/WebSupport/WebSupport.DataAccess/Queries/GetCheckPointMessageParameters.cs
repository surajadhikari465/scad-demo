using Icon.Common.DataAccess;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetCheckPointMessageParameters: IQuery<CheckPointMessageModel>
    {
        public string Region { get; set; }
        public string BusinessUnitId { get; set; }
        public string ScanCode { get; set; }
    }
}