using System;
using System.Collections.Generic;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mammoth.Esb.HierarchyClassListener.Commands;
using TIBCO.EMS;
using Mammoth.Common.DataAccess;
using Icon.Esb.Schemas.Wfm.Contracts;

namespace Mammoth.Esb.HierarchyClassListener.Tests
{
    [TestClass]
    public class MammothHierarchyClassListenerTests
    {
        private MammothHierarchyClassListener listener;
        private DvsListenerSettings listenerSettings;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IHierarchyClassService<AddOrUpdateHierarchyClassRequest>> mockAddOrUpdateHierarchyClassService;
        private Mock<IHierarchyClassService<DeleteBrandRequest>> mockDeleteBrandService;
        private Mock<IHierarchyClassService<DeleteMerchandiseClassRequest>> mockDeleteMerchandiseService;
        private Mock<IHierarchyClassService<DeleteNationalClassRequest>> mockDeleteNationalService;
        private Mock<ILogger<MammothHierarchyClassListener>> mockLogger;
        private Mock<IMessageParser<List<HierarchyClassModel>>> mockMessageParser;
        private Mock<IDvsSubscriber> mockSubscriber;
        private DvsMessage mockDvsMessage;
        private Mock<IHierarchyClassService<DeleteManufacturerRequest>> mockDeleteManufacturerService;

        [TestInitialize]
        public void Initialize()
        {
            listenerSettings = new DvsListenerSettings();
            mockEmailClient = new Mock<IEmailClient>();
            mockAddOrUpdateHierarchyClassService = new Mock<IHierarchyClassService<AddOrUpdateHierarchyClassRequest>>();
            mockDeleteBrandService = new Mock<IHierarchyClassService<DeleteBrandRequest>>();
            mockDeleteMerchandiseService = new Mock<IHierarchyClassService<DeleteMerchandiseClassRequest>>();
            mockDeleteNationalService = new Mock<IHierarchyClassService<DeleteNationalClassRequest>>();
            mockLogger = new Mock<ILogger<MammothHierarchyClassListener>>();
            mockMessageParser = new Mock<IMessageParser<List<HierarchyClassModel>>>();
            mockSubscriber = new Mock<IDvsSubscriber>();
            mockDeleteManufacturerService = new Mock<IHierarchyClassService<DeleteManufacturerRequest>>();

            this.listener = new MammothHierarchyClassListener(listenerSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockMessageParser.Object,
                mockAddOrUpdateHierarchyClassService.Object,
                mockDeleteBrandService.Object,
                mockDeleteMerchandiseService.Object,
                mockDeleteNationalService.Object,
                mockDeleteManufacturerService.Object);

            mockDvsMessage = new DvsMessage(new DvsSqsMessage(), "");
        }

        [TestMethod]
        public void HandleMessage_SuccessfulAddOrUpdateMessage_ShouldAcknowledgeMessageAndNotLogError()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(new List<HierarchyClassModel>
                {
                    new HierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "test",
                        HierarchyId = Hierarchies.Brands
                    }
                });

            //When
            listener.HandleMessage(mockDvsMessage);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
            mockAddOrUpdateHierarchyClassService.Verify(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorParsingMessage_ShouldThrowException()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Throws(new Exception("TestMessage"));

            try
            {
                // When
                listener.HandleMessage(mockDvsMessage);
            }
            catch (Exception ex)
            {
                // Then
                Assert.AreEqual(ex.Message, "TestMessage");
            }
        }

        [TestMethod]
        public void HandleMessage_ErrorCallingHierarchyClasses_ShouldThrowException()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(new List<HierarchyClassModel>
                {
                    new HierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "test",
                        HierarchyId = Hierarchies.Brands
                    }
                });
            mockAddOrUpdateHierarchyClassService.Setup(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()))
                .Throws(new Exception("TestMessage"));

            try
            {
                // When
                listener.HandleMessage(mockDvsMessage);
            }
            catch (Exception ex)
            {
                // Then
                Assert.AreEqual(ex.Message, "TestMessage");
            }            
        }

        [TestMethod]
        public void HandleMessage_MessageHasBrandDeletes_ShouldCallBrandDeleteService()
        {
            // Given
            List<HierarchyClassModel> brands = new List<HierarchyClassModel>();
            brands.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Unit Test Brand 1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Brands,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(brands);

            // When
            listener.HandleMessage(mockDvsMessage);

            // Then
            mockAddOrUpdateHierarchyClassService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Never);
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.Is<DeleteBrandRequest>(r =>
                r.HierarchyClasses[0].HierarchyClassId == brands[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == brands[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == brands[0].HierarchyId)), Times.Once);
            mockDeleteMerchandiseService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()), Times.Never);
            mockDeleteNationalService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteNationalClassRequest>()), Times.Never);
        }

        [TestMethod]
        public void HandleMessage_MessageHasNationalDeletes_ShouldCallNationalDeleteService()
        {
            // Given
            List<HierarchyClassModel> nationalClasses = new List<HierarchyClassModel>();
            nationalClasses.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Unit Test National 1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.National,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(nationalClasses);

            // When
            listener.HandleMessage(mockDvsMessage);

            // Then
            mockAddOrUpdateHierarchyClassService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Never);
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()), Times.Never);
            mockDeleteMerchandiseService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()), Times.Never);
            mockDeleteNationalService.Verify(ds => ds.ProcessHierarchyClasses(It.Is<DeleteNationalClassRequest>(r =>
                r.HierarchyClasses[0].HierarchyClassId == nationalClasses[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == nationalClasses[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == nationalClasses[0].HierarchyId)), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageHasMerchandiseDeletes_ShouldCallMerchandiseDeleteService()
        {
            // Given
            List<HierarchyClassModel> merchandiseClasses = new List<HierarchyClassModel>();
            merchandiseClasses.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Unit Test Merchandise 1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Merchandise,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(merchandiseClasses);

            // When
            listener.HandleMessage(mockDvsMessage);

            // Then
            mockAddOrUpdateHierarchyClassService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Never);
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()), Times.Never);
            mockDeleteMerchandiseService.Verify(ds => ds.ProcessHierarchyClasses(It.Is<DeleteMerchandiseClassRequest>(r =>
                r.HierarchyClasses[0].HierarchyClassId == merchandiseClasses[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == merchandiseClasses[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == merchandiseClasses[0].HierarchyId)), Times.Once);
            mockDeleteNationalService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteNationalClassRequest>()), Times.Never);
        }

        [TestMethod]
        public void HandleMessage_MessageHasOnlyAddOrUpdateActions_ShouldCallOnlyAddOrUpdateHierarchyClassService()
        {
            // Given
            List<HierarchyClassModel> brands = new List<HierarchyClassModel>();
            brands.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.AddOrUpdate,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Unit Test Brand 1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Brands,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(brands);

            // When
            listener.HandleMessage(mockDvsMessage);

            // Then
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()), Times.Never);
            mockDeleteMerchandiseService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()), Times.Never);
            mockAddOrUpdateHierarchyClassService.Verify(s => s.ProcessHierarchyClasses(It.Is<AddOrUpdateHierarchyClassRequest>(r =>
                r.HierarchyClasses[0].Action == brands[0].Action
                && r.HierarchyClasses[0].HierarchyClassId == brands[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == brands[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == brands[0].HierarchyId)), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_SuccessfulAddOrUpdateManufacturerMessage_ShouldAcknowledgeMessageAndNotLogError()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(new List<HierarchyClassModel>
                {
                    new HierarchyClassModel
                    {
                        Action = ActionEnum.AddOrUpdate,
                        HierarchyClassId = 1,
                        HierarchyClassName = "test",
                        HierarchyId = Hierarchies.Manufacturer
                    }
                });

            //When
            listener.HandleMessage(mockDvsMessage);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<DvsMessage>()), Times.Once);
            mockAddOrUpdateHierarchyClassService.Verify(m => m.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageHasManufacturerDeletes_ShouldCallManufacturerDeleteService()
        {
            // Given
            List<HierarchyClassModel> manufacturer = new List<HierarchyClassModel>();
            manufacturer.Add(new HierarchyClassModel
            {
                Action = Icon.Esb.Schemas.Wfm.Contracts.ActionEnum.Delete,
                HierarchyClassId = 4747454,
                HierarchyClassName = "Manufacturer1",
                HierarchyClassParentId = 0,
                HierarchyId = Hierarchies.Manufacturer,
                Timestamp = DateTime.UtcNow
            });

            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<DvsMessage>()))
                .Returns(manufacturer);

            // When
            listener.HandleMessage(mockDvsMessage);

            // Then
            mockAddOrUpdateHierarchyClassService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassRequest>()), Times.Never);
            mockDeleteBrandService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteBrandRequest>()), Times.Never);
            mockDeleteMerchandiseService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteMerchandiseClassRequest>()), Times.Never);
            mockDeleteNationalService.Verify(ds => ds.ProcessHierarchyClasses(It.IsAny<DeleteNationalClassRequest>()), Times.Never);
            mockDeleteManufacturerService.Verify(ds => ds.ProcessHierarchyClasses(It.Is<DeleteManufacturerRequest>(r =>
                r.HierarchyClasses[0].HierarchyClassId == manufacturer[0].HierarchyClassId
                && r.HierarchyClasses[0].HierarchyClassName == manufacturer[0].HierarchyClassName
                && r.HierarchyClasses[0].HierarchyId == manufacturer[0].HierarchyId)), Times.Once);
        }
    }
}