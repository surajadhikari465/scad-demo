using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubteamEventController.DataAccess.Commands
{
    public class UpdateSubTeamCommandHandler : ICommandHandler<UpdateSubTeamCommand>
    {
        private Irma.Framework.IrmaContext irmaContext;

        public UpdateSubTeamCommandHandler(Irma.Framework.IrmaContext irmaContext)
        {
            this.irmaContext = irmaContext;
        }
        public void Execute(UpdateSubTeamCommand data)
        {
            var subTeam = irmaContext.SubTeam.FirstOrDefault(st => st.Dept_No == data.PosDepartmentNumber);

            if (subTeam == null)
            {
                subTeam = irmaContext.SubTeam.FirstOrDefault(st => st.SubTeam_Name == data.SubTeamName);

                if (subTeam == null)
                {
                    throw new ArgumentException(
                        String.Format("Unable to update Sub Team. No Sub Team exists with SubTeam_Name equal to {0} or Dept_No equal to {1} for Region {2}",
                            data.SubTeamName,
                            data.PosDepartmentNumber,
                            data.Region));
                }
            }

            if (data.PosDepartmentNumber != 0)
            {
                //subTeam.POSDept = data.PosDepartmentNumber;
                subTeam.Dept_No = data.PosDepartmentNumber;
            }

            if(!String.IsNullOrWhiteSpace(data.SubTeamName) && subTeam.SubTeam_Name != data.SubTeamName)
            {
                subTeam.SubTeam_Name = data.SubTeamName;
            }

            if(data.SubTeamNumber != 0)
            {
                var storeSubTeams = subTeam.StoreSubTeam.Where(sst => sst.PS_SubTeam_No != data.SubTeamNumber);
                foreach (var storeSubTeam in storeSubTeams)
                {
                    storeSubTeam.PS_SubTeam_No = data.SubTeamNumber;
                }
            }

            if(data.TeamNumber != 0)
            {
                var storeSubTeams = subTeam.StoreSubTeam.Where(sst => sst.PS_Team_No != data.TeamNumber);
                foreach (var storeSubTeam in storeSubTeams)
                {
                    storeSubTeam.PS_Team_No = data.TeamNumber;
                }
            }
            
            irmaContext.SaveChanges();
        }
    }
}
