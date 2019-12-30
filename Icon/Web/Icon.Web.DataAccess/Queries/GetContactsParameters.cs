using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.DataAccess.Queries
{
    public class GetContactsParameters : IQuery<List<ContactModel>>
    {
        public int HierarchyClassId { get; set; }
    }
}
