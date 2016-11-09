using System;
using System.Text;
using TIBCO.EMS;

namespace Icon.Esb.Subscriber
{
    public class EsbMessage : IEsbMessage
    {
        private TextMessage tibcoMessage;

        public string MessageText { get; set; }

        public EsbMessage(TextMessage tibcoMessage)
        {
            this.tibcoMessage = tibcoMessage;
            this.MessageText = tibcoMessage.Text;
        }

        public void Acknowledge()
        {
            tibcoMessage.Acknowledge();
        }

        public string GetProperty(string propertyName)
        {
            if (tibcoMessage.PropertyExists(propertyName))
            {
                return tibcoMessage.GetStringProperty(propertyName);
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            return tibcoMessage.ToString();
        }
    }
}