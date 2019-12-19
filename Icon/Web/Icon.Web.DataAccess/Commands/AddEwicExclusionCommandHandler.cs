using Icon.Common.DataAccess;
using Icon.Framework;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddEwicExclusionCommandHandler : ICommandHandler<AddEwicExclusionCommand>
    {
        private readonly IconContext context;

        public AddEwicExclusionCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddEwicExclusionCommand data)
        {
            var agenciesById = data.Agencies.Select(a => a.AgencyId).ToList();
            var agencies = context.Agency.Where(a => agenciesById.Contains(a.AgencyId)).ToList();

            foreach (var agency in agencies)
            {
                var scanCodeForExclusion = context.ScanCode.Single(sc => sc.scanCode == data.ScanCode);
                scanCodeForExclusion.Agency.Add(agency);
            }

            context.SaveChanges();
        }
    }
}
