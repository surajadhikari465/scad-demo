using Mammoth.Common;
using Mammoth.Common.ControllerApplication.Http;
using Mammoth.Common.DataAccess;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mammoth.Price.Controller.Tests.Services
{
    [TestClass]
    public class PriceServiceTests
    {
        private PriceService service;
        private Mock<IHttpClientWrapper> mockClientWrapper;
        private Mock<ILogger> mockLogger;
        private List<PriceEventModel> data;
        private List<PriceModel> model;

        [TestInitialize]
        public void Initialize()
        {
            mockClientWrapper = new Mock<IHttpClientWrapper>();
            mockLogger = new Mock<ILogger>();

            service = new PriceService(mockClientWrapper.Object, mockLogger.Object);

            data = new List<PriceEventModel>();
            model = new List<PriceModel>();
        }

        [TestMethod]
        public void PriceServiceProcess_GivenNoEvents_ShouldNotProcessData()
        {
            //Given
            data = new List<PriceEventModel>();

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()), Times.Never);
            mockClientWrapper.Verify(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()), Times.Never);
            mockClientWrapper.Verify(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void PriceServiceProcess_MessageFailedWithPutCall_ShouldReprocessMessagesOneByOne()
        {
            //Given
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.Price }
                };

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            foreach (var il in data)
            {
                Assert.IsNull(il.ErrorMessage);
            }

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 3
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }

        [TestMethod]
        public void PriceServiceProcess_MesssageFailedOnReprocessWithPutCalls_ShouldSetErrorMessage()
        {
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.Price }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
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

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 3
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }

        [TestMethod]
        public void PriceServiceProcess_DataHasRowsWithOnlyPriceEventType_ClientPutAsJsonAsyncOnlyCalled()
        {
            //Given
            data = new List<PriceEventModel>();
            data.Add(new PriceEventModel { EventTypeId = IrmaEventTypes.Price, ScanCode = "1" });
            data.Add(new PriceEventModel { EventTypeId = IrmaEventTypes.Price, ScanCode = "2" });
            data.Add(new PriceEventModel { EventTypeId = IrmaEventTypes.Price, ScanCode = "3" });
            mockClientWrapper.Setup(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()), Times.Once);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()), Times.Never);
            mockClientWrapper.Verify(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void PriceServiceProcess_DataHasRowsWithOnlyRollbackEventType_ClientPostAsJsonAsyncOnlyCalled()
        {
            //Given
            data = new List<PriceEventModel>();
            data.Add(new PriceEventModel { EventTypeId = IrmaEventTypes.PriceRollback, ScanCode = "1" });
            data.Add(new PriceEventModel { EventTypeId = IrmaEventTypes.PriceRollback, ScanCode = "2" });
            data.Add(new PriceEventModel { EventTypeId = IrmaEventTypes.PriceRollback, ScanCode = "3" });
            mockClientWrapper.Setup(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockClientWrapper.Verify(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()), Times.Never);
            mockClientWrapper.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()), Times.Once);
            mockClientWrapper.Verify(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void PriceServiceProcess_MessageFailedWithPostCallForPriceRollback_ShouldReprocessMessagesInBatches()
        {
            //Given
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "4", EventTypeId = IrmaEventTypes.PriceRollback }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            foreach (var il in data)
            {
                Assert.IsNull(il.ErrorMessage);
            }

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 4
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3")
                    && e.Select(p => p.ScanCode).Contains("4"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 2
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 2
                    && e.Select(p => p.ScanCode).Contains("3")
                    && e.Select(p => p.ScanCode).Contains("4"))), Times.Exactly(1));
        }

        [TestMethod]
        public void PriceServiceProcess_MesssageFailedOnReprocessWithPostCall_ShouldSetErrorMessage()
        {
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.PriceRollback }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
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

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 3
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }

        [TestMethod]
        public void PriceServiceProcess_DataHasPriceUpdateAndRollbackEventTypesAndMessageFailedOnReprocessForPutCallForPriceEvents_ShouldSetErrorMessageDetails()
        {
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.PriceRollback }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse));

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            model = new List<PriceModel>
            {
                new PriceModel { ScanCode = "1" },
                new PriceModel { ScanCode = "2" },
                new PriceModel { ScanCode = "3" }
            };

            foreach (var il in data.Where(d => d.EventTypeId == IrmaEventTypes.Price))
            {
                Assert.AreEqual(errorResponse.ReasonPhrase, il.ErrorMessage);
                Assert.AreEqual(errorResponse.Content.ReadAsStringAsync().Result, il.ErrorDetails);
                Assert.AreEqual(Constants.SourceSystem.MammothWebApi, il.ErrorSource);
            }

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 2
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Once);
        }

        [TestMethod]
        public void PriceServiceProcess_DataHasPriceAndRollbackEventTypesAndMessageFailedOnReprocessForPostCallForRollbackEvents_ShouldSetErrorMessageAndErrorDetails()
        {
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.PriceRollback }
                };
            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse))
                .Returns(Task.FromResult(errorResponse));


            //When
            service.Process(data);

            //Then
            foreach (var il in data.Where(d => d.EventTypeId == IrmaEventTypes.PriceRollback))
            {
                Assert.AreEqual(errorResponse.ReasonPhrase, il.ErrorMessage);
                Assert.AreEqual(errorResponse.Content.ReadAsStringAsync().Result, il.ErrorDetails);
                Assert.AreEqual(Constants.SourceSystem.MammothWebApi, il.ErrorSource);
            }

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 2
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }

        [TestMethod]
        public void PriceServiceProcess_DataHasPriceAndRollbackEventTypesAndMessagesFailedOnReprocess_ShouldSetErrorMessageAndErrorDetails()
        {
            data = new List<PriceEventModel>
                {
                    new PriceEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.Price },
                    new PriceEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.PriceRollback },
                    new PriceEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.PriceRollback }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockClientWrapper.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
                .Returns(Task.FromResult(errorResponse));

            mockClientWrapper.SetupSequence(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<PriceModel>>()))
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

            mockClientWrapper.Verify(m => m.PutAsJsonAsync(Uris.PriceUpdate,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 2
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockClientWrapper.Verify(m => m.PostAsJsonAsync(Uris.PriceRollback,
                It.Is<IEnumerable<PriceModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }
    }
}
