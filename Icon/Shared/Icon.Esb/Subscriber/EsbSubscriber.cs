using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Icon.Esb.Subscriber
{
    public class EsbSubscriber : EsbConnection, IEsbSubscriber
    {
        private MessageConsumer consumer;

        public event EventHandler<EsbMessageEventArgs> MessageReceived;

        public EsbSubscriber(EsbConnectionSettings settings) : base(settings) { }

        public override void OpenConnection()
        {
            base.OpenConnection();
            consumer = session.CreateConsumer(destination);
        }

        public void BeginListening()
        {
            BeginListening("");
        }
        public void BeginListening(string clientId)
        {
            consumer.MessageHandler += messagConsumer_MessageHandler;
            connection.ClientID = clientId;
            connection.Start();
        }

        private void messagConsumer_MessageHandler(object sender, EMSMessageEventArgs args)
        {
            EsbMessageEventArgs message = new EsbMessageEventArgs
            {
                Message = new EsbMessage(args.Message as TextMessage)
            };

            var handler = MessageReceived;
            if (handler != null)
            {
                handler(sender, message);
            }
        }
        public void SetTibcoClientId(string clientId)
        {
            connection.ClientID = clientId;
        }
        internal void HandleMessage(Message message)
        {
            EsbMessageEventArgs esbMessage = new EsbMessageEventArgs
            {
                Message = new EsbMessage(message as TextMessage)
            };

            var handler = MessageReceived;
            if (handler != null)
            {
                handler(this, esbMessage);
            }
        }

        public override void Dispose()
        {
            if (consumer != null)
                consumer.Close();
            MessageReceived = null;
            base.Dispose();
        }
    }
}
