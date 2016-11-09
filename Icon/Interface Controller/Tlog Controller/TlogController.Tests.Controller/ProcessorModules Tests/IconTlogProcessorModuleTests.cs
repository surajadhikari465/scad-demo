using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TlogController.Controller.ProcessorModules;
using Icon.Framework;
using Icon.Logging;
using Moq;
using TlogController.DataAccess.Queries;
using System.Collections.Generic;
using TlogController.DataAccess.BulkCommands;
using TlogController.DataAccess.Infrastructure;
using TlogController.DataAccess.Interfaces;
using TlogController.Common;

namespace TlogController.Tests.Controller
{
    [TestClass]
    public class IconTlogProcessorModuleTests
    {
        private Mock<ILogger<IconTlogProcessorModule>> mockLogger;
        private Mock<IQueryHandler<GetBusinessUnitToRegionCodeMappingQuery, Dictionary<int, string>>> mockGetBuToRegionMappingQuery;
        private Mock<IQueryHandler<BulkUpdateItemMovementInProcessCommand, List<ItemMovement>>> mockBulkUpdateItemMovementHandler;
        private Mock<IBulkCommandHandler<BulkUpdateItemMovementCommand>> mockBulkUpdateItemMovementCommandHandler;
        private IconTlogProcessorModule module;

        [TestInitialize]
        public void InitializeData()
        {
            this.mockLogger = new Mock<ILogger<IconTlogProcessorModule>>();
            this.mockGetBuToRegionMappingQuery = new Mock<IQueryHandler<GetBusinessUnitToRegionCodeMappingQuery, Dictionary<int, string>>>();
            this.mockBulkUpdateItemMovementHandler = new Mock<IQueryHandler<BulkUpdateItemMovementInProcessCommand, List<ItemMovement>>>();
            this.mockBulkUpdateItemMovementCommandHandler = new Mock<IBulkCommandHandler<BulkUpdateItemMovementCommand>>();

            this.module = new IconTlogProcessorModule(this.mockLogger.Object, this.mockGetBuToRegionMappingQuery.Object, this.mockBulkUpdateItemMovementHandler.Object, mockBulkUpdateItemMovementCommandHandler.Object);
        }

        [TestCleanup]
        public void CleanupData()
        {
            Cache.BusinessUnitToRegionCode.Clear();
            Cache.CacheCreatedDate = default(DateTime);
        }

        [TestMethod]
        public void LoadBusinessUnitToRegionMapping_CacheBuToRegionCodeHasValueAndCreateDateGreaterThanToday_GetBuToRegionMappingQueryNotExecuted()
        {
            // Given
            Cache.CacheCreatedDate = DateTime.Now;
            Cache.BusinessUnitToRegionCode.Add(1, "SW");

            // When
            this.module.LoadBusinessUnitToRegionCodeMapping();

            // Then
            this.mockGetBuToRegionMappingQuery.Verify(q => q.Execute(It.IsAny<GetBusinessUnitToRegionCodeMappingQuery>()), Times.Never);
        }

        [TestMethod]
        public void LoadBusinessUnitToRegionMapping_CacheBuToRegionCodeEmptyAndCreateDateLessThanToday_GetBuToRegionMappingQueryExecutedOneTime()
        {
            // Given
            this.mockGetBuToRegionMappingQuery.Setup(q => q.Execute(It.IsAny<GetBusinessUnitToRegionCodeMappingQuery>())).Returns(new Dictionary<int, string>());
            Cache.CacheCreatedDate = DateTime.Now.AddDays(-1);
            // When
            this.module.LoadBusinessUnitToRegionCodeMapping();

            // Then
            this.mockGetBuToRegionMappingQuery.Verify(q => q.Execute(It.IsAny<GetBusinessUnitToRegionCodeMappingQuery>()), Times.Once);
        }
    }
}
