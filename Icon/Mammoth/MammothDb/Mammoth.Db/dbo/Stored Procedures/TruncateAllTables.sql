CREATE PROCEDURE [dbo].[TruncateAllTables] 
AS 
BEGIN
	-- This procedure encapsultes all of the foreign key logic to allow table truncation
	DECLARE @Region NCHAR(2)
	DECLARE @sqlText NVARCHAR(MAX)

	-- Hierarchies
	TRUNCATE TABLE dbo.Hierarchy_Merchandise;
	TRUNCATE TABLE dbo.Hierarchy_NationalClass;
	TRUNCATE TABLE dbo.Tax_Attributes;
	TRUNCATE TABLE dbo.HierarchyClass;
	EXECUTE dbo.DropConstraint @Constraint = 'FK_HierarchyClass_HierarchyID'
	TRUNCATE TABLE dbo.Hierarchy;
	ALTER TABLE dbo.HierarchyClass ADD CONSTRAINT FK_HierarchyClass_HierarchyID FOREIGN KEY (HierarchyID) REFERENCES dbo.Hierarchy(hierarchyID);

	-- Other Item Tables
	TRUNCATE TABLE dbo.ItemOverrides;
	TRUNCATE TABLE dbo.ItemAttributes_Ext;

	-- Item table
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_FL_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_MA_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_MW_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_NA_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_NC_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_NE_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_PN_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_RM_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_SO_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_SP_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_SW_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_TS_Ext_ItemID';
	EXECUTE dbo.DropConstraint @Constraint = 'FK_ItemAttributes_Locale_UK_Ext_ItemID';
	TRUNCATE TABLE dbo.Items;

	-- Region specific tables
	DECLARE cRegion CURSOR FAST_FORWARD FOR
	SELECT Region
	FROM dbo.Regions
	ORDER BY Region

	OPEN cRegion

	FETCH NEXT FROM cRegion INTO @Region

	WHILE @@FETCH_STATUS = 0 BEGIN

		SET @sqlText = ''

		SET @sqlText = 'TRUNCATE TABLE dbo.ItemAttributes_Locale_' + @Region;
		EXEC sp_executesql @sqlText;
		SET @sqlText = 'TRUNCATE TABLE dbo.ItemAttributes_Locale_' + @Region + '_Ext';
		EXEC sp_executesql @sqlText;
		
		FETCH NEXT FROM cRegion INTO @Region

	END

	CLOSE cRegion

	DEALLOCATE cRegion

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