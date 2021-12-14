using System;
using Apache.NMS.ActiveMQ;
using Apache.NMS;


namespace Icon.ActiveMQ
{
    public class ActiveMQConnection: IActiveMQConnection
    {
        protected ConnectionFactory factory;
        protected Connection connection;
        protected Session session;
        protected IDestination destination;

        public string ClientId {
            get => connection.ClientId;
            set => connection.ClientId = value;
        }

        public ActiveMQConnectionSettings Settings { get; set; }

        public bool IsConnected
        {
            get
            {
                return connection != null;
            }
        }

        public ActiveMQConnection(ActiveMQConnectionSettings settings)
        {
            Settings = settings;
        }

        /// <summary>
        /// Opens a ActiveMQ connection with the given clientID
        /// </summary>
        /// <param name="clientId">An identifier to identify the specific connection. Using
        /// existing open connection's clientID will throw Exception</param>
        public virtual void OpenConnection(string clientId)
        {
            Uri serverUri = new Uri(Settings.ServerUrl);
            factory = new ConnectionFactory(serverUri);

            // Will revert connection to null, if any Exception arises
            try
            {
                connection = (Connection)factory.CreateConnection(Settings.JmsUsername, Settings.JmsPassword);
                connection.ClientId = clientId;

                session = (Session)connection.CreateSession(Settings.SessionMode);
                GetDestination();
            }
            catch(Exception)
            {
                this.Dispose();
                throw;
            }
        }

        protected virtual void GetDestination()
        {
            if (Settings.DestinationType.ToLower().Contains("topic"))
            {
                destination = session.GetTopic(Settings.QueueName);
            }
            else
            {
                destination = session.GetQueue(Settings.QueueName);
            }
        }

        /// <summary>
        /// Closes the ActiveMQ Connection
        /// </summary>
        public virtual void Dispose()
        {
            if(session != null)
            {
                session.Close();
            }
            if(connection != null)
            {
                connection.Close();
            }
            session = null;
            connection = null;
        }

    }
}
