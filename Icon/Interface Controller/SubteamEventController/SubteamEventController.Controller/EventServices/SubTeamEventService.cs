using GlobalEventController.Controller.EventServices;
using Icon.Common.DataAccess;
using Icon.Framework;
using Irma.Framework;
using SubteamEventController.DataAccess.Commands;
using SubteamEventController.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SubteamEventController.Controller.EventServices
{
    public class SubTeamEventService : IEventService
    {
        private IrmaContext irmaContext;
        private IQueryHandler<GetSubTeamParameters, HierarchyClass> getSubTeamQuery;
        private ICommandHandler<UpdateSubTeamCommand> updateSubTeamCommandHandler;

        public int? ReferenceId { get; set; }
        public string Message { get; set; }
        public string Region { get; set; }
        public int EventTypeId { get; set; }
        //This is not being used
        public List<Icon.Framework.ScanCode> ScanCodes { get; set; }

        public SubTeamEventService(IrmaContext irmaContext,
            IQueryHandler<GetSubTeamParameters, HierarchyClass> getSubTeamQuery,
            ICommandHandler<UpdateSubTeamCommand> updateSubTeamCommandHandler)
        {
            this.irmaContext = irmaContext;
            this.getSubTeamQuery = getSubTeamQuery;
            this.updateSubTeamCommandHandler = updateSubTeamCommandHandler;
        }

        public void Run()
        {
            if(ReferenceId == null)
            {
                throw new InvalidOperationException(String.Format("Unable to update Sub Team. ReferenceId is null. Event Message: {0}", Message));
            }

            var subTeam = getSubTeamQuery.Search(new GetSubTeamParameters { SubTeamId = ReferenceId.Value });

            if(subTeam == null)
            {
                throw new InvalidOperationException(String.Format("Unable to update Sub Team. No SubTeam found with ReferenceId {0}. Event Message: {1}", ReferenceId, Message));
            }

            HierarchyClassTrait posDeptNumberTrait = subTeam.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.PosDepartmentNumber);
            if (posDeptNumberTrait == null)
            {
                throw new InvalidOperationException(String.Format("Unable to update Sub Team. No POS Department Number associated to Sub Team. ReferenceId {0}. Event Message: {1}", ReferenceId, Message));
            }
            HierarchyClassTrait subTeamNumberTrait = subTeam.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.TeamNumber);
            HierarchyClassTrait teamNameTrait = subTeam.HierarchyClassTrait.FirstOrDefault(hct => hct.traitID == Traits.TeamName);

            int posDeptNumber = 0;
            string subTeamName = subTeam.hierarchyClassName;
            int subTeamNumber = 0;
            string teamName = String.Empty;
            int teamNumber = 0;

            try
            {
                int.TryParse(posDeptNumberTrait.traitValue, out posDeptNumber);
                int.TryParse(subTeam.hierarchyClassName.Split('(')[1].TrimEnd(')'), out subTeamNumber);
                if (subTeamNumberTrait != null)
                {
                    int.TryParse(subTeamNumberTrait.traitValue, out teamNumber);
                }
                if (teamNameTrait != null)
                {
                    teamName = subTeam.HierarchyClassTrait.First(hct => hct.traitID == Traits.TeamName).traitValue;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(String.Format("Unable to update Sub Team. Error occurred when getting Sub Team's traits. ReferenceId {0}. Event Message: {1}", ReferenceId, Message), ex);
            }

            var command = new UpdateSubTeamCommand
            {
                PosDepartmentNumber = posDeptNumber,
                SubTeamName = subTeamName,
                SubTeamNumber = subTeamNumber,
                TeamName = teamName,
                TeamNumber = teamNumber,
                Region = Region
            };

            updateSubTeamCommandHandler.Execute(command);

            irmaContext.SaveChanges();
        }
    }
}
