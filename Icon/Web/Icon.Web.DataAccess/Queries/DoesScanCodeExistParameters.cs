using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class DoesScanCodeExistParameters : IQuery<bool>
    {
        public string ScanCode { get; set; }
    }
}