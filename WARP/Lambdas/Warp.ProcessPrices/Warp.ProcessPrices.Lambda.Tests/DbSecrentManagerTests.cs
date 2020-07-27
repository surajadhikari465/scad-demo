using Warp.ProcessPrices.Common;
using Xunit;

namespace Warp.ProcessPrices.Lambda.Tests
{
    public class DbSecrentManagerTests : IClassFixture<DbSecrentManagerTestsFixture>
    {
        DbSecrentManagerTestsFixture fixture;
        public DbSecrentManagerTests(DbSecrentManagerTestsFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public void DbSecretManager_GetDatabaseSecret_ReturnsDatabaseSecret()
        {
            DbSecretManager.Initialize();
            var secret = DbSecretManager.GetDatabaseSecretAsync("testsecret1234");
            Assert.Equal("test", secret.password);
            Assert.Equal("warp", secret.dbname);
            Assert.Equal("postgres", secret.engine);
            Assert.Equal(3306, secret.port);
            Assert.Equal("testhost", secret.host);
            Assert.Equal("warpadmindev", secret.username);
        }

        [Fact]
        public void DbSecretManager_GetConnectionString_ReturnsConnectionString()
        {

            var expextedValue = "Server=testhost;Port=3306;Database=warp;User Id=warpadmindev;Password=test;";

            DbSecretManager.Initialize();
            var secret = DbSecretManager.GetConnectionString("testsecret1234");
            Assert.Equal(expextedValue, secret);
        }

        [Fact]
        public void DbSecretManager_GetConnectionStringFromDatabaseSecret_ReturnsConnectionStringFromDatabaseSecret()
        {

            var expextedValue = "Server=testhost;Port=3306;Database=warp;User Id=warpadmindev;Password=test;";

            DbSecretManager.Initialize();
            var secret = DbSecretManager.GetDatabaseSecretAsync("testsecret1234");
            var conStr = DbSecretManager.GetConnectionStringFromDatabaseSecret(secret);
            Assert.Equal(expextedValue, conStr);
        }

    }
}
