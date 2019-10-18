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

namespace WebSupport.Tests.Controllers
{
    [TestClass]
    public class RequeEventsControllerTest
    {
        Mock<ILogger> logger;
        RegenerateEventsController controller;
        

        [TestInitialize]
        public void Initialize()
        {
            logger = new Mock<ILogger>();
            controller = new RegenerateEventsController(logger.Object);
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
            Assert.AreEqual(expectedRegions.Length, viewModelResult.Regions.Length);
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
            var view = GetViewModel();

            var context = new Mock<ControllerContext>();
            var value = new NameValueCollection { ["ActionGet"] = String.Empty };
            context.Setup(x => x.HttpContext.Request.Form).Returns(value);
            this.controller.ControllerContext = context.Object;

            //Act
            SqlTransaction trans;
            var sql = "INSERT into dbo.ItemHistory(Store_No, Item_Key, DateStamp, CreatedBy, SubTeam_No, Adjustment_ID) VALUES(809,33516,'2000-01-01',160,905,1)";

            using(var con = new SqlConnection(ConfigurationManager.ConnectionStrings[$"IRMA_{view.Region}"].ConnectionString))
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
            Assert.AreEqual(view.ResultTable.Rows.Count, 1);
        }

        [TestMethod]
        public void RequeueEventsController_Index_Submit_OneRecordsShouldBeSubmit()
        {
            //Given
            int id = 0;
            var view = GetViewModel();

            //Act
            var sql = @"INSERT INTO dbo.ItemHistory(Store_No, Item_Key, DateStamp, CreatedBy, SubTeam_No, Adjustment_ID) VALUES(809,33516,'2000-01-01',160,905,1);
                        SELECT SCOPE_IDENTITY()";

            using(var con = new SqlConnection(ConfigurationManager.ConnectionStrings[$"IRMA_{view.Region}"].ConnectionString))
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

                cmd.CommandText = $"DELETE FROM dbo.ItemHistory WHERE ItemHistoryID = {id.ToString()} and DateStamp = '{view.DateFrom}'; DELETE FROM amz.InventoryQueue WHERE KeyID = '{id.ToString()}' AND EventTypeCode = '{view.EventType}';";
                cmd.ExecuteNonQuery();
            }

            //Assert
            Assert.AreEqual(this.controller.ViewData["FYI"], $"Event code {view.EventType}: 1 events have been submitted.");
        }

        RegenerateEventViewModel GetViewModel()
        {
            return new RegenerateEventViewModel()
            {
                Region = "FL",
                EventType = "INV_ADJ",
                DateFrom = new DateTime(2000, 1, 1),
                DateTo = new DateTime(2000, 1, 1)
            };
        }
    }
}