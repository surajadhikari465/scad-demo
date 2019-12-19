using Icon.Common.DataAccess;
using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetPickListOptionsParameters : IQuery<IEnumerable<PickListModel>>
    {
        public int AttributeId { get; set; }

        public bool ReturnAll { get; set; }
    }
}
