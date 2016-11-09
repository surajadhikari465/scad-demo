using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Linq;

namespace Icon.ApiController.DataAccess.Commands
{
    public class UpdateStagedProductStatusCommandHandler : ICommandHandler<UpdateStagedProductStatusCommand>
    {
        private ILogger<UpdateStagedProductStatusCommandHandler> logger;
        private IRenewableContext<IconContext> globalContext;

        public UpdateStagedProductStatusCommandHandler(
            ILogger<UpdateStagedProductStatusCommandHandler> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(UpdateStagedProductStatusCommand data)
        {
            if (data.PublishedHierarchyClasses == null || data.PublishedHierarchyClasses.Count == 0)
            {
                logger.Warn("UpdateStagedProductStatusCommandHandler.Execute() was provided a null or empty list.  Check method invocation logic in HierarchyQueueProcessor.");
                return;
            }

            logger.Info("Checking for staged products which are waiting on hierarchy class transmission to ESB...");

            var stagedProducts = globalContext.Context.MessageQueueProduct.Where(mq =>
                mq.MessageStatusId == MessageStatusTypes.Staged &&
                (
                    data.PublishedHierarchyClasses.Contains(mq.BrandId) ||
                    data.PublishedHierarchyClasses.Contains(mq.MerchandiseClassId) ||
                    data.PublishedHierarchyClasses.Contains(mq.TaxClassId))
                )
                .ToList();

            // Need special processing for the Financial Hierarchy because the MessageQueueProduct 
            // stores the PeopleSoft number in the in the FinancialClassId instead of the HierarchyClassId.
            var publishedFinancialHierarchyClasses = globalContext.Context.HierarchyClass
                .Where(hc => hc.Hierarchy.hierarchyName == HierarchyNames.Financial && data.PublishedHierarchyClasses.Contains(hc.hierarchyClassID))
                .ToList();

            if (publishedFinancialHierarchyClasses.Count > 0)
            {
                var peopleSoftNumbers = publishedFinancialHierarchyClasses
                    .Select(fin => fin.hierarchyClassName.Split('(')[1].TrimEnd(')'))
                    .ToList();

                stagedProducts.AddRange(globalContext.Context.MessageQueueProduct.Where(mq =>
                    mq.MessageStatusId == MessageStatusTypes.Staged && peopleSoftNumbers.Contains(mq.FinancialClassId)));
            }

            foreach (var product in stagedProducts)
            {
                if (ProductHasAllHierarchiesSentToEsb(product))
                {
                    product.MessageStatusId = MessageStatusTypes.Ready;

                    globalContext.Context.SaveChanges();

                    logger.Info(String.Format("Hierarchy sequencing resolved for scan code {0}.  All hierarchies for this product appear to be sent to ESB.  The message status for MessageQueueId {1} has been updated to Ready.",
                        product.ScanCode, product.MessageQueueId));
                }
            }
        }

        private bool ProductHasAllHierarchiesSentToEsb(MessageQueueProduct product)
        {
            var allHierarchiesAreSentToEsb = globalContext.Context.HierarchyClass
                .Where(hc =>
                    hc.hierarchyClassID == product.MerchandiseClassId ||
                    hc.hierarchyClassID == product.BrandId ||
                    (hc.hierarchyID == Hierarchies.Financial && hc.hierarchyClassName.Contains(product.FinancialClassName)))
                .ToList()
                .All(hc => hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.SentToEsb && !String.IsNullOrWhiteSpace(hct.traitValue)));

            return allHierarchiesAreSentToEsb;
        }
    }
}
