using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddBrandMessageCommandHandler : ICommandHandler<AddBrandMessageCommand>
    {
        private IconContext context;

        public AddBrandMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddBrandMessageCommand data)
        {
            var message = new MessageQueueHierarchy();

            message.MessageTypeId = MessageTypes.Hierarchy;
            message.MessageStatusId = MessageStatusTypes.Ready;
            message.MessageHistoryId = null;
            message.MessageActionId = data.Action;
            message.InsertDate = DateTime.Now;
            message.HierarchyId = data.Brand.hierarchyID;
            message.HierarchyName = HierarchyNames.Brands;
            message.HierarchyLevelName = HierarchyLevelNames.Brand;
            message.ItemsAttached = true;
            message.HierarchyClassId = data.Brand.hierarchyClassID.ToString();
            message.HierarchyClassName = data.Brand.hierarchyClassName;
            message.HierarchyLevel = data.Brand.hierarchyLevel.Value;
            message.HierarchyParentClassId = data.Brand.hierarchyParentClassID;
            message.InProcessBy = null;
            message.ProcessedDate = null;

            context.MessageQueueHierarchy.Add(message);
            context.SaveChanges();
        }
    }
}
