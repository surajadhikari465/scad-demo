using Dapper;
using Icon.Common.DataAccess;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class UpsertGpmStatusCommandHandler : ICommandHandler<UpsertGpmStatusCommandParameters>
    {
        private MammothContext mammothDbContext;

        public UpsertGpmStatusCommandHandler(MammothContext connection)
        {
            this.mammothDbContext = connection;
        }

        public void Execute(UpsertGpmStatusCommandParameters commandData)
        {
            if (commandData != null && !String.IsNullOrWhiteSpace(commandData.Region))
            {
                var gpmStatusForRegion = mammothDbContext.RegionGpmStatuses
                    .SingleOrDefault(rgs => rgs.Region.Equals(commandData.Region));
                if (gpmStatusForRegion == default(RegionGpmStatus))
                {
                    gpmStatusForRegion = new RegionGpmStatus
                    {
                        Region = commandData.Region,
                        IsGpmEnabled = commandData.IsGpmEnabled
                    };
                    mammothDbContext.RegionGpmStatuses.Add(gpmStatusForRegion);
                }
                else
                {
                    gpmStatusForRegion.Region = commandData.Region;
                    gpmStatusForRegion.IsGpmEnabled = commandData.IsGpmEnabled;
                }
                var status = mammothDbContext.SaveChanges();
            }
        }
    }
}
