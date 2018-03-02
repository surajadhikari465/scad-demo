using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Transactions;

namespace Mammoth.Esb.ProductListener.Tests.Queries
{
    [TestClass]
    public class GetPrimeAffinityItemStoreModelsQueryTests
    {
        private const string TestStoreName = "Test";
        private const string TestItemTypeCode = "TST";

        private GetPrimeAffinityItemStoreModelsQuery query;
        private GetPrimeAffinityItemStoreModelsParameters parameters;
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
            query = new GetPrimeAffinityItemStoreModelsQuery(sqlDbProvider);
            parameters = new GetPrimeAffinityItemStoreModelsParameters();
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
        public void GetPrimeAffinityItemStoreModels_SomeItemsAreAuthorizedAndSomeAreNot_ReturnsAuthorizedItems()
        {
            //Given
            var itemLocaleAttribute1 = InsertItemLocaleAttribute(testItemIds[0], testBusinessUnitIds[0], true);
            var itemLocaleAttribute2 = InsertItemLocaleAttribute(testItemIds[0], testBusinessUnitIds[1], true);
            var itemLocaleAttribute3 = InsertItemLocaleAttribute(testItemIds[1], testBusinessUnitIds[0], false);
            var itemLocaleAttribute4 = InsertItemLocaleAttribute(testItemIds[2], testBusinessUnitIds[0], false);
            var itemLocaleAttribute5 = InsertItemLocaleAttribute(testItemIds[2], testBusinessUnitIds[1], true);
            var itemLocaleAttributesAuthorized = new List<dynamic>
            {
                itemLocaleAttribute1,
                itemLocaleAttribute2,
                itemLocaleAttribute5
            };
            parameters.ItemIds = testItemIds;

            //When
            var result = query.Search(parameters);

            //Then
            Assert.AreEqual(3, result.Count());
            foreach (var itemLocaleAttributes in itemLocaleAttributesAuthorized)
            {
                var primeAffinityModel = result.Single(pa => pa.ItemId == itemLocaleAttributes.ItemID && pa.BusinessUnitId == itemLocaleAttributes.BusinessUnitID);
                AssertModelIsEqualToLocaleAttributes(primeAffinityModel, itemLocaleAttributes);
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
    }
}