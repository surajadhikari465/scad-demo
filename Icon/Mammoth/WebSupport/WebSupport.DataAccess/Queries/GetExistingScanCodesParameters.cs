namespace WebSupport.DataAccess.Queries
{
    using System.Collections.Generic;

    using Icon.Common.DataAccess;

    public class GetExistingScanCodesParameters : IQuery<List<string>>
    {
        public string Region { get; set; }
        public List<string> ScanCodes { get; set; }
    }
}
