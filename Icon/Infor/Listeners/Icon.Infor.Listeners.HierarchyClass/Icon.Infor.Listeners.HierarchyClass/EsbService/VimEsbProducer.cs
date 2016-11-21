using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Esb;
using TIBCO.EMS;

namespace Icon.Infor.Listeners.HierarchyClass.EsbService
{
    public class VimEsbProducer : IEsbProducer
    {
        private Session session;
        private Connection connection;
        private Destination destination;
        private MessageProducer producer;

        public bool IsConnected
        {
            get
            {
                return connection != null && !connection.IsClosed;
            }
        }

        public EsbConnectionSettings Settings { get; set; }

        public event EventHandler<EMSException> ExceptionHandlers;

        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage(message);

            Send(textMessage, messageProperties);
        }

        public void Send(string message, string messageId, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage(message);
            textMessage.MessageID = messageId;

            Send(textMessage, messageProperties);
        }

        private void Send(TextMessage textMessage, Dictionary<string, string> messageProperties = null)
        {
            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            producer.Send(destination, textMessage);
        }

        public void OpenConnection()
        {
            ConnectionFactory factory = new ConnectionFactory(Settings.ServerUrl);
            connection = factory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword);
            session = connection.CreateSession(false, Settings.SessionMode);
            destination = session.CreateQueue(Settings.QueueName);
            producer = session.CreateProducer(null);
        }

        private void ConnectionExceptionHandler(object sender, EMSExceptionEventArgs args)
        {
            EMSException exception = args.Exception;

            var handler = ExceptionHandlers;
            if (handler != null)
            {
                handler(sender, exception);
            }
        }

        public void Dispose()
        {
            if (session != null && !session.IsClosed)
            {
                session.Close();
            }

            if (connection != null && !connection.IsClosed)
            {
                connection.Close();
            }

            ExceptionHandlers = null;
        }
    }
}
