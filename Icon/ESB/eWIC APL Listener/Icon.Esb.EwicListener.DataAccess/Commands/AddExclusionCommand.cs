using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Esb.EwicAplListener.DataAccess.Commands
{
    public class AddExclusionCommand : ICommandHandler<AddExclusionParameters>
    {
        private readonly IRenewableContext<IconContext> globalContext;

        public AddExclusionCommand(IRenewableContext<IconContext> globalContext)
        {
            this.globalContext = globalContext;
        }

        public void Execute(AddExclusionParameters data)
        {
            Agency agency = globalContext.Context.Agency.Single(a => a.AgencyId == data.AgencyId);

            string trimmedScanCode = data.ExclusdedScanCode.TrimStart('0');
            ScanCode excludedScanCode = globalContext.Context.ScanCode.Single(sc => sc.scanCode == trimmedScanCode);

            agency.ScanCode.Add(excludedScanCode);
            globalContext.Context.SaveChanges();
        }
    }
}
