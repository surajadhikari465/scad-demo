using GPMService.Producer.Helpers;
using GPMService.Producer.Service;
using GPMService.Producer.Service.ESB.Listener;
using SimpleInjector;
using System;

namespace GPMService.Producer
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(int instance, string serviceType)
        {
            Container container = new Container();
            RegisterServiceImplementation(container, serviceType);
            return container;
        }

        private static void RegisterServiceImplementation(Container container, string serviceType)
        {
            switch (serviceType)
            {
                case Constants.ProducerType.NearRealTime:
                    container.Register<IGPMProducerService, NearRealTimeProducerService>();
                    break;
                default:
                    throw new ArgumentException(
                        $"No type implementation exists for service type argument {serviceType}");
            }
        }
    }
}
