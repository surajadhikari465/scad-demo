using Mammoth.Logging;
using MammothGpmPrice.Service;
using MammothGpmService.Controller;
using MammothGpmService.DataAccess;
using MammothGpmService.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MammothGpmService.AmazonUploader;

namespace MammothGPMService.Test.Controller
{
	[TestClass]
	public class PriceDataControllerTests
	{
		//private variables
		private PriceDataContoller priceDataController;
		private Mock<IGetPriceDataHandler> getCombinedPriceHandler;
		private Mock<IAmazonFileUploader> getAmazonUploader;
		private Mock<ILogger> mockLogger;

		[TestInitialize]
		public void Initialize()
		{
			getAmazonUploader = new Mock<IAmazonFileUploader>();
			getCombinedPriceHandler = new Mock<IGetPriceDataHandler>();
			mockLogger = new Mock<ILogger>();
			priceDataController = new PriceDataContoller(getCombinedPriceHandler.Object, getAmazonUploader.Object);
		}


		[TestMethod]
		public void PriceDataContoller_ReturnDataFromDbOnce()
		{
			//Given
			getCombinedPriceHandler.Setup(m => m.GetAuditFileByRegion("FL")).Returns(true);

			//When
			priceDataController.Execute();

			//Then
			getCombinedPriceHandler.Verify(m => m.GetAuditFileByRegion("FL"), Times.Once);
		}

		[TestMethod]
		public void PriceDataContoller_RunAmazonServiceOnce()
		{
			//Given
			getCombinedPriceHandler.Setup(m => m.GetAuditFileByRegion("FL")).Returns(true);
			getAmazonUploader.Setup(m => m.SendMyFileToS3("FL")).Returns(true);

			//When
			priceDataController.Execute();

			//Then
			getCombinedPriceHandler.Verify(m => m.GetAuditFileByRegion("FL"), Times.Once);
			getAmazonUploader.Verify(m => m.SendMyFileToS3("FL"), Times.Once);
		}

		[TestMethod]
		public void PriceDataContoller_DeleteLocalRegionFilesAfterUploadingToS3()
		{
			//Given
			getCombinedPriceHandler.Setup(m => m.GetAuditFileByRegion("FL")).Returns(true);
			getAmazonUploader.Setup(m => m.SendMyFileToS3("FL")).Returns(true);

			//When
			priceDataController.Execute();

			//Then
			getCombinedPriceHandler.Verify(m => m.GetAuditFileByRegion("FL"), Times.Once);
			getCombinedPriceHandler.Verify(m => m.DeleteFile("FL"), Times.Once);
			getAmazonUploader.Verify(m => m.SendMyFileToS3("FL"), Times.Once);
		}

		[TestMethod]
		public void PriceDataContoller_DeleteLocalRegionFilesWhenNoDataInRegionFile()
		{
			//Given
			getCombinedPriceHandler.Setup(m => m.GetAuditFileByRegion("FL")).Returns(false);
			getAmazonUploader.Setup(m => m.SendMyFileToS3("FL")).Returns(false);

			//When
			priceDataController.Execute();

			//Then
			getCombinedPriceHandler.Verify(m => m.GetAuditFileByRegion("FL"), Times.Once);
			getCombinedPriceHandler.Verify(m => m.DeleteFile("FL"), Times.Once);
			getAmazonUploader.Verify(m => m.SendMyFileToS3("FL"), Times.Never);
		}
	}
}
