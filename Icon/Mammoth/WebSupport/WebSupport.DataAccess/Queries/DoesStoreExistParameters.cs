using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Queries
{
    public class DoesStoreExistParameters : IQuery<bool>
    {
        public string BusinessUnitId { get; set; }
    }
}
