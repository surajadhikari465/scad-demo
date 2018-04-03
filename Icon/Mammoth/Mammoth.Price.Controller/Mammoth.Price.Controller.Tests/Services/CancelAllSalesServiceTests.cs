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
    public class CancelAllSalesServiceTests
    {
        private CancelAllSalesService service;
        private Mock<IHttpClientWrapper> mockHttpClient;
        private Mock<ILogger> mockLogger;
        private List<CancelAllSalesEventModel> data;
        private List<CancelAllSalesModel> model;

        [TestInitialize]
        public void Initialize()
        {
            mockHttpClient = new Mock<IHttpClientWrapper>();
            mockLogger = new Mock<ILogger>();

            service = new CancelAllSalesService(mockHttpClient.Object, mockLogger.Object);
            {
                mockHttpClient = new Mock<IHttpClientWrapper>();
                mockLogger = new Mock<ILogger>();

                service = new CancelAllSalesService(mockHttpClient.Object, mockLogger.Object);

                data = new List<CancelAllSalesEventModel>();
                model = new List<CancelAllSalesModel>();
            }
        }

        [TestMethod]
        public void CancelAllSalesServiceProcess_GivenNoEvents_ShouldNotProcessData()
        {
            //Given
            data = new List<CancelAllSalesEventModel>();

            //When
            service.Process(data);

            //Then
            mockHttpClient.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()), Times.Never);
            mockHttpClient.Verify(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()), Times.Never);
            mockHttpClient.Verify(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
        }

        [TestMethod]
        public void CancelAllSalesServiceProcess_MessageFailedWithPutCall_ShouldReprocessMessagesOneByOne()
        {
            //Given
            data = new List<CancelAllSalesEventModel>
                {
                    new CancelAllSalesEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.CancelAllSales },
                    new CancelAllSalesEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.CancelAllSales },
                    new CancelAllSalesEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.CancelAllSales }
                };

            mockHttpClient.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()))
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

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 3
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }

        [TestMethod]
        public void CancelAllSalesServiceProcess_MesssageFailedOnReprocessWithPutCalls_ShouldSetErrorMessage()
        {
            data = new List<CancelAllSalesEventModel>
                {
                    new CancelAllSalesEventModel { ScanCode = "1", EventTypeId = IrmaEventTypes.CancelAllSales },
                    new CancelAllSalesEventModel { ScanCode = "2", EventTypeId = IrmaEventTypes.CancelAllSales },
                    new CancelAllSalesEventModel { ScanCode = "3", EventTypeId = IrmaEventTypes.CancelAllSales }
                };

            HttpResponseMessage errorResponse = new HttpResponseMessage(HttpStatusCode.InternalServerError);
            errorResponse.ReasonPhrase = HttpStatusCode.InternalServerError.ToString();
            errorResponse.Content = new StringContent(@"[{ ""error"":""testing there was an error"" }]");

            mockHttpClient.SetupSequence(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()))
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

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 3
                    && e.Select(p => p.ScanCode).Contains("1")
                    && e.Select(p => p.ScanCode).Contains("2")
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("1"))), Times.Exactly(1));

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("2"))), Times.Exactly(1));

            mockHttpClient.Verify(m => m.PutAsJsonAsync(Uris.CancelAllSales,
                It.Is<IEnumerable<CancelAllSalesModel>>(e => e.Count() == 1
                    && e.Select(p => p.ScanCode).Contains("3"))), Times.Exactly(1));
        }

        [TestMethod]
        public void CancelAllSalesServiceProcess_DataHasRowsWithOnlyCancelAllSalesEventType_ClientPutAsJsonAsyncOnlyCalled()
        {
            //Given
            data = new List<CancelAllSalesEventModel>();
            data.Add(new CancelAllSalesEventModel { EventTypeId = IrmaEventTypes.CancelAllSales, ScanCode = "1" });
            data.Add(new CancelAllSalesEventModel { EventTypeId = IrmaEventTypes.CancelAllSales, ScanCode = "2" });
            data.Add(new CancelAllSalesEventModel { EventTypeId = IrmaEventTypes.CancelAllSales, ScanCode = "3" });
            mockHttpClient.Setup(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()))
                .Returns(Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)));

            //When
            service.Process(data);

            //Then
            mockHttpClient.Verify(m => m.PutAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()), Times.Once);
            mockHttpClient.Verify(m => m.PostAsJsonAsync(It.IsAny<string>(), It.IsAny<IEnumerable<CancelAllSalesModel>>()), Times.Never);
            mockHttpClient.Verify(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<List<int>>()), Times.Never);
        }
    }
}
