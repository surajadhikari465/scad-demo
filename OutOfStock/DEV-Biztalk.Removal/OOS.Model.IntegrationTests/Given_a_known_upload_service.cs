using Magnum.TestFramework;
using MassTransit;
using OOS.Model.UnitTests;
using OOSCommon;
using Rhino.Mocks;
using StructureMap;

namespace OOS.Model.IntegrationTests
{
    [Scenario]
    public abstract class Given_a_known_upload_service
    {
        protected KnownUploadService sut;

        [Given]
        public void Setup()
        {
            Bootstrap();
            sut = CreateObjectUnderTest();
            Given();
        }

        private void Bootstrap()
        {
            ObjectFactory.Configure(config =>
            {
                config.For<ICreateKnownUploader>().Use<KnownUploaderFactory>();
                config.For<ILogService>().Use<LogService>();
                config.For<IConfigure>().Use(MockConfigurator.New);
                config.For<IConfigurator>().Use(MockConfigurator.New());
                config.For<IServiceBus>().Use(MockRepository.GenerateStub<IServiceBus>());
            });
        }

        private KnownUploadService CreateObjectUnderTest()
        {
            return ObjectFactory.GetInstance<KnownUploadService>();
        }

        protected abstract void Given();
    }
}
