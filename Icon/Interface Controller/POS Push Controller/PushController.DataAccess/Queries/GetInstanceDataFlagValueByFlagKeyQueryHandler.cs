using Icon.Logging;
using Irma.Framework;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetInstanceDataFlagValueByFlagKeyQueryHandler : IQueryHandler<GetInstanceDataFlagValueByFlagKeyQuery, bool>
    {
        public Boolean Execute(GetInstanceDataFlagValueByFlagKeyQuery parameters)
        {
            return parameters.Context.Database.SqlQuery<bool>("EXEC GetInstanceDataFlagValue {0}, {1}", parameters.FlagKey, parameters.StoreNo).FirstOrDefault();
        }
    }
}
