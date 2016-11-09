using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Icon.Web.Common;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
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
    public class NationalClassControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>> mockHierarchyClassQuery;
        private Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> mockGetHierarchyQuery;
        private Mock<IManagerHandler<DeleteNationalHierarchyManager>> mockDeleteManager;
        private Mock<IManagerHandler<AddNationalHierarchyManager>> mockAddManager;
        private Mock<IManagerHandler<UpdateNationalHierarchyManager>> mockUpdateManager;
        private Mock<ControllerContext> mockContext;
        private NationalClassController controller;
        private Mock<ICommandHandler<ClearHierarchyClassCacheCommand>> mockClearHierarchyClassCacheCommandHandler;

        [TestInitialize]
        public void Initialize()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockHierarchyClassQuery = new Mock<IQueryHandler<GetHierarchyClassByIdParameters, HierarchyClass>>();
            this.mockGetHierarchyQuery = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            this.mockDeleteManager = new Mock<IManagerHandler<DeleteNationalHierarchyManager>>();
            this.mockAddManager = new Mock<IManagerHandler<AddNationalHierarchyManager>>();
            this.mockUpdateManager = new Mock<IManagerHandler<UpdateNationalHierarchyManager>>();
            this.mockContext = new Mock<ControllerContext>();
            this.mockClearHierarchyClassCacheCommandHandler = new Mock<ICommandHandler<ClearHierarchyClassCacheCommand>>();

            this.controller = new NationalClassController(mockLogger.Object,
                mockGetHierarchyQuery.Object,
                mockHierarchyClassQuery.Object,
                mockUpdateManager.Object,
                mockAddManager.Object,
                mockDeleteManager.Object);

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
            ViewResult nonZeroResult = controller.Create(1) as ViewResult;
            
            // Then.
            Assert.IsNotNull(nonZeroResult);
        }
       

        [TestMethod]
        public void Create_ValidParametersParentIdNonZeroMerch_ClassLevelIsPopulated()
        {
            // Given.
            var natHierarchy = GetFakeHierarchy();
            var natHierarchyClass = GetFakeHierarchyClass();
            natHierarchyClass.Hierarchy.hierarchyName = HierarchyNames.National;
            natHierarchyClass.hierarchyLevel = 3;

            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(natHierarchy);
            mockHierarchyClassQuery.Setup(q => q.Search(It.IsAny<GetHierarchyClassByIdParameters>())).Returns(natHierarchyClass);

            NationalClassViewModel expectedModel = new NationalClassViewModel();
            expectedModel.HierarchyLevel = natHierarchyClass.hierarchyLevel + 1;
            
            // When.
            ViewResult zeroParentIdResult = controller.Create(1) as ViewResult;
            var actualModel = zeroParentIdResult.Model as NationalClassViewModel;

            // Then.
             Assert.AreEqual(expectedModel.HierarchyLevel, actualModel.HierarchyLevel);                
        }
                
        [TestMethod]
        public void Create_ValidModelState_CommandShouldBeExecuted()
        {
            // Given.
            NationalClassViewModel viewModel = new NationalClassViewModel()
            {
                HierarchyId = 6,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            ViewResult result = controller.Create(viewModel) as ViewResult;

            // Then.
            mockAddManager.Verify(command => command.Execute(It.IsAny<AddNationalHierarchyManager>()), Times.Once);
        }

        [TestMethod]
        public void Create_InvalidModelState_ShouldReturnViewModelObject()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Create(new NationalClassViewModel()) as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create_DuplicateHierarchyClass_ViewDataShouldContainErrorMessage()
        {
            // Given.
            var natHierarchy = GetFakeHierarchy();
            natHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.National;

            mockAddManager.Setup(c => c.Execute(It.IsAny<AddNationalHierarchyManager>())).Throws(new CommandException("error"));
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(natHierarchy);

            NationalClassViewModel viewModel = new NationalClassViewModel()
            {
                HierarchyId = 6,
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
            var natHierarchy = GetFakeHierarchy();
            natHierarchy.FirstOrDefault().hierarchyName = HierarchyNames.National;

            mockAddManager.Setup(c => c.Execute(It.IsAny<AddNationalHierarchyManager>())).Throws(new CommandException("error"));
            mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(natHierarchy);

            NationalClassViewModel viewModel = new NationalClassViewModel()
            {
                HierarchyId = 6,
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
        public void Edit_ValidModelState_CommandShouldBeExecuted()
        {
            // Given.
            NationalClassViewModel viewModel = new NationalClassViewModel()
            {
                HierarchyId = 2,
                HierarchyClassId = 1,
                HierarchyParentClassId = 2,
                HierarchyClassName = "Test",
                HierarchyLevel = 1
            };

            // When.
            ViewResult result = controller.Edit(viewModel) as ViewResult;

            // Then.
            mockUpdateManager.Verify(command => command.Execute(It.IsAny<UpdateNationalHierarchyManager>()), Times.Once);
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
        public void Delete_PostWithValidModelState_CommandShouldBeExecuted()
        {
            // Given.
            NationalClassViewModel viewModel = new NationalClassViewModel()
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
            mockDeleteManager.Verify(command => command.Execute(It.IsAny<DeleteNationalHierarchyManager>()), Times.Once);
        }

        [TestMethod]
        public void Delete_PostWithInvalidModelState_ShouldReturnViewModelObject()
        {
            // Given.
            controller.ModelState.AddModelError("test", "test");

            // When.
            ViewResult result = controller.Delete(new NationalClassViewModel()) as ViewResult;

            // Then.
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Delete_HierarchyClassHasItemAssociations_HierarchyClassShouldNotBeDeleted()
        {
            // Given.
            NationalClassViewModel viewModel = new NationalClassViewModel()
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
            NationalClassViewModel viewModel = new NationalClassViewModel()
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
               

        private List<Hierarchy> GetFakeHierarchy()
        {
            List<Hierarchy> hierarchy = new List<Hierarchy>();
            hierarchy.Add(new Hierarchy
                {
                    hierarchyID = 6,
                    hierarchyName = HierarchyNames.National,
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
                hierarchyID = 6,
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
                hierarchyID = 6,
                hierarchyName = "Fake Hierarchy 1",
                HierarchyClass = new List<HierarchyClass> { GetFakeHierarchyClassWithSubclasses() }
            });
            return hierarchy;
        }

        private HierarchyClass GetFakeHierarchyClass()
        {
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = 6,
                hierarchyClassID = 1,
                hierarchyParentClassID = null,
                hierarchyLevel = HierarchyLevels.Gs1Brick,
                hierarchyClassName = "Fake Hierarchy Class Name",
                Hierarchy = new Hierarchy 
                    { 
                        hierarchyID = 6, 
                        hierarchyName = HierarchyNames.Merchandise, 
                        HierarchyPrototype = GetFakeHierarchyPrototypeList()
                    },
                HierarchyPrototype = new HierarchyPrototype { hierarchyID = 1, hierarchyLevel = 1, hierarchyLevelName = "Level1", itemsAttached = true}
            };

            return hierarchyClass;
        }

        private List<HierarchyPrototype> GetFakeHierarchyPrototypeList()
        {
            List<HierarchyPrototype> prototype = new List<HierarchyPrototype>();
            HierarchyPrototype prototypeLevel1 = new HierarchyPrototype { hierarchyID = 6, hierarchyLevel = 1, hierarchyLevelName = "Level1", itemsAttached = true };
            HierarchyPrototype prototypeLevel2 = new HierarchyPrototype { hierarchyID = 6, hierarchyLevel = 2, hierarchyLevelName = "Level2", itemsAttached = true };
            HierarchyPrototype prototypeLevel3 = new HierarchyPrototype { hierarchyID = 6, hierarchyLevel = 3, hierarchyLevelName = "Level3", itemsAttached = true };
            HierarchyPrototype prototypeLevel4 = new HierarchyPrototype { hierarchyID = 6, hierarchyLevel = 4, hierarchyLevelName = "Level4", itemsAttached = true };
           
            prototype.Add(prototypeLevel1);
            prototype.Add(prototypeLevel2);
            prototype.Add(prototypeLevel3);
            prototype.Add(prototypeLevel4);

            return prototype;
        }

        private HierarchyClass GetFakeHierarchyClassWithItemAssociations()
        {
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = 6,
                hierarchyClassID = 1,
                hierarchyParentClassID = null,
                hierarchyLevel = 4,
                hierarchyClassName = "Fake Hierarchy Class Name",
                Hierarchy = new Hierarchy() { hierarchyID = 6, hierarchyName = "Fake Hierarchy 1" },
                ItemHierarchyClass = new List<ItemHierarchyClass> { new ItemHierarchyClass() }
            };

            return hierarchyClass;
        }

        private HierarchyClass GetFakeHierarchyClassWithSubclasses()
        {
            HierarchyClass hierarchyClass = new HierarchyClass()
            {
                hierarchyID = 6,
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

        private NationalClassViewModel GetFakeHierarchyClassViewModel(HierarchyClass hierarchyClass)
        {
            NationalClassViewModel model = new NationalClassViewModel
            {
                HierarchyClassId = hierarchyClass.hierarchyClassID,
                HierarchyClassName = hierarchyClass.hierarchyClassName,
                HierarchyId = hierarchyClass.hierarchyID,
                HierarchyParentClassId = hierarchyClass.hierarchyParentClassID,
                HierarchyParentClassName = HierarchyClassAccessor.GetHierarchyParentName(hierarchyClass),
                HierarchyLevel = hierarchyClass.hierarchyLevel,
                HierarchyName = hierarchyClass.Hierarchy.hierarchyName,
                NationalClassCode = "5"
            };

            return model;
        }
    }
}
