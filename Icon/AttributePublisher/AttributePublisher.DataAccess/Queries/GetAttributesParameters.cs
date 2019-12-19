using AttributePublisher.DataAccess.Models;
using Icon.Common.DataAccess;
using System.Collections.Generic;

namespace AttributePublisher.DataAccess.Queries
{
    public class GetAttributesParameters : IQuery<List<AttributeModel>>
    {
        public int RecordsPerQuery { get; set; }
    }
}
