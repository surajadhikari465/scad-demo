using Moq;
using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.Subscriber;
using Icon.Dvs.Model;
using Mammoth.Esb.ProductListener.MessageParsers;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Common.DataAccess.CommandQuery;

namespace Mammoth.Esb.ProductListener.Tests
{
    [TestClass]
    public class ProductListenerTests
    {
        private AddOrUpdateProductsCommandHandler commandHandler;
        private DeleteProductsExtendedAttributesCommandHandler commandDeleteHandler;       
        private SqlDbProvider dbProvider;
        private MessageArchiveCommandHandler commandMessageArchiveHandler;
        private DvsSqsMessage sqsMessage;

        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            dbProvider.Connection.Open();
            dbProvider.Transaction = dbProvider.Connection.BeginTransaction();

            commandHandler = new AddOrUpdateProductsCommandHandler(dbProvider);
            commandDeleteHandler = new DeleteProductsExtendedAttributesCommandHandler(dbProvider);
            commandMessageArchiveHandler = new MessageArchiveCommandHandler(dbProvider);

            sqsMessage = new DvsSqsMessage()
            {
                MessageAttributes = new Dictionary<string, string>() {
                    { "IconMessageID", "1" },
                    { "toBeReceivedBy", "ALL" }
                },
                S3BucketName = "SampleBucket",
                S3Key = "SampleS3Key"
            };
        }

        [TestCleanup]
        public void Cleanup()
        {
            dbProvider.Transaction.Rollback();
            dbProvider.Transaction.Dispose();
            dbProvider.Connection.Dispose();
        }

        [TestMethod]
        [Ignore]
        public void AddOrUpdateProducts_ProductWithScanCodeAlreadyExist_ShouldUpdateOneItem()
        {
            //Given
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText("TestMessages/ItemsWithDuplicateScanCode.xml"));

            //When
            var mockCache = new Mock<IHierarchyClassCache>();
            var taxDictionary = new Dictionary<string, int>
            {
                { "9989000", 1 },
            };
            mockCache.Setup(m => m.GetTaxDictionary()).Returns(taxDictionary);
            var mapperCache = new HierarchyClassIdMapper(mockCache.Object);

            mapperCache.PopulateHierarchyClassDatabaseIds(new List<GlobalAttributesModel>() { new GlobalAttributesModel() { MessageTaxClassHCID = "9989000" } });
            var mockLogger = new Mock<Icon.Logging.ILogger<ProductListener>>();

            var listener = new ProductListener(
                listenerSettings: DvsListenerSettings.CreateSettingsFromConfig(),
                subscriber: new Mock<IDvsSubscriber>().Object,
                emailClient: new Mock<IEmailClient>().Object,
                logger: new Mock<Icon.Logging.ILogger<ProductListener>>().Object,
                messageParser: new ProductMessageParser(),
                hierarchyClassIdMapper: mapperCache,
                addOrUpdateProductsCommandHandler: commandHandler,
                deleteProductsExtendedAttrCommandHandler: commandDeleteHandler,
                messageArchiveCommandHandler: commandMessageArchiveHandler);

            listener.HandleMessage(message);

            //Then
            var newItemId = dbProvider.Connection.ExecuteScalar(
                sql: "SELECT ItemID FROM dbo.Items WHERE ItemId in(9999999)",
                transaction: dbProvider.Transaction);
            Assert.IsNull(newItemId);
        }

        [TestMethod]
        public void AddOrUpdateProducts_ProductWithScanCodeAlreadyExist_ShouldUpdateOneItem_Mock()
        {
            //Given
            var message = new DvsMessage(sqsMessage, System.IO.File.ReadAllText("TestMessages/ItemsWithDuplicateScanCode.xml"));

            var mockAddUpdate = new Mock<ICommandHandler<AddOrUpdateProductsCommand>>();
            var mockMessageArchive = new Mock<ICommandHandler<MessageArchiveCommand>>();
            mockAddUpdate.SetupSequence(x => x.Execute(It.IsAny<AddOrUpdateProductsCommand>()))
                .Throws(new System.Exception())
                .Throws(new System.Exception())
                .Pass();

            var mockDelete = new Mock<ICommandHandler<DeleteProductsExtendedAttributesCommand>>();
            var mockCache = new Mock<IHierarchyClassCache>();
            var taxDictionary = new Dictionary<string, int>
            {
                { "9989000", 1 },
            };
            mockCache.Setup(m => m.GetTaxDictionary()).Returns(taxDictionary);
            var mapperCache = new HierarchyClassIdMapper(mockCache.Object);
            mapperCache.PopulateHierarchyClassDatabaseIds(new List<GlobalAttributesModel>() { new GlobalAttributesModel() { MessageTaxClassHCID = "9989000" } });

            var listener = new ProductListener(
                listenerSettings: DvsListenerSettings.CreateSettingsFromConfig(),
                subscriber: new Mock<IDvsSubscriber>().Object,
                emailClient: new Mock<IEmailClient>().Object,
                logger: new Mock<Icon.Logging.ILogger<ProductListener>>().Object,
                messageParser: new ProductMessageParser(),
                hierarchyClassIdMapper: mapperCache,
                addOrUpdateProductsCommandHandler: mockAddUpdate.Object,
                deleteProductsExtendedAttrCommandHandler: mockDelete.Object,
                messageArchiveCommandHandler: mockMessageArchive.Object);

            try
            {
                listener.HandleMessage(message);
            }
            catch(Exception ex)
            {
                // Then
                Assert.AreEqual(ex.Message, "There was an issue in processing the message");
            }
            mockAddUpdate.Verify(x => x.Execute(It.IsAny<AddOrUpdateProductsCommand>()), Times.Exactly(3));
        }
    }
}