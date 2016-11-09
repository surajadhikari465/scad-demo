using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddEwicMappingCommandHandler : ICommandHandler<AddEwicMappingCommand>
    {
        private readonly IconContext context;

        public AddEwicMappingCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddEwicMappingCommand data)
        {
            // The Mapping table uses scanCodeID to ensure integrity with the ScanCode table.
            int scanCodeId = context.ScanCode.Single(sc => sc.scanCode == data.WfmScanCode).scanCodeID;
            Mapping mapping;
            
            foreach (var agency in data.Agencies)
            {
                mapping = new Mapping
                {
                    AgencyId = agency.AgencyId,
                    AplScanCode = data.AplScanCode,
                    ScanCodeId = scanCodeId
                };

                context.Mapping.Add(mapping);
            }

            context.SaveChanges();
        }
    }
}
