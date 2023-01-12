using System;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.Model;
using Icon.Dvs;
using Icon.Dvs.Subscriber;
using Icon.Common.Email;
using Icon.Logging;
using IrmaPriceListenerService.DataAccess;
using IrmaPriceListenerService.Archive;

namespace IrmaPriceListenerService.Listener
{
    public class IrmaPriceListener : ListenerApplication<IrmaPriceListener>
    {
        private IIrmaPriceDAL irmaPriceDAL;
        private IMessageArchiver messageArchiver;

        public IrmaPriceListener(
            DvsListenerSettings settings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<IrmaPriceListener> logger,
            IIrmaPriceDAL irmaPriceDAL,
            IMessageArchiver messageArchiver
        ): base(settings, subscriber, emailClient, logger)
        {
            this.irmaPriceDAL = irmaPriceDAL;
            this.messageArchiver = messageArchiver;
        }

        public override void HandleMessage(DvsMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
