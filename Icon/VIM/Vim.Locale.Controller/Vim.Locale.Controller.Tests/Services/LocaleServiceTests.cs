using Vim.Common;
using Vim.Common.ControllerApplication.Http;
using Vim.Common.DataAccess;
using Vim.Locale.Controller.DataAccess;
using Vim.Locale.Controller.DataAccess.Models;
using Vim.Locale.Controller.Services;
using Vim.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Vim.Locale.Controller.Tests.Services
{
    [TestClass]
    public class LocaleServiceTests
    {
        private LocaleService service;
        private Mock<IHttpClientWrapper> mockClientWrapper;
        private Mock<ILogger> mockLogger;
        private List<LocaleEventModel> data;
        private List<VimStoreModel> stores;

        [TestInitialize]
        public void Initialize()
        {
            mockClientWrapper = new Mock<IHttpClientWrapper>();
            mockLogger = new Mock<ILogger>();

            service = new LocaleService(mockClientWrapper.Object, mockLogger.Object);

            data = new List<LocaleEventModel>();
            stores = new List<VimStoreModel>();
        }

        [TestMethod]
        public void Process_AllTypesOfLocaleEventsExist_ShouldUpdateAddAndDeleteEachLocale()
        {
            //Given
            data = new List<LocaleEventModel>
                {
                    new LocaleEventModel { EventReferenceId = 1, EventTypeId = EventTypes.LocaleAdd, StoreModel = new VimStoreModel { StoreName = "Test Add", PSBU = 0 } },
                    new LocaleEventModel { EventReferenceId = 2, EventTypeId = EventTypes.LocaleUpdate , StoreModel = new VimStoreModel { StoreName = "Test Update", PSBU = 9 }}
                };

            foreach (LocaleEventModel localeEvent in data)
            {
                stores.Add(localeEvent.StoreModel);
            }

            mockClientWrapper.Setup(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<VimStoreModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                It.Is<IEnumerable<VimStoreModel>>(e => e.Count() == 2
                && e.Contains(stores.First(s => s.PSBU == 0 && s.StoreName == "Test Add"))
                && e.Contains(stores.First(s => s.PSBU == 9 && s.StoreName == "Test Update"))
                && e.All(s => s.Fax == string.Empty)
                && e.All(s => s.Addr2 == string.Empty)
                && e.All(s => s.Addr1 == string.Empty)
                && e.All(s => s.City == string.Empty)
                && e.All(s => s.Country == string.Empty)
                && e.All(s => s.Phone == string.Empty)
                && e.All(s => s.LastUser == string.Empty)
                && e.All(s => s.PostalCode == string.Empty)
                && e.All(s => s.PosType == string.Empty)
                && e.All(s => s.Region == string.Empty)
                && e.All(s => s.RegStoreNum == string.Empty)
                && e.All(s => s.StateProvince == string.Empty)
                && e.All(s => s.Status == string.Empty)
                && e.All(s => s.StoreAbbreviation == string.Empty)
                && e.All(s => s.TimeZone == string.Empty)
            )));
        }

        [TestMethod]
        public void Process_DataIsEmpy_ShouldNotCallHttpClient()
        {
            //Given
            data = new List<LocaleEventModel>();

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<VimStoreModel>>()), Times.Never);
        }

        [TestMethod]
        public void Process_MessageFailed_ShouldReprocessMessagesOneByOne()
        {
            //Given
            data = new List<LocaleEventModel>
                {
                    new LocaleEventModel { EventReferenceId = 1, EventTypeId = EventTypes.LocaleAdd, StoreModel = new VimStoreModel { StoreName = "Test Add", PSBU = 0 } },
                    new LocaleEventModel { EventReferenceId = 2, EventTypeId = EventTypes.LocaleUpdate , StoreModel = new VimStoreModel { StoreName = "Test Update 1", PSBU = 1 }},
                    new LocaleEventModel { EventReferenceId = 3, EventTypeId = EventTypes.LocaleUpdate , StoreModel = new VimStoreModel { StoreName = "Test Update 2", PSBU = 2 }}
                };

            foreach (LocaleEventModel localeEvent in data)
            {
                stores.Add(localeEvent.StoreModel);
            }

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<VimStoreModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                It.Is<IEnumerable<VimStoreModel>>(e => e.Count() == 3
                    && e.Contains(stores.First(s => s.PSBU == 0 && s.StoreName == "Test Add"))
                    && e.Contains(stores.First(s => s.PSBU == 1 && s.StoreName == "Test Update 1"))
                    && e.Contains(stores.First(s => s.PSBU == 2 && s.StoreName == "Test Update 2")))), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                It.Is<VimStoreModel[]>(s => s.Contains(stores[0]))), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                 It.Is<VimStoreModel[]>(s => s.Contains(stores[1]))), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                 It.Is<VimStoreModel[]>(s => s.Contains(stores[2]))), Times.Once);
        }

        [TestMethod]
        public void Process_MesssageFailedOnReprocess_ShouldSetErrorMessage()
        {

            //Given
            data = new List<LocaleEventModel>
                {
                    new LocaleEventModel { EventReferenceId = 1, EventTypeId = EventTypes.LocaleAdd, StoreModel = new VimStoreModel { StoreName = "Test Add", PSBU = 0 } },
                    new LocaleEventModel { EventReferenceId = 2, EventTypeId = EventTypes.LocaleUpdate , StoreModel = new VimStoreModel { StoreName = "Test Update 1", PSBU = 1 }},
                    new LocaleEventModel { EventReferenceId = 3, EventTypeId = EventTypes.LocaleUpdate , StoreModel = new VimStoreModel { StoreName = "Test Update 2", PSBU = 2 }}
                };

            foreach (LocaleEventModel localeEvent in data)
            {
                stores.Add(localeEvent.StoreModel);
            }

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<VimStoreModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)));

            //When
            service.Process(data);

            //Then
            foreach (var s in data)
            {
                Assert.AreEqual("Bad Request", s.ErrorMessage);
            }

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                It.Is<IEnumerable<VimStoreModel>>(e => e.Count() == 3
                    && e.Contains(stores.First(s => s.PSBU == 0 && s.StoreName == "Test Add"))
                    && e.Contains(stores.First(s => s.PSBU == 1 && s.StoreName == "Test Update 1"))
                    && e.Contains(stores.First(s => s.PSBU == 2 && s.StoreName == "Test Update 2")))), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                It.Is<VimStoreModel[]>(s => s.Contains(stores[0]))), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                 It.Is<VimStoreModel[]>(s => s.Contains(stores[1]))), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.StoreAddOrUpdateUri,
                 It.Is<VimStoreModel[]>(s => s.Contains(stores[2]))), Times.Once);
        }
    }
}
