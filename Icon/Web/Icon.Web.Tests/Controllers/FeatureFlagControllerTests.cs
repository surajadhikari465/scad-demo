using System;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Common.DataAccess;
using Icon.Framework;
using System.Collections.Generic;
using Icon.Web.DataAccess.Commands;
using AutoMapper;
using Icon.Logging;
using System.Web.Mvc;
using Icon.Web.Mvc.Controllers;
using System.Security.Principal;
using Icon.Web.Mvc.Models;
using System.Linq;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.Tests.Unit.Controllers
{
    [TestClass]
    public class FeatureFlagControllerTests
    {
        Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<FeatureFlagModel>>, IEnumerable<FeatureFlagModel>>> getFeatureFlagsQuery;
        Mock<ICommandHandler<AddFeatureFlagCommand>> addFeatureFlagCommandHandler;
        Mock<ICommandHandler<UpdateFeatureFlagCommand>> updateFeatureFlagCommandHandler;
        Mock<IMapper> mapper;
        Mock<ILogger> logger;
        Mock<ControllerContext> mockControllerContext;
        FeatureFlagController featureFlagController;
        List<FeatureFlagModel> featureFlagsMemoryDB;


        /// <summary>
        /// Runs once for every test
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            this.getFeatureFlagsQuery = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<FeatureFlagModel>>, IEnumerable<FeatureFlagModel>>>();
            this.addFeatureFlagCommandHandler = new Mock<ICommandHandler<AddFeatureFlagCommand>>();
            this.updateFeatureFlagCommandHandler = new Mock<ICommandHandler<UpdateFeatureFlagCommand>>();
            this.mapper = new Mock<IMapper>();
            this.logger = new Mock<ILogger>();

            this.featureFlagsMemoryDB = new List<FeatureFlagModel>() { 
                new FeatureFlagModel { 
                    FeatureFlagId = 1, 
                    FlagName = "Flag name", 
                    Description = "Description", 
                    Enabled = true, 
                    CreatedDateUtc = DateTime.Now, 
                    LastModifiedDateUtc = DateTime.Now, 
                    LastModifiedBy = "Me" 
                } 
            };
            this.getFeatureFlagsQuery.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<FeatureFlagModel>>>()))
                .Returns(featureFlagsMemoryDB);
            
            this.addFeatureFlagCommandHandler.Setup(m=> m.Execute(It.IsAny<AddFeatureFlagCommand>()))
                .Callback((AddFeatureFlagCommand data) => {
                    data.FeatureFlag.FeatureFlagId = this.featureFlagsMemoryDB.Count + 1;
                    this.featureFlagsMemoryDB.Add(data.FeatureFlag); 
                });

            this.updateFeatureFlagCommandHandler.Setup(m => m.Execute(It.IsAny<UpdateFeatureFlagCommand>()))
                .Callback((UpdateFeatureFlagCommand data) =>
                {
                    var index = this.featureFlagsMemoryDB.FindIndex(ff => ff.FeatureFlagId == data.FeatureFlag.FeatureFlagId);
                    this.featureFlagsMemoryDB[index] = data.FeatureFlag;
                });

            this.mapper.Setup(m => m.Map<FeatureFlagModel>(It.IsAny<FeatureFlagViewModel>()))
                        .Returns( (FeatureFlagViewModel ff) => 
                        {
                            return new FeatureFlagModel { 
                                FeatureFlagId = ff.FeatureFlagId,
                                FlagName = ff.FlagName,
                                Description = ff.Description,
                                Enabled = ff.Enabled,
                                CreatedDateUtc = ff.CreatedDateUtc ?? DateTime.MinValue,
                                LastModifiedDateUtc =ff.LastModifiedDateUtc ?? DateTime.MinValue,
                                LastModifiedBy = ff.LastModifiedBy,

                            };
                        });
            Mock<IIdentity> mockIdentity = new Mock<IIdentity>();
            mockIdentity.SetupGet(i => i.Name).Returns("Test User");
            mockIdentity.SetupGet(i => i.IsAuthenticated).Returns(true);
            
            this.mockControllerContext = new Mock<ControllerContext>();
            this.mockControllerContext.SetupGet(m => m.HttpContext.User).Returns(new GenericPrincipal(mockIdentity.Object, null));
            this.featureFlagController = new FeatureFlagController(
                this.getFeatureFlagsQuery.Object,
                this.addFeatureFlagCommandHandler.Object,
                this.updateFeatureFlagCommandHandler.Object,
                this.mapper.Object,
                this.logger.Object);
            this.featureFlagController.ControllerContext = this.mockControllerContext.Object;
        }


        [TestMethod]
        public void Index_InitialPageLoad_ShouldReturnView()
        {
            // When
            var result = this.featureFlagController.Index() as ViewResult;

            // Then
            Assert.IsNotNull(result);
            var resultModel = (FeatureFlagGridViewModel)result.Model;
            Assert.AreEqual(1, resultModel.FeatureFlags.Count());

        }

        [TestMethod]
        public void Index_SaveChangesInGrid_ShouldReturnView_update()
        {
            var updateRequest = @"
                    [
                        {
                            ""type"": ""row"",
                            ""tid"": ""404e"",
                            ""row"": {
                                ""FeatureFlagId"": 1,
                                ""FlagName"": ""feature-flag"",
                                ""Description"": ""Feature name Z"",
                                ""Enabled"": true,
                                ""CreatedDateUtc"": ""2020-06-24T06:53:01-05:00"",
                                ""LastModifiedDateUtc"": ""2020-06-24T10:34:15-05:00"",
                                ""LastModifiedBy"": ""WFM\\UserX""
                            },
                            ""rowId"": 3
                        }
                    ]";
            // When
            var result = this.featureFlagController.SaveChangesInGrid(updateRequest);

            // Then
            Assert.IsNotNull(result);

            //System.Collections.Generic.Dictionary<string, bool>
            var resultData = (Dictionary<string, bool>)((System.Web.Mvc.JsonResult)result).Data;
            Assert.IsTrue(resultData["Success"]);
            Assert.AreEqual(1, this.featureFlagsMemoryDB.Count);
            var featureFlag = this.featureFlagsMemoryDB[0];
            Assert.AreEqual(1, featureFlag.FeatureFlagId);
            Assert.AreEqual("feature-flag", featureFlag.FlagName);
            Assert.AreEqual("Feature name Z", featureFlag.Description);
            Assert.AreEqual(true, featureFlag.Enabled);
            Assert.AreEqual(DateTime.Parse("2020-06-24T06:53:01-05:00"), featureFlag.CreatedDateUtc);
            Assert.AreEqual(DateTime.Parse("2020-06-24T10:34:15-05:00"), featureFlag.LastModifiedDateUtc);
            Assert.AreEqual("Test User", featureFlag.LastModifiedBy);
        }


        [TestMethod]
        public void Index_SaveChangesInGrid_ShouldReturnView_add()
        {
            var updateRequest = @"
                    [
                        {
                            ""type"": ""newrow"",
                            ""tid"": ""404e"",
                            ""row"": {
                                ""FeatureFlagId"": 1,
                                ""FlagName"": ""feature-flag"",
                                ""Description"": ""Feature name Z"",
                                ""Enabled"": true,
                                ""CreatedDateUtc"": ""2020-06-24T06:53:01-05:00"",
                                ""LastModifiedDateUtc"": ""2020-06-24T10:34:15-05:00"",
                                ""LastModifiedBy"": ""WFM\\UserX""
                            },
                            ""rowId"": 3
                        }
                    ]";
            // When
            var result = this.featureFlagController.SaveChangesInGrid(updateRequest);

            // Then
            Assert.IsNotNull(result);

            //System.Collections.Generic.Dictionary<string, bool>
            var resultData = (Dictionary<string, bool>)((System.Web.Mvc.JsonResult)result).Data;
            Assert.IsTrue(resultData["Success"]);
            Assert.AreEqual(2, this.featureFlagsMemoryDB.Count);
            var featureFlag = this.featureFlagsMemoryDB[1];
            Assert.AreEqual(2, featureFlag.FeatureFlagId);
            Assert.AreEqual("feature-flag", featureFlag.FlagName);
            Assert.AreEqual("Feature name Z", featureFlag.Description);
            Assert.AreEqual(true, featureFlag.Enabled);
            Assert.AreEqual(DateTime.Parse("2020-06-24T06:53:01-05:00"), featureFlag.CreatedDateUtc);
            Assert.AreEqual(DateTime.Parse("2020-06-24T10:34:15-05:00"), featureFlag.LastModifiedDateUtc);
            Assert.AreEqual("Test User", featureFlag.LastModifiedBy);
        }

        [TestMethod]
        public void Index_SaveChangesInGrid_BadRequest()
        {
            // When
            var result = this.featureFlagController.SaveChangesInGrid(null);

            // Then
            Assert.IsNotNull(result);

            var codeResult = (System.Web.Mvc.HttpStatusCodeResult)result;
            Assert.AreEqual(400, codeResult.StatusCode);
            Assert.AreEqual("ig_transactions is null", codeResult.StatusDescription);
        }

    }
}


