using PushController.DataAccess.Interfaces;
using System;
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
