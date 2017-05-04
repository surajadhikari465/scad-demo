CREATE PROCEDURE [dbo].[Truncate_Items] AS BEGIN

	ALTER TABLE dbo.ItemAttributes_Ext DROP CONSTRAINT FK_ItemAttributes_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_FL_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_FL_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_MA_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_MA_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_MW_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_MW_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_NA_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_NA_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_NC_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_NC_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_NE_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_NE_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_PN_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_PN_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_RM_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_RM_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_SO_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_SO_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_SP_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_SP_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_SW_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_SW_Ext_ItemID;
	ALTER TABLE dbo.ItemAttributes_Locale_UK_Ext DROP CONSTRAINT FK_ItemAttributes_Locale_UK_Ext_ItemID;

	TRUNCATE TABLE dbo.Items;

	ALTER TABLE dbo.ItemAttributes_Ext
	ADD CONSTRAINT FK_ItemAttributes_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items(ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_FL_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_FL_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_MA_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_MA_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_MW_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_MW_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_NA_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_NA_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_NC_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_NC_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_NE_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_NE_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_PN_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_PN_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_RM_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_RM_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_SO_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_SO_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_SP_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_SP_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_SW_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_SW_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

	ALTER TABLE dbo.ItemAttributes_Locale_UK_Ext 
	ADD CONSTRAINT FK_ItemAttributes_Locale_UK_Ext_ItemID FOREIGN KEY (ItemID) REFERENCES dbo.Items (ItemID)

END