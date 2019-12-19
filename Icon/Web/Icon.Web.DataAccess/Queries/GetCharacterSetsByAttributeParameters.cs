using Icon.Common.DataAccess;
using System.Collections.Generic;
using Icon.Common.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetCharacterSetsByAttributeParameters : IQuery<List<AttributeCharacterSetModel>>
    {
        public int AttributeId { get; set; }
    }
}
