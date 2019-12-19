using GlobalEventController.DataAccess.Commands;
using Irma.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace GlobalEventController.Tests.DataAccess.CommandTests
{
    [TestClass]
    public class AddTaxClassCommandHandlerTests
    {
        private AddTaxClassCommandHandler handler;
        private AddTaxClassCommand command;
        private IrmaDbContextFactory contextFactory;
        private IrmaContext context;
        private TransactionScope transaction;
       
        [TestInitialize]
        public void InitializeData()
        {
            this.transaction = new TransactionScope();
            this.context = new IrmaContext();
            this.command = new AddTaxClassCommand();
            this.contextFactory = new IrmaDbContextFactory();
            this.handler = new AddTaxClassCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void CleanupData()
        {
            this.transaction.Dispose();
        }

        [TestMethod]
        public void AddTaxClassCommandHandler_TaxClassDoesNotExist_TaxClassAdded()
        {
            // Given
            this.command.TaxClassDescription = "3142143 GlobalController Tax Class";
            this.command.TaxCode = "3142143";

            // When
            this.handler.Handle(this.command);

            // Then
            TaxClass actual = this.context.TaxClass.AsNoTracking().FirstOrDefault(tc => tc.TaxClassDesc == this.command.TaxClassDescription);
            if (actual == null)
            {
                Assert.Fail(String.Format("The Tax Class [{0}] was not added as expected.", this.command.TaxClassDescription));
            }

            var entry = this.context.Entry(actual);
            Assert.AreEqual(this.command.TaxClassDescription, actual.TaxClassDesc);
            Assert.AreEqual(this.command.TaxCode, actual.ExternalTaxGroupCode);
        }

        [TestMethod]
        public void AddTaxClassCommandHandler_TaxClassExist_TaxClassAdded()
        {
            // Given
            this.command.TaxClassDescription = "3142143 GlobalController Tax Class Testing Tax description length";
            this.command.TaxCode = "3142143";

            // When
            this.handler.Handle(this.command);

            // Then
           
            TaxClass actual = this.context.TaxClass.AsNoTracking().FirstOrDefault(tc => tc.TaxClassDesc == this.command.TaxClassDescription);

            var entry = this.context.Entry(actual);
            Assert.AreEqual(this.command.TaxClassDescription, actual.TaxClassDesc);
            Assert.AreEqual(this.command.TaxCode, actual.ExternalTaxGroupCode);
        }

        [TestMethod]
        public void AddTaxFlag_TaxFlagExist_TaxFlagAdded()
        {
            // Given
            this.command.TaxClassDescription = "3142143 GlobalController Tax Class Testing Tax description length";
            this.command.TaxCode = "3142143";

            // When
            this.handler.Handle(this.command);

            // Then
            string sql = @"select count(*) from dbo.TaxFlag with (nolock) where TaxClassId = (SELECT TaxclassId from Taxclass with (nolock) where ExternalTaxGroupCode = '3142143') ";
            SqlConnection sqlConnection = new SqlConnection(context.Database.Connection.ConnectionString);
            SqlCommand slqCommand = new SqlCommand(sql, sqlConnection);
            sqlConnection.Open();
            var taxClassIdCount = slqCommand.ExecuteScalar();
            sqlConnection.Close();
            Assert.AreEqual(52, taxClassIdCount);
        }
    }
}