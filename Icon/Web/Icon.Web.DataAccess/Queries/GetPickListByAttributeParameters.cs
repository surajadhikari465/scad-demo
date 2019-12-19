using Icon.Common.DataAccess;
using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPickListByAttributeParameters : IQuery<List<PickListModel>>
    {
        public int AttributeId { get; set; }
    }
}
