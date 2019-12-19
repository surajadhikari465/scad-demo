using Icon.Common.DataAccess;

namespace Icon.Web.DataAccess.Queries
{
    public class DoesAttributeExistOnItemsParameters : IQuery<bool>
    {
        public int AttributeId { get; set; }
    }
}
