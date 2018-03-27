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
    public class DeleteGpmStatusCommandHandler : ICommandHandler<DeleteGpmStatusCommandParameters>
    {
        private MammothContext mammothDbContext;

        public DeleteGpmStatusCommandHandler(MammothContext connection)
        {
            this.mammothDbContext = connection;
        }

        public void Execute(DeleteGpmStatusCommandParameters commandData)
        {
            if (commandData != null && !String.IsNullOrWhiteSpace(commandData.Region))
            {
                var gpmStatusForRegion = mammothDbContext.RegionGpmStatuses
                    .SingleOrDefault(rgs => rgs.Region.Equals(commandData.Region));
                if (gpmStatusForRegion != default(RegionGpmStatus))
                {
                    mammothDbContext.RegionGpmStatuses.Remove(gpmStatusForRegion);
                    var status = mammothDbContext.SaveChanges();
                }
            }
        }
    }
}
