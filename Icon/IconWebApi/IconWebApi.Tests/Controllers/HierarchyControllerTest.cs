using Microsoft.VisualStudio.TestTools.UnitTesting;
using IconWebApi.Controllers;
using System.Collections.Generic;
using IconWebApi.DataAccess.Models;
using Moq;
using IconWebApi.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Logging;
using System.Web.Http.Results;
using System.Linq;

namespace IconWebApi.Tests.Controllers
{
    [TestClass]
    public class HierarchyControllerTest
    {
        private HierarchyController controller;
        private Mock<IQueryHandler<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>>> mockGetContactsByHierarchyClassIdsQueryhandler;
        private Mock<ILogger> mockLogger;
        private IEnumerable<AssociatedContact> testAssociatedContactModels = new HashSet<AssociatedContact>();

        [TestInitialize]
        public void InitializeTests()
        {
            this.mockLogger = new Mock<ILogger>();

            this.mockGetContactsByHierarchyClassIdsQueryhandler = new Mock<IQueryHandler<GetContactsByHierarchyClassIdsQuery, IEnumerable<AssociatedContact>>>();

            this.controller = new HierarchyController(
                this.mockGetContactsByHierarchyClassIdsQueryhandler.Object,
                this.mockLogger.Object);
            testAssociatedContactModels = BuildAssociatedContactModels();
        }

        [TestMethod]
        public void HierarchyController_hierarchyClassIdsListIsNull_ReturnsBadRequestResponse()
        {
            // Given
            List<int> hierarchyClassIds = null;
            var expectedMessage = "HierarchyClassIds are requried.";

            // When
            var result = this.controller.Contacts(hierarchyClassIds) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void HierarchyController_InvalidhierarchyClassIdsList_ReturnsInvalidHierarchIdErrorMessage()
        {
            // Given
            List<int> hierarchyClassIds = new List<int>() { -1, 0 };
            var expectedMessage = "No Brands found matching this hierarchy class ID.";

            // When
            var result = (this.controller.Contacts(hierarchyClassIds) as JsonResult<List<AssociatedContact>>).Content.ToList();

            // Then
            Assert.AreEqual(expectedMessage, result[0].ErrorMessage, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void HierarchyController_ValidhierarchyClassIdsList_ReturnContacts()
        {
            // Given
            var expectedNoBrandFoundMessage = "No Brands found matching this hierarchy class ID.";
            var expectedNoContactsFoundMessage = "No contacts found.";
            var expectedHierarchyClassName = "Test";

            List<int> hierarchyClassIds = new List<int>() { 1, 2, 3, 4 };
            this.mockGetContactsByHierarchyClassIdsQueryhandler.Setup(s => s.Search(It.IsAny<GetContactsByHierarchyClassIdsQuery>()))
                .Returns(testAssociatedContactModels);

            // When
            var result = (this.controller.Contacts(hierarchyClassIds) as JsonResult<List<AssociatedContact>>).Content.ToList();

            // Then
            Assert.AreEqual(4, result.Count);
            Assert.AreEqual(expectedHierarchyClassName, result[0].HierarchyClassName, "The actual message did not match the expected message.");
            Assert.AreEqual(expectedNoContactsFoundMessage, result[1].ErrorMessage, "The actual message did not match the expected message.");
            Assert.AreEqual(expectedNoContactsFoundMessage, result[2].ErrorMessage, "The actual message did not match the expected message.");
            Assert.AreEqual(expectedNoBrandFoundMessage, result[3].ErrorMessage, "The actual message did not match the expected message.");
        }

        private IEnumerable<AssociatedContact> BuildAssociatedContactModels()
        {
            var testAssociatedContactModels = new HashSet<AssociatedContact>();

            testAssociatedContactModels.Add(new AssociatedContact
            {
                HierarchyClassId = 1,
                HierarchyClassName = "Test",
                ContactEmail = "TestContact",
                ContactName = "TestContactName",
                ErrorMessage = ""
            });

            testAssociatedContactModels.Add(new AssociatedContact
            {
                HierarchyClassId = 2,
                HierarchyClassName = null,
                ContactEmail = null,
                ContactName = null,
                ErrorMessage = ""
            });

            testAssociatedContactModels.Add(new AssociatedContact
            {
                HierarchyClassId = 3,
                HierarchyClassName = "Test",
                ContactEmail = null,
                ContactName = null,
                ErrorMessage = ""
            });

            return testAssociatedContactModels;
        }
    }
}