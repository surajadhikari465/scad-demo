using Moq;
using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Icon.Esb.Subscriber;
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
            Mock<IEsbMessage> message = new Mock<IEsbMessage>();
            message.Setup(m => m.MessageText).Returns(System.IO.File.ReadAllText("TestMessages/ItemsWithDuplicateScanCode.xml"));
            var args = new EsbMessageEventArgs() { Message = message.Object };

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
                listenerApplicationSettings: new Icon.Esb.ListenerApplication.ListenerApplicationSettings(),
                esbConnectionSettings: new Icon.Esb.EsbConnectionSettings() { SessionMode = TIBCO.EMS.SessionMode.NoAcknowledge },
                subscriber: null,
                emailClient: null,
                logger: new Mock<Icon.Logging.ILogger<ProductListener>>().Object,
                messageParser: new ProductMessageParser(),
                hierarchyClassIdMapper: mapperCache,
                addOrUpdateProductsCommandHandler: commandHandler,
                deleteProductsExtendedAttrCommandHandler: commandDeleteHandler,
                messageArchiveCommandHandler: commandMessageArchiveHandler);

            listener.HandleMessage(null, args);

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
            Mock<IEsbMessage> message = new Mock<IEsbMessage>();
            message.Setup(m => m.MessageText).Returns(System.IO.File.ReadAllText("TestMessages/ItemsWithDuplicateScanCode.xml"));
            var args = new EsbMessageEventArgs() { Message = message.Object };

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

            //When
            var listener = new ProductListener(
                listenerApplicationSettings: new Icon.Esb.ListenerApplication.ListenerApplicationSettings(),
                esbConnectionSettings: new Icon.Esb.EsbConnectionSettings() { SessionMode = TIBCO.EMS.SessionMode.NoAcknowledge },
                subscriber: null,
                emailClient: null,
                logger: new Mock<Icon.Logging.ILogger<ProductListener>>().Object,
                messageParser: new ProductMessageParser(),
                hierarchyClassIdMapper: mapperCache,
                addOrUpdateProductsCommandHandler: mockAddUpdate.Object,
                deleteProductsExtendedAttrCommandHandler: mockDelete.Object,
                messageArchiveCommandHandler: mockMessageArchive.Object);

            listener.HandleMessage(null, args);

            //Then
            mockAddUpdate.Verify(x => x.Execute(It.IsAny<AddOrUpdateProductsCommand>()), Times.Exactly(3));
        }
    }
}