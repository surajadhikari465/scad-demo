using Icon.Web.Common.Utility;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.Extensions;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Unit.Extensions
{
    [TestClass] [Ignore]
    public class ExtensionsTests
    {
        ItemSearchViewModel viewModel;

        [TestInitialize]
        public void Initialize()
        {
            viewModel = new ItemSearchViewModel();
        }

        [TestMethod]
        public void GetSearchParameters_ItemSearchModelWithNoSetProperties_ShouldPopulateSearchParameters()
        {
            //Given
            ItemSearchViewModel viewModel = new ItemSearchViewModel();

            //When
            var result = viewModel.GetSearchParameters(true, 10);

            //Then
            Assert.AreEqual(result.ScanCode, viewModel.ScanCode);
            Assert.AreEqual(result.BrandName, viewModel.BrandName);
            Assert.AreEqual(result.PartialBrandName, viewModel.PartialBrandName);
            Assert.AreEqual(result.ProductDescription, viewModel.ProductDescription);
            Assert.AreEqual(result.PosDescription, viewModel.PosDescription);
            Assert.AreEqual(result.RetailSize, viewModel.RetailSize);
            Assert.AreEqual(result.RetailUom, viewModel.SelectedRetailUom);
            Assert.AreEqual(result.DeliverySystem, viewModel.DeliverySystem);
            Assert.AreEqual(result.MerchandiseHierarchy, viewModel.MerchandiseHierarchy);
            Assert.AreEqual(result.TaxRomance, viewModel.TaxHierarchy);
            Assert.AreEqual(result.PosDescription, viewModel.PosDescription);
            Assert.AreEqual(result.SearchStatus, viewModel.Status.Single(s => s.Value == viewModel.SelectedStatusId.ToString()).Text.ToEnum<SearchStatus>());
            Assert.AreEqual(result.HiddenItemStatus, viewModel.HiddenStatus.Single(s => s.Value == viewModel.SelectedHiddenItemStatusId.ToString()).Text.ToEnum<HiddenStatus>());
            Assert.AreEqual(result.FoodStampEligible, viewModel.SelectedFoodStampId);
            Assert.AreEqual(result.DepartmentSale, viewModel.SelectedDepartmentSaleId);
            Assert.AreEqual(result.PosScaleTare, viewModel.PosScaleTare);
            Assert.AreEqual(result.PackageUnit, viewModel.PackageUnit);
            Assert.AreEqual(result.PartialScanCode, viewModel.PartialScanCode);
            Assert.AreEqual(result.NationalClass, viewModel.NationalHierarchy);
            Assert.AreEqual(result.AlcoholByVolume, viewModel.AlcoholByVolume);
            Assert.AreEqual(result.CaseinFree, viewModel.CaseinFree.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.DrainedWeight, viewModel.DrainedWeight);
            Assert.AreEqual(result.DrainedWeightUom, viewModel.DrainedWeightUom);
            Assert.AreEqual(result.FairTradeCertified, viewModel.FairTradeCertified);
            Assert.AreEqual(result.Hemp, viewModel.Hemp.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.LocalLoanProducer, viewModel.LocalLoanProducer.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.MainProductName, viewModel.MainProductName);
            Assert.AreEqual(result.NutritionRequired, viewModel.NutritionRequired.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.OrganicPersonalCare, viewModel.OrganicPersonalCare.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.Paleo, viewModel.Paleo.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.ProductFlavorType, viewModel.ProductFlavorType);
            Assert.AreEqual(result.AnimalWelfareRating, viewModel.ItemSignAttributes.AnimalWelfareRating);
            Assert.AreEqual(result.Biodynamic, viewModel.ItemSignAttributes.SelectedBiodynamicOption);
            Assert.AreEqual(result.MilkType, viewModel.ItemSignAttributes.CheeseMilkType);
            Assert.AreEqual(result.CheeseRaw, viewModel.ItemSignAttributes.SelectedCheeseRawOption);
            Assert.AreEqual(result.EcoScaleRating, viewModel.ItemSignAttributes.EcoScaleRating);
            Assert.AreEqual(result.GlutenFreeAgency, viewModel.ItemSignAttributes.GlutenFreeAgency);
            Assert.AreEqual(result.KosherAgency, viewModel.ItemSignAttributes.KosherAgency);
            Assert.AreEqual(result.Msc, viewModel.ItemSignAttributes.SelectedMscOption);
            Assert.AreEqual(result.NonGmoAgency, viewModel.ItemSignAttributes.NonGmoAgency);
            Assert.AreEqual(result.OrganicAgency, viewModel.ItemSignAttributes.OrganicAgency);
            Assert.AreEqual(result.PremiumBodyCare, viewModel.ItemSignAttributes.SelectedPremiumBodyCareOption);
            Assert.AreEqual(result.SeafoodFreshOrFrozen, viewModel.ItemSignAttributes.SeafoodFreshOrFrozen);
            Assert.AreEqual(result.SeafoodCatchType, viewModel.ItemSignAttributes.SeafoodCatchType);
            Assert.AreEqual(result.VeganAgency, viewModel.ItemSignAttributes.VeganAgency);
            Assert.AreEqual(result.Vegetarian, viewModel.ItemSignAttributes.SelectedVegetarianOption);
            Assert.AreEqual(result.WholeTrade, viewModel.ItemSignAttributes.SelectedWholeTradeOption);
            Assert.AreEqual(result.Notes, viewModel.Notes);
            Assert.AreEqual(result.GrassFed, viewModel.ItemSignAttributes.SelectedGrassFedOption);
            Assert.AreEqual(result.PastureRaised, viewModel.ItemSignAttributes.SelectedPastureRaisedOption);
            Assert.AreEqual(result.FreeRange, viewModel.ItemSignAttributes.SelectedFreeRangeOption);
            Assert.AreEqual(result.DryAged, viewModel.ItemSignAttributes.SelectedDryAgedOption);
            Assert.AreEqual(result.AirChilled, viewModel.ItemSignAttributes.SelectedAirChilledOption);
            Assert.AreEqual(result.MadeInHouse, viewModel.ItemSignAttributes.SelectedMadeInHouseOption);
            Assert.AreEqual(result.CreatedDate, Icon.Web.Extensions.Extensions.ConvertDateFormat(viewModel.CreatedDate));
            Assert.AreEqual(result.LastModifiedDate, Icon.Web.Extensions.Extensions.ConvertDateFormat(viewModel.LastModifiedDate));
            Assert.AreEqual(result.LastModifiedUser, viewModel.LastModifiedUser);
            Assert.AreEqual(result.PageIndex, viewModel.Page);
            Assert.AreEqual(result.PageSize, 10);
            Assert.IsTrue(result.GetCountOnly);
        }

        [TestMethod]
        public void GetSearchParameters_ItemSearchModelWithAllProperties_ShouldPopulateSearchParameters()
        {
            //Given
            int i = 0;
            foreach (var prop in typeof(ItemSearchViewModel).GetProperties())
            {
                if(prop.PropertyType == typeof(string))
                    prop.SetValue(viewModel, "test" + i);
                if(prop.PropertyType == typeof(int))
                    prop.SetValue(viewModel, i);
            }

            i = 0;
            foreach (var prop in typeof(ItemSignAttributesSearchViewModel).GetProperties())
            {
                if (prop.PropertyType == typeof(string))
                    prop.SetValue(viewModel.ItemSignAttributes, "test" + i);
                if (prop.PropertyType == typeof(int?))
                    prop.SetValue(viewModel.ItemSignAttributes, i);
            }

            viewModel.SelectedStatusId = (int)SearchStatus.Loaded;
            viewModel.SelectedHiddenItemStatusId = (int)HiddenStatus.Hidden;
            viewModel.CreatedDate = DateTime.Now.ToShortDateString();
            viewModel.LastModifiedDate = DateTime.Now.ToShortDateString();

            //When
            var result = viewModel.GetSearchParameters(true, 20);

            //Then
            Assert.AreEqual(result.ScanCode, viewModel.ScanCode);
            Assert.AreEqual(result.BrandName, viewModel.BrandName);
            Assert.AreEqual(result.PartialBrandName, viewModel.PartialBrandName);
            Assert.AreEqual(result.ProductDescription, viewModel.ProductDescription);
            Assert.AreEqual(result.PosDescription, viewModel.PosDescription);
            Assert.AreEqual(result.RetailSize, viewModel.RetailSize);
            Assert.AreEqual(result.RetailUom, viewModel.SelectedRetailUom);
            Assert.AreEqual(result.DeliverySystem, viewModel.DeliverySystem);
            Assert.AreEqual(result.MerchandiseHierarchy, viewModel.MerchandiseHierarchy);
            Assert.AreEqual(result.TaxRomance, viewModel.TaxHierarchy);
            Assert.AreEqual(result.PosDescription, viewModel.PosDescription);
            Assert.AreEqual(result.SearchStatus, viewModel.Status.Single(s => s.Value == viewModel.SelectedStatusId.ToString()).Text.ToEnum<SearchStatus>());
            Assert.AreEqual(result.HiddenItemStatus, viewModel.HiddenStatus.Single(s => s.Value == viewModel.SelectedHiddenItemStatusId.ToString()).Text.ToEnum<HiddenStatus>());
            Assert.AreEqual(result.FoodStampEligible, viewModel.SelectedFoodStampId);
            Assert.AreEqual(result.DepartmentSale, viewModel.SelectedDepartmentSaleId);
            Assert.AreEqual(result.PosScaleTare, viewModel.PosScaleTare);
            Assert.AreEqual(result.PackageUnit, viewModel.PackageUnit);
            Assert.AreEqual(result.PartialScanCode, viewModel.PartialScanCode);
            Assert.AreEqual(result.NationalClass, viewModel.NationalHierarchy);
            Assert.AreEqual(result.AlcoholByVolume, viewModel.AlcoholByVolume);
            Assert.AreEqual(result.CaseinFree, viewModel.CaseinFree.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.DrainedWeight, viewModel.DrainedWeight);
            Assert.AreEqual(result.DrainedWeightUom, viewModel.DrainedWeightUom);
            Assert.AreEqual(result.FairTradeCertified, viewModel.FairTradeCertified);
            Assert.AreEqual(result.Hemp, viewModel.Hemp.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.LocalLoanProducer, viewModel.LocalLoanProducer.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.MainProductName, viewModel.MainProductName);
            Assert.AreEqual(result.NutritionRequired, viewModel.NutritionRequired.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.OrganicPersonalCare, viewModel.OrganicPersonalCare.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.Paleo, viewModel.Paleo.ConvertYesNoToDatabaseValue());
            Assert.AreEqual(result.ProductFlavorType, viewModel.ProductFlavorType);
            Assert.AreEqual(result.AnimalWelfareRating, viewModel.ItemSignAttributes.AnimalWelfareRating);
            Assert.AreEqual(result.Biodynamic, viewModel.ItemSignAttributes.SelectedBiodynamicOption);
            Assert.AreEqual(result.MilkType, viewModel.ItemSignAttributes.CheeseMilkType);
            Assert.AreEqual(result.CheeseRaw, viewModel.ItemSignAttributes.SelectedCheeseRawOption);
            Assert.AreEqual(result.EcoScaleRating, viewModel.ItemSignAttributes.EcoScaleRating);
            Assert.AreEqual(result.GlutenFreeAgency, viewModel.ItemSignAttributes.GlutenFreeAgency);
            Assert.AreEqual(result.KosherAgency, viewModel.ItemSignAttributes.KosherAgency);
            Assert.AreEqual(result.Msc, viewModel.ItemSignAttributes.SelectedMscOption);
            Assert.AreEqual(result.NonGmoAgency, viewModel.ItemSignAttributes.NonGmoAgency);
            Assert.AreEqual(result.OrganicAgency, viewModel.ItemSignAttributes.OrganicAgency);
            Assert.AreEqual(result.PremiumBodyCare, viewModel.ItemSignAttributes.SelectedPremiumBodyCareOption);
            Assert.AreEqual(result.SeafoodFreshOrFrozen, viewModel.ItemSignAttributes.SeafoodFreshOrFrozen);
            Assert.AreEqual(result.SeafoodCatchType, viewModel.ItemSignAttributes.SeafoodCatchType);
            Assert.AreEqual(result.VeganAgency, viewModel.ItemSignAttributes.VeganAgency);
            Assert.AreEqual(result.Vegetarian, viewModel.ItemSignAttributes.SelectedVegetarianOption);
            Assert.AreEqual(result.WholeTrade, viewModel.ItemSignAttributes.SelectedWholeTradeOption);
            Assert.AreEqual(result.Notes, viewModel.Notes);
            Assert.AreEqual(result.GrassFed, viewModel.ItemSignAttributes.SelectedGrassFedOption);
            Assert.AreEqual(result.PastureRaised, viewModel.ItemSignAttributes.SelectedPastureRaisedOption);
            Assert.AreEqual(result.FreeRange, viewModel.ItemSignAttributes.SelectedFreeRangeOption);
            Assert.AreEqual(result.DryAged, viewModel.ItemSignAttributes.SelectedDryAgedOption);
            Assert.AreEqual(result.AirChilled, viewModel.ItemSignAttributes.SelectedAirChilledOption);
            Assert.AreEqual(result.MadeInHouse, viewModel.ItemSignAttributes.SelectedMadeInHouseOption);
            Assert.AreEqual(result.CreatedDate, Icon.Web.Extensions.Extensions.ConvertDateFormat(viewModel.CreatedDate));
            Assert.AreEqual(result.LastModifiedDate, Icon.Web.Extensions.Extensions.ConvertDateFormat(viewModel.LastModifiedDate));
            Assert.AreEqual(result.LastModifiedUser, viewModel.LastModifiedUser);
            Assert.AreEqual(result.PageIndex, viewModel.Page);
            Assert.AreEqual(result.PageSize, 20);
            Assert.IsTrue(result.GetCountOnly);
        }
    }
}