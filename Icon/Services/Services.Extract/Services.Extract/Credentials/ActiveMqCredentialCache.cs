using System;
using System.Collections.Generic;
using System.Linq;
using Services.Extract.Config;

namespace Services.Extract.Credentials
{
    public class ActiveMqCredentialCache: IActiveMqCredentialsCache
    {
        public Dictionary<string, ActiveMqCredential> Credentials { get; set; }
        public void Refresh()
        {
            Credentials = ActiveMqCredentialConfigSection.Config.SettingsList.ToDictionary(d => d.ProfileName, d => new ActiveMqCredential(
                d.ServerUrl, d.JmsUsername, d.JmsPassword, d.QueueName, d.SessionMode, d.ReconnectDelay, d.TransactionType, d.TransactionId, d.MessageType));
        }
    }
}
