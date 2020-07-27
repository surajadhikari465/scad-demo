using System;
using Amazon.SecretsManager.Model;

namespace Warp.ProcessPrices.Lambda.Tests
{
    public class DbSecrentManagerTestsFixture : IDisposable
    {
        public DbSecrentManagerTestsFixture()
        {
            // ... initialize data ...

            var client = new Amazon.SecretsManager.AmazonSecretsManagerClient();
            var response = client.CreateSecretAsync(new CreateSecretRequest()
            {
                SecretString = @"{
                      ""password"": ""test"",
                      ""dbname"": ""warp"",
                      ""engine"": ""postgres"",
                      ""port"": 3306,
                      ""host"": ""testhost"",
                      ""username"": ""warpadmindev""
                       }",
                Name = "testsecret1234", Description = "test secret"
            }).Result;
        }

        public void Dispose()
        {
            // ... clean up test data ...
            
            var client = new Amazon.SecretsManager.AmazonSecretsManagerClient();
            var response = client.DeleteSecretAsync(new DeleteSecretRequest()
                {ForceDeleteWithoutRecovery = true, SecretId = "testsecret1234"}).Result;
            
        }
    }
}