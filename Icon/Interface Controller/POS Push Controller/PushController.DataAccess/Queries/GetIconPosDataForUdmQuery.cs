using Icon.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetIconPosDataForUdmQuery : IQuery<List<IRMAPush>> 
    {
        public int Instance { get; set; }
    }
}
