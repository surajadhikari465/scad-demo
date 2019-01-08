using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothGpmService.AmazonUploader;
using MammothGpmService.DataAccess;
using MammothGpmService.Models;

namespace MammothGpmService.Controller
{
	public class PriceDataContoller 	{
		private IGetPriceDataHandler getPriceDataHandler;
		private ILogger logger;
		private IAmazonFileUploader amazonUploader;
		private static readonly string[] regions = {"FL","MA","MW","NA","RM","SO","NC","NE","PN","SP","SW","UK"};
		public PriceDataContoller(IGetPriceDataHandler gh, IAmazonFileUploader amz)
		{
			this.getPriceDataHandler = gh;
			this.amazonUploader = amz;
		}

		public void Execute()
		{
			foreach (var region in regions)
			{
				bool dataFlag = false;
				try
				{
					dataFlag = getPriceDataHandler.GetAuditFileByRegion(region);
				}
				catch (Exception ex)
				{
					logger.Error("Application error occured due to database connection with error message: " + ex.Message);
				}
				if (dataFlag == true)
				{
					try
					{
						amazonUploader.SendMyFileToS3(region);
					}
					catch (Exception ex)
					{
						logger.Error("Application error occured in connecting with S3 bucket with error message: " + ex.Message);
					}
				}
				try
				{
					getPriceDataHandler.DeleteFile(region);
				}
				catch (Exception ex)
				{
					logger.Error("Application could not delete the local file with error message: " + ex.Message);
				}
			}
		}
	}
}
