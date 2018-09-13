using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vim.Locale.Controller.DataAccess.Queries;
using Vim.Common.DataAccess;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Transactions;
using Dapper;
using Vim.Locale.Controller.DataAccess.Models;
using System.Collections.Generic;

namespace Vim.Locale.Controller.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetLocaleEventsQueryHandlerTests
    {
        private GetLocaleEventsQueryHandler queryHandler;
        private SqlDbProvider dbProvider;
        private TransactionScope transaction;

        private int queryInstance = 55;
        private int eventInstance = 56;
        private int psBU = 0;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbProvider = new SqlDbProvider();
            dbProvider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            dbProvider.Connection.Open();

            queryHandler = new GetLocaleEventsQueryHandler(dbProvider);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetLocaleEvents_EventsExistWithSameInstance_ShouldReturnEvents()
        {
            //Given  
            var parentLocaleId = dbProvider.Connection.Query<int>(
                @"select top 1 localeID from dbo.Locale where localeTypeID = 3",
                transaction: dbProvider.Transaction).First();
            var regionCode = dbProvider.Connection.Query<string>(
                @"select traitValue from LocaleTrait lt
                    join Trait t on lt.TraitId = t.TraitId
                    join Locale pl on pl.localeID = lt.localeID
                    join Locale l  on l.parentLocaleID = pl.localeID
                   where traitDesc = 'Region Abbreviation'
                     and l.localeID = @localeID",
                    new { localeID = parentLocaleId },
                transaction: dbProvider.Transaction).First();
            var localeId = dbProvider.Connection.Query<int>(
                @"insert into dbo.Locale(ownerOrgPartyID, localeName, localeOpenDate,localeCloseDate,localeTypeID,parentLocaleID)
                  values (1,'VimTest',DATEADD(year,-1,GETDATE()),GETDATE(),4,@parentLocaleId)
                  select cast(SCOPE_IDENTITY() as int)",
                new { parentLocaleId = parentLocaleId },
                transaction: dbProvider.Transaction).First();
            dbProvider.Connection.Execute(
                @"insert into localeTrait(traitID, localeID, uomID, traitValue )
                  values((select traitID from Trait where traitDesc = 'PS Business Unit ID'), @LocaleId, Null, @PSBU)",
                new { LocaleId = localeId, PSBU = psBU.ToString() },
                transaction: dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"insert into vim.EventQueue(EventTypeId, EventReferenceId, EventMessage, InProcessBy)
                  values(1, @LocaleId, Null, @Instance)",
                new { LocaleId = localeId, Instance = eventInstance },
                transaction: dbProvider.Transaction);

            //When
            var results = queryHandler.Search(new GetLocaleEventsQuery
            {
                Instance = eventInstance,
                FirstAttemptWaitTimeInMinute = 0,
                SecondAttemptWaitTimeInMinute = 0,
                ThirdAttemptWaitTimeInMinute = 0
            });

            //Then
            Assert.AreEqual(1, results.Count);
            Assert.AreEqual(localeId, results[0].EventReferenceId);
            Assert.AreEqual(psBU, results[0].StoreModel.PSBU);
            Assert.AreEqual("VimTest", results[0].StoreModel.StoreName);
            Assert.AreEqual(regionCode, results[0].StoreModel.Region);
            Assert.AreEqual("CLOSED", results[0].StoreModel.Status.ToUpper());
            Assert.AreEqual("CREATE", results[0].StoreModel.Action.ToUpper());
            Assert.AreEqual(null, results[0].ErrorMessage);
        }

        [TestMethod]
        public void GetLocaleEvents_EventsDontExistWithSameInstance_ShouldReturnEmptyList()
        {
            //Given
            var localeId = dbProvider.Connection.Query<int>(
                @"insert into dbo.Locale(ownerOrgPartyID, localeName, localeOpenDate,localeCloseDate,localeTypeID,parentLocaleID)
                  values (1,'VimTest',getdate(),null,4,(select top 1 localeID from dbo.Locale where LocaleTypeID = 3))
                  select cast(SCOPE_IDENTITY() as int)",
                transaction: dbProvider.Transaction).First();
            dbProvider.Connection.Execute(
                @"insert into vim.EventQueue(EventTypeId, EventReferenceId, EventMessage, InProcessBy)
                  values(1, @LocaleId, Null, @Instance)",
                new { LocaleId = localeId, Instance = eventInstance },
                transaction: dbProvider.Transaction);

            //When    
            var results = queryHandler.Search(new GetLocaleEventsQuery
            {
                Instance = queryInstance,
                FirstAttemptWaitTimeInMinute = 0,
                SecondAttemptWaitTimeInMinute = 0,
                ThirdAttemptWaitTimeInMinute = 0
            });

            //Then
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetLocaleEvents_StoreIsUnder365Chain_ShouldReturnTSForRegion()
        {
            //Given
            var parentLocaleId = dbProvider.Connection.Query<int>(
                @"select top 1 metro.localeID 
                from dbo.Locale metro
                join dbo.Locale region on metro.parentLocaleID = region.localeID
                join dbo.Locale chain on region.parentLocaleID = chain.localeID
                where metro.localeTypeID = 3
                    and chain.localeName = '365'",
                transaction: dbProvider.Transaction).First();
            var localeId = dbProvider.Connection.Query<int>(
                @"insert into dbo.Locale(ownerOrgPartyID, localeName, localeOpenDate, localeCloseDate, localeTypeID, parentLocaleID)
                  values (1,'VimTest',DATEADD(year,-1,GETDATE()),GETDATE(),4,@parentLocaleId)
                  select cast(SCOPE_IDENTITY() as int)",
                new { parentLocaleId = parentLocaleId },
                transaction: dbProvider.Transaction).First();
            dbProvider.Connection.Execute(
                @"insert into localeTrait(traitID, localeID, uomID, traitValue )
                  values((select traitID from Trait where traitDesc = 'PS Business Unit ID'), @LocaleId, Null, @PSBU)",
                new { LocaleId = localeId, PSBU = psBU.ToString() },
                transaction: dbProvider.Transaction);
            dbProvider.Connection.Execute(
                @"insert into vim.EventQueue(EventTypeId, EventReferenceId, EventMessage, InProcessBy)
                  values(1, @LocaleId, Null, @Instance)",
                new { LocaleId = localeId, Instance = eventInstance },
                transaction: dbProvider.Transaction);

            //When
            var actualStores = queryHandler.Search(new GetLocaleEventsQuery
            {
                Instance = eventInstance,
                FirstAttemptWaitTimeInMinute = 0,
                SecondAttemptWaitTimeInMinute = 0,
                ThirdAttemptWaitTimeInMinute = 0
            });

            //Then
            Assert.AreEqual(1, actualStores.Count);
            foreach (var store in actualStores)
            {
                Assert.AreEqual("VimTest", store.StoreModel.StoreName);
                Assert.AreEqual("TS", store.StoreModel.Region);
            }
        }
    }
}

