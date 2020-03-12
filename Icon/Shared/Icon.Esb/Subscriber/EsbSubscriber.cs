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

        public override void OpenConnection(string clientId)
        {
            base.OpenConnection(clientId);
            consumer = session.CreateConsumer(destination);
        }

        public void BeginListening()
        {
            consumer.MessageHandler += messagConsumer_MessageHandler;
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
