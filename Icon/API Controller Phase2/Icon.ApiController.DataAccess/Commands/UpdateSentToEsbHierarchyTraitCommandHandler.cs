using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateSentToEsbHierarchyTraitCommandHandler : ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>
    {
        private ILogger<UpdateSentToEsbHierarchyTraitCommandHandler> logger;
        private IRenewableContext<IconContext> globalContext;

        public UpdateSentToEsbHierarchyTraitCommandHandler(
            ILogger<UpdateSentToEsbHierarchyTraitCommandHandler> logger, 
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(UpdateSentToEsbHierarchyTraitCommand data)
        {
            if (data.PublishedHierarchyClasses == null || data.PublishedHierarchyClasses.Count == 0)
            {
                logger.Warn("UpdateSentToEsbHierarchyTraitCommandHandler.Execute() was provided a null or empty list.  Check method invocation logic in HierarchyQueueProcessor.");
                return;
            }

            var hierarchyClasses = globalContext.Context.HierarchyClass
                .Include(hc => hc.HierarchyClassTrait.Select(hct => hct.Trait))
                .Where(hc => data.PublishedHierarchyClasses.Contains(hc.hierarchyClassID))
                .ToList();

            foreach (var hierarchyClass in hierarchyClasses)
            {
                var sentToEsbTrait = hierarchyClass.HierarchyClassTrait.SingleOrDefault(hct => hct.Trait.traitCode == TraitCodes.SentToEsb);

                if (sentToEsbTrait != null)
                {
                    sentToEsbTrait.traitValue = (DateTime.UtcNow + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow)).ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture);
                }
                else
                {
                    logger.Warn(String.Format("HierarchyClass: {0} [{1}] does not have the Sent To ESB trait.  Attempting to add the trait for this hierarchy class...",
                        hierarchyClass.hierarchyClassName, hierarchyClass.hierarchyClassID));

                    var newSentToEsbTrait = new HierarchyClassTrait
                    {
                        hierarchyClassID = hierarchyClass.hierarchyClassID,
                        traitID = globalContext.Context.Trait.Single(t => t.traitCode == TraitCodes.SentToEsb).traitID,
                        traitValue = (DateTime.UtcNow + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow)).ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture)
                    };

                    globalContext.Context.HierarchyClassTrait.Add(newSentToEsbTrait);

                    logger.Info(String.Format("Successfully added the Sent To ESB trait for hierarchyClass: {0} [{1}].",
                        hierarchyClass.hierarchyClassName, hierarchyClass.hierarchyClassID));
                }
            }

            globalContext.Context.SaveChanges();

            logger.Info("The Sent To ESB trait has been successfully updated for all hierarchy classes in the mini-bulk.");
        }
    }
}
