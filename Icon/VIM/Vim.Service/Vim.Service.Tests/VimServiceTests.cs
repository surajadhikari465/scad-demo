namespace Vim.Service.Tests
{
    using Icon.Common.Email;
    using Icon.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Vim.Service.Models;

    [TestClass]
    public class VimServiceTests
    {
        private Mock<ILogger<VimService>> mockLogger;
        private Mock<IVimBrandClient> mockClient;
        private Mock<IEmailClient> mockEmail;
        private IVimService vimService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger<VimService>>();
            this.mockClient = new Mock<IVimBrandClient>();
            this.mockEmail = new Mock<IEmailClient>();

            this.vimService = new VimService(
                this.mockClient.Object,
                this.mockLogger.Object,
                this.mockEmail.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void VimService_WhenInitializeContainer_GivenHttpClient_ShouldInjectHttpClient()
        {
            // Given
            var expectedImplementationName = "HttpBrandClient";

            // When
            var container = SimpleInjectorInitializer.InitializeContainer();

            // Then
            string actualImplementationTypeName = container.GetInstance<IVimBrandClient>().GetType().Name;
            Assert.AreEqual(expectedImplementationName, actualImplementationTypeName);                        
        }

        [TestMethod]
        public void VimService_WhenSendBrands_GivenAnyBrand_ShouldLogBrandsAsJson()
        {
            // Given
            var expectedJson = "[{\"IconId\":0,\"Name\":\"Test Brand Name\",\"Abbreviation\":\"TBN\"}]";
            var testBrand = new VimBrand
            {
                IconId = 0,
                Name = "Test Brand Name",
                Abbreviation = "TBN"
            };

            this.mockClient.Setup(c => c.SendBrandsToVimAsync(It.IsAny<IEnumerable<VimBrand>>()))
                .Returns(() => Task.Run(() => new VimBrandResponse { IsSuccessful = true }));

            // When
            this.vimService.SendBrandsAsync(
                new VimBrandRequest { Brands = new List<VimBrand> { testBrand } })
                    .GetAwaiter()
                    .GetResult();

            // Then
            this.mockLogger.Verify(
                l => l.Info(It.Is<string>(
                    msg => msg.Equals(expectedJson, System.StringComparison.InvariantCultureIgnoreCase))), 
                Times.Once);
        }

        [TestMethod]
        public void VimService_WhenSendBrands_GivenInvalidBrand_ShouldLogFailure()
        {
            // Given
            var testBrand = new VimBrand
            {
                IconId = 0,
                Name = "Test Brand Name",
                Abbreviation = "TBN"
            };

            this.mockClient.Setup(c => c.SendBrandsToVimAsync(It.IsAny<IEnumerable<VimBrand>>()))
                .Returns(() => Task.Run(() => new VimBrandResponse { IsSuccessful = false }));

            // When
            this.vimService.SendBrandsAsync(
                new VimBrandRequest { Brands = new List<VimBrand> { testBrand } })
                    .GetAwaiter()
                    .GetResult();

            // Then
            this.mockLogger.Verify(
                l => l.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void VimService_WhenSendsBrands_GivenInvalidBrand_ShouldSendEmail()
        {
            // Given
            var testBrand = new VimBrand
            {
                IconId = 0,
                Name = "Test Brand Name",
                Abbreviation = "TBN"
            };

            this.mockClient.Setup(c => c.SendBrandsToVimAsync(It.IsAny<IEnumerable<VimBrand>>()))
                .Returns(() => Task.Run(() => new VimBrandResponse { IsSuccessful = false }));

            // When
            this.vimService.SendBrandsAsync(
                new VimBrandRequest { Brands = new List<VimBrand> { testBrand } })
                    .GetAwaiter()
                    .GetResult();

            // Then
            this.mockEmail.Verify(
                l => l.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
