IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'JDASync_Item_Update')
	BEGIN
		PRINT 'Dropping Trigger JDASync_Item_Update'
		DROP  Trigger JDASync_Item_Update
	END
GO

PRINT 'Creating Trigger JDASync_Item_Update'
GO

CREATE Trigger dbo.JDASync_Item_Update 
ON [dbo].[Item] FOR UPDATE

AS

-- ****************************************************************************************************************
-- Procedure: JDASync_Item_Update()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-05-01	KM		12176	Insert NULL for Discontinue_Item, as the column has been dropped from the Item table;
-- ****************************************************************************************************************

BEGIN
	-- this is critical to the functioning of the audit
	-- it allows us to compare null to null
	SET ANSI_NULLS OFF

    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @SyncJDA bit
    
    SELECT @SyncJDA = dbo.fn_InstanceDataValue('SyncJDA', NULL)

	-- only if the instance data flag is set
	If @SyncJDA = 1
	BEGIN
	
		-- check to see if the ItemType_ID has changed
		DECLARE @ItemType_ID_Orig int, @ItemType_ID_New int
		
		SELECT @ItemType_ID_Orig = ItemType_ID FROM Deleted
		SELECT @ItemType_ID_New = ItemType_ID FROM Inserted
		
		IF @ItemType_ID_Orig <> @ItemType_ID_New
		BEGIN
			-- insert a 'changed' sync row in the *JDA_ItemIdentifierSync* table
			-- for each identifier for an item if the item's ItemType_ID changes
			INSERT INTO JDA_ItemIdentifierSync
			(
				ActionCode,
				ApplyDate,
				Item_Key,
				Identifier,
				National_Identifier,
				ItemType_ID
			)
			SELECT
				'C',
				GetDate(),
				Inserted.Item_Key,
				ItemIdentifier.Identifier,
				ItemIdentifier.National_Identifier,
				Inserted.ItemType_ID
			FROM Inserted
				JOIN ItemIdentifier (NOLOCK)
					ON ItemIdentifier.Item_Key = Inserted.Item_Key
		END
		
		-- now insert into the JDA_ItemSync table
		Insert INTO JDA_ItemSync
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
		SELECT
			-- there should be no updates after the item is deleted
			CASE WHEN Inserted.Deleted_Item = 1 AND Deleted.Deleted_Item = 0 THEN 'D' ELSE 'C' END,
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
		FROM
			Inserted
			JOIN Deleted 
				ON Deleted.Item_Key = Inserted.Item_Key
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
		WHERE
			 -- we don't care about updates after the delete flag is set
			 Deleted.Deleted_Item = 0
			-- we care only if any of the columns we are tracking changes
			AND
			(
			Inserted.Deleted_Item <> Deleted.Deleted_Item
			OR SUBSTRING(Inserted.Item_Description, 1, 30) <> SUBSTRING(Deleted.Item_Description, 1, 30)
			OR Inserted.ProdHierarchyLevel4_ID <> Deleted.ProdHierarchyLevel4_ID
			OR Inserted.Package_Desc1 <> Deleted.Package_Desc1
			OR Inserted.Package_Desc2 <> Deleted.Package_Desc2
			OR Inserted.Package_Unit_Id <> Deleted.Package_Unit_Id
			OR Inserted.Brand_ID <> Deleted.Brand_ID
			OR Inserted.Retail_Unit_ID <> Deleted.Retail_Unit_ID
			OR Inserted.WFM_Item <> Deleted.WFM_Item
			OR Inserted.Vendor_Unit_ID <> Deleted.Vendor_Unit_ID
			OR Inserted.Manager_ID <> Deleted.Manager_ID
			OR Inserted.Deleted_Item <> Deleted.Deleted_Item
			)
	END
	
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('JDASync_Item_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
	
	-- reset it
	SET ANSI_NULLS ON

END
GO