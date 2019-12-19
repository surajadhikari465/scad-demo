using Dapper;
using Icon.Common.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class DoesAttributeExistOnItemsQueryHandlerTests
    {
        private DoesAttributeExistOnItemsQueryHandler queryHandler;
        private DoesAttributeExistOnItemsParameters parameters;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
        private int attributeId;
        private ItemTestHelper itemTestHelper;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = SqlConnectionBuilder.CreateIconConnection();
            queryHandler = new DoesAttributeExistOnItemsQueryHandler(sqlConnection);
            attributeId = SqlDataGenerator.CreateAttribute(sqlConnection, "TestAttribute");
            itemTestHelper = new ItemTestHelper();
            itemTestHelper.Initialize(sqlConnection);
            parameters = new DoesAttributeExistOnItemsParameters { AttributeId = attributeId };
        }

        [TestCleanup]
        public void Cleanup()
        {
            sqlConnection.Dispose();
            transaction.Dispose();
        }

        [TestMethod]
        public void DoesAttributeExistOnItems_AttributeExistsOnItems_ReturnsTrue()
        {
            //Given
            itemTestHelper.TestItem.ItemAttributesJson = @"{""TestAttribute"":""1""}";
            sqlConnection.Execute("UPDATE dbo.Item SET ItemAttributesJson = @ItemAttributesJson WHERE ItemId = @ItemId", itemTestHelper.TestItem);

            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DoesAttributeExistOnItems_AttributeDoesntExistOnItems_ReturnsFalse()
        {
            //When
            var result = queryHandler.Search(parameters);

            //Then
            Assert.IsFalse(result);
        }
    }
}
