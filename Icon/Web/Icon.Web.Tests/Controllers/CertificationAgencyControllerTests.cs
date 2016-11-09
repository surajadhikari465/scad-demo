using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Controllers;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class CertificationAgencyControllerTests
    {
        private CertificationAgencyController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>> mockGetCertificationAgenciesQuery;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockGetHierarchyClassQuery;
        private Mock<IManagerHandler<AddCertificationAgencyManager>> mockAddCertificationAgencyManagerHandler;
        private Mock<IManagerHandler<UpdateCertificationAgencyManager>> mockUpdateCertificationAgencyManagerHandler;
        private Mock<IManagerHandler<DeleteHierarchyClassManager>> mockDeleteHierarchyClassManagerHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetCertificationAgenciesQuery = new Mock<IQueryHandler<GetCertificationAgenciesParameters, List<CertificationAgencyModel>>>();
            mockGetHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            mockAddCertificationAgencyManagerHandler = new Mock<IManagerHandler<AddCertificationAgencyManager>>();
            mockUpdateCertificationAgencyManagerHandler = new Mock<IManagerHandler<UpdateCertificationAgencyManager>>();
            mockDeleteHierarchyClassManagerHandler = new Mock<IManagerHandler<DeleteHierarchyClassManager>>();

            controller = new CertificationAgencyController(mockLogger.Object,
                mockGetCertificationAgenciesQuery.Object,
                mockGetHierarchyClassQuery.Object,
                mockAddCertificationAgencyManagerHandler.Object,
                mockUpdateCertificationAgencyManagerHandler.Object,
                mockDeleteHierarchyClassManagerHandler.Object);
        }

        [TestMethod]
        public void Create_ValidViewModel_ShouldReturnSuccess()
        {
            //Given
            var viewModel = new CertificationAgencyViewModel
                {
                    AgencyName = "Test Agency"
                };

            //When
            var result = controller.Create(viewModel);

            //Then
            var successMessage = controller.ViewData["SuccessMessage"];

            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.AreEqual(String.Format("Successfully added certification agency {0}.", viewModel.AgencyName), successMessage);
            mockAddCertificationAgencyManagerHandler.Verify(m => m.Execute(It.IsAny<AddCertificationAgencyManager>()), Times.Once);
        }

        [TestMethod]
        public void Create_ViewModelWithInvalidModelState_ShouldReturnViewWithNoMessage()
        {
            //Given
            var viewModel = new CertificationAgencyViewModel();
            var modelError = new ModelState();
            modelError.Errors.Add("Test");
            controller.ModelState.Add("AgencyName", modelError);

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.IsFalse(controller.ModelState.IsValid);
            Assert.IsNotNull(result);
            Assert.AreEqual(viewModel, result.Model);
            mockAddCertificationAgencyManagerHandler.Verify(m => m.Execute(It.IsAny<AddCertificationAgencyManager>()), Times.Never);
        }

        [TestMethod]
        public void Create_ViewModelWithInvalidHierarchyClassName_ShouldClearModelStateErrorAndReturnSuccessMessage()
        {
            //Given
            var viewModel = new CertificationAgencyViewModel { AgencyName = "Test Agency" };
            var modelError = new ModelState();
            modelError.Errors.Add("Test");
            controller.ModelState.Add("HierarchyClassName", modelError);

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            Assert.IsTrue(controller.ModelState.IsValid);
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.Model);
            mockAddCertificationAgencyManagerHandler.Verify(m => m.Execute(It.IsAny<AddCertificationAgencyManager>()), Times.Once);
        }

        [TestMethod]
        public void Create_ManagerThrowsCommandException_ShouldReturnFailureMessage()
        {
            //Given
            var viewModel = new CertificationAgencyViewModel { AgencyName = "Test Agency" };
            mockAddCertificationAgencyManagerHandler.Setup(m => m.Execute(It.IsAny<AddCertificationAgencyManager>()))
                .Throws(new CommandException("Test Exception"));

            //When
            var result = controller.Create(viewModel) as ViewResult;

            //Then
            var errorMessage = controller.ViewData["ErrorMessage"];
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Exception", errorMessage);
        }

        [TestMethod]
        public void Edit_GetWithCertificationAgencyHasNoAgencyTraits_ShouldReturnViewModelWithFalseTraits()
        {
            //Given
            var hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(5)
                .WithHierarchyClassName("Test Hierarchy Class")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyLevel(HierarchyLevels.Parent)
                .Build();
            mockGetHierarchyClassQuery.Setup(m => m.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(hierarchyClass);

            //When
            var result = controller.Edit(5) as ViewResult;

            //Then
            var model = result.Model as CertificationAgencyViewModel;
            Assert.AreEqual(hierarchyClass.hierarchyClassID, model.HierarchyClassId);
            Assert.AreEqual(hierarchyClass.hierarchyClassName, model.AgencyName);
            Assert.AreEqual(hierarchyClass.hierarchyID, model.HierarchyId);
            Assert.AreEqual(hierarchyClass.hierarchyLevel, model.HierarchyLevel);
            Assert.IsFalse(model.GlutenFree);
            Assert.IsFalse(model.Kosher);
            Assert.IsFalse(model.NonGMO);
            Assert.IsFalse(model.Organic);
            Assert.IsFalse(model.Vegan);
        }

        [TestMethod]
        public void Edit_GetWithCertificationAgencyHasAllAgencyTraits_ShouldReturnViewModelWithAllTraits()
        {
            //Given
            var hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(5)
                .WithHierarchyClassName("Test Hierarchy Class")
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyLevel(HierarchyLevels.Parent)
                .WithGlutenFreeTrait("1")
                .WithKosherTrait("1")
                .WithNonGmoTrait("1")
                .WithOrganicTrait("1")
                .WithVeganTrait("1")
                .Build();
            mockGetHierarchyClassQuery.Setup(m => m.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(hierarchyClass);

            //When
            var result = controller.Edit(5) as ViewResult;

            //Then
            var model = result.Model as CertificationAgencyViewModel;
            Assert.AreEqual(hierarchyClass.hierarchyClassID, model.HierarchyClassId);
            Assert.AreEqual(hierarchyClass.hierarchyClassName, model.AgencyName);
            Assert.AreEqual(hierarchyClass.hierarchyID, model.HierarchyId);
            Assert.AreEqual(hierarchyClass.hierarchyLevel, model.HierarchyLevel);
            Assert.IsTrue(model.GlutenFree);
            Assert.IsTrue(model.Kosher);
            Assert.IsTrue(model.NonGMO);
            Assert.IsTrue(model.Organic);
            Assert.IsTrue(model.Vegan);
        }

        [TestMethod]
        public void Edit_ManagerIsSuccessful_ShouldReturnSuccessMessage()
        {
            //Given
            var viewModel = new CertificationAgencyViewModel
                {
                    AgencyName = "Test Agency",
                    HierarchyClassId = 5,
                    HierarchyId = Hierarchies.CertificationAgencyManagement,
                    HierarchyLevel = HierarchyLevels.Parent,
                    GlutenFree = true,
                    Kosher = true,
                    NonGMO = true,
                    Organic = true,
                    Vegan = true
                };

            //When
            var result = controller.Edit(viewModel) as ViewResult;

            //Then
            mockUpdateCertificationAgencyManagerHandler.Verify(m => m.Execute(
                It.Is<UpdateCertificationAgencyManager>(man =>
                    man.Agency.hierarchyClassID == viewModel.HierarchyClassId
                    && man.Agency.hierarchyID == viewModel.HierarchyId
                    && man.Agency.hierarchyClassName == viewModel.AgencyName
                    && man.Agency.hierarchyLevel == viewModel.HierarchyLevel
                    && man.GlutenFree == "1"
                    && man.Kosher == "1"
                    && man.NonGMO == "1"
                    && man.Organic == "1"
                    && man.Vegan == "1")), 
                Times.Once);
            Assert.AreEqual("Agency update was successful.", controller.ViewData["SuccessMessage"]);
            Assert.AreEqual(viewModel, result.Model);
        }

        [TestMethod]
        public void Edit_AgencyNameHasTrailingWhiteSpace_ShouldTrimAgencyName()
        {

            //Given
            var viewModel = new CertificationAgencyViewModel
            {
                AgencyName = "    Test Agency    ",
                HierarchyClassId = 5,
                HierarchyId = Hierarchies.CertificationAgencyManagement,
                HierarchyLevel = HierarchyLevels.Parent
            };

            //When
            var result = controller.Edit(viewModel) as ViewResult;

            //Then
            mockUpdateCertificationAgencyManagerHandler.Verify(m => m.Execute(
                It.Is<UpdateCertificationAgencyManager>(man =>
                    man.Agency.hierarchyClassID == viewModel.HierarchyClassId
                    && man.Agency.hierarchyID == viewModel.HierarchyId
                    && man.Agency.hierarchyClassName == "Test Agency"
                    && man.Agency.hierarchyLevel == viewModel.HierarchyLevel
                    && man.GlutenFree == "0"
                    && man.Kosher == "0"
                    && man.NonGMO == "0"
                    && man.Organic == "0"
                    && man.Vegan == "0")),
                Times.Once);
            Assert.AreEqual("Agency update was successful.", controller.ViewData["SuccessMessage"]);
            Assert.AreEqual(viewModel, result.Model);
        }

        [TestMethod]
        public void Edit_ManagerThrowsException_ShouldReturnErrorMessage()
        {
            //Given
            var viewModel = new CertificationAgencyViewModel { AgencyName = "Test Agency" };
            mockUpdateCertificationAgencyManagerHandler.Setup(m => m.Execute(It.IsAny<UpdateCertificationAgencyManager>()))
                .Throws(new CommandException("Test Exception"));

            //When
            var result = controller.Edit(viewModel) as ViewResult;

            //Then
            Assert.AreEqual("Test Exception", controller.ViewData["ErrorMessage"]);
            Assert.AreEqual(viewModel, result.Model);
        }

        [TestMethod]
        public void Delete_HierarchyClassIdIsLessThan1_ShouldRedirectToIndex()
        {
            //When
            var result = controller.Delete(0) as RedirectToRouteResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_HierarchyClassExist_ShouldReturnAgency()
        {
            //Given
            var hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(1)
                .WithGlutenFreeTrait("1")
                .WithKosherTrait("1")
                .WithNonGmoTrait("1")
                .WithOrganicTrait("1")
                .WithVeganTrait("1")
                .Build();
            mockGetHierarchyClassQuery.Setup(m => m.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(hierarchyClass);

            //When
            var result = controller.Delete(hierarchyClass.hierarchyClassID) as ViewResult;

            //Then
            var model = result.Model as CertificationAgencyViewModel;
            Assert.AreEqual(hierarchyClass.hierarchyClassID, model.HierarchyClassId);
            Assert.AreEqual(hierarchyClass.hierarchyClassName, model.AgencyName);
            Assert.IsTrue(model.GlutenFree);
            Assert.IsTrue(model.Kosher);
            Assert.IsTrue(model.NonGMO);
            Assert.IsTrue(model.Organic);
            Assert.IsTrue(model.Vegan);
        }

        [TestMethod]
        public void Delete_AgencyIsAssociatedToItems_ShouldReturnErrorMessage()
        {
            //Given
            var hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(1)
                .WithItemSignAttribute(new List<ItemSignAttribute> { new ItemSignAttribute() })
                .Build();
            var viewModel = new CertificationAgencyViewModel();
            mockGetHierarchyClassQuery.Setup(m => m.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(hierarchyClass);

            //When
            var result = controller.Delete(viewModel) as ViewResult;

            //Then
            var model = result.Model as CertificationAgencyViewModel;
            Assert.AreEqual(hierarchyClass.hierarchyClassID, model.HierarchyClassId);
            Assert.AreEqual("Error: This hierarchy class is assigned as at least one item's certification agency, so it cannot be deleted.", controller.ViewData["ErrorMessage"]);
        }

        [TestMethod]
        public void Delete_DeleteIsSuccessful_ShouldRedirectToIndex()
        {
            //Given
            var hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(1)
                .Build();
            var viewModel = new CertificationAgencyViewModel();
            mockGetHierarchyClassQuery.Setup(m => m.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(hierarchyClass);

            //When
            var result = controller.Delete(viewModel) as RedirectToRouteResult;

            //Then
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_DeleteThrowsCommandException_ShouldReturnErrorMessage()
        {
            //Given
            var hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyClassId(1)
                .Build();
            var viewModel = new CertificationAgencyViewModel();
            mockGetHierarchyClassQuery.Setup(m => m.Search(It.IsAny<GetHierarchyClassByIdParameters>()))
                .Returns(hierarchyClass);
            mockDeleteHierarchyClassManagerHandler.Setup(m => m.Execute(It.IsAny<DeleteHierarchyClassManager>()))
                .Throws(new CommandException("Test Exception"));

            //When
            var result = controller.Delete(viewModel) as ViewResult;

            //Then
            var model = result.Model as CertificationAgencyViewModel;
            Assert.AreEqual(hierarchyClass.hierarchyClassID, model.HierarchyClassId);
            Assert.AreEqual("Error: There was a problem with applying this delete on the database.", controller.ViewData["ErrorMessage"]);
        }

        [TestMethod]
        public void All_ShouldReturnAllCertificationAgencies()
        {
            //Given
            List<CertificationAgencyModel> models = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel { HierarchyClassName = "Test" }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(models);

            //When
            var result = controller.All() as ViewResult;

            //Then
            var model = result.Model as IQueryable<CertificationAgencyViewModel>;
            Assert.AreEqual(1, model.Count());
            Assert.AreEqual(models[0].HierarchyClassName, model.First().AgencyName);
        }

        [TestMethod]
        public void GetAnimalWelfareRatings_ShouldReturnAnimalWelfareRatings()
        {
            //When
            var result = controller.GetAnimalWelfareRatings() as JsonResult;

            //Then
            var agencies = result.Data as dynamic[];
            var dictionary = AnimalWelfareRatings.AsDictionary;

            AssertAgenciesAreEqualToDictionary(agencies, dictionary);
        }

        [TestMethod]
        public void GetMilkTypes_ShouldReturnMilkTypes()
        {
            //When
            var result = controller.GetMilkTypes() as JsonResult;

            //Then
            var agencies = result.Data as dynamic[];
            var dictionary = MilkTypes.AsDictionary;

            AssertAgenciesAreEqualToDictionary(agencies, dictionary);
        }

        [TestMethod]
        public void GetEcoScaleRatings_ShouldReturnEcoScaleRatings()
        {
            //When
            var result = controller.GetEcoScaleRatings() as JsonResult;

            //Then
            var agencies = result.Data as dynamic[];
            var dictionary = EcoScaleRatings.AsDictionary;

            AssertAgenciesAreEqualToDictionary(agencies, dictionary);
        }

       
        [TestMethod]
        public void GetSeafoodFreshOrFrozenTypes_ShouldReturnSeafoodFreshOrFrozenTypes()
        {
            //When
            var result = controller.GetSeafoodFreshOrFrozenTypes() as JsonResult;

            //Then
            var agencies = result.Data as dynamic[];
            var dictionary = SeafoodFreshOrFrozenTypes.AsDictionary;

            AssertAgenciesAreEqualToDictionary(agencies, dictionary);
        }

        [TestMethod]
        public void GetSeafoodCatchTypes_ShouldReturnSeafoodCatchTypes()
        {
            //When
            var result = controller.GetSeafoodCatchTypes() as JsonResult;

            //Then
            var agencies = result.Data as dynamic[];
            var dictionary = SeafoodCatchTypes.AsDictionary;

            AssertAgenciesAreEqualToDictionary(agencies, dictionary);
        }

        private void AssertAgenciesAreEqualToDictionary(dynamic[] agencies, Dictionary<int, string> dictionary)
        {
            var dictionaryArray = dictionary.ToArray();
            var idProperty = agencies[0].GetType().GetProperty("id");
            var nameProperty = agencies[0].GetType().GetProperty("name");

            for (int i = 0; i < agencies.Length; i++)
            {
                Assert.AreEqual(idProperty.GetValue(agencies[i]), dictionaryArray[i].Key);
                Assert.AreEqual(nameProperty.GetValue(agencies[i]), dictionaryArray[i].Value);
            }
        }

        [TestMethod]
        public void GetGlutenFreeAgencies_GlutenFreeAgenciesExist_ShouldReturnGlutenFreeAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        GlutenFree = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var agencies = controller.GetGlutenFreeAgencies().Data as dynamic[];

            //Then
            AssertAgenciesAreEqualToCertificationAgencyModels(agencies, certificationAgencyModels);
        }

        [TestMethod]
        public void GetKosherAgencies_KosherAgenciesExist_ShouldReturnKosherAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        Kosher = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var agencies = controller.GetKosherAgencies().Data as dynamic[];

            //Then
            AssertAgenciesAreEqualToCertificationAgencyModels(agencies, certificationAgencyModels);
        }

        [TestMethod]
        public void GetNonGmoAgencies_NonGmoAgenciesExist_ShouldReturnNonGmoAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        NonGMO = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var agencies = controller.GetNonGmoAgencies().Data as dynamic[];

            //Then
            AssertAgenciesAreEqualToCertificationAgencyModels(agencies, certificationAgencyModels);
        }

        [TestMethod]
        public void GetOrganicAgencies_OrganicAgenciesExist_ShouldReturnOrganicAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        Organic = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var agencies = controller.GetOrganicAgencies().Data as dynamic[];

            //Then
            AssertAgenciesAreEqualToCertificationAgencyModels(agencies, certificationAgencyModels);
        }

        [TestMethod]
        public void GetVeganAgencies_VeganAgenciesExist_ShouldReturnVeganAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        Vegan = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var agencies = controller.GetVeganAgencies().Data as dynamic[];

            //Then
            AssertAgenciesAreEqualToCertificationAgencyModels(agencies, certificationAgencyModels);
        }

        private void AssertAgenciesAreEqualToCertificationAgencyModels(dynamic[] agencies, List<CertificationAgencyModel> certificationAgencyModels)
        {
            var idProperty = agencies[0].GetType().GetProperty("id");
            var nameProperty = agencies[0].GetType().GetProperty("name");

            for (int i = 0; i < agencies.Length; i++)
            {
                Assert.AreEqual(idProperty.GetValue(agencies[i]), certificationAgencyModels[i].HierarchyClassId);
                Assert.AreEqual(nameProperty.GetValue(agencies[i]), certificationAgencyModels[i].HierarchyClassName);
            }
        }

        [TestMethod]
        public void CertificationAgencyAutocomplete_GluteFreeTraitId_ShouldReturnGlutenFreeAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        GlutenFree = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var result = controller.CertificationAgencyAutocomplete(Traits.GlutenFree, "Test").Data as dynamic[];

            //Then
            var nameProperty = result[0].GetType().GetProperty("value");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(certificationAgencyModels[0].HierarchyClassName, nameProperty.GetValue(result[0]));
        }

        [TestMethod]
        public void CertificationAgencyAutocomplete_KosherTraitId_ShouldReturnKosherAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        Kosher = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var result = controller.CertificationAgencyAutocomplete(Traits.Kosher, "Test").Data as dynamic[];

            //Then
            var nameProperty = result[0].GetType().GetProperty("value");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(certificationAgencyModels[0].HierarchyClassName, nameProperty.GetValue(result[0]));
        }

        [TestMethod]
        public void CertificationAgencyAutocomplete_OrganicTraitId_ShouldReturnOrganicAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        Organic = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var result = controller.CertificationAgencyAutocomplete(Traits.Organic, "Test").Data as dynamic[];

            //Then
            var nameProperty = result[0].GetType().GetProperty("value");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(certificationAgencyModels[0].HierarchyClassName, nameProperty.GetValue(result[0]));
        }

        [TestMethod]
        public void CertificationAgencyAutocomplete_NonGmoTraitId_ShouldReturnNonGmoAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        NonGMO = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var result = controller.CertificationAgencyAutocomplete(Traits.NonGmo, "Test").Data as dynamic[];

            //Then
            var nameProperty = result[0].GetType().GetProperty("value");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(certificationAgencyModels[0].HierarchyClassName, nameProperty.GetValue(result[0]));
        }

        [TestMethod]
        public void CertificationAgencyAutocomplete_VeganTraitId_ShouldReturnVeganAgencies()
        {
            //Given
            var certificationAgencyModels = new List<CertificationAgencyModel>
                {
                    new CertificationAgencyModel
                    {
                        HierarchyClassId = 5,
                        HierarchyClassName = "Test",
                        Vegan = "1"
                    }
                };
            mockGetCertificationAgenciesQuery.Setup(m => m.Search(It.IsAny<GetCertificationAgenciesParameters>()))
                .Returns(certificationAgencyModels);

            //When
            var result = controller.CertificationAgencyAutocomplete(Traits.Vegan, "Test").Data as dynamic[];

            //Then
            var nameProperty = result[0].GetType().GetProperty("value");
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(certificationAgencyModels[0].HierarchyClassName, nameProperty.GetValue(result[0]));
        }
    }
}
