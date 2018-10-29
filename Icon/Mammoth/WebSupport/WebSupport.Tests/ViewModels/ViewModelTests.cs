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

        [TestMethod]
        public void CheckPointRequestViewModel_Constructor_SetsInitialStoreOption()
        {
            //Act
            var viewModel = new CheckPointRequestViewModel();

            //Assert
            Assert.IsNotNull(viewModel.OptionsForStores);
            Assert.AreEqual("- Select Region First -", viewModel.OptionsForStores.Single().Text);
        }

        [TestMethod]
        public void CheckPointRequestViewModel_Constructor_InitializesRegionOptions()
        {
            //Act
            var viewModel = new CheckPointRequestViewModel();

            //Assert
            Assert.IsNotNull(viewModel.OptionsForRegion);
        }

        [TestMethod]
        public void CheckPointRequestViewModel_SetRegions_ConvertsRegionStringsToSelectListItems()
        {
            //Arrange
            var regions = new List<string> { "AA", "BB", "CC", "DD" };
            var viewModel = new CheckPointRequestViewModel();

            //Act
            viewModel.SetRegions(regions);

            //Assert
            Assert.IsNotNull(viewModel.OptionsForRegion);
            Assert.AreEqual(regions.Count, viewModel.OptionsForRegion.Count());
            for (int i=0; i< regions.Count; i++)
            {
                Assert.AreEqual(regions[i], viewModel.OptionsForRegion.FirstOrDefault(o=>o.Text==regions[i]).Text);
            }
        }

        [TestMethod]
        public void CheckPointRequestViewModel_SetStoreOptions_ConvertsStoreViewModelsToSelectListItems()
        {
            //Arrange
            var stores = new List<StoreViewModel> {
                new StoreViewModel() { Name="store A", BusinessUnit = "11111", Abbreviation = "AAA" },
                new StoreViewModel() { Name="store B", BusinessUnit = "22222", Abbreviation = "BBB" },
                new StoreViewModel() { Name="store C", BusinessUnit = "33333", Abbreviation = "CCC" }
             };

            var viewModel = new CheckPointRequestViewModel();

            //Act
            viewModel.SetStoreOptions(stores);

            //Assert
            Assert.IsNotNull(viewModel.OptionsForStores);
            Assert.AreEqual(stores.Count, viewModel.OptionsForStores.Count());
            for (int i = 0; i < stores.Count; i++)
            {
                var expectedText = $"{stores[i].BusinessUnit}: {stores[i].Name}";
                var selectListOption = viewModel.OptionsForStores.FirstOrDefault(o => o.Text == expectedText);
                Assert.AreEqual(expectedText, selectListOption.Text);
                Assert.AreEqual(stores[i].BusinessUnit, selectListOption.Value);
            }
        }

        [TestMethod]
        public void CheckPointRequestViewModel_ScanCodesList_SplitsInputStrringIntoScanCodes()
        {
            //Arrange
            var viewModel = new CheckPointRequestViewModel();

            //Act
            viewModel.ScanCodes = $"11111111{Environment.NewLine}222222222{Environment.NewLine}333333{Environment.NewLine}444444444{Environment.NewLine}5555555{Environment.NewLine}";

            //Assert
            Assert.IsNotNull(viewModel.ScanCodesList);
            Assert.AreEqual(5, viewModel.ScanCodesList.Count);
        }

        [TestMethod]
        public void CheckPointRequestViewModel_StoresAsIntList_ConvertsStringsToIntList()
        {
            //Arrange
            var viewModel = new CheckPointRequestViewModel();

            //Act
            viewModel.Stores = new string[] { "10001", "10010", "10100" };

            //Assert
            Assert.IsNotNull(viewModel.StoresAsIntList);
            Assert.AreEqual(3, viewModel.StoresAsIntList.Count);
        }

    }
}
