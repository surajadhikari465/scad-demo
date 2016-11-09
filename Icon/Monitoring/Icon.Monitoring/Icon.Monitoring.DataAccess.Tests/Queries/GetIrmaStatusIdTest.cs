using Dapper;
using Icon.Monitoring.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Monitoring.DataAccess.Tests.Queries
{
    [TestClass()]
    public class GetIrmaPushIdTest
    {
        private SqlDbProvider provider;
        private GetIrmaPushIdQuery query;
        private GetIrmaPushIdParameters parameters;

        [TestInitialize]
        public void Initialize()
        {
            provider = new SqlDbProvider();
            provider.Connection = new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            provider.Connection.Open();
            provider.Transaction = provider.Connection.BeginTransaction();
            query = new GetIrmaPushIdQuery(provider);
            parameters = new GetIrmaPushIdParameters();
        }

        [TestCleanup]
        public void CleanUp()
        {
            provider.Transaction.Rollback();
            provider.Connection.Close();
        }

        [TestMethod]
        public void GetIrmaPushIdSearch_UnprocessedRowsExist_ReturnId()
        {
            //Given
            int result, insertId;
            insertId= InsertIrmaPushTable(true);

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(insertId, result);

        }

        [TestMethod]
        public void GetIrmaPushIdSearch_UnprocessedRowsDontExist_ReturnId()
        {
            //Given
            int result;
            InsertIrmaPushTable(false);

            //When
            result = query.Search(parameters);

            //Then
            Assert.AreEqual(0, result);

        }
        private int InsertIrmaPushTable(bool isUnprocessed)
        {
            return provider.Connection.Query<int>(
        @"INSERT INTO app.IRMAPush
           ([RegionCode]
           ,[BusinessUnit_ID]
           ,[Identifier]
           ,[ChangeType]
           ,[InsertDate]
           ,[RetailSize]
           ,[RetailPackageUom]
           ,[TMDiscountEligible]
           ,[Case_Discount]
           ,[AgeCode]
           ,[Recall_Flag]
           ,[Restricted_Hours]
           ,[Sold_By_Weight]
           ,[ScaleForcedTare]
           ,[Quantity_Required]
           ,[Price_Required]
           ,[QtyProhibit]
           ,[VisualVerify]
           ,[RestrictSale]
           ,[PosScaleTare]
           ,[LinkedIdentifier]
           ,[Price]
           ,[RetailUom]
           ,[Multiple]
           ,[SaleMultiple]
           ,[Sale_Price]
           ,[Sale_Start_Date]
           ,[Sale_End_Date]
           ,[InProcessBy]
           ,[InUdmDate]
           ,[EsbReadyDate]
           ,[UdmFailedDate]
           ,[EsbReadyFailedDate])
     VALUES
           (@RegionCode
           ,@BusinessUnit_ID
           ,@Identifier
           ,@ChangeType
           ,@InsertDate
           ,@RetailSize
           ,@RetailPackageUom
           ,@TMDiscountEligible
           ,@Case_Discount
           ,@AgeCode
           ,@Recall_Flag
           ,@Restricted_Hours
           ,@Sold_By_Weight
           ,@ScaleForcedTare
           ,@Quantity_Required
           ,@Price_Required
           ,@QtyProhibit
           ,@VisualVerify
           ,@RestrictSale
           ,@PosScaleTare
           ,@LinkedIdentifier
           ,@Price
           ,@RetailUom
           ,@Multiple
           ,@SaleMultiple
           ,@Sale_Price
           ,@Sale_Start_Date
           ,@Sale_End_Date
           ,@InProcessBy
           ,@InUdmDate
           ,@EsbReadyDate
           ,@UdmFailedDate
           ,@EsbReadyFailedDate)
            select SCOPE_IDENTITY()",
                            new
                            {
                                RegionCode = "test",
                                BusinessUnit_ID = 0,
                                Identifier = "test",
                                ChangeType = "test",
                                InsertDate = DateTime.Now,
                                RetailSize = 0m,
                                RetailPackageUom = "test",
                                TMDiscountEligible = false,
                                Case_Discount = false,
                                AgeCode = 0,
                                Recall_Flag = false,
                                Restricted_Hours = false,
                                Sold_By_Weight = false,
                                ScaleForcedTare = false,
                                Quantity_Required = false,
                                Price_Required = false,
                                QtyProhibit = false,
                                VisualVerify = false,
                                RestrictSale = false,
                                PosScaleTare = 0,
                                LinkedIdentifier = "test",
                                Price = 0.00m,
                                RetailUom = "test",
                                Multiple = 0,
                                SaleMultiple = 0,
                                Sale_Price = 0.00m,
                                Sale_Start_Date = (DateTime?)null,
                                Sale_End_Date = (DateTime?)null,
                                InProcessBy = 0,
                                InUdmDate = isUnprocessed ?(DateTime?)null: (DateTime?)DateTime.Now,
                                EsbReadyDate =isUnprocessed ? (DateTime?)null : (DateTime?)DateTime.Now,
                                UdmFailedDate = isUnprocessed ? (DateTime?)null : (DateTime?)DateTime.Now,
                                EsbReadyFailedDate = isUnprocessed ? (DateTime?)null : (DateTime?)DateTime.Now
                            },
                            provider.Transaction)
                            .FirstOrDefault();
        }

    }
}
