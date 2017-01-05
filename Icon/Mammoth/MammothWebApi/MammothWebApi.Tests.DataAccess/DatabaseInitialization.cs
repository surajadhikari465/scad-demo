using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;

namespace MammothWebApi.Tests.DataAccess
{
    [TestClass]
    public sealed class DatabaseInitialization
    {
        private static SqlDbProvider db;
        private static string connectionString = @"Data Source=MAMMOTH-DB01-DEV\MAMMOTH;Initial Catalog=Mammoth_Dev;Integrated Security=SSPI";

        [AssemblyInitialize]
        public static void AssemblyIniitialize(TestContext context)
        {
            db = new SqlDbProvider();
            AddLookupDataToDatabase();
            //AddEsbLookupDataToDatabase();
            //AddItemTypesToDatabase();
            //AddAttributesToDatabase();
        }

        private static void AddLookupDataToDatabase()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                db.Connection.Open();
                string sql = @"SET IDENTITY_INSERT [app].[App] ON;
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Web Api')
	                                INSERT INTO app.App (AppID, AppName) VALUES (1, 'Web Api');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'ItemLocale Controller')
	                                INSERT INTO app.App (AppID, AppName) VALUES (2, 'ItemLocale Controller');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Price Controller')
	                                INSERT INTO app.App (AppID, AppName) VALUES (3, 'Price Controller');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'API Controller')
	                                INSERT INTO app.App (AppID, AppName) VALUES (4, 'API Controller');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Product Listener')
	                                INSERT INTO app.App (AppID, AppName) VALUES (5, 'Product Listener');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Locale Listener')
	                                INSERT INTO app.App (AppID, AppName) VALUES (6, 'Locale Listener');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Hierarchy Class Listener')
	                                INSERT INTO app.App (AppID, AppName) VALUES (7, 'Hierarchy Class Listener');
                                IF NOT EXISTS (SELECT * FROM app.App WHERE AppName = 'Mammoth Data Purge')
	                                INSERT INTO app.App (AppID, AppName) VALUES (8, 'Mammoth Data Purge');
                                SET IDENTITY_INSERT [app].[App] OFF;
                                PRINT '...Populating Attributes data';

                                DECLARE @Today datetime;
                                SET @Today = GETDATE();

                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'AGE')
	                                INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) VALUES (2,'AGE','Age Restrict',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NA')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'NA','Authorized For Sale',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CSD')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CSD','Case Discount Eligible',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CHB')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CHB','Chicago Baby',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CLA')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CLA','Color Added',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'COP')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'COP','Country of Processing',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'DSC')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'DSC','Discontinued',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EST')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'EST','Electronic Shelf Tag',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EX')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'EX','Exclusive',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LTD')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LTD','Label Type Desc',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LSC')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LSC','Linked Scan Code',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LI')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LI','Local Item',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LCY')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LCY','Locality',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SRP')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SRP','MSRP',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NDS')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'NDS','Number of Digits Sent To Scale',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'ORN')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'ORN','Origin',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'PCD')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'PCD','Product Code',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RES')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'RES','Restricted Hours',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SET')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SET','Scale Extra Text',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SC')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SC','Sign Caption',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SBW')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SBW','Sold by Weight',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'TU')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'TU','Tag UOM',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'TMD')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'TMD','TM Discount Eligible',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SHT')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SHT','Sign Romance Short',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LNG')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LNG','Sign Romance Long',@Today);
                                IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RTU')
	                                INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'RTU','Retail Unit',@Today);
                                
                                IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'USD')
                                BEGIN
	                                INSERT INTO Currency (CurrencyCode, CurrencyDesc)
	                                VALUES ('USD', 'US Dollar')
                                END
                                IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'CAD')
                                BEGIN
	                                INSERT INTO Currency (CurrencyCode, CurrencyDesc)
	                                VALUES ('CAD', 'Canadian Dollar')
                                END
                                IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'GBP')
                                BEGIN
	                                INSERT INTO Currency (CurrencyCode, CurrencyDesc)
	                                VALUES ('GBP', 'Pound Sterling')
                                END
                                
	
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Merchandise')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (1, N'Merchandise', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Brands')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (2, N'Brands', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Tax')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (3, N'Tax', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Browsing')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (4, N'Browsing', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Financial')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (5, N'Financial', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'National')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (6, N'National', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Hierarchy WHERE hierarchyName = N'Certification Agency Management')
	                                INSERT [dbo].[Hierarchy] ([hierarchyID], [hierarchyName], [AddedDate], [ModifiedDate]) VALUES (7, N'Certification Agency Management', CAST(N'2015-10-29 23:51:27.587' AS DateTime), NULL)
                                

                                SET IDENTITY_INSERT [dbo].[ItemPriceType] ON 
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'REG')
	                                INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (1, N'REG', N'Regular Price')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemPriceType WHERE ItemPriceTypeCode = N'TPR')
	                                INSERT [dbo].[ItemPriceType] ([ItemPriceTypeId], [ItemPriceTypeCode], [ItemPriceTypeDesc]) VALUES (2, N'TPR', N'Temporary Price Reduction')
                                
                                SET IDENTITY_INSERT [dbo].[ItemPriceType] OFF
                                
                                SET IDENTITY_INSERT [dbo].[ItemTypes] ON 
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'RTL')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (1, N'RTL', N'Retail Sale')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'DEP')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (2, N'DEP', N'Deposit')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'TAR')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (3, N'TAR', N'Tare')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'RTN')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (4, N'RTN', N'Return')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'CPN')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (5, N'CPN', N'Coupon')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'NRT')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (6, N'NRT', N'Non-Retail')
                                
                                IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'FEE')
	                                INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (7, N'FEE', N'Fee')
                                
                                SET IDENTITY_INSERT [dbo].[ItemTypes] OFF
                                
                                SET IDENTITY_INSERT esb.MessageAction ON
                                

                                IF NOT EXISTS (SELECT 1 FROM esb.MessageAction WHERE MessageActionName = 'AddOrUpdate')
	                                INSERT INTO esb.MessageAction(MessageActionId, MessageActionName) VALUES (1, 'AddOrUpdate')
                                IF NOT EXISTS (SELECT 1 FROM esb.MessageAction WHERE MessageActionName = 'Delete')
	                                INSERT INTO esb.MessageAction(MessageActionId, MessageActionName) VALUES (2, 'Delete')
                                

                                SET IDENTITY_INSERT esb.MessageAction OFF
                                
                                SET IDENTITY_INSERT esb.MessageStatus ON
                                

                                IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Ready')
	                                INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(1, 'Ready')
                                
                                IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Sent')
	                                INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(2, 'Sent')
                                
                                IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Failed')
	                                INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(3, 'Failed')
                                
                                IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Associated')
	                                INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(4, 'Associated')
                                
                                IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Staged')
	                                INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(5, 'Staged')
                                
                                IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Consumed')
	                                INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(6, 'Consumed')
                                

                                SET IDENTITY_INSERT esb.MessageStatus OFF
                                
                                SET IDENTITY_INSERT esb.MessageType ON
                                

                                IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Item Locale')
                                BEGIN
	                                INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
	                                VALUES	(1, 'Item Locale')
                                END

                                IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Price')
                                BEGIN
	                                INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
	                                VALUES (2, 'Price')
                                END
                                

                                SET IDENTITY_INSERT esb.MessageType OFF
                                
                                SET IDENTITY_INSERT dbo.Uom ON
                                

                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'EA')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (1, N'EA', N'EACH')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'LB')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (2, N'LB', N'POUND')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'CT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (3, N'CT', N'COUNT')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'OZ')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (4, N'OZ', N'OUNCE')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'CS')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (5, N'CS', N'CASE')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'PK')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (6, N'PK', N'PACK')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'LT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (7, N'LT', N'LITER')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'PT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (8, N'PT', N'PINT')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'KG')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (9, N'KG', N'KILOGRAM')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'ML')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (10, N'ML', N'MILLILITER')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'GL')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (11, N'GL', N'GALLON')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'GR')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (12, N'GR', N'GRAM')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'CG')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (13, N'CG', N'CENTIGRAM')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'FT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (14, N'FT', N'FEET')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'YD')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (15, N'YD', N'YARD')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'QT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (16, N'QT', N'QUART')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'SQFT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (17, N'SQFT', N'SQUARE FOOT')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'MT')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (18, N'MT', N'METER')
                                
                                IF NOT EXISTS (SELECT * FROM dbo.Uom WHERE UomCode = N'FZ')
	                                INSERT [dbo].[Uom] ([UomId], [UomCode], [UomName]) VALUES (19, N'FZ', N'FLUID OUNCES')
                                

                                SET IDENTITY_INSERT dbo.Uom OFF
                                
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'FL')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Florida','FL');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'MA')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Mid Atlantic','MA');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'MW')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Mid West','MW');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'NA')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('North Atlantic','NA');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'NC')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Northern California','NC');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'NE')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('North East','NE');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'PN')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Pacific Northwest','PN');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'RM')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Rocky Mountain','RM');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'SO')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('South','SO');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'SP')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Southern Pacific','SP');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'SW')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('Southwest','SW');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'TS')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('365','TS');
                                IF NOT EXISTS (SELECT 1 FROM dbo.Regions WHERE Region = 'UK')
	                                INSERT INTO dbo.Regions (RegionName, Region) VALUES ('United Kingdom','UK');";
                db.Connection.Execute(sql, transaction: db.Transaction);
            }
        }

        private static void AddAttributesToDatabase()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                string sql = @"PRINT '...Populating Attributes data';

                        DECLARE @Today datetime;
                        SET @Today = GETDATE();

                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'AGE')
	                        INSERT INTO Attributes (AttributeGroupID, AttributeCode, AttributeDesc, AddedDate) VALUES (2,'AGE','Age Restrict',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NA')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'NA','Authorized For Sale',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CSD')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CSD','Case Discount Eligible',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CHB')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CHB','Chicago Baby',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'CLA')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'CLA','Color Added',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'COP')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'COP','Country of Processing',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'DSC')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'DSC','Discontinued',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EST')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'EST','Electronic Shelf Tag',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'EX')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'EX','Exclusive',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LTD')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LTD','Label Type Desc',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LSC')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LSC','Linked Scan Code',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LI')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LI','Local Item',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LCY')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LCY','Locality',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SRP')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SRP','MSRP',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'NDS')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'NDS','Number of Digits Sent To Scale',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'ORN')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'ORN','Origin',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'PCD')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'PCD','Product Code',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RES')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'RES','Restricted Hours',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SET')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SET','Scale Extra Text',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SC')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SC','Sign Caption',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SBW')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SBW','Sold by Weight',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'TU')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'TU','Tag UOM',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'TMD')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'TMD','TM Discount Eligible',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'SHT')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'SHT','Sign Romance Short',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'LNG')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'LNG','Sign Romance Long',@Today);
                        IF NOT EXISTS (SELECT 1 FROM Attributes WHERE AttributeCode = 'RTU')
	                        INSERT INTO Attributes (AttributeGroupID,AttributeCode,AttributeDesc,AddedDate) VALUES (2,'RTU','Retail Unit',@Today);";

                int affectedRows = db.Connection.Execute(sql, transaction: db.Transaction);
            }   
        }

        private static void AddItemTypesToDatabase()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                string sql = @"SET IDENTITY_INSERT [dbo].[ItemTypes] ON 
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'RTL')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (1, N'RTL', N'Retail Sale')
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'DEP')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (2, N'DEP', N'Deposit')
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'TAR')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (3, N'TAR', N'Tare')
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'RTN')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (4, N'RTN', N'Return')
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'CPN')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (5, N'CPN', N'Coupon')
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'NRT')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (6, N'NRT', N'Non-Retail')
                        IF NOT EXISTS (SELECT 1 FROM ItemTypes WHERE itemTypeCode = N'FEE')
	                        INSERT [dbo].[ItemTypes] ([itemTypeID], [itemTypeCode], [itemTypeDesc]) VALUES (7, N'FEE', N'Fee')
                        SET IDENTITY_INSERT [dbo].[ItemTypes] OFF";
                db.Connection.Execute(sql, transaction: db.Transaction);
            }   
        }

        private static void AddEsbLookupDataToDatabase()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                string sql = @"SET IDENTITY_INSERT esb.MessageAction ON
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageAction WHERE MessageActionName = 'AddOrUpdate')
	                            INSERT INTO esb.MessageAction(MessageActionId, MessageActionName) VALUES (1, 'AddOrUpdate')
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageAction WHERE MessageActionName = 'Delete')
	                            INSERT INTO esb.MessageAction(MessageActionId, MessageActionName) VALUES (2, 'Delete')
                            SET IDENTITY_INSERT esb.MessageAction OFF

                            SET IDENTITY_INSERT esb.MessageStatus ON
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Ready')
	                            INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(1, 'Ready')
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Sent')
	                            INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(2, 'Sent')
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Failed')
	                            INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(3, 'Failed')
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Associated')
	                            INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(4, 'Associated')
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Staged')
	                            INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(5, 'Staged')
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageStatus WHERE MessageStatusName = 'Consumed')
	                            INSERT INTO esb.MessageStatus(MessageStatusId, MessageStatusName) VALUES	(6, 'Consumed')
                            SET IDENTITY_INSERT esb.MessageStatus OFF
                            
                            SET IDENTITY_INSERT esb.MessageType ON
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Item Locale')
                            BEGIN
	                            INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
	                            VALUES	(1, 'Item Locale')
                            END
                            IF NOT EXISTS (SELECT 1 FROM esb.MessageType WHERE MessageTypeName = 'Price')
                            BEGIN
	                            INSERT INTO esb.MessageType(MessageTypeId, MessageTypeName)
	                            VALUES (2, 'Price')
                            END
                            SET IDENTITY_INSERT esb.MessageType OFF
                            ";

                db.Connection.Execute(sql, transaction: db.Transaction);
            }
        }

        private static void AddCurrencyToDatabase()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                string sql = @"IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'USD')
                            BEGIN
	                            INSERT INTO Currency (CurrencyCode, CurrencyDesc)
	                            VALUES ('USD', 'US Dollar')
                            END
                            IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'CAD')
                            BEGIN
	                            INSERT INTO Currency (CurrencyCode, CurrencyDesc)
	                            VALUES ('CAD', 'Canadian Dollar')
                            END
                            IF NOT EXISTS (SELECT 1 FROM Currency WHERE CurrencyCode = 'GBP')
                            BEGIN
	                            INSERT INTO Currency (CurrencyCode, CurrencyDesc)
	                            VALUES ('GBP', 'Pound Sterling')
                            END";

                db.Connection.Execute(sql, transaction: db.Transaction);
            }
        }
    }
}
