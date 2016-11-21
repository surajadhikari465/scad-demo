using Icon.Esb;
using Icon.Esb.ConfigReader;
using System;
using TIBCO.EMS;

namespace Icon.Infor.Listeners.HierarchyClass.EsbService
{
    public class VimEsbConnectionSettings : EsbConnectionSettings
    {
        public VimEsbConnectionSettings()
        {
            var connectionConfiguration = EsbConnectionConfigReader.GetConfig().Connections["VIM"];

            ServerUrl = connectionConfiguration.ServerUrl;
            JmsUsername = connectionConfiguration.JmsUsername;
            JmsPassword = connectionConfiguration.JmsPassword;
            QueueName = connectionConfiguration.QueueName;
            SessionMode = (SessionMode)Enum.Parse(typeof(SessionMode), connectionConfiguration.SessionMode);
        }
    }
}