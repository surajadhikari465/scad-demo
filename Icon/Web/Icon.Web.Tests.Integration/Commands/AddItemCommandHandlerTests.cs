using Dapper;
using Icon.Web.DataAccess.Commands;
using Icon.Web.Tests.Integration.TestHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Transactions;
using Icon.Common;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class AddItemCommandHandlerTests
    {
        private AddItemCommandHandler commandHandler;
        private AddItemCommand command;
        private SqlConnection dbConnection;
        private TransactionScope transaction;
        private ItemTestHelper itemTestHelper;
        private int barcodeTypeId;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            dbConnection = SqlConnectionBuilder.CreateIconConnection();
            commandHandler = new AddItemCommandHandler(dbConnection);
            command = new AddItemCommand();
            itemTestHelper = new ItemTestHelper();

            StageData();
            itemTestHelper.Initialize(dbConnection, true, false);
            command.BrandsHierarchyClassId = itemTestHelper.TestItem.BrandsHierarchyClassId;
            command.FinancialHierarchyClassId = itemTestHelper.TestItem.FinancialHierarchyClassId;
            command.ItemAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(itemTestHelper.TestItem.ItemAttributesJson);
            command.MerchandiseHierarchyClassId = itemTestHelper.TestItem.MerchandiseHierarchyClassId;
            command.NationalHierarchyClassId = itemTestHelper.TestItem.NationalHierarchyClassId;
            command.TaxHierarchyClassId = itemTestHelper.TestItem.TaxHierarchyClassId;
            command.ManufacturerHierarchyClassId = itemTestHelper.TestItem.ManufacturerHierarchyClassId;
            command.SelectedBarCodeTypeId = barcodeTypeId;
            command.ScanCode = null;
            command.ItemTypeCode = itemTestHelper.TestItemTypes[1].ItemTypeCode;
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void AddItem_PluScanCodeIsAvailable_CreatesItem()
        {
            //When
            commandHandler.Execute(command);

            //Then
            Assert.IsNotNull(command.ScanCode);
        }

        [TestMethod]
        [ExpectedException(typeof(SqlException))]
        public void AddItem_PluScanCodeIsNotAvailable_NoItemCreated()
        {   
            //Given
            commandHandler.Execute(command);
            StageDataWithZeroRange();
            command.SelectedBarCodeTypeId = barcodeTypeId;

            //When
            commandHandler.Execute(command);
        }

        [TestMethod]
        public void AddItem_ScanCodeWithScalePLU_ScanCodeWithIncrement100k()
        {
            //When
            StageDataforScalePLU();
            command.SelectedBarCodeTypeId = barcodeTypeId;
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(command.ScanCode, "10000100000");
        }

        [TestMethod]
        public void AddItem_GapExistsInScanCodes_ScanCodeFromGapIsUsed()
        {
            //When
            StageDataWithGaps();
            command.SelectedBarCodeTypeId = barcodeTypeId;
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(command.ScanCode, "1000000001");
        }

        private void StageData()
        {
            barcodeTypeId = dbConnection.QueryFirst<int>($@"
                            INSERT INTO[dbo].[BarcodeType]([BarCodeType],[BeginRange],[EndRange]) 
                            VALUES ('Test1',-10,-200)  
                            SELECT SCOPE_IDENTITY()"
                            );

            dbConnection.Execute($@"INSERT INTO[dbo].[BarcodeTypeRangePool]([barcodeTypeId],[scancode]) 
                        VALUES (@barcodeTypeId,@scanCode)", new
                        {
                            barcodeTypeId = barcodeTypeId,
                            scanCode = "-9"
                        }
                    );
        }

        private void StageDataWithZeroRange()
        {
            barcodeTypeId = dbConnection.QueryFirst<int>($@"
                            INSERT INTO[dbo].[BarcodeType]([BarCodeType],[BeginRange],[EndRange]) 
                            VALUES ('Test2',-10,-10)  
                            SELECT SCOPE_IDENTITY()");
        }

        private void StageDataforScalePLU()
        {
            barcodeTypeId = dbConnection.QueryFirst<int>($@"
                            INSERT INTO[dbo].[BarcodeType]([BarCodeType],[BeginRange],[EndRange],[ScalePLU]) 
                            VALUES ('Scale PLU Test1',10000000000,20000000000,1)  
                            SELECT SCOPE_IDENTITY()");

             dbConnection.Execute($@"
                            INSERT INTO[dbo].[scancode]([ItemId],[scancode],[scancodeTypeId],[localeId]) 
                            VALUES ('1','10000000000',1,1)");

            dbConnection.Execute($@"
                            INSERT INTO[dbo].[BarcodeTypeRangePool]([barcodeTypeId],[scancode]) 
                            VALUES (@barcodeTypeId,@scanCode)", new
                            {
                                barcodeTypeId= barcodeTypeId,
                                scanCode="10000100000"
                             }
                         );
        }

        private void StageDataWithGaps()
        {
            barcodeTypeId = dbConnection.QueryFirst<int>($@"INSERT INTO[dbo].[BarcodeType]([BarCodeType],[BeginRange],[EndRange],[ScalePLU]) 
                            VALUES ('Scale PLU Test1',1000000000,2000000000,0)  
                            SELECT SCOPE_IDENTITY()"
            );

           dbConnection.Execute($@"INSERT INTO[dbo].[scancode]([ItemId],[scancode],[barCodeTypeId],[localeId],[scanCodeTypeId]) 
                            VALUES (@itemId,@scanCode,@barCodeTypeId,@localeId,@scanCodeTypeId)",new
           {
               itemId = 1,
               scanCode = "1000000000",
               barCodeTypeId = barcodeTypeId,
               localeId = 1,
               scanCodeTypeId=1
           });

            dbConnection.Execute($@"INSERT INTO[dbo].[BarcodeTypeRangePool]([barcodeTypeId],[scancode],[assigned]) 
                            VALUES (@barcodeTypeId,@scanCode,@assigned)", new
            {
                barcodeTypeId = barcodeTypeId,
                scanCode = "1000000000",
                assigned=1
            });

            // we're not inserting a scan code for 1000000001 but we are creating a BarcodeTypeRangePool record which creates a gap in the scan code records
            dbConnection.Execute($@"INSERT INTO[dbo].[BarcodeTypeRangePool]([barcodeTypeId],[scancode]) 
                            VALUES (@barcodeTypeId,@scanCode)", new
            {
                barcodeTypeId = barcodeTypeId,
                scanCode = "1000000001"
            });

            dbConnection.Execute($@"INSERT INTO[dbo].[scancode]([ItemId],[scancode],[barCodeTypeId],[localeId],[scanCodeTypeId]) 
                            VALUES (@itemId,@scanCode,@barCodeTypeId,@localeId,@scanCodeTypeId)", new
            {
                itemId = 1,
                scanCode = "1000000002",
                barCodeTypeId = barcodeTypeId,
                localeId = 1,
                scanCodeTypeId=1
            });

            dbConnection.Execute($@"INSERT INTO[dbo].[BarcodeTypeRangePool]([barcodeTypeId],[scancode],[assigned]) 
                            VALUES (@barcodeTypeId,@scanCode,@assigned)", new
            {
                barcodeTypeId = barcodeTypeId,
                scanCode = "1000000002",
                assigned=1
            });
        }
    }
}