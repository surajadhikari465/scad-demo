﻿using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Icon.Esb.Producer
{
    public class EsbProducer : EsbConnection, IEsbProducer
    {
        private MessageProducer producer;

        public EsbProducer(EsbConnectionSettings settings) : base(settings) { }

        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            TextMessage textMessage = session.CreateTextMessage();
            textMessage.Text = message;

            if (messageProperties != null)
            {
                foreach (var property in messageProperties)
                {
                    textMessage.SetStringProperty(property.Key, property.Value);
                }
            }

            producer.Send(textMessage);
        }

        public override void OpenConnection()
        {
            base.OpenConnection();
            producer = session.CreateProducer(destination);
            producer.DeliveryMode = DeliveryMode.PERSISTENT;
        }

        public override void Dispose()
        {
            if (producer != null)
            {
                producer.Close();
            }

            base.Dispose();
        }
    }
}
