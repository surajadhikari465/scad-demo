using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.DataAccess;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.ItemLocale.Controller.Services;
using Mammoth.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using MoreLinq;
using Mammoth.Common;

namespace Mammoth.ItemLocale.Controller.Tests.Services
{
    [TestClass]
    public class ItemLocaleServiceTests
    {
        private ItemLocaleService service;
        private Mock<IHttpClientWrapper> mockClientWrapper;
        private Mock<ILogger> mockLogger;
        private List<ItemLocaleEventModel> data;
        private ItemLocaleControllerApplicationSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockClientWrapper = new Mock<IHttpClientWrapper>();
            mockLogger = new Mock<ILogger>();
            this.settings = new ItemLocaleControllerApplicationSettings { CurrentRegion = "FL", ApiRowLimit = 100 };
            service = new ItemLocaleService(mockClientWrapper.Object, mockLogger.Object, settings);

            data = new List<ItemLocaleEventModel>();
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_GivenNoEvents_ShouldNotProcessData()
        {
            //Given
            data = new List<ItemLocaleEventModel>();

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<ItemLocaleEventModel>>()), Times.Never);
            mockClientWrapper.Verify(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<ItemLocaleEventModel>>()), Times.Never);
            mockClientWrapper.Verify(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_ItemLocaleAddUpdateMessageFailed_ShouldReprocessMessagesInBatches()
        {
            //Given
            data = new List<ItemLocaleEventModel>
                {
                    new ItemLocaleEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate },
                    new ItemLocaleEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate },
                    new ItemLocaleEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate },
                    new ItemLocaleEventModel { ScanCode = "4", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate }
                };
            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 4
                    && e.Contains(data.First(hc => hc.ScanCode == "1" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "2" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "3" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "4" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate)))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 2
                    && e.Contains(data[0])
                    && e.Contains(data[1]))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 2
                    && e.Contains(data[2])
                    && e.Contains(data[3]))), Times.Exactly(1));
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_DeauthorizeMessageFailed_ShouldReprocessMessagesInBatches()
        {
            //Given
            data = new List<ItemLocaleEventModel>
                {
                    new ItemLocaleEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.ItemDelete },
                    new ItemLocaleEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.ItemDelete },
                    new ItemLocaleEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.ItemDelete },
                    new ItemLocaleEventModel { ScanCode = "4", EventTypeId = IrmaEventTypes.ItemDelete }
                };

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 4)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 2
                    && e.ToList()[0].ScanCode == data[0].ScanCode
                    && e.ToList()[1].ScanCode == data[1].ScanCode)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 2
                    && e.ToList()[0].ScanCode == data[2].ScanCode
                    && e.ToList()[1].ScanCode == data[3].ScanCode)), Times.Exactly(1));
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_ItemLocaleAddOrUpdateMesssageFailedOnReprocess_ShouldSetErrorMessage()
        {
            data = new List<ItemLocaleEventModel>
                {
                    new ItemLocaleEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate },
                    new ItemLocaleEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate },
                    new ItemLocaleEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing itemLocale there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse));

            //When
            service.Process(data);

            //Then
            foreach (var il in data)
            {
                Assert.AreEqual(errorResponse.ReasonPhrase, il.ErrorMessage);
                Assert.AreEqual(errorResponse.Content.ReadAsStringAsync().Result, il.ErrorDetails);
                Assert.AreEqual(Constants.SourceSystem.MammothWebApi, il.ErrorSource);
            }

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 3
                    && e.Contains(data.First(hc => hc.ScanCode == "1" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "2" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "3" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate)))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[0]))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[1]))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[2]))), Times.Exactly(1));
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_DeauthorizeMesssageFailedOnReprocess_ShouldSetErrorMessage()
        {
            data = new List<ItemLocaleEventModel>
                {
                    new ItemLocaleEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing itemLocale there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse));

            //When
            service.Process(data);

            //Then
            foreach (var il in data)
            {
                Assert.AreEqual(errorResponse.ReasonPhrase, il.ErrorMessage);
                Assert.AreEqual(errorResponse.Content.ReadAsStringAsync().Result, il.ErrorDetails);
                Assert.AreEqual(Constants.SourceSystem.MammothWebApi, il.ErrorSource);
            }


            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 3)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[0].ScanCode && e.ToList()[0].Region == data[0].Region)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[1].ScanCode && e.ToList()[0].Region == data[1].Region)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[2].ScanCode && e.ToList()[0].Region == data[2].Region)), Times.Exactly(1));
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_BothEventTypesMessageFailed_ShouldReprocessForBothEventTypes()
        {
            // Given
            data = new List<ItemLocaleEventModel>
                {
                    new ItemLocaleEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "4", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "5", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate, Region = "MW" }
                };

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // When
            service.Process(data);

            // Then
            foreach (var il in data)
            {
                Assert.IsNull(il.ErrorMessage);
            }

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 3
                    && e.Contains(data.First(hc => hc.ScanCode == "1" && hc.EventTypeId == IrmaEventTypes.ItemDelete))
                    && e.Contains(data.First(hc => hc.ScanCode == "2" && hc.EventTypeId == IrmaEventTypes.ItemDelete))
                    && e.Contains(data.First(hc => hc.ScanCode == "3" && hc.EventTypeId == IrmaEventTypes.ItemDelete)))),
                Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[0].ScanCode && e.ToList()[0].Region == data[0].Region
                    && e.ToList()[0].EventTypeId == IrmaEventTypes.ItemDelete)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[1].ScanCode && e.ToList()[0].Region == data[1].Region
                    && e.ToList()[0].EventTypeId == IrmaEventTypes.ItemDelete)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[2].ScanCode && e.ToList()[0].Region == data[2].Region
                    && e.ToList()[0].EventTypeId == IrmaEventTypes.ItemDelete)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 2
                    && e.Contains(data.First(hc => hc.ScanCode == "4" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "5" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate)))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[3]))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[4]))), Times.Exactly(1));
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_BothEventTypesMessageFailedOnReprocess_ShouldSetErrorMessageForBothEventTypes()
        {
            // Given
            data = new List<ItemLocaleEventModel>
                {
                    new ItemLocaleEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.ItemDelete, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "4", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate, Region = "MW" },
                    new ItemLocaleEventModel { ScanCode = "5", EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate, Region = "MW" }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing itemLocale there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse));

            // When
            service.Process(data);

            // Then
            foreach (var il in data)
            {
                Assert.AreEqual(errorResponse.ReasonPhrase, il.ErrorMessage);
                Assert.AreEqual(errorResponse.Content.ReadAsStringAsync().Result, il.ErrorDetails);
                Assert.AreEqual(Constants.SourceSystem.MammothWebApi, il.ErrorSource);
            }


            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 3)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[0].ScanCode && e.ToList()[0].Region == data[0].Region)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[1].ScanCode && e.ToList()[0].Region == data[1].Region)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.ToList()[0].ScanCode == data[2].ScanCode && e.ToList()[0].Region == data[2].Region)), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 2
                    && e.Contains(data.First(hc => hc.ScanCode == "4" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate))
                    && e.Contains(data.First(hc => hc.ScanCode == "5" && hc.EventTypeId == IrmaEventTypes.ItemLocaleAddOrUpdate)))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[3]))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 1
                    && e.Contains(data[4]))), Times.Exactly(1));
        }

        [TestMethod]
        public void ItemLocaleServiceProcess_ItemLocaleDataExceedsApiRowLimitSetting_WebApiCalledInBatchesMoreThanOnce()
        {
            // Given
            this.settings.ApiRowLimit = 10;

            for (int i = 0; i < this.settings.ApiRowLimit * 3; i++)
            {
                data.Add(new ItemLocaleEventModel { ScanCode = i.ToString(), EventTypeId = IrmaEventTypes.ItemLocaleAddOrUpdate, Region = "MW" });
            }

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.Is<string>(s => s == Uris.ItemLocaleUpdate), It.IsAny<IEnumerable<ItemLocaleEventModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            // When
            service.Process(data);

            // Then
            foreach (var il in data)
            {
                Assert.IsNull(il.ErrorMessage);
            }


            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.ItemLocaleUpdate,
                It.Is<IEnumerable<ItemLocaleEventModel>>(e => e.Count() == 10)), Times.Exactly(3));
        }
    }
}
