using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class AddLocaleMessageCommandHandler : ICommandHandler<AddLocaleMessageCommand>
    {
        private IconContext context;

        public AddLocaleMessageCommandHandler(IconContext context)
        {
            this.context = context;
        }

        public void Execute(AddLocaleMessageCommand data)
        {
            MessageQueueLocale messageQueueLocale = Mapper.Map<MessageQueueLocale>(data.Locale);
            messageQueueLocale.MessageTypeId = MessageTypes.Locale;
            messageQueueLocale.MessageStatusId = MessageStatusTypes.Ready;
            messageQueueLocale.InsertDate = DateTime.Now;

            messageQueueLocale.InProcessBy = null;
            messageQueueLocale.ProcessedDate = null;

            context.MessageQueueLocale.Add(messageQueueLocale);
            context.SaveChanges();
        }
    }
}