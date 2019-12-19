using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using Icon.Web.Controllers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Extensions;
using Icon.Web.Mvc.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class HierarchyControllerTests
    {
        private Mock<ILogger> mockLogger;
        private Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>> mockGetHierarchyQuery;
        private HierarchyController controller;
        private HierarchySearchViewModel viewModel;
        private Mock<IQueryHandler<GetHierarchiesParameters, IEnumerable<HierarchyModel>>> mockGetHierarchiesQueryHandler;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockGetHierarchyQuery = new Mock<IQueryHandler<GetHierarchyParameters, List<Hierarchy>>>();
            this.mockGetHierarchiesQueryHandler = new Mock<IQueryHandler<GetHierarchiesParameters, IEnumerable<HierarchyModel>>>();
            this.controller = new HierarchyController(
                this.mockLogger.Object, 
                new Mvc.Utility.IconWebAppSettings()
                {
                },
                this.mockGetHierarchyQuery.Object,
                mockGetHierarchiesQueryHandler.Object);
            this.viewModel = new HierarchySearchViewModel();
        }

        [TestMethod]
        public void Index_EmptyHierarchySearchViewModel_ResultViewModelPopulatedWithHierarchies()
        {
            // Given
            List<Hierarchy> hierarchies = new List<Hierarchy>();
            hierarchies.Add(new Hierarchy { hierarchyID = Hierarchies.Merchandise, hierarchyName = "Test Hierarchy 1" });
            hierarchies.Add(new Hierarchy { hierarchyID = Hierarchies.Tax, hierarchyName = "Test Hierarchy 2" });
            this.mockGetHierarchyQuery.Setup(q => q.Search(It.IsAny<GetHierarchyParameters>())).Returns(hierarchies);

            IEnumerable<SelectListItem> hierarchySelectList = hierarchies.Select(h => new DropDownViewModel
                {
                    Id = h.hierarchyID,
                    Name = h.hierarchyName
                })
                .ToSelectListItem();

            // When
            ViewResult result = this.controller.Index(this.viewModel) as ViewResult;
            HierarchySearchViewModel model = result.Model as HierarchySearchViewModel;

            // Then
            Assert.AreEqual(hierarchySelectList.Count(), model.Hierarchies.Count());
        }
    }
}
