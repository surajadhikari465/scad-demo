using Irma.Framework;
using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetInstanceDataFlagStoreValuesQuery : IQuery<IEnumerable<InstanceDataFlagStoreValues>>
    {
        public string FlagKey { get; set; }
        public IrmaContext Context { get; set; }
    }
}
