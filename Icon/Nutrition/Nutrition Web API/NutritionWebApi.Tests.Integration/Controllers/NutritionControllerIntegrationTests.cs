using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NutritionWebApi.Common.Models;
using NutritionWebApi.Common;
using NutritionWebApi.DataAccess.Commands;
using NutritionWebApi.DataAccess.Queries;
using NutritionWebApi.Controllers;
using NutritionWebApi.Common.Interfaces;
using Icon.Logging;
using NutritionWebApi.Tests.Common.Builders;
using System.Data.SqlClient;
using System.Linq;
using System.Data;
using Dapper;

namespace NutritionWebApi.Tests.Integration.Controllers
{
    [TestClass]
    public class NutritionControllerIntegrationTests
    {
        private IDbConnectionProvider connectionProvider;
        private string connectionString; 
        private IQueryHandler<GetNutritionItemQuery, List<NutritionItemModel>> getNutritionItemQuery;
        private ICommandHandler<AddOrUpdateNutritionItemCommand> updateNutritionItemCommandHandler;
        private ICommandHandler<DeleteNutritionCommand> deleteNutritionCommandHandler;
        private Mock<ILogger> logger;
        private NutritionController controller;

         [TestInitialize]
        public void Initialize()
        {
            this.connectionProvider = new DbConnectionProvider();
            this.connectionString = ApiConfigSettings.Instance.ConnectionString;
            this.connectionProvider.Connection = new SqlConnection(this.connectionString);            
            this.connectionProvider.Connection.Open();

            getNutritionItemQuery = new GetNutritionItemQueryHandler(this.connectionProvider);
            updateNutritionItemCommandHandler = new AddOrUpdateNutritionItemCommandHandler(this.connectionProvider);
						deleteNutritionCommandHandler = new DeleteNutritionCommandHandler(this.connectionProvider);
            logger = new Mock<ILogger>();

            controller = new NutritionController(getNutritionItemQuery, updateNutritionItemCommandHandler, deleteNutritionCommandHandler, logger.Object);
        }
        
        [TestMethod]
        public void NutritionController_Integration_Post_50Items_SuccessfullyInsertsItem()
        {
            // Given.
            List<NutritionItemModel> list = BuildNutritionItemList(50);
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // When
            var response = controller.AddOrUpdateNutritionItem(list) as OkNegotiatedContentResult<string>;

            // Assert  
            Assert.IsNotNull(response);
        }

        private List<NutritionItemModel> BuildNutritionItemList(int count)
        {
            List<NutritionItemModel> itemList = new List<NutritionItemModel>();
            for (int i = 1; i <= count; i++)
            {
                NutritionItemModelBuilder builder = new NutritionItemModelBuilder().WithPlu((i).ToString())
                    .WithRecipeName("Integration Test Recipe " + (i).ToString());
                itemList.Add(builder);
            }
            return itemList;
        }

        [TestMethod]
        public void DeleteNutritionItems_FivePlus_RemovesRecordsFromDatabase()
        {
            // Given.
            var list = BuildNutritionItemList(5);
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            // When
            controller.AddOrUpdateNutritionItem(list);
            var plu = this.connectionProvider.Connection.Query<string>(sql: "SELECT Plu FROM nutrition.ItemNutrition WHERE PLU IN('1','2','3','4','5');", commandType: CommandType.Text).ToList();
            var response = controller.DeleteNutritionItem(plu) as OkNegotiatedContentResult<string>;

            // Assert  
            Assert.IsNotNull(response);
        }
    }
}
