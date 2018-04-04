using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.Mvc.Excel.Validators;
using Moq;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using Icon.Framework;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Excel.Models;
using System.Linq;

namespace Icon.Web.Tests.Unit.Excel.Validators
{
    [TestClass] [Ignore]
    public class CertificationAgencyValidatorTests
    {
        private CertificationAgencyValidator<ItemExcelModel> validator;
        private Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>> mockGetCertificationAgenciesQuery;

        [TestInitialize]
        public void Initialize()
        {
            this.mockGetCertificationAgenciesQuery = new Mock<IQueryHandler<GetCertificationAgenciesByTraitParameters, List<HierarchyClass>>>();

            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesByTraitParameters>()))
                .Returns(new List<HierarchyClass> { new HierarchyClass { hierarchyClassName = "Test", hierarchyClassID = 10 } });
        }

        [TestMethod]
        public void CertificationAgencyValidator_PropertyNotValid_ShouldSetError()
        {
            //Given
            validator = new CertificationAgencyValidator<ItemExcelModel>(i => i.GlutenFree, Traits.Codes.GlutenFree, mockGetCertificationAgenciesQuery.Object);

            IEnumerable<ItemExcelModel> excelModels = new List<ItemExcelModel>
            {
                new ItemExcelModel { GlutenFree = "Not Exists|1" },
                new ItemExcelModel { GlutenFree = "Not Exists|1" },
                new ItemExcelModel { GlutenFree = "Not Exists|1" }
            };

            //When
            validator.Validate(excelModels);

            //Then
            foreach (var model in excelModels)
            {
                Assert.IsNotNull(model.Error);
            }
        }

        [TestMethod]
        public void CertificationAgencyValidator_PropertyIsValid_ShouldNotSetError()
        {
            //Given
            validator = new CertificationAgencyValidator<ItemExcelModel>(i => i.GlutenFree, Traits.Codes.GlutenFree, mockGetCertificationAgenciesQuery.Object);

            IEnumerable<ItemExcelModel> excelModels = new List<ItemExcelModel>
            {
                new ItemExcelModel { GlutenFree = "Test|10" },
                new ItemExcelModel { GlutenFree = "Test|10" },
                new ItemExcelModel { GlutenFree = "Test|10" }
            };

            //When
            validator.Validate(excelModels);

            //Then
            foreach (var model in excelModels)
            {
                Assert.IsNull(model.Error);
            }
        }

        [TestMethod]
        public void CertificationAgencyValidator_MixtureOfValidPropertiesAndNoneValidProperties_ShouldSetErrorOnNonValidModelsAndNotSetErrorOnValidModels()
        {
            //Given
            validator = new CertificationAgencyValidator<ItemExcelModel>(i => i.GlutenFree, Traits.Codes.GlutenFree, mockGetCertificationAgenciesQuery.Object);

            IEnumerable<ItemExcelModel> excelModels = new List<ItemExcelModel>
            {
                new ItemExcelModel { GlutenFree = "Test|10" },
                new ItemExcelModel { GlutenFree = "Not Exists|1" },
                new ItemExcelModel { GlutenFree = "Test|10" }
            };

            //When
            validator.Validate(excelModels);

            //Then
            Assert.IsTrue(excelModels.Count(m => m.Error == null) == 2);
            Assert.IsTrue(excelModels.Count(m => m.Error != null) == 1);
        }
    }
}
