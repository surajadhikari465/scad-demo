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
    public class GetSequenceIdFromBusinessUnitIdQueryHandlerTest
    {
        private GetSequenceIdFromBusinessUnitIdQueryHandler getSequenceIdFromBusinessUnitIdQueryHandler;
        private SqlDbProvider dbProvider;
        [TestInitialize]
        public void Initialize()
        {
            dbProvider = new SqlDbProvider();     
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbProvider.Connection.Open();
        
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.dbProvider.Connection.Close();
            this.dbProvider.Connection.Dispose();
        }

        [TestMethod]
        public void GetSequenceId_FromBusinessUnitId_ShouldGetSequenceIdFromdatabase()
        {
            getSequenceIdFromBusinessUnitIdQueryHandler = new GetSequenceIdFromBusinessUnitIdQueryHandler(dbProvider);
            int localeId = getAnyStoreLocaleId();
            this.dbProvider.Connection.Execute(@"   IF EXISTS(Select 1 FROM [infor].[LocaleSequence] WHERE LocaleId=" + localeId+ @")
                                                    BEGIN
                                                        UPDATE [infor].[LocaleSequence]
                                                        SET [SequenceId] = 1
                                                        WHERE [LocaleId] = " + localeId.ToString() +
                                                    @"  END 
                                                    ELSE
                                                    BEGIN
                                                        INSERT INTO [infor].[LocaleSequence]
                                                                   ([SequenceId]
                                                                   ,[LocaleId]
                                                                   ,[InforMessageId]
                                                                   ,[InsertDateUtc]
                                                                   ,[ModifiedDateUtc])
                                                             VALUES (1,"
                                                                     + localeId.ToString() + @",
			                                                         'B9029BE1-FCBD-4E64-A95B-1204A74120BF',
			                                                         GETDATE(),
			                                                         GETDATE()
			                                                         )
                                                    END");

            GetSequenceIdFromBusinessUnitIdParameters getSequenceIdFromBusinessUnitIdParameters = new GetSequenceIdFromBusinessUnitIdParameters();
            getSequenceIdFromBusinessUnitIdParameters.businessUnitId = getBusinessUnitIdFromLocaleId(localeId);
            int returnedSequenceID = getSequenceIdFromBusinessUnitIdQueryHandler.Search(getSequenceIdFromBusinessUnitIdParameters);
            Assert.AreEqual(1, returnedSequenceID);
        }
        private int getAnyStoreLocaleId()
        {
            return Convert.ToInt32(this.dbProvider.Connection.ExecuteScalar(@" DECLARE @businessUnitIdTraitId INT = (SELECT traitID FROM Trait WHERE traitCode = 'BU')
                                                                                SELECT TOP 1 l.localeId FROM Locale l
                                                                                JOIN dbo.LocaleTrait lt ON l.localeID = lt.localeID
			                                                                    AND lt.traitID = @businessUnitIdTraitId 
                                                                                where localetypeid=4"));
        }
        private int getBusinessUnitIdFromLocaleId(int localeId)
        {
            return Convert.ToInt32(this.dbProvider.Connection.ExecuteScalar(@" DECLARE @businessUnitIdTraitId INT = (SELECT traitID FROM Trait WHERE traitCode = 'BU')
                                                                                SELECT TOP 1 lt.traitValue 
                                                                                FROM dbo.LocaleTrait lt 
			                                                                    WHERE lt.traitID = @businessUnitIdTraitId 
                                                                                AND localeId = " + localeId.ToString()));
        }
    }
}