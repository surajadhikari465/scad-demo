namespace WebSupport.DataAccess.Test.Commands
{
    using System.Data.Entity;
    using System.Linq;

    using DataAccess.Commands;
    using Irma.Framework;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using System.Collections.Generic;
    [TestClass]
    public class CreateEventsForRegionCommandHandlerTests
    {
        private CreateEventsForRegionCommandHandler commandHandler;
        private IrmaContext context;
        private DbContextTransaction transaction;
        private Mock<IIrmaContextFactory> contextFactory;

        [TestInitialize]
        public void Initialize()
        {
            this.context = new IrmaContext();

            this.contextFactory = new Mock<IIrmaContextFactory>();
            this.contextFactory.Setup(m => m.CreateContext(It.IsAny<string>()))
                               .Returns(this.context);

            this.commandHandler = new CreateEventsForRegionCommandHandler(this.contextFactory.Object);

            this.transaction = this.context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.transaction.Rollback();
        }

        [TestMethod]
        public void CreateEventsForRegion_WhenSingleScanCodeAndItemLocale_ThenEventsAreCreatedForEachStore()
        {
            // Given
            var scanCode = "4011";
            var eventType = EventConstants.ItemLocaleAddOrUpdateEvent;

            // When
            commandHandler.Execute(new CreateEventsForRegionCommand
            {
                Region = "FL",
                EventType = eventType,
                ScanCodes = new[] { scanCode }
            });

            // Then
            var itemEvents = this.context.MammothItemLocaleChangeQueue
                .Where(i => scanCode == i.Identifier)
                .ToList();
            var storeCount = this.context.Database.SqlQuery<int>(
                @"
                DECLARE @ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'))
                SELECT count(*)
	            FROM Store s
	            WHERE
		            s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
		            AND (s.WFM_Store = 1 OR s.Mega_Store = 1 )
		            AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)")
                .First();

            Assert.AreEqual(storeCount, itemEvents.Count());
            Assert.IsTrue(itemEvents.All(i => i.MammothItemChangeEventType.EventTypeName.Equals(EventConstants.ItemLocaleAddOrUpdateEvent)));
            Assert.IsTrue(itemEvents.All(i => i.Store_No != null));
        }

        [TestMethod]
        public void CreateEventsForRegion_WhenSingleScanCodeAndItemPrice_ThenSingleEventIsCreated()
        {
            // Given
            var scanCode = "4011";
            var eventType = EventConstants.ItemPriceEvent;

            // When
            commandHandler.Execute(new CreateEventsForRegionCommand
            {
                Region = "FL",
                EventType = eventType,
                ScanCodes = new[] { scanCode }
            });

            // Then
            var itemEvents = this.context.PriceChangeQueue.Where(i => i.Identifier.Equals(scanCode))
                .ToList();

            // var numberOfPrices = this.context.Price.Count(p => p.Item.ItemIdentifier.
        }

        [TestMethod]
        public void CreateEventsForRegion_WhenMultipleScanCodeAndItemLocale_ThenMultipleEventsAreCreated()
        {
            // Given
            var scanCodes = new[] { "4011", "4012", "4013" };
            var eventType = EventConstants.ItemLocaleAddOrUpdateEvent;

            // When
            commandHandler.Execute(new CreateEventsForRegionCommand
            {
                Region = "FL",
                EventType = eventType,
                ScanCodes = scanCodes
            });

            // Then
            var itemEvents = this.context.MammothItemLocaleChangeQueue
                .Where(i => scanCodes.Contains(i.Identifier))
                .ToList();
            var storeCount = this.context.Database.SqlQuery<int>(
                @"
                DECLARE @ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'))
                SELECT count(*)
	            FROM Store s
	            WHERE
		            s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
		            AND (s.WFM_Store = 1 OR s.Mega_Store = 1 )
		            AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)")
                .First();

            Assert.AreEqual(scanCodes.Count() * storeCount, itemEvents.Count());
            Assert.IsTrue(itemEvents.All(i => i.MammothItemChangeEventType.EventTypeName.Equals(EventConstants.ItemLocaleAddOrUpdateEvent)));
            Assert.IsTrue(itemEvents.All(i => i.Store_No != null));
        }

        [TestMethod]
        public void CreateEventsForRegion_WhenMultipleScanCodeAndItemPrice_ThenMultipleEventsAreCreated()
        {
            // Given           
            var eventType = EventConstants.ItemPriceEvent;

            // get 3 scan codes that don't have events
            var scanCodes = this.context.Database.SqlQuery<string>(@"DECLARE @sentPriceBatchStatus INT = (SELECT PriceBatchStatusID FROM PriceBatchStatus WHERE PriceBatchStatusDesc = 'Sent');
                        SELECT top 3  CONVERT(VARCHAR(13) ,[Item_Key])
                        FROM [ItemCatalog_Test].[dbo].[Item]
                        WHERE Item_Key not in (
		                        SELECT Item_Key FROM PriceBatchDetail pbd 
		                        JOIN PriceBatchHeader pbh ON pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
		                        AND pbh.PriceBatchStatusID = @sentPriceBatchStatus)").Select(ik=>ik).ToArray<string>();
                        
            // from mammoth.GenerateEvents sproc
            var numberOfStores = this.context.Database.SqlQuery<int>(
                @"declare @ExcludedStoreNo varchar(250) = (select dbo.fn_GetAppConfigValue('LabAndClosedStoreNo', 'IRMA Client'));
                
                select count(*)
                from Store s
                where 
                    (s.WFM_Store = 1 OR s.Mega_Store = 1)
                    and (Internal = 1 and BusinessUnit_ID is not null)
                    and s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))"
            ).First();

            // Count any pre-existing item price events before we create our own events.                            
            var preExistingEventCount =
                this.context.PriceChangeQueue
                .Count(i => scanCodes.Contains(i.Identifier));


            // When
            commandHandler.Execute(new CreateEventsForRegionCommand
            {
                Region = "FL",
                EventType = eventType,
                ScanCodes = scanCodes
            });

            // Then
            var itemEvents = this.context.PriceChangeQueue
                .Where(i => scanCodes.Contains(i.Identifier))
                .ToList();
                        
            Assert.AreEqual(numberOfStores , itemEvents.Count() - preExistingEventCount);
            Assert.IsTrue(itemEvents.All(i => i.ItemChangeEventType.EventTypeName.Equals(EventConstants.ItemPriceEvent)));
            Assert.IsTrue(itemEvents.All(i => i.Store_No != null));
        }
    }
}
