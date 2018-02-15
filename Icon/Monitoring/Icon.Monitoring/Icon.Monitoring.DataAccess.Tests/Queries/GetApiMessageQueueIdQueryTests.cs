using Dapper;
using Icon.Monitoring.Common;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass]
    public class GetApiMessageQueueIdQueryTests
    {
        private SqlDbProvider provider;
        private GetApiMessageQueueIdQuery query;
        private GetApiMessageQueueIdParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetApiMessageQueueIdQuery(provider);
            parameters = new GetApiMessageQueueIdParameters();
        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
        }

        [TestMethod]
        public void GetApiMessageQueueIdSearch_DataExists_ReturnsMessageQueueId()
        {
            //Given
            int messageQueueId = InsertTestMessageQueueProduct();
            Assert.AreNotEqual(0, messageQueueId);

            parameters.MessageQueueType = MessageQueueTypes.Product;

            //When
            int result = query.Search(parameters);

            //Then
            Assert.AreEqual(messageQueueId, result);
        }

        [TestMethod]
        public void GetApiMessageQueueIdSearch_DataDoesntExist_Returns0()
        {
            //Given
            parameters.MessageQueueType = MessageQueueTypes.Product;
            UpdateMessageQueue_SetMessageQueueIdNotEqual1();

            //When
            int result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result);
        }

        private int InsertTestMessageQueueProduct()
        {
            return provider.Connection.Query<int>(
                    @"Set Identity_Insert app.MessageQueueProduct ON  insert into app.MessageQueueProduct
                    ([MessageQueueId]
                   ,[MessageTypeId]
                   ,[MessageStatusId]
                   ,[MessageHistoryId]
                   ,[InsertDate]
                   ,[ItemId]
                   ,[LocaleId]
                   ,[ItemTypeCode]
                   ,[ItemTypeDesc]
                   ,[ScanCodeId]
                   ,[ScanCode]
                   ,[ScanCodeTypeId]
                   ,[ScanCodeTypeDesc]
                   ,[ProductDescription]
                   ,[PosDescription]
                   ,[PackageUnit]
                   ,[RetailSize]
                   ,[RetailUom]
                   ,[FoodStampEligible]
                   ,[ProhibitDiscount]
                   ,[DepartmentSale]
                   ,[BrandId]
                   ,[BrandName]
                   ,[BrandLevel]
                   ,[BrandParentId]
                   ,[BrowsingClassId]
                   ,[BrowsingClassName]
                   ,[BrowsingLevel]
                   ,[BrowsingParentId]
                   ,[MerchandiseClassId]
                   ,[MerchandiseClassName]
                   ,[MerchandiseLevel]
                   ,[MerchandiseParentId]
                   ,[TaxClassId]
                   ,[TaxClassName]
                   ,[TaxLevel]
                   ,[TaxParentId]
                   ,[FinancialClassId]
                   ,[FinancialClassName]
                   ,[FinancialLevel]
                   ,[FinancialParentId]
                   ,[InProcessBy]
                   ,[ProcessedDate]
                   ,[AnimalWelfareRating]
                   ,[Biodynamic]
                   ,[CheeseMilkType]
                   ,[CheeseRaw]
                   ,[EcoScaleRating]
                   ,[GlutenFreeAgency]
                   ,[HealthyEatingRating]
                   ,[KosherAgency]
                   ,[Msc]
                   ,[NonGmoAgency]
                   ,[OrganicAgency]
                   ,[PremiumBodyCare]
                   ,[SeafoodFreshOrFrozen]
                   ,[SeafoodCatchType]
                   ,[VeganAgency]
                   ,[Vegetarian]
                   ,[WholeTrade]
                   ,[GrassFed]
                   ,[PastureRaised]
                   ,[FreeRange]
                   ,[DryAged]
                   ,[AirChilled]
                   ,[MadeInHouse])
                values (@MessageQueueId
                      ,@MessageTypeId
                      ,@MessageStatusId
                      ,@MessageHistoryId
                      ,@InsertDate
                      ,@ItemId
                      ,@LocaleId
                      ,@ItemTypeCode
                      ,@ItemTypeDesc
                      ,@ScanCodeId
                      ,@ScanCode
                      ,@ScanCodeTypeId
                      ,@ScanCodeTypeDesc
                      ,@ProductDescription
                      ,@PosDescription
                      ,@PackageUnit
                      ,@RetailSize
                      ,@RetailUom
                      ,@FoodStampEligible
                      ,@ProhibitDiscount
                      ,@DepartmentSale
                      ,@BrandId
                      ,@BrandName
                      ,@BrandLevel
                      ,@BrandParentId
                      ,@BrowsingClassId
                      ,@BrowsingClassName
                      ,@BrowsingLevel
                      ,@BrowsingParentId
                      ,@MerchandiseClassId
                      ,@MerchandiseClassName
                      ,@MerchandiseLevel
                      ,@MerchandiseParentId
                      ,@TaxClassId
                      ,@TaxClassName
                      ,@TaxLevel
                      ,@TaxParentId
                      ,@FinancialClassId
                      ,@FinancialClassName
                      ,@FinancialLevel
                      ,@FinancialParentId
                      ,@InProcessBy
                      ,@ProcessedDate
                      ,@AnimalWelfareRating
                      ,@Biodynamic
                      ,@CheeseMilkType
                      ,@CheeseRaw
                      ,@EcoScaleRating
                      ,@GlutenFreeAgency
                      ,@HealthyEatingRating
                      ,@KosherAgency
                      ,@Msc
                      ,@NonGmoAgency
                      ,@OrganicAgency
                      ,@PremiumBodyCare
                      ,@SeafoodFreshOrFrozen
                      ,@SeafoodCatchType
                      ,@VeganAgency
                      ,@Vegetarian
                      ,@WholeTrade
                      ,@GrassFed
                      ,@PastureRaised
                      ,@FreeRange
                      ,@DryAged
                      ,@AirChilled
                      ,@MadeInHouse)
                      select SCOPE_IDENTITY()
                      Set Identity_Insert app.MessageQueueProduct OFF",
                            new
                            {
                                MessageQueueId = 1,
                                MessageTypeId = 6,
                                MessageStatusId = 1,
                                MessageHistoryId = (int?)null,
                                InsertDate = DateTime.Now,
                                ItemId = 0,
                                LocaleId = 0,
                                ItemTypeCode = "123",
                                ItemTypeDesc = "TEST",
                                ScanCodeId = 0,
                                ScanCode = "TEST",
                                ScanCodeTypeId = 0,
                                ScanCodeTypeDesc = "TEST",
                                ProductDescription = "TEST",
                                PosDescription = "TEST",
                                PackageUnit = "TEST",
                                RetailSize = "TEST",
                                RetailUom = "TEST",
                                FoodStampEligible = "TEST",
                                ProhibitDiscount = false,
                                DepartmentSale = "TEST",
                                BrandId = 0,
                                BrandName = "TEST",
                                BrandLevel = 0,
                                BrandParentId = 0,
                                BrowsingClassId = 0,
                                BrowsingClassName = "TEST",
                                BrowsingLevel = 0,
                                BrowsingParentId = 0,
                                MerchandiseClassId = 0,
                                MerchandiseClassName = "TEST",
                                MerchandiseLevel = 0,
                                MerchandiseParentId = 0,
                                TaxClassId = 0,
                                TaxClassName = "TEST",
                                TaxLevel = 0,
                                TaxParentId = 0,
                                FinancialClassId = 0,
                                FinancialClassName = "TEST",
                                FinancialLevel = 0,
                                FinancialParentId = 0,
                                InProcessBy = (int?)null,
                                ProcessedDate = (DateTime?)null,
                                AnimalWelfareRating = "1",
                                Biodynamic = "1",
                                CheeseMilkType = "1",
                                CheeseRaw = "1",
                                EcoScaleRating = "1",
                                GlutenFreeAgency = "1",
                                HealthyEatingRating = "1",
                                KosherAgency = "1",
                                Msc = "1",
                                NonGmoAgency = "1",
                                OrganicAgency = "1",
                                PremiumBodyCare = "1",
                                SeafoodFreshOrFrozen = "1",
                                SeafoodCatchType = "1",
                                VeganAgency = "1",
                                Vegetarian = "1",
                                WholeTrade = "1",
                                GrassFed = "1",
                                PastureRaised = "1",
                                FreeRange = "1",
                                DryAged = "1",
                                AirChilled = "1",
                                MadeInHouse = "1"
                            },
                            provider.Transaction)
                            .FirstOrDefault();
        }

        private void UpdateMessageQueue_SetMessageQueueIdNotEqual1()
        {
            provider.Connection.Query<int>(
                @" UPDATE [app].[MessageQueueProduct]
                 SET [MessageStatusId] = 3", 
                null, 
                provider.Transaction)
                .FirstOrDefault();

        }
    }


}