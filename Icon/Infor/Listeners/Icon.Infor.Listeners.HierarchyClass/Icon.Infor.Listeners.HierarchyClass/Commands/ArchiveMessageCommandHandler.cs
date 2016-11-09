using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;
using System.Xml.Linq;

namespace Icon.Infor.Listeners.HierarchyClass.Commands
{
    public class ArchiveMessageCommandHandler : ICommandHandler<ArchiveMessageCommand>
    {
        private IRenewableContext<IconContext> context;

        public ArchiveMessageCommandHandler(IRenewableContext<IconContext> context)
        {
            this.context = context;
        }

        public void Execute(ArchiveMessageCommand data)
        {
            context.Context.InforMessageHistory.Add(new InforMessageHistory
            {
                InforMessageId = Guid.Parse(data.Message.GetProperty("IconMessageID")),
                Message = RemoveUtf8Encoding(data.Message.MessageText),
                MessageStatusId = MessageStatusTypes.Consumed,
                MessageTypeId = MessageTypes.InforHierarchy,
                InsertDate = DateTime.Now
            });

            context.Context.SaveChanges();
        }

        private string RemoveUtf8Encoding(string messageText)
        {
            var xDoc = XDocument.Parse(messageText);
            return xDoc.ToString();
        }
    }
}
