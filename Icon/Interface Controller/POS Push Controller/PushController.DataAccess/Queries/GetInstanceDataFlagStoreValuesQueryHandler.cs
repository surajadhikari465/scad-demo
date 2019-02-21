using PushController.Common.Models;
using PushController.DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PushController.DataAccess.Queries
{
    public class GetInstanceDataFlagStoreValuesQueryHandler : IQueryHandler<GetInstanceDataFlagStoreValuesQuery, IEnumerable<InstanceDataFlagStoreValues>>
    {
        public IEnumerable<InstanceDataFlagStoreValues> Execute(GetInstanceDataFlagStoreValuesQuery parameters)
        {
            return parameters.Context.Database.SqlQuery<InstanceDataFlagStoreValues>("EXEC dbo.GetInstanceDataFlagStoreValues {0}", parameters.FlagKey).ToList();
        }
    }
}
