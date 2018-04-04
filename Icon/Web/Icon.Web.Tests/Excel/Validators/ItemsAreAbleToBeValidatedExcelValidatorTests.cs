using Icon.Web.Mvc.Excel.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Queries;
using Moq;
using Icon.Web.Mvc.Excel.Models;

namespace Icon.Web.Tests.Unit.Excel.Validators
{
    [TestClass] [Ignore]
    public class ItemsAreAbleToBeValidatedExcelValidatorTests
    {
        private Mock<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>> mockGetScanCodesNotReadyToValidateQuery;
        private ItemsAreAbleToBeValidatedExcelValidator validator;

        [TestInitialize]
        public void Initialize()
        {
            mockGetScanCodesNotReadyToValidateQuery = new Mock<IQueryHandler<GetScanCodesNotReadyToValidateParameters, List<string>>>();
            validator = new ItemsAreAbleToBeValidatedExcelValidator(mockGetScanCodesNotReadyToValidateQuery.Object);
        }

        [TestMethod]
        public void ItemsAreAbleToBeValidated_ItemsAreAbleToBeValidated_ShouldNotSetError()
        {
            //Given
            List<ItemExcelModel> models = new List<ItemExcelModel>
            {
                new ItemExcelModel { ScanCode = "12341" },
                new ItemExcelModel { ScanCode = "12342" },
                new ItemExcelModel { ScanCode = "12343" },
                new ItemExcelModel { ScanCode = "12344" },
                new ItemExcelModel { ScanCode = "12345" },
            };
            mockGetScanCodesNotReadyToValidateQuery.Setup(m => m.Search(It.IsAny<GetScanCodesNotReadyToValidateParameters>()))
                .Returns(new List<string>());

            //When
            validator.Validate(models);

            //Then
            foreach (var model in models)
            {
                Assert.IsNull(model.Error);
            }
        }

        [TestMethod]
        public void ItemsAreAbleToBeValidated_ItemsAreNotAbleToBeValidated_ShouldSetError()
        {
            //Given
            List<ItemExcelModel> models = new List<ItemExcelModel>
            {
                new ItemExcelModel { ScanCode = "12341" },
                new ItemExcelModel { ScanCode = "12342" },
                new ItemExcelModel { ScanCode = "12343" },
                new ItemExcelModel { ScanCode = "12344" },
                new ItemExcelModel { ScanCode = "12345" },
            };
            mockGetScanCodesNotReadyToValidateQuery.Setup(m => m.Search(It.IsAny<GetScanCodesNotReadyToValidateParameters>()))
                .Returns(models.Select(m => m.ScanCode).ToList());

            //When
            validator.Validate(models);

            //Then
            foreach (var model in models)
            {
                Assert.IsNotNull(model.Error);
            }
        }

        [TestMethod]
        public void ItemsAreAbleToBeValidated_SomeItemsAreNotAbleToBeValidated_ShouldSetErrorItemsNotReadyToBeValidated()
        {
            //Given
            List<ItemExcelModel> models = new List<ItemExcelModel>
            {
                new ItemExcelModel { ScanCode = "12341" },
                new ItemExcelModel { ScanCode = "12342" },
                new ItemExcelModel { ScanCode = "12343" },
                new ItemExcelModel { ScanCode = "12344" },
                new ItemExcelModel { ScanCode = "12345" },
            };
            mockGetScanCodesNotReadyToValidateQuery.Setup(m => m.Search(It.IsAny<GetScanCodesNotReadyToValidateParameters>()))
                .Returns(models
                    .Skip(1)
                    .Take(2)
                    .Select(m => m.ScanCode)
                    .ToList());

            //When
            validator.Validate(models);

            //Then
            Assert.IsNull(models[0].Error);
            Assert.IsNotNull(models[1].Error);
            Assert.IsNotNull(models[2].Error);
            Assert.IsNull(models[3].Error);
            Assert.IsNull(models[4].Error);
        }
    }
}
