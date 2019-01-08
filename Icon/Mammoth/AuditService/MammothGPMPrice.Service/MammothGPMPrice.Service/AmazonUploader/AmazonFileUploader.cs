using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MammothGpmService.AmazonUploader
{
	public class AmazonFileUploader : IAmazonFileUploader
	{
		// Specify your bucket region (an example region is shown).
		private static string accessId = ConfigurationManager.AppSettings["AWSAccessKey"].ToString();
		private static string secretKeyId = ConfigurationManager.AppSettings["AWSSecretKey"].ToString();
		private static string bucketName = ConfigurationManager.AppSettings["AWSBucketName"].ToString();
		private static string directoryName = ConfigurationManager.AppSettings["AWSDirectoryName"].ToString();
		private static string localFileName = ConfigurationManager.AppSettings["LocalFilePath"].ToString();
		private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;
		private static IAmazonS3 s3Client;

		public bool SendMyFileToS3(string region)
		{
			AmazonS3Config config = new AmazonS3Config();
			config.RegionEndpoint = bucketRegion; 
			config.ServiceURL = ConfigurationManager.AppSettings["AWSBucketRegion"].ToString();
			s3Client = new AmazonS3Client(accessId, secretKeyId, config);
			WritingAnObjectAsync(region).Wait();
			Console.WriteLine("Upload to S3 is done successfully");
			return true;
		}

		private static async Task WritingAnObjectAsync(string region)
		{
			try
			{
				var dataObjectIntoS3 = new PutObjectRequest
				{
					BucketName = bucketName,
					Key = directoryName + "\\Mammoth_Gpm_Price_" + region + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".txt",
					FilePath = localFileName
				};
				PutObjectResponse response1 = await s3Client.PutObjectAsync(dataObjectIntoS3);
			}
			catch (AmazonS3Exception e)
			{
				Console.WriteLine("Error encountered ***. Message:'{0}' when writing to Price Directory of S3 bucket.", e.Message);
			}
			catch (Exception e)
			{
				Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing to Price Directory of S3 bucket.", e.Message);
			}
		}
	}
}
