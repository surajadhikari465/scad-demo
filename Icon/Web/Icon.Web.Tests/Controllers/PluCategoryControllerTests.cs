using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Common;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Tests.Common.Builders;
using Infragistics.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass] [Ignore]
    public class PluCategoryControllerTests
    {
        private Mock<ILogger> logger;
        private Mock<IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>>> mockPluCategoriesQuery;
        private Mock<IManagerHandler<UpdatePluCategoryManager>> mockUpdateCommand;
        private Mock<IManagerHandler<AddPluCategoryManager>> mockAddCommand;
        private Mock<IQueryHandler<GetPluCategoryByIdParameters, PLUCategory>> mockGetPluCageroyByIdQuery;
        private Mock<ICommandHandler<DeletePluCategoryByIdCommand>> mockDeletePluCategoryByIdCommand;
        
        private PluCategoryController controller;
        private List<PluCategoryViewModel> selectedRows;
        private Mock<HttpSessionStateBase> mockSession;
        private Mock<ControllerContext> mockContext;
        private PluCategoryGridViewModel viewModel;
        private List<PLUCategory> pluCategories;
        private PLUCategory pluCategory;        
        private string testUser = "TestUser";
        
        [TestInitialize]
        public void InitializeData()
        {
            logger = new Mock<ILogger>();
            mockPluCategoriesQuery = new Mock<IQueryHandler<GetPluCategoriesParameters, List<PLUCategory>>>();
            mockAddCommand = new Mock<IManagerHandler<AddPluCategoryManager>>();
            mockUpdateCommand = new Mock<IManagerHandler<UpdatePluCategoryManager>>();
            mockSession = new Mock<HttpSessionStateBase>();
            mockContext = new Mock<ControllerContext>();
            mockDeletePluCategoryByIdCommand = new Mock<ICommandHandler<DeletePluCategoryByIdCommand>>();
            mockGetPluCageroyByIdQuery = new Mock<IQueryHandler<GetPluCategoryByIdParameters, PLUCategory>>();
            controller = new PluCategoryController(logger.Object,
                mockPluCategoriesQuery.Object,
                mockAddCommand.Object,
                mockUpdateCommand.Object,
                mockGetPluCageroyByIdQuery.Object, mockDeletePluCategoryByIdCommand.Object);

            //Setup up Username
            mockContext.SetupGet(c => c.HttpContext.User.Identity.Name).Returns(testUser);
            controller.ControllerContext = mockContext.Object;

            // Setup Selected Row Data used for controller actions returning Json results
            selectedRows = new List<PluCategoryViewModel>
            {
                new TestPluCategoryBuilder().WithPluCategoryID(1).WithPluCategoryName("Cat1"),
                new TestPluCategoryBuilder().WithPluCategoryID(2).WithPluCategoryName("Cat2"),
                new TestPluCategoryBuilder().WithPluCategoryID(3).WithPluCategoryName("Cat3")
            };

            // Setup Mock Session
            mockSession.SetupSet(s => s["grid_search_results"] = new List<ItemViewModel>());
            mockContext.Setup(c => c.HttpContext.Session).Returns(mockSession.Object);
            controller.ControllerContext = mockContext.Object;

            // Setup Data for Search Method
            viewModel = new PluCategoryGridViewModel();
            pluCategories = new List<PLUCategory>();

            this.pluCategory = new TestPluCategoryBuilder().WithPluCategoryID(1).WithPluCategoryName("Cat1");

            pluCategories.Add(pluCategory);

            mockPluCategoriesQuery.Setup(iq => iq.Search(It.IsAny<GetPluCategoriesParameters>())).Returns(pluCategories);            
        }

        [TestMethod]
        public void Index_Get_ReturnsPartialViewResultWithViewModelListPopulated()
        {
            // Given
            var expected = pluCategory;

            // When
            var result = controller.Index() as ViewResult;
            var model = result.Model as PluCategoryGridViewModel;
            var actual = model.PluCategories;

            // Then
            Assert.AreEqual(1, actual.Count());
            Assert.AreEqual(expected.PluCategoryID, actual.Select(i => i.PluCategoryId).FirstOrDefault(), "PluCategoryID mismatch");
            Assert.AreEqual(expected.PluCategoryName, actual.Select(i => i.PluCategoryName).FirstOrDefault(), "PluCategoryName mismatch");
            Assert.AreEqual(expected.BeginRange, actual.Select(i => i.BeginRange).FirstOrDefault(), "BeginRange mismatch");
            Assert.AreEqual(expected.EndRange, actual.Select(i => i.EndRange).FirstOrDefault(), "EndRange mismatch");            
        }

        [TestMethod]
        public void Create_Get_ShouldReturnAnEmptyPluCategoryViewModel()
        {
            //When
            var result = controller.Create() as ViewResult;
            var model = result.Model as PluCategoryViewModel;

            //Then
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.PluCategoryId);
            Assert.IsNull(model.BeginRange);
            Assert.IsNull(model.EndRange);
            Assert.IsNull(model.PluCategoryName);
        }
        
        [TestMethod]
        public void Create_PostWithValidViewModel_DefaultViewShouldBeReturnedWithSuccessMessage()
        {
            // Given.
            var viewModel = new PluCategoryViewModel
            {
                PluCategoryName = "Test",
                BeginRange = "15",
                EndRange = "20"
            };

            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedSuccessMessage = String.Format("Successfully added category {0}.", "Test");

            mockAddCommand.Verify(t => t.Execute(It.IsAny<AddPluCategoryManager>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(expectedSuccessMessage, viewData["SuccessMessage"]);
        }

        [TestMethod]
        public void Create_UpdateIsNotSuccessful_DefaultViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            string errorMessage = "An unexpected error occurred.";
            mockAddCommand.Setup(t => t.Execute(It.IsAny<AddPluCategoryManager>())).Throws(new CommandException(errorMessage));
                      
            var viewModel = new PluCategoryViewModel
            {
                PluCategoryName = "Test",
                BeginRange = "15",
                EndRange = "20"
            };
            
            // When.
            var result = controller.Create(viewModel) as ViewResult;

            // Then.
            var viewData = result.ViewData;
            string expectedErrorMessage = "Error: An unexpected error occurred.";

            mockAddCommand.Verify(t => t.Execute(It.IsAny<AddPluCategoryManager>()), Times.Once);
            logger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(expectedErrorMessage, viewData["ErrorMessage"]);
        }

        [TestMethod]
        public void SaveChangesInGrid_SuccessfulUpdate_ShouldReturnSuccessJsonResult()
        {
            //Given
            PluCategoryViewModel testPluCategory = new TestPluCategoryBuilder();
            List<Transaction<PluCategoryViewModel>> pluViewModels = new List<Transaction<PluCategoryViewModel>>
            {
                new Transaction<PluCategoryViewModel> { row = testPluCategory }
            };
            mockContext.SetupGet(m => m.HttpContext.Request.Form)
                .Returns(new NameValueCollection 
                {
                    { "ig_transactions", new JavaScriptSerializer().Serialize(pluViewModels) }
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.IsTrue(Convert.ToBoolean(result.GetDataProperty("Success")));
            mockUpdateCommand.Verify(m => m.Execute(It.IsAny<UpdatePluCategoryManager>()), Times.Once);
        }
        
        [TestMethod]
        public void SaveChangesInGrid_InvalidPluRange_ShouldReturnUnsuccessfulJsonResult()
        {
            //Given
            PluCategoryViewModel testPluCategory = new TestPluCategoryBuilder()
                .WithBeginRange("5")
                .WithEndRange("1");
            List<Transaction<PluCategoryViewModel>> pluViewModels = new List<Transaction<PluCategoryViewModel>>
            {
                new Transaction<PluCategoryViewModel> { row = testPluCategory }
            };
            mockContext.SetupGet(m => m.HttpContext.Request.Form)
                .Returns(new NameValueCollection 
                {
                    { "ig_transactions", new JavaScriptSerializer().Serialize(pluViewModels) }
                });

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.IsFalse(Convert.ToBoolean(result.GetDataProperty("Success")));
            Assert.AreEqual("Category Range exception: PLU Category start value must be less than end value.", result.GetDataProperty("Error"));
            mockUpdateCommand.Verify(m => m.Execute(It.IsAny<UpdatePluCategoryManager>()), Times.Never);
        }

        [TestMethod]
        public void SaveChangesInGrid_ErrorThrownWhenUpdating_ShouldReturnUnsuccessfulJsonResult()
        {
            //Given
            PluCategoryViewModel testPluCategory = new TestPluCategoryBuilder();
            List<Transaction<PluCategoryViewModel>> pluViewModels = new List<Transaction<PluCategoryViewModel>>
            {
                new Transaction<PluCategoryViewModel> { row = testPluCategory }
            };
            mockContext.SetupGet(m => m.HttpContext.Request.Form)
                .Returns(new NameValueCollection 
                {
                    { "ig_transactions", new JavaScriptSerializer().Serialize(pluViewModels) }
                });
            mockUpdateCommand.Setup(m => m.Execute(It.IsAny<UpdatePluCategoryManager>()))
                .Throws(new CommandException("Test Exception"));

            //When
            var result = controller.SaveChangesInGrid() as JsonResult;

            //Then
            Assert.IsFalse(Convert.ToBoolean(result.GetDataProperty("Success")));
            Assert.AreEqual("Test Exception", result.GetDataProperty("Error"));
            mockUpdateCommand.Verify(m => m.Execute(It.IsAny<UpdatePluCategoryManager>()), Times.Once);
        }

        [TestMethod]
        public void PluCategoryControllerDeleteGet_InitialPageLoad_PluCategoryInformationShouldBeDisplayed()
        {
            mockGetPluCageroyByIdQuery.Setup(q => q.Search(It.IsAny<GetPluCategoryByIdParameters>())).Returns(pluCategory);

            // When.
            var result = controller.Delete(pluCategory.PluCategoryID) as ViewResult;

            // Then.
            var viewModel = result.Model as PluCategoryViewModel;

            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(viewModel.PluCategoryId, pluCategory.PluCategoryID);
            Assert.AreEqual(viewModel.PluCategoryName, pluCategory.PluCategoryName);
        }

        [TestMethod]
        public void PluCategoryControllerEditPost_PluCategoryInformationIsNotUpdatedSuccessfully_DefaultViewShouldBeReturnedWithErrorMessage()
        {
            // Given.
            string errorMessage = "Error: There was a problem with applying this delete on the database.";
            mockDeletePluCategoryByIdCommand.Setup(t => t.Execute(It.IsAny<DeletePluCategoryByIdCommand>())).Throws(new CommandException(errorMessage));

            var viewModel = new PluCategoryViewModel
            {
                PluCategoryId = pluCategory.PluCategoryID,
                PluCategoryName = pluCategory.PluCategoryName
            };

            // When.
            var result = controller.Delete(viewModel) as ViewResult;

            // Then.
            var returnedViewModel = result.Model as PluCategoryViewModel;
            var viewData = result.ViewData;

            mockDeletePluCategoryByIdCommand.Verify(t => t.Execute(It.IsAny<DeletePluCategoryByIdCommand>()), Times.Once);
            Assert.AreEqual(result.ViewName, String.Empty);
            Assert.AreEqual(errorMessage, viewData["ErrorMessage"]);
        }

    }
}
