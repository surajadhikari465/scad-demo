using Icon.Common.DataAccess;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetAttributeByAttributeIdParameters : IQuery<AttributeModel>
    {
        public int AttributeId { get; set; }
    }
}