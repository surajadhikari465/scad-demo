using System;
using Icon.Common.DataAccess;
using Icon.Framework;

namespace Icon.Web.DataAccess.Commands
{
    public class AddManufacturerMessageCommandHandler : ICommandHandler<AddManufacturerMessageCommand>
    {
        private IconContext context;

        public AddManufacturerMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddManufacturerMessageCommand data)
        {
            var message = new MessageQueueHierarchy
            {
                MessageTypeId = MessageTypes.Hierarchy,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                MessageActionId = data.Action,
                InsertDate = DateTime.Now,
                HierarchyId = data.Manufacturer.hierarchyID,
                HierarchyName = HierarchyNames.Manufacturer,
                HierarchyLevelName = HierarchyLevelNames.Manufacturer,
                ItemsAttached = true,
                HierarchyClassId = data.Manufacturer.hierarchyClassID.ToString(),
                HierarchyClassName = data.Manufacturer.hierarchyClassName,
                HierarchyLevel = data.Manufacturer.hierarchyLevel.Value,
                HierarchyParentClassId = data.Manufacturer.hierarchyParentClassID,
                InProcessBy = null,
                ProcessedDate = null,
                ZipCode = data.ZipCode,
                ArCustomerId = data.ArCustomerId
            };

            context.MessageQueueHierarchy.Add(message);
            context.SaveChanges();
        }
    }
}
