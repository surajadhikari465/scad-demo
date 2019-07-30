using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.Controllers;
using MammothWebApi.DataAccess.Models.DataMonster;
using MammothWebApi.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NLog;

namespace MammothWebApi.Tests.Controllers
{
    [TestClass]
    public class ItemControllerTests
    {

        private Mock<IQueryHandler<GetItemsQuery, ItemComposite>> mockGetItemsQueryHandler;
        private Mock<IQueryHandler<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>>> mockGetItemsBySearchCriteriaQueryHandler;
        private ItemController itemController;

        [TestInitialize]
        public void InitializeTest()
        {
            
            this.mockGetItemsQueryHandler = new Mock<IQueryHandler<GetItemsQuery, ItemComposite>>();
            this.mockGetItemsBySearchCriteriaQueryHandler = new Mock<IQueryHandler<GetItemsBySearchCriteriaQuery, IEnumerable<ItemDetail>>>();
            this.itemController = new ItemController(this.mockGetItemsQueryHandler.Object, this.mockGetItemsBySearchCriteriaQueryHandler.Object);
        }

        [TestMethod]
        public void ItemController_GetValidItem_ReturnsData()
        {
            //given 
            var scanCodes = new List<string> {"4011"};
            
            // when
            var response = this.itemController.GetItemsByScanCodes(scanCodes);

            //then 
            Assert.IsNotNull(response, "The OkResult response is null.");
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<ItemComposite>));
        }
    }
}
