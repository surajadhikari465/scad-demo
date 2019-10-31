using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.SqlClient;
using WebSupport.Controllers;
using WebSupport.ViewModels;
using WebSupport.DataAccess;
using System.Configuration;
using System.Collections.Specialized;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using WebSupport.DataAccess.Queries;
using WebSupport.DataAccess.TransferObjects;
using WebSupport.Models;

namespace WebSupport.Tests.Controllers
{
    [TestClass]
    public class RegenerateEventsControllerTest
    {
		private Mock<ILogger> mockLogger;
		private RegenerateEventsController controller;
		private Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>> mockQueryForStores;

		[TestInitialize]
        public void Initialize()
        {
			mockLogger = new Mock<ILogger>();
			mockQueryForStores = new Mock<IQueryHandler<GetStoresForRegionParameters, IList<StoreTransferObject>>>();
			controller = new RegenerateEventsController(mockLogger.Object, mockQueryForStores.Object);
        }

        [TestMethod]
        public void RequeueEventsController_Index_Get_ShouldReturnViewResult()
        {
            // Act
            var result = controller.Index();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void RequeueEventsController_Get_ShouldReturnViewResult_WithRegionOption()
        {
            //Given
            var expectedRegions = DataConstants.WholeFoodsRegions.ToArray();

            // Act
            var result = controller.Index();

            // Assert
            var viewModelResult = (RegenerateEventViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedRegions.Length + 1, viewModelResult.OptionsForRegion.Count());
        }

        [TestMethod]
        public void RequeueEventsController_Get_ShouldReturnViewResult_WithEventTypes()
        {
            //Given
            var expectedEvents = QueueEventTypes.Events.ToArray();

            // Act
            var result = controller.Index();

            // Assert
            var viewModelResult = (RegenerateEventViewModel)((ViewResult)result).Model;
            Assert.AreEqual(expectedEvents.Length, viewModelResult.Events.Length);
        }

        [TestMethod]
        public void RequeueEventsController_Index_Get_ShouldGetOneRecord()
        {
            //Given
            var view = Get2000ViewModel();

			var region = StaticData.WholeFoodsRegions.ElementAt(view.RegionIndex);

			var context = new Mock<ControllerContext>();
            var value = new NameValueCollection { ["ActionGet"] = String.Empty };
            context.Setup(x => x.HttpContext.Request.Form).Returns(value);
            this.controller.ControllerContext = context.Object;

            //Act
            SqlTransaction trans;
            var sql = "INSERT into dbo.ItemHistory(Store_No, Item_Key, DateStamp, CreatedBy, SubTeam_No, Adjustment_ID) VALUES(809,33516,'2000-01-01 11:10:23',160,905,1)";

            using(var con = new SqlConnection(ConfigurationManager.ConnectionStrings[$"IRMA_{region}"].ConnectionString))
            {
                con.Open();
                trans = con.BeginTransaction();

                try
                {
                    using(var cmd = new SqlCommand(sql, con, trans))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    this.controller.Index(view);
                }
                catch{}
                finally
                {
                    trans.Rollback();
                }
            }

            //Assert
            Assert.AreEqual(1, view.ResultTable.Rows.Count);
        }

        [TestMethod]
        public void RequeueEventsController_Index_Submit_OneRecordsShouldBeSubmit()
        {
            //Given
            int id = 0;
            var view = Get1990ViewModel();
			var region = StaticData.WholeFoodsRegions.ElementAt(view.RegionIndex);

			//Act
			var sql = @"INSERT INTO dbo.ItemHistory(Store_No, Item_Key, DateStamp, CreatedBy, SubTeam_No, Adjustment_ID) VALUES(809,33516,'1990-01-01 15:11:23',160,905,1);
                        SELECT SCOPE_IDENTITY()";

            using(var con = new SqlConnection(ConfigurationManager.ConnectionStrings[$"IRMA_{region}"].ConnectionString))
            {
                con.Open();

                var cmd = new SqlCommand(sql, con);
                int.TryParse(cmd.ExecuteScalar().ToString(), out id);
                 
                var context = new Mock<ControllerContext>();
                var value = new NameValueCollection
                {
                    ["cbIsSelected"] = id.ToString(),
                    ["ActionSubmit"] = String.Empty
                };

                context.Setup(x => x.HttpContext.Request.Form).Returns(value);
                this.controller.ControllerContext = context.Object;
                controller.Index(view);

                cmd.CommandText = $"DELETE FROM dbo.ItemHistory WHERE ItemHistoryID = {id.ToString()} and DateStamp = '{view.StartDatetime}'; DELETE FROM amz.InventoryQueue WHERE KeyID = '{id.ToString()}' AND EventTypeCode = '{view.EventType}';";
                cmd.ExecuteNonQuery();
            }

            //Assert
            Assert.AreEqual(this.controller.ViewData["FYI"], $"Event code {view.EventType}: 1 events have been submitted.");
        }

        RegenerateEventViewModel Get2000ViewModel()
        {
            return new RegenerateEventViewModel()
            {
                RegionIndex = 1,
                EventType = "INV_ADJ",
				StoreIndex = 0,
				StartDatetime = new DateTime(2000, 1, 1),
				EndDatetime = new DateTime(2000, 1, 2)
            };
        }

		RegenerateEventViewModel Get1990ViewModel()
		{
			return new RegenerateEventViewModel()
			{
				RegionIndex = 1,
				EventType = "INV_ADJ",
				StoreIndex = 0,
				StartDatetime = new DateTime(1990, 1, 1),
				EndDatetime = new DateTime(1990, 1, 2)
			};
		}
	}
}