using Icon.Esb;
using Icon.Esb.Producer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infor.Services.NewItem.Tests.Integration
{
    [TestClass]
    public class InvalidMessageTests
    {
        [Ignore] //Only used for Assembly testing with Infor, should be removed after Assembly testing
        [TestMethod]
        public void InvalidMessageToEsb()
        {
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("QueueName"));
            producer.OpenConnection();
            var message = "This message is Invalid and should not go through the ESB.";

            producer.Send(message, new Dictionary<string, string> { { "IconMessageID", 12345.ToString() } });
        }
    }
}
