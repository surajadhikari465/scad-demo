using Dapper;
using Mammoth.Common.DataAccess.DbProviders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;

namespace Icon.Infor.Listeners.Price.Integration.Tests
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
            //TurnOffCheckFkCheckContraints();
            //AddItemTypesToDatabase();
            //AddAttributesToDatabase();
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            //TurnCheckContraintOn();
        }

        private static void TurnOffCheckFkCheckContraints()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                string sql = @"ALTER TABLE [dbo].[ItemAttributes_Locale_FL_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_FL_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_MA_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_MA_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_MW_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_MW_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_NA_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_NA_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_NC_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_NC_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_NE_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_NE_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_PN_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_PN_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_RM_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_RM_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_SO_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_SO_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_SP_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_SP_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_SW_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_SW_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_TS_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_TS_Ext_ItemID]
                            ALTER TABLE [dbo].[ItemAttributes_Locale_UK_Ext] NOCHECK CONSTRAINT [FK_ItemAttributes_Locale_UK_Ext_ItemID]
                            ";
                db.Connection.Execute(sql, transaction: db.Transaction);
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

        private static void TurnCheckContraintOn()
        {
            using (db.Connection = new SqlConnection(connectionString))
            {
                string sql = @"ALTER TABLE [dbo].[ItemAttributes_Locale_FL_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_FL_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_MA_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_MA_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_MW_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_MW_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_NA_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_NA_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_NC_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_NC_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_NE_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_NE_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_PN_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_PN_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_RM_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_RM_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_SO_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_SO_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_SP_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_SP_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_SW_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_SW_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_TS_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_TS_Ext_ItemID]
                                
                                ALTER TABLE [dbo].[ItemAttributes_Locale_UK_Ext] CHECK CONSTRAINT [FK_ItemAttributes_Locale_UK_Ext_ItemID]
                                ";
                db.Connection.Execute(sql, transaction: db.Transaction);
            }
        }
    }
}
