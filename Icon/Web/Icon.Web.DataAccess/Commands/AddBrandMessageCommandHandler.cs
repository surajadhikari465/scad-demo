using Icon.Common.DataAccess;
using Icon.Framework;
using System;

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
            var message = new MessageQueueHierarchy
            {
                MessageTypeId = MessageTypes.Hierarchy,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageHistoryId = null,
                MessageActionId = data.Action,
                InsertDate = DateTime.Now,
                HierarchyId = data.Brand.hierarchyID,
                HierarchyName = HierarchyNames.Brands,
                HierarchyLevelName = HierarchyLevelNames.Brand,
                ItemsAttached = true,
                HierarchyClassId = data.Brand.hierarchyClassID.ToString(),
                HierarchyClassName = data.Brand.hierarchyClassName,
                HierarchyLevel = data.Brand.hierarchyLevel.Value,
                HierarchyParentClassId = data.Brand.hierarchyParentClassID,
                InProcessBy = null,
                ProcessedDate = null,
                BrandAbbreviation = data.BrandAbbreviation,
                ZipCode = data.ZipCode,
                Designation = data.Designation,
                Locality = data.Locality,
                ParentCompany = data.ParentCompany
            };
            context.MessageQueueHierarchy.Add(message);
            context.SaveChanges();
        }
    }
}