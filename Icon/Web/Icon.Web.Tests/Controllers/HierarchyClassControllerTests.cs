using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class HierarchyClassControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockHierarchyClassQuery;
        private Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> mockGetHierarchyQuery;
        private Mock<IManagerHandler<DeleteHierarchyClassManager>> mockDeleteHierarchyClassManager;
        private Mock<IManagerHandler<AddHierarchyClassManager>> mockAddManager;
        private Mock<IManagerHandler<UpdateHierarchyClassManager>> mockUpdateManager;
        private Mock<ControllerContext> mockContext;
        private HierarchyClassController controller;
        private Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>> mockGetHierarchyClassesQueryHandler;
        private Mock<IQueryHandler<GetMerchandiseHierarchyClassTraitsParameters, IEnumerable<MerchandiseHierarchyClassTrait>>> mockGetMerchandiseHierarchyTraitsQueryHandler;
        private Mock<IDonutCacheManager> mockCacheManager;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            this.mockGetHierarchyQuery = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            this.mockDeleteHierarchyClassManager = new Mock<IManagerHandler<DeleteHierarchyClassManager>>();
            this.mockAddManager = new Mock<IManagerHandler<AddHierarchyClassManager>>();
            this.mockUpdateManager = new Mock<IManagerHandler<UpdateHierarchyClassManager>>();
            this.mockContext = new Mock<ControllerContext>();
            this.mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, IEnumerable<HierarchyClassModel>>>();
            this.mockGetMerchandiseHierarchyTraitsQueryHandler = new Mock<IQueryHandler<GetMerchandiseHierarchyClassTraitsParameters, IEnumerable<MerchandiseHierarchyClassTrait>>>();
            this.mockCacheManager = new Mock<IDonutCacheManager>();

            this.controller = new HierarchyClassController(mockLogger.Object,
                mockGetHierarchyQuery.Object,
                mockHierarchyClassQuery.Object,
                mockDeleteHierarchyClassManager.Object,
                mockAddManager.Object,
                mockUpdateManager.Object,
                mockGetHierarchyClassesQueryHandler.Object,
                mockGetMerchandiseHierarchyTraitsQueryHandler.Object,
                mockCacheManager.Object);

            mockContext.SetupGet(c => c.HttpContext.User.Identity.Name).Returns("Test User");
            controller.ControllerContext = mockContext.Object;
        }

        [TestMethod]
        public void Create_GetWithValidParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(GetFakeHierarchy());
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());

            // When.
            ViewResult nonZeroResult = controller.Create(1, 1) as ViewResult;

            // Then.
            Assert.IsNotNull(nonZeroResult);
        }

        [TestMethod]
        public void Create_ValidParametersParentIdNonZeroNonMerch_ViewResultPopulated()
        {
            // Given.
            var hierarchy = GetFakeHierarchy();
            var hierarchyClass = GetFakeHierarchyClass();
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchy);
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(hierarchyClass);

            HierarchyClassViewModel expectedModel = new HierarchyClassViewModel();
            expectedModel.HierarchyId = hierarchyClass.hierarchyID;
            expectedModel.HierarchyName = hierarchyClass.Hierarchy.hierarchyName;
            expectedModel.HierarchyLevel = hierarchyClass.hierarchyLevel + 1;
            expectedModel.HierarchyParentClassId = hierarchyClass.hierarchyClassID;
            expectedModel.HierarchyParentClassName = hierarchyClass.hierarchyClassName;

            // When.
            ViewResult nonZeroParentIdResult = controller.Create(1, 1) as ViewResult;
            var actualModel = nonZeroParentIdResult.Model as HierarchyClassViewModel;

            // Then.
            Assert.AreEqual(actualModel.HierarchyId, expectedModel.HierarchyId, "Expected HierarchyId does not match actual HierarchyId");
            Assert.AreEqual(actualModel.HierarchyName, expectedModel.HierarchyName, "Expected HierarchyName does not match actual HierarchyName");
            Assert.AreEqual(actualModel.HierarchyLevel, expectedModel.HierarchyLevel, "Expected HierarchyLevel does not match actual HierarchyLevel");
            Assert.AreEqual(actualModel.HierarchyParentClassId, expectedModel.HierarchyParentClassId, "Expected HierarchyParentClassId does not match actual HierarchyParentClassId");
            Assert.AreEqual(actualModel.HierarchyParentClassName, expectedModel.HierarchyParentClassName, "Expected HierarchyParentClassName does not match actual HierarchyParentClassName");
        }

        [TestMethod]
        public void Create_ValidParametersParentIdNonZeroMerch_SubTeamListPopulated()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            var merchHierarchyClass = GetFakeHierarchyClass();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;
            merchHierarchyClass.Hierarchy.hierarchyName = HierarchyNames.Merchandise;
            merchHierarchyClass.hierarchyLevel = 4;

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(merchHierarchyClass);

            HierarchyClassViewModel expectedModel = new HierarchyClassViewModel();
            var dropDown = financialHierarchy.FirstOrDefault().HierarchyClass.Select(hc => new SelectListItem
            {
                Value = hc.hierarchyClassID.ToString(),
                Text = hc.hierarchyClassName
            }).ToList();
            expectedModel.SubTeamList = dropDown.OrderBy(st => st.Value);

            // When.
            ViewResult zeroParentIdResult = controller.Create(1, 1) as ViewResult;
            var actualModel = zeroParentIdResult.Model as HierarchyClassViewModel;

            // Then.

            for (int i = 0; i < actualModel.SubTeamList.ToList().Count(); i++)
            {
                Assert.AreEqual(expectedModel.SubTeamList.ToList()[i].Selected, actualModel.SubTeamList.ToList()[i].Selected);
                Assert.AreEqual(expectedModel.SubTeamList.ToList()[i].Value, actualModel.SubTeamList.ToList()[i].Value);
                Assert.AreEqual(expectedModel.SubTeamList.ToList()[i].Text, actualModel.SubTeamList.ToList()[i].Text);
            }
        }

        [TestMethod]
        public void Create_InvalidParameters_MethodShouldRedirectToIndex()
        {
            // Given.

            // When.
            var nullParentIdResult = controller.Create(null, 1) as RedirectToRouteResult;
            var negativeHierarchyIdResult = controller.Create(1, -1) as RedirectToRouteResult;
            var zeroHierarchyIdResult = controller.Create(1, 0) as RedirectToRouteResult;
            var negativeParentIdResult = controller.Create(-1, 1) as RedirectToRouteResult;

            string routeName = "Index";

            // Then.
            Assert.AreEqual(routeName, nullParentIdResult.RouteValues["action"]);
            Assert.AreEqual(routeName, negativeHierarchyIdResult.RouteValues["action"]);
            Assert.AreEqual(routeName, zeroHierarchyIdResult.RouteValues["action"]);
            Assert.AreEqual(routeName, negativeParentIdResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Create_ValidModelState_CommandShouldBeExecutedAndHierarchyCacheShouldBeCleared()
        {
            // Given.

            List<Hierarchy> subTeams = new List<Hierarchy>
            {
                new Hierarchy
                {
                    hierarchyID = Hierarchies.Financial,
                    hierarchyName = "Financial",
                    HierarchyClass = new List<HierarchyClass>
                    {
                        new HierarchyClass
                        {
                            hierarchyID = Hierarchies.Financial,
                            hierarchyClassID = 1,
                            hierarchyClassName = "TestSubTeam1",
                            hierarchyParentClassID = null
                        }
                    }
                }
            };

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            this.mockGetHierarchyQuery.Setup(s => s.Search(It.IsAny<GetHierarchyParameters>())).Returns(subTeams);

            // When.
            ViewResult result = controller.Create(viewModel) as ViewResult;

            // Then.
            mockAddManager.Verify(command => command.Execute(It.IsAny<AddHierarchyClassManager>()), Times.Once);
            mockCacheManager.Verify(x => x.ClearCacheForController(It.Is<string>(a => a == "HierarchyClass")));
        }

        [TestMethod]
        public void Create_InvalidModelState_ShouldReturnViewModelObject()
        {
            // Given.
            List<Hierarchy> subTeams = new List<Hierarchy>
            {
                new Hierarchy
                {
                    hierarchyID = Hierarchies.Financial,
                    hierarchyName = "Financial",
                    HierarchyClass = new List<HierarchyClass>
                    {
                        new HierarchyClass
                        {
                            hierarchyID = Hierarchies.Financial,
                            hierarchyClassID = 1,
                            hierarchyClassName = "TestSubTeam1",
                            hierarchyParentClassID = null
                        }
                    }
                }
            };

            controller.ModelState.AddModelError("test", "test");
            this.mockGetHierarchyQuery.Setup(s => s.Search(It.IsAny<GetHierarchyParameters>())).Returns(subTeams);

            // When.
            ViewResult result = controller.Create(new HierarchyClassViewModel()) as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create_DuplicateHierarchyClass_ViewDataShouldContainErrorMessage()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;

            mockAddManager.Setup(c => c.Execute(It.IsAny<AddHierarchyClassManager>())).Throws(new CommandException("error"));
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            ViewResult result = controller.Create(viewModel) as ViewResult;
            string message = result.ViewData["ErrorMessage"].ToString();

            // Then.
            Assert.AreEqual("error", message);
        }

        [TestMethod]
        public void Create_CommandException_ViewDataShouldContainErrorMessage()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;

            mockAddManager.Setup(c => c.Execute(It.IsAny<AddHierarchyClassManager>())).Throws(new CommandException("error"));
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            ViewResult result = controller.Create(viewModel) as ViewResult;
            string message = result.ViewData["ErrorMessage"].ToString();

            // Then.
            Assert.AreEqual("error", message);
        }

        [TestMethod]
        public void Create_UnexpectedError_ErrorShouldBeLogged()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;

            mockAddManager.Setup(c => c.Execute(It.IsAny<AddHierarchyClassManager>())).Throws(new CommandException("error"));
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            controller.Create(viewModel);

            // Then.
            mockLogger.Verify(logger => logger.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Edit_ValidParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(GetFakeHierarchy());

            // When.
            ViewResult nonZeroResult = controller.Edit(1) as ViewResult;

            // Then.
            Assert.IsNotNull(nonZeroResult);
        }

        [TestMethod]
        public void EditGet_HierarchyClassIdIsBrand_ShouldRedirectToBrandEditAction()
        {
            // Given.
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyClassId(11).Build();
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(hierarchyClass);

            // When.
            var result = controller.Edit(11) as RedirectToRouteResult;
            string expectedController = "Brand";
            string expectedAction = "Edit";
            int expectedHierarchyClassId = 11;

            // Then.
            Assert.AreEqual(expectedAction, result.RouteValues["action"]);
            Assert.AreEqual(expectedController, result.RouteValues["controller"]);
            Assert.AreEqual(expectedHierarchyClassId, result.RouteValues["hierarchyClassId"]);
        }

        [TestMethod]
        public void EditGet_HierarchyClassIdIsFinancial_ShouldRedirectToHierarchyIndexAction()
        {
            // Given.
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Financial).WithHierarchyClassId(11).Build();
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(hierarchyClass);

            // When.
            var result = controller.Edit(11) as RedirectToRouteResult;
            string expectedController = "Hierarchy";
            string expectedAction = "Index";

            // Then.
            Assert.AreEqual(expectedAction, result.RouteValues["action"]);
            Assert.AreEqual(expectedController, result.RouteValues["controller"]);
        }

        [TestMethod]
        public void Edit_ValidParameters_ViewResultPopulated()
        {
            // Given.
            var hierarchyClass = GetFakeHierarchyClass();
            hierarchyClass.Hierarchy.hierarchyName = HierarchyNames.Merchandise;
            hierarchyClass.hierarchyLevel = 5;

            var hierarchyList = GetFakeHierarchy();
            var hierarchy = hierarchyList.FirstOrDefault();
            hierarchy.hierarchyName = HierarchyNames.Financial;

            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(hierarchyClass);
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchyList);

            HierarchyClassViewModel expectedModel = new HierarchyClassViewModel
            {
                HierarchyId = hierarchyClass.hierarchyID,
                HierarchyName = hierarchyClass.Hierarchy.hierarchyName,
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                HierarchyClassName = hierarchyClass.hierarchyClassName,
                HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass),
                HierarchyLevel = hierarchyClass.hierarchyLevel,
                TaxAbbreviation = HierarchyClassAccessor.GetTaxAbbreviation(hierarchyClass),
                SubTeam = HierarchyClassAccessor.GetSubTeam(hierarchyClass),
                NonMerchandiseTrait = HierarchyClassAccessor.GetNonMerchandiseTrait(hierarchyClass)
            };

            expectedModel.SubTeamList = GetFakeHierarchyDropDown(hierarchy);
            expectedModel.SelectedSubTeam = 0;

            // When.
            ViewResult result = controller.Edit(1) as ViewResult;
            var actualModel = result.Model as HierarchyClassViewModel;

            // Then.
            Assert.AreEqual(expectedModel.HierarchyId, actualModel.HierarchyId, "Expected HierarchyId did not match actual HierarchyId");
            Assert.AreEqual(expectedModel.HierarchyName, actualModel.HierarchyName, "Expected HierarchyName did not match actual HierarchyName");
            Assert.AreEqual(expectedModel.HierarchyClassId, actualModel.HierarchyClassId, "Expected HierarchyClassId did not match actual HierarchyClassId");
            Assert.AreEqual(expectedModel.HierarchyClassName, actualModel.HierarchyClassName, "Expected HierarchyClassName did not match actual HierarchyClassName");
            Assert.AreEqual(expectedModel.HierarchyParentClassId, actualModel.HierarchyParentClassId, "Expected HierarchyParentClassId did not match actual HierarchyParentClassId");
            Assert.AreEqual(expectedModel.HierarchyParentClassName, actualModel.HierarchyParentClassName, "Expected HierarchyParentClassName did not match actual HierarchyParentClassName");
            Assert.AreEqual(expectedModel.HierarchyLevel, actualModel.HierarchyLevel, "Expected HierarchyLevel did not match actual HierarchyLevel");
            Assert.AreEqual(expectedModel.TaxAbbreviation, actualModel.TaxAbbreviation, "Expected TaxAbbreviation did not match actual TaxAbbreviation");
            Assert.AreEqual(expectedModel.SubTeam, actualModel.SubTeam, "Expected SubTeam did not match actual SubTeam");
            Assert.AreEqual(expectedModel.NonMerchandiseTrait, actualModel.NonMerchandiseTrait, "Expected NonMerchandiseTrait did not match actual NonMerchandiseTrait");
        }

        [TestMethod]
        public void Edit_InvalidParameters_MethodShouldRedirectToIndex()
        {
            // Given.

            // When.
            var negativeClassIdResult = controller.Edit(-1) as RedirectToRouteResult;
            var zeroClassIdResult = controller.Edit(0) as RedirectToRouteResult;

            string routeName = "Index";

            // Then.
            Assert.AreEqual(routeName, negativeClassIdResult.RouteValues["action"]);
            Assert.AreEqual(routeName, zeroClassIdResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Edit_ValidModelState_CommandShouldBeExecutedAndHierarchCacheShouldBeCleared()
        {
            // Given.
            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            ViewResult result = controller.Edit(viewModel) as ViewResult;

            // Then.
            mockUpdateManager.Verify(command => command.Execute(It.IsAny<UpdateHierarchyClassManager>()), Times.Once);
            mockCacheManager.Verify(x => x.ClearCacheForController(It.Is<string>(a => a == "HierarchyClass")));
        }

        [TestMethod]
        public void Edit_InvalidModelState_ReturnViewWithViewModel()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");
            HierarchyClassViewModel expectedModel = GetFakeHierarchyClassViewModel(GetFakeHierarchyClass());
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            // When.
            ViewResult result = controller.Edit(expectedModel) as ViewResult;
            var actualModel = result.Model as HierarchyClassViewModel;

            // Then.
            Assert.AreEqual(expectedModel.HierarchyId, actualModel.HierarchyId, "Expected HierarchyId did not match actual HierarchyId");
            Assert.AreEqual(expectedModel.HierarchyName, actualModel.HierarchyName, "Expected HierarchyName did not match actual HierarchyName");
            Assert.AreEqual(expectedModel.HierarchyClassId, actualModel.HierarchyClassId, "Expected HierarchyClassId did not match actual HierarchyClassId");
            Assert.AreEqual(expectedModel.HierarchyClassName, actualModel.HierarchyClassName, "Expected HierarchyClassName did not match actual HierarchyClassName");
            Assert.AreEqual(expectedModel.HierarchyParentClassId, actualModel.HierarchyParentClassId, "Expected HierarchyParentClassId did not match actual HierarchyParentClassId");
            Assert.AreEqual(expectedModel.HierarchyParentClassName, actualModel.HierarchyParentClassName, "Expected HierarchyParentClassName did not match actual HierarchyParentClassName");
            Assert.AreEqual(expectedModel.HierarchyLevel, actualModel.HierarchyLevel, "Expected HierarchyLevel did not match actual HierarchyLevel");
            Assert.AreEqual(expectedModel.TaxAbbreviation, actualModel.TaxAbbreviation, "Expected TaxAbbreviation did not match actual TaxAbbreviation");
            Assert.AreEqual(expectedModel.SubTeam, actualModel.SubTeam, "Expected SubTeam did not match actual SubTeam");
            Assert.AreEqual(expectedModel.SelectedSubTeam, actualModel.SelectedSubTeam, "Expected SelectedSubTeam did not match actual SelectedSubTeam");
            Assert.AreEqual(expectedModel.SubTeamList, actualModel.SubTeamList, "Expected SubTeamList did not match actual SubTeamList");
            Assert.AreEqual(expectedModel.NonMerchandiseTrait, actualModel.NonMerchandiseTrait, "Expected NonMerchandiseTrait did not match actual NonMerchandiseTrait");
        }

        [TestMethod]
        public void Edit_InvalidModelState_ViewDataShouldContainErrorMessage()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");
            HierarchyClassViewModel viewModel = GetFakeHierarchyClassViewModel(GetFakeHierarchyClass());
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            var expected = "There was an error during update: test";

            // When.
            ViewResult result = controller.Edit(viewModel) as ViewResult;
            var actual = controller.ViewData["ErrorMessage"].ToString();

            // Then.
            Assert.AreEqual(expected, actual, "Expected ViewData did not match Actual View Data");
        }

        [TestMethod]
        public void Edit_UnexpectedError_ErrorShouldBeLogged()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;

            mockUpdateManager.Setup(c => c.Execute(It.IsAny<UpdateHierarchyClassManager>())).Throws(new CommandException("error"));
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            controller.Edit(viewModel);

            // Then.
            mockLogger.Verify(logger => logger.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Edit_SuccessfulUpdate_TempDataShouldContainSuccessMessage()
        {
            // Given.
            var expected = "Update was successful.";
            var viewModel = GetFakeHierarchyClassViewModel(GetFakeHierarchyClass());

            // When.
            var result = controller.Edit(viewModel) as RedirectToRouteResult;
            var actual = controller.TempData["SuccessMessage"].ToString();

            // Then.
            Assert.AreEqual(expected, actual, "The expected TempData message did not match the actual TempData message.");
        }

        [TestMethod]
        public void Edit_NullTaxAbbreviation_UpdateFailsAndErrorMessageViewData()
        {
            // Given
            var expected = "Tax Abbreviation is required.";
            var viewModel = GetFakeHierarchyClassViewModel(GetFakeHierarchyClass());

            viewModel.HierarchyName = "Tax";
            viewModel.TaxAbbreviation = null;

            // When
            var result = controller.Edit(viewModel) as ViewResult;
            var actual = controller.ViewData["ErrorMessage"].ToString();

            // Then
            Assert.AreEqual(expected, actual, String.Format("The expected error message '{0}' did not match actual error message {1}", expected, actual));
        }

        [TestMethod]
        public void Delete_GetWithValidParameters_ViewResultShouldNotBeNull()
        {
            // Given.
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());

            // When.
            ViewResult nonZeroResult = controller.Delete(1) as ViewResult;

            // Then.
            Assert.IsNotNull(nonZeroResult);
        }

        [TestMethod]
        public void Delete_GetWithValidParameters_Brand_MethodShouldRedirectToIndex()
        {
            // Given.
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(GetFakeHierarchyWithSubclasses());

            var viewModel = new HierarchyClassViewModel
            {
                HierarchyId = Hierarchies.Brands
            };

            // When.
            var nonZeroResult = controller.Delete(viewModel) as RedirectToRouteResult;

            string routeName = "Index";
            string controllerName = "Brand";

            // Then.
            Assert.AreEqual(routeName, nonZeroResult.RouteValues["action"]);
            Assert.AreEqual(controllerName, nonZeroResult.RouteValues["controller"]);
        }

        [TestMethod]
        public void Delete_GetWithValidParameters_Tax_MethodShouldRedirectToIndex()
        {
            // Given.
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(GetFakeHierarchyWithSubclasses());

            var viewModel = new HierarchyClassViewModel
            {
                HierarchyId = Hierarchies.Tax
            };

            // When.
            var nonZeroResult = controller.Delete(viewModel) as RedirectToRouteResult;

            string routeName = "Index";
            string controllerName = "Hierarchy";
            int selectedHierarchyId = Hierarchies.Tax;

            // Then.
            Assert.AreEqual(routeName, nonZeroResult.RouteValues["action"]);
            Assert.AreEqual(controllerName, nonZeroResult.RouteValues["controller"]);
            Assert.AreEqual(selectedHierarchyId, nonZeroResult.RouteValues["SelectedHierarchyId"]);
        }

        [TestMethod]
        public void Delete_GetWithInvalidParameters_MethodShouldRedirectToIndex()
        {
            // Given.
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());

            // When.
            var negativeClassIdResult = controller.Delete(-1) as RedirectToRouteResult;
            var zeroClassIdResult = controller.Delete(0) as RedirectToRouteResult;

            string routeName = "Index";

            // Then.
            Assert.AreEqual(routeName, negativeClassIdResult.RouteValues["action"]);
            Assert.AreEqual(routeName, zeroClassIdResult.RouteValues["action"]);
        }

        [TestMethod]
        public void Delete_PostWithValidModelState_CommandShouldBeExecutedAndHierarchyCacheShouldBeCleared()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());

            // When.
            ViewResult result = controller.Delete(viewModel) as ViewResult;

            // Then.
            mockDeleteHierarchyClassManager.Verify(command => command.Execute(It.IsAny<DeleteHierarchyClassManager>()), Times.Once);
            mockCacheManager.Verify(x => x.ClearCacheForController(It.Is<string>(a => a == "HierarchyClass")));
        }

        [TestMethod]
        public void Delete_PostWithInvalidModelState_ShouldReturnViewModelObject()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Delete(new HierarchyClassViewModel()) as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_HierarchyClassHasItemAssociations_HierarchyClassShouldNotBeDeleted()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);
            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClassWithItemAssociations());

            // When.
            ViewResult result = controller.Delete(viewModel) as ViewResult;

            // Then.
            Assert.IsNotNull(result.ViewData["ErrorMessage"]);
        }

        [TestMethod]
        public void Delete_HierarchyClassHasSubclasses_HierarchyClassShouldNotBeDeleted()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);
            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClassWithSubclasses());

            // When.
            ViewResult result = controller.Delete(viewModel) as ViewResult;

            // Then.
            Assert.IsNotNull(result.ViewData["ErrorMessage"]);
        }

        [TestMethod]
        public void Delete_PostWithUnexpectedError_ErrorShouldBeLogged()
        {
            // Given.
            var financialHierarchy = GetFakeHierarchy();
            financialHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.Financial;
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(financialHierarchy);
            mockDeleteHierarchyClassManager.Setup(c => c.Execute(It.IsAny<DeleteHierarchyClassManager>())).Throws(new CommandException("error"));
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(GetFakeHierarchyClass());

            HierarchyClassViewModel viewModel = new HierarchyClassViewModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            controller.Delete(viewModel);

            // Then.
            mockLogger.Verify(logger => logger.Error(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void Edit_UserNameIsPopulated_ShouldPassUserNameToManager()
        {
            //When
            controller.Edit(new HierarchyClassViewModel()
            {
                HierarchyClassName = "Test"
            });

            //Then
            mockUpdateManager.Verify(m => m.Execute(It.Is<UpdateHierarchyClassManager>(man => man.UserName == "Test User")));
        }


        [TestMethod]
        public void ComboDataSource_NofilterPassedWithInitialSelectionPassed_SingleMatchingRecordShouldBeReturned()
        {
            // Given. 
            NameValueCollection queryString = new NameValueCollection();
            queryString["initialSelection"] = "1";
            this.mockContext.SetupGet(m => m.HttpContext.Request.QueryString).Returns(queryString);
            controller.ControllerContext = mockContext.Object;

            IEnumerable<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>() { this.GetFakeHierarchyClassModel() };

            var hierarchy = this.GetFakeHierarchy();

            this.mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchy);
            this.mockGetHierarchyClassesQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(hierarchyClasses);

            // When.
            var result = controller.ComboDataSource(1) as ContentResult;
            var model = JsonConvert.DeserializeObject<List<HierarchyClassModel>>(result.Content);

            // Then.
            Assert.AreEqual(1, model.Count);
        }

        /// <summary>
        /// Tests that when searching for hierarchies with a combobox filter calls the search function
        /// with the filter and that the filter is parsed from the contains query.
        /// </summary>
        [TestMethod]
        public void ComboDataSource_FilterPassed_SearchIsCalledWithCorrectFilter()
        {
            // Given.
            NameValueCollection queryString = new NameValueCollection();
            queryString["$filter"] = "indexof(tolower(HierarchyLineage),'test') ge 0";
            this.mockContext.SetupGet(m => m.HttpContext.Request.QueryString).Returns(queryString);
            controller.ControllerContext = mockContext.Object;

            IEnumerable<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>() { this.GetFakeHierarchyClassModel(), this.GetFakeHierarchyClassModel2() };

            var hierarchy = this.GetFakeHierarchy();

            this.mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchy);
            this.mockGetHierarchyClassesQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(hierarchyClasses);

            // When.
            var result = controller.ComboDataSource(1) as ContentResult;
            var model = JsonConvert.DeserializeObject<List<HierarchyClassModel>>(result.Content);

            // Then.
            this.mockGetHierarchyClassesQueryHandler.Verify(x => x.Search(It.Is<GetHierarchyClassesParameters>(v => v.HierarchyLineageFilter == "test")));
        }

        /// <summary>
        /// Tests that when searching for hierarchies with a filter < 2 characters doesn't return anything
        /// </summary>
        [TestMethod]
        public void ComboDataSource_FilterPassed_FilterLessThan2CharactersReturnsNothing()
        {
            // Given.
            NameValueCollection queryString = new NameValueCollection();
            queryString["$filter"] = "indexof(tolower(HierarchyClassName),'f') ge 0";
            this.mockContext.SetupGet(m => m.HttpContext.Request.QueryString).Returns(queryString);
            controller.ControllerContext = mockContext.Object;

            IEnumerable<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>() { this.GetFakeHierarchyClassModel(), this.GetFakeHierarchyClassModel2() };

            var hierarchy = this.GetFakeHierarchy();

            this.mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchy);
            this.mockGetHierarchyClassesQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(hierarchyClasses);

            // When.
            var result = controller.ComboDataSource(1) as ContentResult;
            var model = JsonConvert.DeserializeObject<List<HierarchyClassModel>>(result.Content);

            // Then.
            Assert.AreEqual(0, model.Count);
            this.mockGetHierarchyClassesQueryHandler.Verify(x => x.Search(It.IsAny<GetHierarchyClassesParameters>()), Times.Never);
        }

        /// <summary>
        /// Tests that when searching for hierarchies with a filter > 1 characters returns something
        /// </summary>
        [TestMethod]
        public void ComboDataSource_FilterPassed_FilterGreaterThan1CharactersReturnsSomething()
        {
            // Given.
            NameValueCollection queryString = new NameValueCollection();
            queryString["$filter"] = "indexof(tolower(HierarchyLineage),'fak') ge 0";
            this.mockContext.SetupGet(m => m.HttpContext.Request.QueryString).Returns(queryString);
            controller.ControllerContext = mockContext.Object;

            IEnumerable<HierarchyClassModel> hierarchyClasses = new List<HierarchyClassModel>() { this.GetFakeHierarchyClassModel() };

            var hierarchy = this.GetFakeHierarchy();

            this.mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchy);
            this.mockGetHierarchyClassesQueryHandler.Setup(q => q.Search(It.IsAny<GetHierarchyClassesParameters>())).Returns(hierarchyClasses);

            // When.
            var result = controller.ComboDataSource(1) as ContentResult;
            var model = JsonConvert.DeserializeObject<List<HierarchyClassModel>>(result.Content);

            // Then.
            this.mockGetHierarchyClassesQueryHandler.Verify(x => x.Search(It.IsAny<GetHierarchyClassesParameters>()));
        }

        private List<Hierarchy> GetFakeHierarchy()
        {
            List<Hierarchy> hierarchy = new List<Hierarchy>();
            hierarchy.Add(new Hierarchy
            {
                hierarchyID = 5,
                hierarchyName = HierarchyNames.Merchandise,
                HierarchyClass = new List<HierarchyClass> { GetFakeHierarchyClass() },
                HierarchyPrototype = GetFakeHierarchyPrototypeList()
            });
            return hierarchy;
        }

        private List<Hierarchy> GetFakeHierarchyWithItemAssociations()
        {
            List<Hierarchy> hierarchy = new List<Hierarchy>();
            hierarchy.Add(new Hierarchy
            {
                hierarchyID = 1,
                hierarchyName = "Fake Hierarchy 1",
                HierarchyClass = new List<HierarchyClass> { GetFakeHierarchyClassWithItemAssociations() }
            });
            return hierarchy;
        }

        private List<Hierarchy> GetFakeHierarchyWithSubclasses()
        {
            List<Hierarchy> hierarchy = new List<Hierarchy>();
            hierarchy.Add(new Hierarchy
            {
                hierarchyID = 1,
                hierarchyName = "Fake Hierarchy 1",
                HierarchyClass = new List<HierarchyClass> { GetFakeHierarchyClassWithSubclasses() }
            });
            return hierarchy;
        }

        private HierarchyClass GetFakeHierarchyClass(int hierarchyId = 1)
        {
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = hierarchyId,
                hierarchyClassID = 1,
                hierarchyParentClassID = null,
                hierarchyLevel = HierarchyLevels.Gs1Brick,
                hierarchyClassName = "Fake Hierarchy Class Name",
                Hierarchy = new Hierarchy
                {
                    hierarchyID = 1,
                    hierarchyName = HierarchyNames.Merchandise,
                    HierarchyPrototype = GetFakeHierarchyPrototypeList()
                },
                HierarchyPrototype = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 1, hierarchyLevelName = "Level1", itemsAttached = true }
            };

            return hierarchyClass;
        }

        private HierarchyClassModel GetFakeHierarchyClassModel()
        {
            HierarchyClassModel hierarchyClass = new HierarchyClassModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 1,
                HierarchyParentClassId = null,
                HierarchyLevel = HierarchyLevels.Gs1Brick,
                HierarchyClassName = "Fake Hierarchy Class Name",
                HierarchyLineage = "Lineage"
            };

            return hierarchyClass;
        }

        private HierarchyClassModel GetFakeHierarchyClassModel2()
        {
            HierarchyClassModel hierarchyClass = new HierarchyClassModel()
            {
                HierarchyId = 1,
                HierarchyClassId = 2,
                HierarchyParentClassId = null,
                HierarchyLevel = HierarchyLevels.Gs1Brick,
                HierarchyClassName = "Fake Hierarchy Class Name 2",
                HierarchyLineage = "Lineage 2"
            };

            return hierarchyClass;
        }


        private List<HierarchyPrototype> GetFakeHierarchyPrototypeList()
        {
            List<HierarchyPrototype> prototype = new List<HierarchyPrototype>();
            HierarchyPrototype prototypeLevel1 = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 1, hierarchyLevelName = "Level1", itemsAttached = true };
            HierarchyPrototype prototypeLevel2 = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 2, hierarchyLevelName = "Level2", itemsAttached = true };
            HierarchyPrototype prototypeLevel3 = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 3, hierarchyLevelName = "Level3", itemsAttached = true };
            HierarchyPrototype prototypeLevel4 = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 4, hierarchyLevelName = "Level4", itemsAttached = true };
            HierarchyPrototype prototypeLevel5 = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 5, hierarchyLevelName = "Level5", itemsAttached = true };

            prototype.Add(prototypeLevel1);
            prototype.Add(prototypeLevel2);
            prototype.Add(prototypeLevel3);
            prototype.Add(prototypeLevel4);
            prototype.Add(prototypeLevel5);

            return prototype;
        }

        private HierarchyClass GetFakeHierarchyClassWithItemAssociations()
        {
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = 1,
                hierarchyClassID = 1,
                hierarchyParentClassID = null,
                hierarchyLevel = null,
                hierarchyClassName = "Fake Hierarchy Class Name",
                Hierarchy = new Hierarchy() { hierarchyID = 1, hierarchyName = "Fake Hierarchy 1" },
                ItemHierarchyClass = new List<ItemHierarchyClass> { new ItemHierarchyClass() }
            };

            return hierarchyClass;
        }

        private HierarchyClass GetFakeHierarchyClassWithSubclasses()
        {
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = 1,
                hierarchyClassID = 1,
                hierarchyParentClassID = 1,
                hierarchyLevel = null,
                hierarchyClassName = "Fake Hierarchy Class Name",
                Hierarchy = new Hierarchy() { hierarchyID = 1, hierarchyName = "Fake Hierarchy 1" },
                ItemHierarchyClass = new List<ItemHierarchyClass> { new ItemHierarchyClass() }
            };

            return hierarchyClass;
        }

        private List<SelectListItem> GetFakeHierarchyDropDown(Hierarchy hierarchy)
        {
            var dropDown = hierarchy.HierarchyClass.Select(hc => new SelectListItem { Text = hc.hierarchyClassName, Value = hc.hierarchyClassID.ToString() }).ToList();
            dropDown.Add(new SelectListItem { Selected = true, Value = (0).ToString(), Text = String.Empty });

            return dropDown;
        }

        private HierarchyClassViewModel GetFakeHierarchyClassViewModel(HierarchyClass hierarchyClass)
        {
            HierarchyClassViewModel model = new HierarchyClassViewModel
            {
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                HierarchyClassName = hierarchyClass.hierarchyClassName,
                HierarchyId = hierarchyClass.hierarchyID,
                HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass),
                HierarchyLevel = hierarchyClass.hierarchyLevel,
                HierarchyName = hierarchyClass.Hierarchy.hierarchyName,
                SelectedSubTeam = 0,
                SubTeam = null,
                TaxAbbreviation = null,
                SubTeamList = GetFakeHierarchyDropDown(GetFakeHierarchy().FirstOrDefault())
            };

            return model;
        }
    }
}
