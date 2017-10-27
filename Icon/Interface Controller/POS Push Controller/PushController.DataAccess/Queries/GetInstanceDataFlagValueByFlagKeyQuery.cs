using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using Irma.Framework;

namespace PushController.DataAccess.Queries
{
   public class GetInstanceDataFlagValueByFlagKeyQuery: IQuery<bool>
    {
        public string FlagKey { get; set; }
        public string StoreNo { get; set; }
        public IrmaContext Context { get; set; }
    }
}
