using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.ViewModels;

namespace WebSupport.Tests.ViewModels
{
    [TestClass]
    public class ViewModelTests
    {
        [TestMethod]
        public void PriceResetRequestViewModel_HasEmptyConstructor()
        {
            //Arrange
            //Act
            var viewModel = new PriceResetRequestViewModel();
            //Assert
            Assert.IsNotNull(viewModel);
        }

        [TestMethod]
        public void PriceResetRequestViewModel_SetsInitialStoreOption_WithPrompt()
        {
            //Arrange
            var expected = "- select region first -";
            //Act
            var viewModel = new PriceResetRequestViewModel();
            //Assert
            Assert.AreEqual(expected, viewModel.OptionsForStores.ElementAt(0).Text);
            Assert.AreEqual("0", viewModel.OptionsForStores.ElementAt(0).Value);
        }

        [TestMethod]
        public void PriceResetRequestViewModel_SetRegionAndSystemOptions_CanSetRegionOptions()
        {
            //Arrange
            var regionSelections = new List<string> { "AA", "BB", "CC", "DD" };
            var viewModel = new PriceResetRequestViewModel();
            //Act
            viewModel.SetRegionAndSystemOptions(regionSelections, null);
            //Assert
            Assert.AreEqual(4, viewModel.OptionsForRegion.Count());
            Assert.IsInstanceOfType(viewModel.OptionsForRegion.ElementAt(3), typeof(SelectListItem));
            Assert.AreEqual("DD", viewModel.OptionsForRegion.ElementAt(3).Text);
        }

        [TestMethod]
        public void PriceResetRequestViewModel_CanInitializeWithSystemOptions()
        {
            var systemSelections = new List<string> { "MegaWarSim", "YETI" };
            var viewModel = new PriceResetRequestViewModel();
            //Act
            viewModel.SetRegionAndSystemOptions(null, systemSelections);
            //Assert
            Assert.AreEqual(2, viewModel.OptionsForDestinationSystem.Count());
            Assert.IsInstanceOfType(viewModel.OptionsForDestinationSystem.ElementAt(1), typeof(SelectListItem));
            Assert.AreEqual("YETI", viewModel.OptionsForDestinationSystem.ElementAt(1).Text);
        }

        [TestMethod]
        public void StoreViewModel_HasEmptyConstructor()
        {
            //Arrange
            //Act
            var viewModel = new StoreViewModel();
            //Assert
            Assert.IsNotNull(viewModel);
        }
        [TestMethod]

        public void StoreViewModel_CanInitializeWithTransferObject()
        {
            //Arrange
            var sto = new StoreTransferObject
            {
                Name = "My Store",
                BusinessUnit = "11111",
                Abbreviation = "MNS"
            };
            //Act
            var viewModel = new StoreViewModel(sto);
            //Assert
            Assert.AreEqual("My Store", viewModel.Name);
            Assert.AreEqual("11111", viewModel.BusinessUnit);
            Assert.AreEqual("MNS", viewModel.Abbreviation);
        }
    }
}
