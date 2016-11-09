using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Extensions;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class UpdateSubTeamCommandHandler : ICommandHandler<UpdateSubTeamCommand>
    {
        private IconContext context;

        public UpdateSubTeamCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(UpdateSubTeamCommand data)
        {
            // Setup formatted sub-team name, ex: Grocery (1000)
            string subTeam = String.Format("{0} ({1})", data.SubTeamName.Trim(), data.PeopleSoftNumber.Trim());

            // Check for duplicate sub-teams
            var duplicateSubTeams = context.HierarchyClass
                                           .Where(hc => hc.hierarchyClassID != data.HierarchyClassId
                                                     && hc.hierarchyClassName == subTeam 
                                                     && hc.Hierarchy.hierarchyName == HierarchyNames.Financial);

            if (!duplicateSubTeams.Any())
            {
                HierarchyClass updatedHierarchyClass = context.HierarchyClass.Find(data.HierarchyClassId);

                // Get current sub-team name so we can find any other HierarchyClassTraits assigned to it
                string oldSubTeam = updatedHierarchyClass.hierarchyClassName;

                // Make update to HierarchyClass
                updatedHierarchyClass.hierarchyClassName = subTeam;

                // Make sure all other HierarchyClassTraits associated to a Sub-Team change get updated too
                List<HierarchyClassTrait> subTeamAssociations = context.HierarchyClassTrait
                    .Where(hct => hct.Trait.traitCode == TraitCodes.MerchFinMapping && hct.traitValue == oldSubTeam)
                    .ToList();

                foreach (var association in subTeamAssociations)
                {
                    association.traitValue = subTeam;
                }

                context.SaveChanges();

                bool peopleSoftChange = oldSubTeam.ParsePeopleSoftNumber() != data.PeopleSoftNumber.Trim();

                data.UpdatedHierarchyClass = updatedHierarchyClass;
                data.PeopleSoftChanged = peopleSoftChange;
            }
            else
            {
                throw new DuplicateValueException(String.Format("SubTeam {0} already exists. Please pick another name.", subTeam));
            }
        }
    }
}
