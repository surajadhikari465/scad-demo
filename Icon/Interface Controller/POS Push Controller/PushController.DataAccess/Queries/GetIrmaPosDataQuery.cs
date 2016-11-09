using Irma.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetIrmaPosDataQuery : IQuery<List<IConPOSPushPublish>> 
    {
        public IrmaContext Context { get; set; }
        public int Instance { get; set; }
    }
}
