IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Item_Insert')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Item_Insert'
		DROP  Trigger JDASync_Item_Insert
	END
GO

PRINT 'Creating Trigger JDASync_Item_Insert'
GO

CREATE Trigger dbo.JDASync_Item_Insert 
ON [dbo].[Item] FOR INSERT

AS

-- ****************************************************************************************************************
-- Procedure: JDASync_Item_Insert()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-05-01	KM		12176	Insert NULL for Discontinue_Item, as the column has been dropped from the Item table;
-- ****************************************************************************************************************

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
		INSERT INTO JDA_ItemSync
		(
			ActionCode,
			ApplyDate,
			Item_Key,
			Item_Description,
			JDA_Dept,
			JDA_SubDept,
			JDA_Class,
			JDA_SubClass,
			Package_Desc1,
			Package_Desc2,
			Package_Unit_Id,
			JDA_Brand_ID,
			Retail_Unit_ID,
			Deleted_Item,
			Discontinue_Item,
			WFM_Item,
			Vendor_Unit_ID,
			Manager_ID,
			IRMA_Subteam_No,
			IRMA_Category_ID,
			IRMA_ProdHierarchyLevel3_ID,
			IRMA_ProdHierarchyLevel4_ID,
			IRMA_Brand_ID,
			IRMA_Package_Unit_ID,
			IRMA_Retail_Unit_ID,
			IRMA_Vendor_Unit_ID
		)
		SELECT DISTINCT
			'A',
			GetDate(),
			Inserted.Item_Key,
			SUBSTRING(Inserted.Item_Description, 1, 30),
			jhm.JDA_Dept,
			jhm.JDA_SubDept,
			jhm.JDA_Class,
			jhm.JDA_SubClass,
			Inserted.Package_Desc1,
			Inserted.Package_Desc2,
			jium_package.JDA_ID,
			jibm.JDA_ID,
			jium_retail.JDA_ID,
			Inserted.Deleted_Item,
			NULL,
			Inserted.WFM_Item,
			jium_vendor.JDA_ID,
			Inserted.Manager_ID,
			Inserted.Subteam_No,
			Inserted.Category_ID,
			level4.ProdHierarchyLevel3_ID,
			Inserted.ProdHierarchyLevel4_ID,
			Inserted.Brand_ID,
			Inserted.Package_Unit_ID,
			Inserted.Retail_Unit_ID,
			Inserted.Vendor_Unit_ID	
		FROM Inserted
			LEFT JOIN JDA_ItemBrandMapping jibm (NOLOCK)
				ON jibm.Brand_ID = Inserted.Brand_ID
			LEFT JOIN ProdHierarchyLevel4 level4 (NOLOCK)
				ON level4.ProdHierarchyLevel4_ID = Inserted.ProdHierarchyLevel4_ID
			LEFT JOIN JDA_HierarchyMapping jhm (NOLOCK)
				ON jhm.Subteam_No = Inserted.Subteam_No
				AND jhm.Category_ID = Inserted.Category_ID
				AND jhm.ProdHierarchyLevel3_ID = level4.ProdHierarchyLevel3_ID
				AND jhm.ProdHierarchyLevel4_ID = Inserted.ProdHierarchyLevel4_ID
			LEFT JOIN JDA_ItemUnitMapping jium_package (NOLOCK)
				ON jium_package.Unit_ID = Inserted.Package_Unit_Id
			LEFT JOIN JDA_ItemUnitMapping jium_retail (NOLOCK)
				ON jium_retail.Unit_ID = Inserted.Retail_Unit_ID
			LEFT JOIN JDA_ItemUnitMapping jium_vendor (NOLOCK)
				ON jium_vendor.Unit_ID = Inserted.Vendor_Unit_ID
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Item_Insert trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO