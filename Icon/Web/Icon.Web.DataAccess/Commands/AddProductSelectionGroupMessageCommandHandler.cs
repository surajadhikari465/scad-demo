using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Linq;

namespace Icon.Web.DataAccess.Commands
{
    public class AddProductSelectionGroupMessageCommandHandler : ICommandHandler<AddProductSelectionGroupMessageCommand>
    {
        private IconContext context;

        public AddProductSelectionGroupMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddProductSelectionGroupMessageCommand data)
        {
            if (data.ProductSelectionGroupTypeId == 0)
            {
                throw new ArgumentException("There is no valid ProductSelectionGroupTypeId.");
            }

            var message = new MessageQueueProductSelectionGroup
            {
                InsertDate = DateTime.Now,
                MessageActionId = MessageActionTypes.AddOrUpdate,
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ProductSelectionGroup,
                ProductSelectionGroupId = data.ProductSelectionGroupId,
                ProductSelectionGroupName = data.ProductSelectionGroupName,
                ProductSelectionGroupTypeId = data.ProductSelectionGroupTypeId,
                ProductSelectionGroupTypeName = context.ProductSelectionGroupType.Single(type => type.ProductSelectionGroupTypeId == data.ProductSelectionGroupTypeId).ProductSelectionGroupTypeName
            };

            context.MessageQueueProductSelectionGroup.Add(message);
            context.SaveChanges();
        }
    }
}
