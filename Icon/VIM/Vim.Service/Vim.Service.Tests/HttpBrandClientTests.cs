namespace Vim.Service.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Vim.Service.Brand.HttpClient;

    [TestClass]
    public class HttpBrandClientTests
    {
        private IVimBrandClient client;

        [TestInitialize]
        public void Initialize()
        {
            this.client = new HttpBrandClient();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void HttpBrandClient_WhenSendBrandsToVim_GivenNewValidBrand_ShouldReturnTrue()
        {
            // Given
            var testBrand = new VimBrand
            {
                IconId = 0,
                Name = "New Test Brand",
                Abbreviation = "NTB"               
            };

            // When
            var sendBrandsTask = this.client.SendBrandsToVimAsync(new List<VimBrand> { testBrand });
            var response = sendBrandsTask.GetAwaiter().GetResult();

            // Then
            Assert.IsTrue(response.IsSuccessful);
        }
    }
}
