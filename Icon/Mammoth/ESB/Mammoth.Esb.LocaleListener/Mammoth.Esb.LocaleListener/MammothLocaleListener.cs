using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.Models;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.LocaleListener
{
    public class MammothLocaleListener : ListenerApplication<MammothLocaleListener>
    {
        private IMessageParser<List<LocaleModel>> messageParser;
        private ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler;

        public MammothLocaleListener(
            DvsListenerSettings listenerSettings,
            IDvsSubscriber subscriber, 
            IEmailClient emailClient,
            ILogger<MammothLocaleListener> logger,
            IMessageParser<List<LocaleModel>> messageParser,
            ICommandHandler<AddOrUpdateLocalesCommand> addOrUpdateLocalesCommandHandler)
            :base(listenerSettings, subscriber, emailClient, logger)
        {
            this.messageParser = messageParser;
            this.addOrUpdateLocalesCommandHandler = addOrUpdateLocalesCommandHandler;
        }

        public override void HandleMessage(DvsMessage message)
        {
            // Not handling Exceptions, since it'll be taken care by base class
            List<LocaleModel> locales = messageParser.ParseMessage(message);
            if (locales != null && locales.Any())
            {
                addOrUpdateLocalesCommandHandler.Execute(new AddOrUpdateLocalesCommand { Locales = locales });
            }
        }
    }
}
