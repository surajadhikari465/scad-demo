using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using Icon.DbContextFactory;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateSentToEsbHierarchyTraitCommandHandler : ICommandHandler<UpdateSentToEsbHierarchyTraitCommand>
    {
        private ILogger<UpdateSentToEsbHierarchyTraitCommandHandler> logger;
        private IDbContextFactory<IconContext> iconContextFactory;

        public UpdateSentToEsbHierarchyTraitCommandHandler(
            ILogger<UpdateSentToEsbHierarchyTraitCommandHandler> logger, 
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(UpdateSentToEsbHierarchyTraitCommand data)
        {
            if (data.PublishedHierarchyClasses == null || data.PublishedHierarchyClasses.Count == 0)
            {
                logger.Warn("UpdateSentToEsbHierarchyTraitCommandHandler.Execute() was provided a null or empty list.  Check method invocation logic in HierarchyQueueProcessor.");
                return;
            }

            using (var context = iconContextFactory.CreateContext())
            {
                var hierarchyClasses = context.HierarchyClass
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
                        logger.Warn(string.Format("HierarchyClass: {0} [{1}] does not have the Sent To ESB trait.  Attempting to add the trait for this hierarchy class...",
                            hierarchyClass.hierarchyClassName, hierarchyClass.hierarchyClassID));

                        var newSentToEsbTrait = new HierarchyClassTrait
                        {
                            hierarchyClassID = hierarchyClass.hierarchyClassID,
                            traitID = context.Trait.Single(t => t.traitCode == TraitCodes.SentToEsb).traitID,
                            traitValue = (DateTime.UtcNow + TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow)).ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture)
                        };

                        context.HierarchyClassTrait.Add(newSentToEsbTrait);

                        logger.Info(string.Format("Successfully added the Sent To ESB trait for hierarchyClass: {0} [{1}].",
                            hierarchyClass.hierarchyClassName, hierarchyClass.hierarchyClassID));
                    }
                }

                context.SaveChanges();
            }

            logger.Info("The Sent To ESB trait has been successfully updated for all hierarchy classes in the mini-bulk.");
        }
    }
}
