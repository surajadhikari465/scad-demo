using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleInjector.Extensions;
using Icon.Esb.Producer;
using Icon.Framework;
using Contracts = Icon.Service.Library.ContractTypes;

namespace MessageGenerationAdmin
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container();

            container.Register<IPriceMessageService, PriceMessageService>();
            container.Register<ISerializer<Contracts.items>, Serializer<Contracts.items>>();
            container.Register<IEsbProducer>(() => new ConsoleEsbProducer(), Lifestyle.Transient);
            container.Register<IconContext>(Lifestyle.Singleton);

            var priceService = container.GetInstance<IPriceMessageService>();

            priceService.DeleteTpr("4011", 875, 11.1m, DateTime.Now);

            Console.ReadLine();
        }
    }

    class ConsoleEsbProducer : IEsbProducer
    {
        public void Send(string message, Dictionary<string, string> messageProperties = null)
        {
            "------------------- Writing Message ---------------------------".P();
            message.P();
        }

        public Icon.Esb.EsbConnectionSettings Settings
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsConnected
        {
            get { throw new NotImplementedException(); }
        }

        public event EventHandler<TIBCO.EMS.EMSException> ExceptionHandlers;

        public void OpenConnection()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

}
