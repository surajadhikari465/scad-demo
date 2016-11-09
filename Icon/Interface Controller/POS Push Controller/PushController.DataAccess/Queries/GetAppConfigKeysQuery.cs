using Irma.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;

namespace PushController.DataAccess.Queries
{
    public class GetAppConfigKeysQuery : IQuery<List<GetAppConfigKeysResult>>
    {
        public IrmaContext Context { get; set; }
        public string ApplicationName { get; set; }
    }
}
