using System;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Mammoth.Esb.AttributeListener.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Esb.AttributeListener.Commands;
using Mammoth.Common.DataAccess.DbProviders;
using System.Transactions;

namespace Mammoth.Esb.ProductListener.Tests.Commands
{
    [TestClass]
    public class AddOrUpdateAttributesCommandHandlerTests
    {
        private AddOrUpdateAttributesCommandHandler commandHandler;
        private SqlDbProvider dbProvider;
        private SqlConnection sqlConnection;
        private TransactionScope transaction;
 
        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString);
            sqlConnection.Open();

            dbProvider = new SqlDbProvider
            {
                Connection = sqlConnection,
                Transaction = sqlConnection.BeginTransaction()
            };

            commandHandler = new AddOrUpdateAttributesCommandHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
            sqlConnection.Dispose();
        }

        [TestMethod]
        public void AddOrUpdateAttributesCommandHandler_Execute_AddsAttributes_To_Mammoth_Attribute_Table()
        {
            //When
            var attr1 = new AttributeType
            {
                Description = "desc1",
                Group = "GLI",
                Name = "name1",
                TraitCode = "qwe"
            };

            var attr2 = new AttributeType
            {
                Description = "desc2",
                Group = "ITL",
                Name = "name2",
                TraitCode = "yko"
            };

            var attributes = new AttributeType[] { attr1, attr2 };

            //Then
            commandHandler.Execute(new AddOrUpdateAttributesCommand
            {
                Attributes = new AttributesType
                {
                    Attribute = attributes
                }
            });

            var attr1ExistsCount = sqlConnection.QueryFirst<int>("SELECT COUNT(*) from dbo.attributes where attributecode = @TraitCode", new { attr1.TraitCode });
            Assert.AreEqual(1, attr1ExistsCount);

            var attr2ExistsCount = sqlConnection.QueryFirst<int>("SELECT COUNT(*) from dbo.attributes where attributecode = @TraitCode", new { attr2.TraitCode });
            Assert.AreEqual(1, attr2ExistsCount);
        }
    }
}
