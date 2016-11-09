using Icon.Esb;
using Icon.Esb.Producer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProducerDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            EsbProducer producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig());
            producer.OpenConnection();

            Console.WriteLine("Enter something to send to the ESB");
            var message = Console.ReadLine();

            while (!string.IsNullOrWhiteSpace(message))
            {
                producer.Send(message);
                message = Console.ReadLine();
            }
        }
    }
}
