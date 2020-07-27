using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Newtonsoft.Json;

namespace Warp.ProcessPrices.Common
{
    public static class DbSecretManager
    {

        private static bool isInitialized;
        private static SecretsManagerCache cache;

        public static void Initialize()
        {
            var client = new AmazonSecretsManagerClient(new AmazonSecretsManagerConfig {RegionEndpoint = RegionEndpoint.GetBySystemName("us-east-1")});
            cache = new SecretsManagerCache(new SecretCacheConfiguration() { Client = client});
            isInitialized = true;
        }

        public static string GetConnectionString(string secretName)
        {
            var sw = new Stopwatch();
            sw.Start();
            var secret = GetDatabaseSecretAsync(secretName);
            sw.Stop();
            Console.WriteLine($"Get database secret: {sw.ElapsedMilliseconds} ms");
            return GetConnectionStringFromDatabaseSecret(secret);
        }

        public static DatabaseSecret GetDatabaseSecretAsync(string secretName)
        {
            if (!isInitialized) throw new Exception("DbSecrentManager must be initialzied.");
            if (string.IsNullOrEmpty(secretName)) throw new ArgumentNullException(nameof(secretName));

            var awsSecret = cache.GetSecretString(secretName).Result;
            var dbSecret = JsonConvert.DeserializeObject<DatabaseSecret>(awsSecret);
            return dbSecret;
        }

        public static string GetConnectionStringFromDatabaseSecret(DatabaseSecret databaseSecret)
        {
            if (!isInitialized) throw new Exception("DbSecrentManager must be initialzied.");
            if (databaseSecret == null) throw new ArgumentNullException(nameof(databaseSecret));

            var connectionString =
                $"Server={databaseSecret.host};Port={databaseSecret.port};Database={databaseSecret.dbname};User Id={databaseSecret.username};Password={databaseSecret.password};";

            return connectionString;

        }
    }
}