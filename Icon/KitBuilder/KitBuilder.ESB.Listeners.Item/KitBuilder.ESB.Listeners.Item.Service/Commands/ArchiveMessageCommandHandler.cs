using System;
using System.Xml.Linq;
using Icon.Common.DataAccess;
using Icon.DbContextFactory;
using Icon.Framework;
using KitBuilder.DataAccess.DatabaseModels;
using KitBuilder.DataAccess.Repository;

namespace KitBuilder.ESB.Listeners.Item.Service.Commands
{
    public class ArchiveMessageCommandHandler : ICommandHandler<ArchiveMessageCommand>
    {
        private IDbContextFactory<IconContext> contextFactory;

        public ArchiveMessageCommandHandler(IDbContextFactory<IconContext> contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public void Execute(ArchiveMessageCommand data)
        {
            using (var context = contextFactory.CreateContext())
            {
                context.InforMessageHistory.Add(new InforMessageHistory
                {
                    InforMessageId = Guid.Parse(data.Message.GetProperty("IconMessageID")),
                    Message = RemoveUtf8Encoding(data.Message.MessageText),
                    MessageStatusId = MessageStatusTypes.Consumed,
                    MessageTypeId = MessageTypes.InforProduct,
                    InsertDate = DateTime.Now
                });

                context.SaveChanges();
            }
        }

        private string RemoveUtf8Encoding(string messageText)
        {
            var xDoc = XDocument.Parse(messageText);
            return xDoc.ToString();
        }
    }
}
