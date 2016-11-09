using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class RemoveEwicExclusionCommandHandler : ICommandHandler<RemoveEwicExclusionCommand>
    {
        private readonly IconContext context;

        public RemoveEwicExclusionCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RemoveEwicExclusionCommand data)
        {
            var excludedScanCode = context.ScanCode.Single(sc => sc.scanCode == data.ScanCode);

            excludedScanCode.Agency.Clear();

            context.SaveChanges();
        }
    }
}
