using AutoMapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using System;

namespace Icon.Web.DataAccess.Commands
{
    public class AddLocaleMessageCommandHandler : ICommandHandler<AddLocaleMessageCommand>
    {
        private IconContext context;
        private IMapper mapper;

        public AddLocaleMessageCommandHandler(IconContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public void Execute(AddLocaleMessageCommand data)
        {
            MessageQueueLocale messageQueueLocale = mapper.Map<MessageQueueLocale>(data.Locale);
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