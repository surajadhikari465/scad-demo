using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.DataAccess.Commands
{
    public class AddHierarchyClassMessageCommandHandler : ICommandHandler<AddHierarchyClassMessageCommand>
    {
        private IconContext context;

        public AddHierarchyClassMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddHierarchyClassMessageCommand data)
        {
            // If only a tax abbreviation or GL Account changed, then don't generate message.
            if (!data.ClassNameChange)
            {
                return;
            }

            // The Browsing hierarchy isn't supported by R10, so don't generate a message for those changes.
            if (data.HierarchyClass.hierarchyID == Hierarchies.Browsing)
            {
                return;
            }

            MessageQueueHierarchy message = new MessageQueueHierarchy();

            // Hierarchy Class Information
            message.HierarchyLevel = data.HierarchyClass.hierarchyLevel.Value;
            message.HierarchyParentClassId = data.HierarchyClass.hierarchyParentClassID;
            if (context.Hierarchy.Single(h => h.hierarchyID == data.HierarchyClass.hierarchyID).hierarchyName == HierarchyNames.Financial)
            {
                message.HierarchyClassName = data.HierarchyClass.hierarchyClassName.Split('(')[1].Trim(')');
                message.HierarchyClassId = data.HierarchyClass.hierarchyClassName.Split('(')[1].Trim(')');
            }
            else
            {
                message.HierarchyClassName = data.HierarchyClass.hierarchyClassName;
                message.HierarchyClassId = data.HierarchyClass.hierarchyClassID.ToString();
            }

            // Hierarchy information.
            message.HierarchyId = data.HierarchyClass.hierarchyID;
            message.HierarchyName = context.Hierarchy.Single(h => h.hierarchyID == data.HierarchyClass.hierarchyID).hierarchyName;

            // HierarchyPrototype information.
            var prototype = context.HierarchyPrototype.Single(hp => hp.hierarchyLevel == data.HierarchyClass.hierarchyLevel && hp.hierarchyID == data.HierarchyClass.hierarchyID);
            message.HierarchyLevelName = prototype.hierarchyLevelName;
            message.ItemsAttached = prototype.itemsAttached.Value;

            // Message information.
            message.MessageStatusId = MessageStatusTypes.Ready;
            message.MessageTypeId = MessageTypes.Hierarchy;
            message.MessageActionId = data.DeleteMessage ? MessageActionTypes.Delete : MessageActionTypes.AddOrUpdate;
            message.InsertDate = DateTime.Now;

            message.InProcessBy = null;
            message.ProcessedDate = null;

            context.MessageQueueHierarchy.Add(message);
            context.SaveChanges();
        }
    }
}
