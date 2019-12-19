using AttributePublisher.Infrastructure;
using AttributePublisher.Services;
using NLog;

namespace AttributePublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            LogManager.ThrowExceptions = true;
            TopShelfService<AttributePublisherService>
                .Default(SimpleInjectorInitializer.Initialize())
                .Run();
        }
    }
}
