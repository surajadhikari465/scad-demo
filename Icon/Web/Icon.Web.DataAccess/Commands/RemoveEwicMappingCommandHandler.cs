using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class RemoveEwicMappingCommandHandler : ICommandHandler<RemoveEwicMappingCommand>
    {
        private readonly IconContext context;

        public RemoveEwicMappingCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(RemoveEwicMappingCommand data)
        {
            var mappingsForRemoval = context.Mapping.Where(m => m.AplScanCode == data.AplScanCode && m.ScanCode.scanCode == data.WfmScanCode).ToList();

            context.Mapping.RemoveRange(mappingsForRemoval);
            context.SaveChanges();
        }
    }
}
