using Icon.Esb;
using Icon.Esb.Subscriber;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubscriberDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            EsbSubscriber subscriber = new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig());
            subscriber.OpenConnection();
            subscriber.MessageReceived += subscriber_MessageReceived;
            subscriber.BeginListening();

            while (true) { };
        }

        static void subscriber_MessageReceived(object sender, EsbMessageEventArgs e)
        {
            Console.WriteLine(e.Message.MessageText);
        }
    }
}
