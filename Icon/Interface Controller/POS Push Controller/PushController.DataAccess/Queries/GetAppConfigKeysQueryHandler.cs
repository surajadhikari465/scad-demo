using Irma.Framework;
using PushController.DataAccess.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetAppConfigKeysQueryHandler : IQueryHandler<GetAppConfigKeysQuery, List<GetAppConfigKeysResult>>
    {
        public List<GetAppConfigKeysResult> Execute(GetAppConfigKeysQuery parameters)
        {
            return parameters.Context.GetAppConfigKeys(parameters.ApplicationName).ToList();
        }
    }
}
