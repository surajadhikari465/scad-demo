using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
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
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class EwicControllerTests
    {
        private EwicController controller;
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetEwicExclusionsParameters, List<EwicExclusionModel>>> mockGetEwicExclusionsQueryHandler;
        private Mock<IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>>> mockGetAplScanCodesQueryHandler;
        private Mock<IQueryHandler<GetEwicMappingsParameters, List<EwicMappingModel>>> mockGetEwicMappingsQueryHandler;
        private Mock<IQueryHandler<GetItemTraitByTraitCodeParameters, ItemTrait>> mockGetItemTraitQueryHandler;
        private Mock<IManagerHandler<AddEwicExclusionManager>> mockAddExclusionManagerHandler;
        private Mock<IManagerHandler<RemoveEwicExclusionManager>> mockRemoveExclusionManagerHandler;
        private Mock<IManagerHandler<AddEwicMappingManager>> mockAddMappingManagerHandler;
        private Mock<IManagerHandler<RemoveEwicMappingManager>> mockRemoveMappingManagerHandler;
        private string testExclusion;
        private string testMapping;
        private string testAplScanCode;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger>();
            mockGetEwicExclusionsQueryHandler = new Mock<IQueryHandler<GetEwicExclusionsParameters, List<EwicExclusionModel>>>();
            mockGetAplScanCodesQueryHandler = new Mock<IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>>>();
            mockGetEwicMappingsQueryHandler = new Mock<IQueryHandler<GetEwicMappingsParameters, List<EwicMappingModel>>>();
            mockGetItemTraitQueryHandler = new Mock<IQueryHandler<GetItemTraitByTraitCodeParameters, ItemTrait>>();
            mockAddExclusionManagerHandler = new Mock<IManagerHandler<AddEwicExclusionManager>>();
            mockRemoveExclusionManagerHandler = new Mock<IManagerHandler<RemoveEwicExclusionManager>>();
            mockAddMappingManagerHandler = new Mock<IManagerHandler<AddEwicMappingManager>>();
            mockRemoveMappingManagerHandler = new Mock<IManagerHandler<RemoveEwicMappingManager>>();

            controller = new EwicController(
                mockLogger.Object,
                mockGetEwicExclusionsQueryHandler.Object,
                mockGetAplScanCodesQueryHandler.Object,
                mockGetEwicMappingsQueryHandler.Object,
                mockGetItemTraitQueryHandler.Object,
                mockAddExclusionManagerHandler.Object,
                mockRemoveExclusionManagerHandler.Object,
                mockAddMappingManagerHandler.Object,
                mockRemoveMappingManagerHandler.Object);

            testExclusion = "2222222228";
            testMapping = "2222222229";
            testAplScanCode = "2222222299";

            mockGetItemTraitQueryHandler.Setup(q => q.Search(It.IsAny<GetItemTraitByTraitCodeParameters>())).Returns(new ItemTrait { traitValue = "Description" });
        }

        [TestMethod]
        public void Exclusions_InitialPageLoadWithNoCurrentExclusions_ViewModelShouldIncludeEmptyExclusionsList()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel>());

            // When.
            var result = controller.Exclusions() as ViewResult;

            // Then.
            var viewModel = result.Model as EwicExclusionViewModel;

            Assert.AreEqual(0, viewModel.CurrentExclusions.Count);
            Assert.AreEqual(0, viewModel.RemovableEwicExclusions.Count());
            Assert.IsNull(viewModel.RemovedExclusionSelectedId);
            Assert.IsTrue(String.IsNullOrEmpty(viewModel.NewExclusion));
        }

        [TestMethod]
        public void Exclusions_InitialPageLoadWithCurrentExclusions_ViewModelShouldIncludeCurrentExclusions()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            // When.
            var result = controller.Exclusions() as ViewResult;

            // Then.
            var viewModel = result.Model as EwicExclusionViewModel;

            Assert.AreEqual(1, viewModel.CurrentExclusions.Count);
            Assert.AreEqual(1, viewModel.RemovableEwicExclusions.Count());
            Assert.IsNull(viewModel.RemovedExclusionSelectedId);
            Assert.IsTrue(String.IsNullOrEmpty(viewModel.NewExclusion));
        }

        [TestMethod]
        public void AddExclusion_ValidExclusion_AddExclusionCommandShouldBeExecuted()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            var viewModel = new EwicExclusionViewModel
            {
                NewExclusion = testExclusion
            };

            // When.
            var result = controller.AddExclusion(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicExclusionViewModel;
            var viewData = result.ViewData;

            mockAddExclusionManagerHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionManager>()), Times.Once);
            Assert.IsNotNull(viewData["SuccessMessage"]);
            Assert.IsNull(viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void AddExclusion_InvalidModelState_DefaultViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            controller.ModelState.AddModelError("test", "test");

            var viewModel = new EwicExclusionViewModel
            {
                CurrentExclusions = new List<EwicExclusionModel> { new EwicExclusionModel() },
                NewExclusion = testExclusion,
                RemovableEwicExclusions = new List<SelectListItem>()
            };

            // When.
            var result = controller.AddExclusion(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicExclusionViewModel;

            Assert.AreEqual(result.ViewName, "Exclusions");
            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(viewModel.CurrentExclusions.Count, returnedViewModel.CurrentExclusions.Count);
            Assert.AreEqual(testExclusion, returnedViewModel.NewExclusion);
            Assert.AreEqual(viewModel.RemovableEwicExclusions.Count(), returnedViewModel.RemovableEwicExclusions.Count());
        }

        [TestMethod]
        public void AddExclusion_CommandHandlerThrowsException_ViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            string errorMessage = "error";
            mockAddExclusionManagerHandler.Setup(c => c.Execute(It.IsAny<AddEwicExclusionManager>())).Throws(new Exception(errorMessage));

            var viewModel = new EwicExclusionViewModel
            {
                NewExclusion = testExclusion
            };

            // When.
            var result = controller.AddExclusion(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;

            mockAddExclusionManagerHandler.Verify(c => c.Execute(It.IsAny<AddEwicExclusionManager>()), Times.Once);
            Assert.IsNotNull(viewData["ErrorMessage"]);
            Assert.IsNull(viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void RemoveExclusion_ValidExclusion_RemoveExclusionCommandShouldBeExecuted()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            var viewModel = new EwicExclusionViewModel
            {
                RemovedExclusionSelectedId = testExclusion
            };

            // When.
            var result = controller.RemoveExclusion(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicExclusionViewModel;
            var viewData = result.ViewData;

            mockRemoveExclusionManagerHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionManager>()), Times.Once);
            Assert.IsNotNull(viewData["SuccessMessage"]);
            Assert.IsNull(viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void RemoveExclusion_InvalidModelState_DefaultViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            controller.ModelState.AddModelError("test", "test");

            var viewModel = new EwicExclusionViewModel
            {
                CurrentExclusions = new List<EwicExclusionModel> { new EwicExclusionModel() },
                NewExclusion = testExclusion,
                RemovableEwicExclusions = new List<SelectListItem>()
            };

            // When.
            var result = controller.RemoveExclusion(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicExclusionViewModel;

            Assert.AreEqual(result.ViewName, "Exclusions");
            Assert.IsNotNull(returnedViewModel);
            Assert.AreEqual(viewModel.CurrentExclusions.Count, returnedViewModel.CurrentExclusions.Count);
            Assert.AreEqual(testExclusion, returnedViewModel.NewExclusion);
            Assert.AreEqual(viewModel.RemovableEwicExclusions.Count(), returnedViewModel.RemovableEwicExclusions.Count());
        }

        [TestMethod]
        public void RemoveExclusion_CommandHandlerThrowsException_ViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            mockGetEwicExclusionsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicExclusionsParameters>())).Returns(new List<EwicExclusionModel> { new EwicExclusionModel() });

            string errorMessage = "error";
            mockRemoveExclusionManagerHandler.Setup(c => c.Execute(It.IsAny<RemoveEwicExclusionManager>())).Throws(new Exception(errorMessage));

            var viewModel = new EwicExclusionViewModel
            {
                RemovedExclusionSelectedId = testExclusion
            };

            // When.
            var result = controller.RemoveExclusion(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;

            mockRemoveExclusionManagerHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicExclusionManager>()), Times.Once);
            Assert.IsNotNull(viewData["ErrorMessage"]);
            Assert.IsNull(viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void Mappings_InitialPageLoadWithEmptyApl_ViewModelShouldIncludeEmptyAplTargetList()
        {
            // Given.
            mockGetAplScanCodesQueryHandler.Setup(q => q.Search(It.IsAny<GetAplScanCodesParameters>())).Returns(new List<EwicAplScanCodeModel>());

            // When.
            var result = controller.Mappings() as ViewResult;

            // Then.
            var viewModel = result.Model as EwicMappingSearchViewModel;

            Assert.AreEqual(0, viewModel.AplScanCodes.Count());
            Assert.IsNull(viewModel.AplScanCodeSelectedId);
        }

        [TestMethod]
        public void Mappings_InitialPageLoadWithAtLeastOneScanCodeInTheApl_ViewModelShouldIncludeCurrentAplScanCodes()
        {
            // Given.
            var testAplScanCodes = new List<EwicAplScanCodeModel> { new EwicAplScanCodeModel() };

            mockGetAplScanCodesQueryHandler.Setup(q => q.Search(It.IsAny<GetAplScanCodesParameters>())).Returns(testAplScanCodes);

            // When.
            var result = controller.Mappings() as ViewResult;

            // Then.
            var viewModel = result.Model as EwicMappingSearchViewModel;

            Assert.AreEqual(testAplScanCodes.Count, viewModel.AplScanCodes.Count());
            Assert.IsNull(viewModel.AplScanCodeSelectedId);
        }

        [TestMethod]
        public void MappingDetail_AplScanCodeHasNoMappings_MappingsCollectionInViewModelShouldBeEmpty()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            var viewModel = new EwicMappingSearchViewModel();

            // When.
            var result = controller.MappingDetail(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicMappingDetailViewModel;

            Assert.AreEqual(0, returnedViewModel.CurrentMappings.Count);
        }

        [TestMethod]
        public void MappingDetail_AplScanCodeHasThreeMappings_MappingsCollectionInViewModelShouldContainThreeMappings()
        {
            // Given.
            var testMappings = new List<EwicMappingModel>
            {
                new EwicMappingModel(),
                new EwicMappingModel(),
                new EwicMappingModel()
            };

            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(testMappings);

            var viewModel = new EwicMappingSearchViewModel();

            // When.
            var result = controller.MappingDetail(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicMappingDetailViewModel;

            Assert.AreEqual(testMappings.Count, returnedViewModel.CurrentMappings.Count);
        }

        [TestMethod]
        public void AddMapping_ValidMapping_AddMappingCommandShouldBeExecuted()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            var viewModel = new EwicMappingDetailViewModel
            {
                AplScanCode = testAplScanCode,
                NewMapping = testMapping
            };

            // When.
            var result = controller.AddMapping(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicExclusionViewModel;
            var viewData = result.ViewData;

            mockAddMappingManagerHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingManager>()), Times.Once);
            Assert.IsNotNull(viewData["SuccessMessage"]);
            Assert.IsNull(viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void AddMapping_InvalidModelState_MappingDetailViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            controller.ModelState.AddModelError("test", "test");

            var viewModel = new EwicMappingDetailViewModel
            {
                AplScanCode = testAplScanCode,
                NewMapping = testMapping
            };

            // When.
            var result = controller.AddMapping(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicMappingDetailViewModel;

            Assert.AreEqual(result.ViewName, "MappingDetail");
            Assert.AreEqual(viewModel.AplScanCode, returnedViewModel.AplScanCode);
            Assert.AreEqual(viewModel.NewMapping, returnedViewModel.NewMapping);
        }

        [TestMethod]
        public void AddMapping_CommandHandlerThrowsException_ViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            string errorMessage = "error";
            mockAddMappingManagerHandler.Setup(c => c.Execute(It.IsAny<AddEwicMappingManager>())).Throws(new Exception(errorMessage));

            var viewModel = new EwicMappingDetailViewModel
            {
                AplScanCode = testAplScanCode,
                NewMapping = testMapping
            };

            // When.
            var result = controller.AddMapping(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicMappingDetailViewModel;
            var viewData = result.ViewData;

            mockAddMappingManagerHandler.Verify(c => c.Execute(It.IsAny<AddEwicMappingManager>()), Times.Once);
            Assert.IsNotNull(viewData["ErrorMessage"]);
            Assert.IsNull(viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void RemoveMapping_ValidExistingMapping_RemoveMappingCommandShouldBeExecuted()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            var viewModel = new EwicMappingDetailViewModel
            {
                AplScanCode = testAplScanCode,
                RemovableMappedWfmScanCodesSelectedId = testMapping
            };

            // When.
            var result = controller.RemoveMapping(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicExclusionViewModel;
            var viewData = result.ViewData;

            mockRemoveMappingManagerHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicMappingManager>()), Times.Once);
            Assert.IsNotNull(viewData["SuccessMessage"]);
            Assert.IsNull(viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void RemoveMapping_InvalidModelState_MappingDetailViewShouldBeReturnedWithPopulatedViewModel()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            controller.ModelState.AddModelError("test", "test");

            var viewModel = new EwicMappingDetailViewModel
            {
                AplScanCode = testAplScanCode,
                RemovableMappedWfmScanCodesSelectedId = testMapping
            };

            // When.
            var result = controller.RemoveMapping(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicMappingDetailViewModel;

            Assert.AreEqual(result.ViewName, "MappingDetail");
            Assert.AreEqual(viewModel.AplScanCode, returnedViewModel.AplScanCode);
            Assert.AreEqual(viewModel.NewMapping, returnedViewModel.NewMapping);
        }

        [TestMethod]
        public void RemoveMapping_CommandHandlerThrowsException_ViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            mockGetEwicMappingsQueryHandler.Setup(q => q.Search(It.IsAny<GetEwicMappingsParameters>())).Returns(new List<EwicMappingModel>());

            string errorMessage = "error";
            mockRemoveMappingManagerHandler.Setup(c => c.Execute(It.IsAny<RemoveEwicMappingManager>())).Throws(new Exception(errorMessage));

            var viewModel = new EwicMappingDetailViewModel
            {
                AplScanCode = testAplScanCode,
                RemovableMappedWfmScanCodesSelectedId = testMapping
            };

            // When.
            var result = controller.RemoveMapping(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as EwicMappingDetailViewModel;
            var viewData = result.ViewData;

            mockRemoveMappingManagerHandler.Verify(c => c.Execute(It.IsAny<RemoveEwicMappingManager>()), Times.Once);
            Assert.IsNotNull(viewData["ErrorMessage"]);
            Assert.IsNull(viewData["SuccessMessage"]);
        }
    }
}
