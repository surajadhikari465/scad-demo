using System.Collections.Generic;
using Icon.Framework;
using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPluMappingParameters : IQuery<List<Item>>
    {
        public int? ItemId { get; set; }
        public string NationalPlu { get; set; }
        public string RegionalPlu { get; set; }
        public string BrandName { get; set; }
        public string PluDescription { get; set; }   
    }
}
