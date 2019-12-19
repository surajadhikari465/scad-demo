using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Queries;

namespace Icon.Web.Mvc.Models
{
    public class GetItemsAttributesParameters
    {
        public string AttributeName { get; set; }
        public string AttributeValue { get; set; }
        public AttributeSearchOperator SearchOperator { get; set; }
    }
}