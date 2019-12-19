using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class ReprocessFailedIrmaPushCommandHandler : ICommandHandler<ReprocessFailedIrmaPushCommand>
    {
        private IconContext context;

        public ReprocessFailedIrmaPushCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(ReprocessFailedIrmaPushCommand data)
        {
            var failedIrmaPushes = context.IRMAPush.Where(ip => data.IrmaPushIds.Contains(ip.IRMAPushID));
            foreach (var failedIrmaPush in failedIrmaPushes)
            {
                failedIrmaPush.EsbReadyFailedDate = null;
                failedIrmaPush.UdmFailedDate = null;
            }
            context.SaveChanges();
        }
    }
}
