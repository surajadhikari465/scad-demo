using Dapper;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetGpmStatusQuery : IQueryHandler<GetGpmStatusParameters, IList<RegionGpmStatus>>
    {
        private MammothContext mammothDbContext;

        public GetGpmStatusQuery(MammothContext connection)
        {
            this.mammothDbContext = connection;
        }

        public IList<RegionGpmStatus> Search(GetGpmStatusParameters parameters)
        {
            var gpmStatuses = mammothDbContext.RegionGpmStatuses
                .OrderBy(s => s.Region)
                .ToList();

            if (!String.IsNullOrWhiteSpace(parameters.Region))
            {
                return gpmStatuses
                    .Where(rgs => rgs.Region.Equals(parameters.Region))
                    .OrderBy(s => s.Region)
                    .ToList();
            }
            return gpmStatuses;
        }
    }
}
