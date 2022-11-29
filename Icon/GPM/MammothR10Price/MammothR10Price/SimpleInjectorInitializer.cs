using Icon.Common.Email;
using SimpleInjector;
using MammothR10Price.Publish;

namespace MammothR10Price
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer(int instance)
        {
            var container = new Container();
            container.Register<IEmailClient>(() => { return EmailClient.CreateFromConfig(); }, Lifestyle.Singleton);
            container.Register<IErrorEventPublisher, ErrorEventPublisher>();
            
            return container;
        }
    }
}
