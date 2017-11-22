using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Common;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class DbConnectionServiceDecoratorTests
    {
        private Mock<IUpdateService<AddUpdateItemLocale>> mockService;
        private IServiceSettings settings;
        private Mock<IDbConnection> mockConnection;

        private AddUpdateItemLocale serviceData;
        private DbConnectionServiceDecorator<AddUpdateItemLocale> decorator;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockService = new Mock<IUpdateService<AddUpdateItemLocale>>();
            this.settings = new ServiceSettings();
            this.settings.ConnectionString = ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString;

            // Setup command data
            this.serviceData = new AddUpdateItemLocale();
            this.serviceData.ItemLocales = new List<ItemLocaleServiceModel>
            {
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build()
            };
        }

        [TestMethod]
        public void DbConnectionDecorator_ValidServiceCommandData_ServiceHandleMethodCalled()
        {
            // Given
            var db = new Mock<IDbProvider>();
            db.Setup(d => d.Connection).Returns(new SqlConnection(this.settings.ConnectionString));

            this.decorator = new DbConnectionServiceDecorator<AddUpdateItemLocale>(this.mockService.Object, db.Object, this.settings);

            // When
            this.decorator.Handle(this.serviceData);

            // Then
            this.mockService.Verify(s => s.Handle(It.IsAny<AddUpdateItemLocale>()), Times.Once);
        }

        [TestMethod]
        public void DbConnectionDecorator_ValidServiceCommandData_SqlDbConnectionSet()
        {
            // Given
            var mockDb = new Mock<IDbProvider>();
            mockDb.SetupProperty(db => db.Connection);
            this.decorator = new DbConnectionServiceDecorator<AddUpdateItemLocale>(this.mockService.Object, mockDb.Object, this.settings);

            // When
            this.decorator.Handle(this.serviceData);

            // Then
            mockDb.VerifySet(db => db.Connection = It.IsAny<SqlConnection>(), "The Db Connection was not set to a SqlConnection.");
        }

        [TestMethod]
        public void DbConnectionDecorator_ValidServiceCommandData_DbConnectionOpenedOnce()
        {
            // Given
            this.mockConnection = new Mock<IDbConnection>();
            this.mockConnection.Setup(c => c.ConnectionString).Returns(this.settings.ConnectionString);

            var db = new Mock<IDbProvider>();
            db.Setup(d => d.Connection).Returns(this.mockConnection.Object);

            this.decorator = new DbConnectionServiceDecorator<AddUpdateItemLocale>(this.mockService.Object, db.Object, this.settings);

            // When
            this.decorator.Handle(this.serviceData);

            // Then
            this.mockConnection.Verify(c => c.Open(), Times.Once, "The Db Connection Open() method was not called one time.");
        }
    }
}
