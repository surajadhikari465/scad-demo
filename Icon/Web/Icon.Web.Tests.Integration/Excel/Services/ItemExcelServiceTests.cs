using Icon.Framework;
using Icon.Web.Mvc.App_Start;
using Icon.Web.Mvc.Excel.Models;
using Icon.Web.Mvc.Excel.Services;
using Infragistics.Documents.Excel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Tests.Integration.Excel.Services
{
    [TestClass] [Ignore]
    public class ItemExcelServiceTests
    {
        private ItemExcelService excelService;

        [TestInitialize]
        public void Initialize()
        {
            Container container = new Container();
            SimpleInjectorInitializer.InitializeContainer(container);

            excelService = container.GetInstance<ItemExcelService>();
        }

        [TestMethod]
        public void Import_10000ItemSpreadsheetWithAllFieldsPopulated_ShouldImportSuccessfully()
        {
            //Given
            var workbook = Workbook.Load(@"TestDocs\Items_10000.xlsx");

            //When
            var response = excelService.Import(workbook);

            //Then
            Assert.IsNull(response.ErrorMessage);
        }

        [TestMethod]
        public void Export_10Items_ShouldExportAWorkbookWith10Items()
        {
            //Given
            List<ItemExcelModel> items = CreateTestItemModels(10);

            //When
            var response = excelService.Export(new ExportRequest<ItemExcelModel> { Rows = items });

            //Then
            var workbook = response.ExcelWorkbook;
            workbook.SetCurrentFormat(WorkbookFormat.Excel2007);
            workbook.Save("itemexport test.xlsx");
            //Assert.AreEqual(11, workbook.Worksheets[0].Rows.Count(r => !r.Cells.Any()));
        }

        private List<ItemExcelModel> CreateTestItemModels(int count)
        {
            List<ItemExcelModel> models = new List<ItemExcelModel>();
            for (int i = 0; i < count; i++)
            {
                models.Add(new ItemExcelModel
                {
                    ScanCode = "1234" + i,
                    Brand = "Test Brand|10",
                    ProductDescription = "Test",
                    PosDescription = "Test",
                    PackageUnit = "1",
                    FoodStampEligible = "Y",
                    PosScaleTare = "1",
                    RetailSize = "1",
                    Uom = UomCodes.Each,
                    DeliverySystem = DeliverySystems.AsDictionary.Values.First(),
                    Merchandise = "Test Merchandise|10",
                    NationalClass = "Test NationalClass|10",
                    Tax = "Test Tax|10",
                    Browsing = "Test Browsing|10",
                    Validated = "Y",
                    DepartmentSale = "Y",
                    HiddenItem = "N",
                    Notes = "Test",
                    AnimalWelfareRating = AnimalWelfareRatings.Descriptions.Step1,
                    Biodynamic = "Y",
                    CheeseAttributeMilkType = MilkTypes.Descriptions.BuffaloMilk,
                    CheeseAttributeRaw = "N",
                    EcoScaleRating = EcoScaleRatings.Descriptions.BaselineOrange,
                    GlutenFree = "Test GlutenFree|10",
                    Kosher = "Test Kosher|10",
                    Msc = "N",
                    NonGmo = "Test NonGmo|10",
                    Organic = "Test Organic|10",
                    PremiumBodyCare = "Y",
                    SeafoodFreshOrFrozen = SeafoodFreshOrFrozenTypes.Descriptions.Fresh,
                    SeafoodWildOrFarmRaised = SeafoodCatchTypes.Descriptions.FarmRaised,
                    Vegan = "Test Vegan|10",
                    Vegetarian = "Y",
                    WholeTrade = "Y",
                    GrassFed = "Y",
                    PastureRaised = "Y",
                    FreeRange = "Y",
                    DryAged = "Y",
                    AirChilled = "Y",
                    MadeInHouse = "Y",
                    CreatedDate = DateTime.Now.ToString(),
                    LastModifiedDate = DateTime.Now.ToString(),
                    LastModifiedUser = "TestUser"
                });
            }
            return models;
        }
    }
}
