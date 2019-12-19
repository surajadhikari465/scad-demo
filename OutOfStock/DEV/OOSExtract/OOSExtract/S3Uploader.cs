using System;
using System.IO;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Serilog;

namespace OOSExtract
{
    public class S3Uploader
    {
        private readonly RegionEndpoint _bucketRegion = RegionEndpoint.USEast1;
        private IAmazonS3 _client;

        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly string _kmsKey;

        public S3Uploader(string accessKey, string secretKey, string kmsKey)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _kmsKey = kmsKey;
        }

        public void UploadFile(FileStream fs, string bucketName, string keyName)
        {

            var credentials = new BasicAWSCredentials(_accessKey, _secretKey);

            try
            {

                using (_client = new AmazonS3Client(credentials, _bucketRegion))
                {
                    PutObjectRequest request = new PutObjectRequest
                    {
                        InputStream = fs,
                        BucketName = bucketName,
                        Key = keyName,
                        ServerSideEncryptionMethod = ServerSideEncryptionMethod.AWSKMS,
                        ServerSideEncryptionKeyManagementServiceKeyId = _kmsKey
                    };
                    // do it!
                    _client.PutObject(request);
                    // fin!

                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex.Message);
            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }

        }

    }
}