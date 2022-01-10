using Icon.Services.ItemPublisher.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Tests
{
    [TestClass]
    public class SystemListServiceTests
    {
        /// <summary>
        /// Tests that R10 is excluded from the ESB message for non retail items
        /// </summary>
        [TestMethod]
        public void BuildNonReceivingSystem_NonRetailItem_R10IsExcluded()
        {
            //Given.
            var service = new SystemListBuilder(new ServiceSettings());

            // When.
            List<string> nonReceivingSystems = service.BuildNonRetailReceivingSystemsList();

            // Then
            Assert.AreEqual("R10", nonReceivingSystems.First());
        }

        /// <summary>
        /// Tests that R10 is excluded from the ESB message for non retail items and also globally excluded items are excluded
        /// </summary>
        [TestMethod]
        public void BuildNonReceivingSystem_NonRetailItemAndOtherExcluded_R10AndOthersAreExcluded()
        {
            //Given.
            var service = new SystemListBuilder(new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "A"
                }
            });

            // When.
            List<string> nonReceivingSystems = service.BuildNonRetailReceivingSystemsList();

            // Then
            Assert.AreEqual("A", nonReceivingSystems[0]);
            Assert.AreEqual("R10", nonReceivingSystems[1]);
        }

        /// <summary>
        /// Tests that R10 is not excluded for retail messaged and no global exlcusions are applied if there are none in the config
        /// </summary>
        [TestMethod]
        public void BuildNonReceivingSystem_RetailItemNoExclusions_NoSystemsShouldBeExcluded()
        {
            //Given.
            var service = new SystemListBuilder(new ServiceSettings());

            // When.
            List<string> nonReceivingSystems = service.BuildRetailNonReceivingSystemsList();

            // Then
            Assert.AreEqual(0, nonReceivingSystems.Count);
        }

        /// <summary>
        /// Tests that R10 is not excluded for retail messaged and exlcusions are applied if there are any in the config
        /// </summary>
        [TestMethod]
        public void BuildNonReceivingSystem_RetailItemGlobalExclusions_GlobalSystemsShouldBeExcluded()
        {
            //Given.
            var service = new SystemListBuilder(new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "A"
                }
            });

            // When.
            List<string> nonReceivingSystems = service.BuildRetailNonReceivingSystemsList();

            // Then
            Assert.AreEqual(1, nonReceivingSystems.Count);
            Assert.AreEqual("A", nonReceivingSystems.First());
        }

        /// <summary>
        /// Tests that if R10 is exlcuded globally and a non retail item is processed that R10 is excluded once
        /// </summary>
        [TestMethod]
        public void BuildNonReceivingSystem_NonRetailAndAlsoR10GloballyExcluded_R10IsExcludedOnlyOnce()
        {
            // Given.
            var service = new SystemListBuilder(new ServiceSettings()
            {
                NonReceivingSystemsProduct = new List<string>()
                {
                    "R10"
                }
            });

            // When.
            List<string> nonReceivingSystems = service.BuildNonRetailReceivingSystemsList();

            // Then
            Assert.AreEqual(1, nonReceivingSystems.Count);
            Assert.AreEqual("R10", nonReceivingSystems.First());
        }       
    }
}