using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Moq;
using Icon.Logging;
using Icon.Framework;
using Mammoth.Common.DataAccess.DbProviders;
using System.Data.SqlClient;
using System.Configuration;
using Icon.Infor.Listeners.LocaleListener.Models;
using Icon.Esb.Schemas.Wfm.Contracts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data;
using System.Reflection;
using System.Transactions;
using Icon.Infor.Listeners.LocaleListener.Queries;

namespace Icon.Infor.Listeners.LocaleListener.Tests.Commands
{
    [TestClass]
    public class GetSequenceIdFromLocaleIdQueryHandlerTest
    {

        private GetSequenceIdFromLocaleIdQueryHandler getSequenceIdFromLocaleIdQueryHandler;
        private SqlDbProvider dbProvider;
        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbProvider.Connection.Open();
            getSequenceIdFromLocaleIdQueryHandler = new GetSequenceIdFromLocaleIdQueryHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
          
            this.dbProvider.Connection.Close();
            this.dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void GetSequenceId_FromLocaleId_ShouldGetSequenceIdFromdatabase()
        {
            this.dbProvider.Connection.Execute(@"   IF EXISTS(Select 1 FROM [infor].[LocaleSequence] WHERE LocaleId=1)
                                                    BEGIN
                                                        UPDATE [infor].[LocaleSequence]
                                                        SET [SequenceId] = 1
                                                        WHERE [LocaleId] = 1
                                                        END 
                                                    ELSE
                                                    BEGIN
                                                        INSERT INTO [infor].[LocaleSequence]
                                                                   ([LocaleId]
                                                                   ,[SequenceId]
                                                                   ,[InforMessageId]
                                                                   ,[InsertDateUtc]
                                                                   ,[ModifiedDateUtc])
                                                             VALUES (1,
			                                                         1,
			                                                         'B9029BE1-FCBD-4E64-A95B-1204A74120BF',
			                                                         GETDATE(),
			                                                         GETDATE()
			                                                         )
                                                    END");

            GetSequenceIdFromLocaleIdParameters getSequenceIdFromLocaleIdParameters = new GetSequenceIdFromLocaleIdParameters();
            getSequenceIdFromLocaleIdParameters.localeId = 1;
            int returnedSequenceID = getSequenceIdFromLocaleIdQueryHandler.Search(getSequenceIdFromLocaleIdParameters);
            Assert.AreEqual(1, returnedSequenceID);
        }
    }
}