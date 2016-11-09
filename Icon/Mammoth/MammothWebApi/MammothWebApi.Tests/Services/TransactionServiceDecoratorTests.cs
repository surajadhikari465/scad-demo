using Mammoth.Common.DataAccess.DbProviders;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class TransactionServiceDecoratorTests
    {
        private TransactionServiceDecorator<AddUpdateItemLocale> decorator;
        private Mock<IService<AddUpdateItemLocale>> mockService;
        private AddUpdateItemLocale itemLocaleData;
        private string connectionString;
        private Mock<IDbProvider> mockDb;
        private Mock<IDbConnection> mockConnection;
        private Mock<IDbTransaction> mockTransaction;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockService = new Mock<IService<AddUpdateItemLocale>>();
            this.connectionString = @"Data Source=IDW-DB01-DEV\IDWD;Initial Catalog=Mammoth_UnitTest;Integrated Security=SSPI";
            this.mockDb = new Mock<IDbProvider>();
            this.mockConnection = new Mock<IDbConnection>();
            this.mockTransaction = new Mock<IDbTransaction>();
            this.decorator = new TransactionServiceDecorator<AddUpdateItemLocale>(this.mockService.Object, mockDb.Object);

            mockConnection.Setup(c => c.ConnectionString).Returns(connectionString);
            mockDb.Setup(db => db.Transaction).Returns(mockTransaction.Object);
            mockDb.Setup(db => db.Connection).Returns(mockConnection.Object);

            // Setup command service data
            this.itemLocaleData = new AddUpdateItemLocale();
            this.itemLocaleData.ItemLocales = new List<ItemLocaleServiceModel>
            {
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build(),
                new TestItemLocaleServiceModelBuilder().Build()
            };
        }

        [TestMethod]
        public void TransactionServiceDecorator_ValidServiceCommandData_BeginTransactionCalled()
        {
            // Given

            // When
            this.decorator.Handle(this.itemLocaleData);

            // Then
            mockConnection.Verify(c => c.BeginTransaction(), Times.Once, "The BeginTransaction method was not called on the connection.");
        }

        [TestMethod]
        public void TransactionServiceDecorator_NoExceptionThrownDuringCommandService_DataServiceCommandCalled()
        {
            // Given

            // When
            this.decorator.Handle(this.itemLocaleData);

            // Then
            this.mockService.Verify(s => s.Handle(It.IsAny<AddUpdateItemLocale>()), Times.Once, "The Data Service Command was not called or called more than once.");
        }

        [TestMethod]
        public void TransactionServiceDecorator_NoExceptionThrownDuringCommandService_TransactionCommitCalled()
        {
            // Given

            // When
            this.decorator.Handle(this.itemLocaleData);

            // Then
            mockTransaction.Verify(t => t.Commit(), Times.Once, "The transaction Commit method was not called as expected.");
        }

        [TestMethod]
        public void TransactionServiceDecorator_ExceptionThrownDuringCommandService_TransactionRollbackCalled()
        {
            // Given
            this.mockService.Setup(s => s.Handle(It.IsAny<AddUpdateItemLocale>())).Throws(new NullReferenceException());

            // When
            try
            {
                this.decorator.Handle(this.itemLocaleData);
                Assert.Fail();
            }
            catch (Exception)
            {
                // Then
                mockTransaction.Verify(t => t.Rollback(), Times.Once, "The transaction Rollback method was not called as expected.");
            }
        }


    }
}
