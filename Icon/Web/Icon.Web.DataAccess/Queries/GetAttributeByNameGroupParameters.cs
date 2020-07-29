using Icon.Common.DataAccess;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAttributeByNameGroupParameters : IQuery<AttributeModel>
    {
        public string AttributeName { get; set; }
        public string AttributeGroupName { get; set; }
    }
}