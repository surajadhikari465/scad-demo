using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Mammoth.Esb.ProductListener.Tests.Queries
{
    [TestClass]
    public class GetPrimeAffinityItemStoreModelsForActiveSalesQueryTests
    {
        private const string TestStoreName = "Test";
        private const string TestItemTypeCode = "TST";

        private GetPrimeAffinityItemStoreModelsForActiveSalesQuery query;
        private GetPrimeAffinityItemStoreModelsForActiveSalesParameters parameters;
        private List<int> testItemIds;
        private List<int> testBusinessUnitIds;
        private string testRegion = "FL";
        private SqlDbProvider sqlDbProvider;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            sqlDbProvider = new SqlDbProvider
            {
                Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString)
            };
            query = new GetPrimeAffinityItemStoreModelsForActiveSalesQuery(sqlDbProvider);
            parameters = new GetPrimeAffinityItemStoreModelsForActiveSalesParameters
            {
                EligiblePriceTypes = new List<string> { "ISS", "SAL", "FRC" }
            };
            testItemIds = new List<int> { 99999990, 99999991, 99999992 };
            testBusinessUnitIds = new List<int> { 77777770, 77777771, 77777772 };
            InsertTestData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GetPrimeAffinityItemStoreModelsByItemIdAndBusinessUnitId_SomeItemsAreAuthorizedAndSomeAreNotAndSomeHaveActiveSales_ReturnsAuthorizedItems()
        {
            //Given
            var itemLocaleAttribute1 = InsertItemLocaleAttribute(testItemIds[0], testBusinessUnitIds[0], true);
            var itemLocaleAttribute2 = InsertItemLocaleAttribute(testItemIds[0], testBusinessUnitIds[1], true);
            var itemLocaleAttribute3 = InsertItemLocaleAttribute(testItemIds[1], testBusinessUnitIds[0], true);
            var itemLocaleAttribute4 = InsertItemLocaleAttribute(testItemIds[2], testBusinessUnitIds[0], false);
            var itemLocaleAttribute5 = InsertItemLocaleAttribute(testItemIds[2], testBusinessUnitIds[1], true);

            var itemLocaleAttributesAuthorized = new List<dynamic>
            {
                itemLocaleAttribute1,
                itemLocaleAttribute2,
                itemLocaleAttribute3,
                itemLocaleAttribute5
            };

            var price1 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);
            var price2 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[0], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(11), "TST");
            var price3 = InsertTestPrices(testItemIds[0], testBusinessUnitIds[1], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);
            var price4 = InsertTestPrices(testItemIds[1], testBusinessUnitIds[0], DateTime.Today.AddDays(-5), DateTime.Today.AddDays(10), "TST");
            var price5 = InsertTestPrices(testItemIds[2], testBusinessUnitIds[0], DateTime.Today, DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);
            var price6 = InsertTestPrices(testItemIds[2], testBusinessUnitIds[1], DateTime.Today, DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);
            var price7 = InsertTestPrices(testItemIds[2], testBusinessUnitIds[1], DateTime.Today.AddDays(-1), DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);
            var price8 = InsertTestPrices(testItemIds[2], testBusinessUnitIds[2], DateTime.Today, DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);
            var price9 = InsertTestPrices(testItemIds[2], testBusinessUnitIds[2], DateTime.Today.AddDays(1), DateTime.Today.AddDays(10), parameters.EligiblePriceTypes[1]);

            var activeEligiblePrices = new List<dynamic>
            {
                price1,
                price3,
                price6
            };
            parameters.Items = new List<ItemDataAccessModel>
            {
                new ItemDataAccessModel { ItemID = testItemIds[0] },
                new ItemDataAccessModel { ItemID = testItemIds[1] },
                new ItemDataAccessModel { ItemID = testItemIds[2] },
            };

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(3, result.Count());
            foreach (var itemLocaleAttributes in itemLocaleAttributesAuthorized)
            {
                foreach (var price in activeEligiblePrices.Where(p => p.ItemID == itemLocaleAttributes.ItemID && p.BusinessUnitID == itemLocaleAttributes.BusinessUnitID))
                {
                    var primeAffinityModel = result.Single(pa => pa.ItemId == itemLocaleAttributes.ItemID && pa.BusinessUnitId == itemLocaleAttributes.BusinessUnitID);
                    AssertModelIsEqualToLocaleAttributes(primeAffinityModel, itemLocaleAttributes);
                }
            }
        }

        private void AssertModelIsEqualToLocaleAttributes(PrimeAffinityItemStoreModel primeAffinityModel, dynamic itemLocaleAttributes)
        {
            Assert.AreEqual(itemLocaleAttributes.BusinessUnitID, primeAffinityModel.BusinessUnitId);
            Assert.AreEqual(itemLocaleAttributes.ItemID, primeAffinityModel.ItemId);
            Assert.AreEqual(TestItemTypeCode, primeAffinityModel.ItemTypeCode);
            Assert.AreEqual(testRegion, primeAffinityModel.Region);
            Assert.AreEqual("sc" + primeAffinityModel.ItemId, primeAffinityModel.ScanCode);
            Assert.AreEqual(TestStoreName, primeAffinityModel.StoreName);
        }

        private dynamic InsertItemLocaleAttribute(int itemId, int businessUnitId, bool authorized)
        {
            return sqlDbProvider.Connection.QueryFirst<dynamic>(
                @"INSERT INTO dbo.ItemAttributes_Locale_FL
                             (ItemID
                             ,BusinessUnitID
                             ,Discount_Case
                             ,Discount_TM
                             ,Restriction_Age
                             ,Restriction_Hours
                             ,Authorized
                             ,Discontinued
                             ,LocalItem
                             ,ScaleItem
                             ,OrderedByInfor
                             ,DefaultScanCode
                             ,LabelTypeDesc
                             ,Product_Code
                             ,RetailUnit
                             ,Sign_Desc
                             ,Locality
                             ,Sign_RomanceText_Long
                             ,Sign_RomanceText_Short
                             ,AltRetailUOM
                             ,AltRetailSize
                             ,MSRP)
                       VALUES
                             (@ItemId
                             ,@BusinessUnitId
                             ,0
                             ,0
                             ,18
                             ,0
                             ,@Authorized
                             ,0
                             ,0
                             ,0
                             ,0
                             ,0
                             ,''
                             ,''
                             ,''
                             ,''
                             ,''
                             ,''
                             ,''
                             ,''
                             ,0
                             ,0)
                    
                    SELECT *
                    FROM dbo.ItemAttributes_Locale_FL
                    WHERE ItemID = @ItemId
                        AND BusinessUnitID = @BusinessUnitId",
                new
                {
                    ItemId = itemId,
                    BusinessUnitId = businessUnitId,
                    Authorized = authorized
                });
        }

        private void InsertTestData()
        {
            InsertTestItemType();
            foreach (var itemId in testItemIds)
            {
                InsertTestItem(itemId);
            }
            InsertTestLocale();
        }

        private void InsertTestItemType()
        {
            sqlDbProvider.Connection.Execute(
                @"  INSERT INTO dbo.ItemTypes(itemTypeCode) 
                    VALUES (@ItemTypeCode)",
                new { ItemTypeCode = TestItemTypeCode });
        }

        private void InsertTestItem(int itemId)
        {
            sqlDbProvider.Connection.Execute(
                $@" INSERT INTO dbo.Items(
                            ItemID, 
                            ScanCode, 
                            ItemTypeID)
                        SELECT @ItemId, 
                            @ScanCode,
                            ItemTypeID 
                            FROM dbo.ItemTypes 
                        WHERE itemTypeCode = @ItemTypeCode",
                new { ItemId = itemId, ScanCode = "sc" + itemId, ItemTypeCode = TestItemTypeCode });
        }

        private void InsertTestLocale()
        {
            foreach (var businessUnitId in testBusinessUnitIds)
            {
                sqlDbProvider.Connection.Execute(
                $@" INSERT INTO [dbo].[Locales_{testRegion}](
	                    [Region],
	                    [BusinessUnitID],
	                    [StoreName],
	                    [StoreAbbrev])
                    VALUES (@Region, 
                            @BusinessUnitId, 
                            @StoreName, 
                            'TST')",
                new { Region = testRegion, BusinessUnitId = businessUnitId, StoreName = TestStoreName });
            }
        }

        private dynamic InsertTestPrices(int itemId, int businessUnitId, DateTime startDate, DateTime? endDate, string priceType)
        {
            var priceId = sqlDbProvider.Connection.QueryFirst<int>(
                $@" INSERT INTO [dbo].[Price_{testRegion}]
                            ([Region]
                            ,[ItemID]
                            ,[BusinessUnitID]
                            ,[StartDate]
                            ,[EndDate]
                            ,[Price]
                            ,[PriceType]
                            ,[PriceUOM]
                            ,[CurrencyID]
                            ,[Multiple]
                            ,[AddedDate])
                        VALUES
                            (@Region
                            ,@ItemId
                            ,@BusinessUnitId
                            ,@StartDate
                            ,@EndDate
                            ,1.99
                            ,@PriceType
                            ,'EA'
                            ,1
                            ,1
                            ,GETDATE())

                        SELECT SCOPE_IDENTITY()",
                new
                {
                    Region = testRegion,
                    ItemId = itemId,
                    BusinessUnitId = businessUnitId,
                    StartDate = startDate,
                    EndDate = endDate,
                    PriceType = priceType
                });

            return sqlDbProvider.Connection.QueryFirst(
                $@" SELECT * 
                    FROM dbo.Price_{testRegion} 
                    WHERE PriceID = @PriceId",
                new { PriceId = priceId });
        }
    }
}
