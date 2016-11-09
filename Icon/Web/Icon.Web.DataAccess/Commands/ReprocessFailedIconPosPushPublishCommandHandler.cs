using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Irma.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessIconPosPushPublishCommandHandler : ICommandHandler<ReprocessFailedIconPosPushPublishCommand>
    {
        private IrmaContext context;
        public void Execute(ReprocessFailedIconPosPushPublishCommand data)
        {
            using (context = new IrmaContext(data.RegionalConnectionStringName))
            {
                var posPushPublishes = context.IConPOSPushPublish.Where(p => data.IconPosPushPublishIds.Contains(p.IConPOSPushPublishID));

                foreach (var posPushPublish in posPushPublishes)
                {
                    posPushPublish.ProcessingFailedDate = null;
                }
                context.SaveChanges();
            }
        }
    }
}
