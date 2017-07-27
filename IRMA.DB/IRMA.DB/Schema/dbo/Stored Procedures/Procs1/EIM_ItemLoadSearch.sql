
CREATE PROCEDURE [dbo].[EIM_ItemLoadSearch]
	@Identifier				varchar(13),
	@Item_Description		varchar(200),
	@Vendor_ID				integer,
	@Brand_ID				integer,
	@StoreNos				varchar(1000),
	@Subteam_No				integer,
	@Category_ID			integer,
	@Level3_ID				integer,
	@Level4_ID				integer,
	@PS_Vendor_ID			varchar(10),
	@Vendor_Item_ID			varchar(20),
	@DistSubTeam_No			int,
	@ItemChainID			int = 0,
	@Discontinue_Item		bit = 0,
    @Exclude_Not_Available	bit = 0,
    @IncludeDeletedItems	bit = 0,
    @LoadFromSLIM			bit = 0,
    @IsDefaultJurisdiction	integer,
    @JurisdictionId			integer
    
AS

-- **************************************************************************
-- Procedure: EIM_ItemLoadSearch()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from ExtendedItemMaintenanceForm.vb when searching
-- for specific items
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 01/03/2013	BAS   			8755	Updated Item.Discontinue_Item in WHERE clause
--										to account for schema change to StoreItemVendor
--										Note a scalar function is used to check if all
--										stores for that item are discontinued.	
-- 2013-04-01	KM				11669	Modify the usage of the fn_GetDiscontinueStatus function;	
-- 2013-04-01	KM				11670	Modify the join to ItemVendor to include the condition DeleteDate = NULL; Modify
--										the join to Store to include the condition Store.StoreJurisdictionID = ItemTable.StoreJurisdictionID;
--										This should (?) prevent multiple records being returned to the UI for a single item and jurisdiction.							
-- 2013-04-10	KM				11670	When no store or vendor is chosen for the search, the search should not return item/jurisdiction combinations
--										where there is no store associated with that vendor.
-- 2013-05-05	KM				11670	Remove hardcoded references to EIM_Jurisdiction_ItemView;
-- 2015-09-15	KM				11338	Allow the search to work with the new ItemSignAttribute table;
-- 2017-07-21   MZ              22360   Added two alternate jurisdiction fields Sign Romance Short and Sign Romance Long 
--                                      to EIM
-- **************************************************************************

BEGIN
	SET NOCOUNT ON

		DECLARE @SQL nvarchar(max)
		,@oneSQLrow varchar(1000)
		,@currentRow integer
		,@totalRows integer
		,@PriceCost varchar(1000)
		,@TableName varchar(200)
		,@ColumnName varchar(200)
	    
		-- Instance Data Flags
		,@Use4LevelHierarchy bit
		,@UseStoreJurisdictions bit
	
	DECLARE @Item_Key int
		
	-- get the instance data flag values
	SELECT @Use4LevelHierarchy = dbo.fn_InstanceDataValue('FourLevelHierarchy', NULL)
	SELECT @UseStoreJurisdictions = dbo.fn_InstanceDataValue('UseStoreJurisdictions', NULL)

	If @Use4LevelHierarchy IS NULL
	BEGIN
		RAISERROR('The FourLevelHierarchy instance data flag does not exist.', 16, 1)
	END
	
	If @UseStoreJurisdictions IS NULL
	BEGIN
		RAISERROR('The UseStoreJurisdictions instance data flag does not exist.', 16, 1)
	END

	set @SQL = 'SELECT  DISTINCT TOP 2000 Item_TableName.item_key AS ITEM_ITEM_KEY'
	-- ***** Fetch the appropriate UploadAttributes ********

	-- if no store is provided then we can only serve up
	-- item maintenance attributes
	DECLARE attributes_cursor CURSOR FOR
		SELECT DISTINCT TableName,ColumnNameorKey
		FROM UploadAttributeView UploadAttribute (NOLOCK)
		JOIN UploadTypeAttributeView UploadTypeAttribute (NOLOCK) ON UploadTypeAttribute.UploadAttribute_ID = UploadAttribute.UploadAttribute_ID
		WHERE IsActive = 1 and IsCalculated = 0
			and (Len(@StoreNos) > 0 or UploadTypeAttribute.UploadType_Code = 'ITEM_MAINTENANCE')
			and (@UseStoreJurisdictions = 1 OR (@UseStoreJurisdictions = 0 AND LOWER(ColumnNameorKey) != 'isdefaultjurisdiction'))

	OPEN attributes_cursor
	FETCH NEXT FROM attributes_cursor INTO @TableName,@ColumnName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		
		IF @LoadFromSLIM = 1
		BEGIN
			-- IMPORTANT: for now, vendor deals are only available when loading SLIM data
			-- for that reason the vendor deal attributes have 'slim_vendordealview'
			-- as their table name
			 
			IF LOWER(@TableName) = LOWER('slim_vendordealview')
			BEGIN
				select @SQL = @SQL + ',' + @TableName + '.'+@ColumnName	 + ' as ' + upper(@TableName+'_'+@ColumnName)
			END
			ELSE IF LOWER(@ColumnName) = LOWER('National_Identifier')
			BEGIN
				-- this is needed because the national_identifier column is of type tinyint
				-- while holding a boolean value
				-- dot net will not read it properly unless type cast
				select @SQL = @SQL + ', CAST(ItemIdentifier_TableName.national_identifier as bit) as ' + upper(@TableName+'_'+@ColumnName)
			END
			ELSE
			BEGIN
				select @SQL = @SQL + ',' + @TableName + '_TableName' + '.'+@ColumnName	 + ' as ' + upper(@TableName+'_'+@ColumnName)
			END
		END
		ELSE
		BEGIN
			
			IF LOWER(@TableName) = LOWER('price') AND LOWER(@ColumnName) = LOWER('linkeditem')
			BEGIN
				-- this lets us use an alias to a joined table to convert the linked item key
				-- to the identifier for display
				select @SQL = @SQL + ', linkeditemidentifier.identifier as ' + upper(@TableName+'_'+@ColumnName)
			END
			ELSE IF LOWER(@ColumnName) = LOWER('National_Identifier')
			BEGIN
				-- this is needed because the national_identifier column is of type tinyint
				-- while holding a boolean value
				-- dot net will not readi properly unless type cast
				select @SQL = @SQL + ', CAST(ItemIdentifier_TableName.national_identifier as bit) as ' + upper(@TableName+'_'+@ColumnName)
			END
			ELSE IF LOWER(@TableName) <> LOWER('slim_vendordealview')
			BEGIN
				select @SQL = @SQL + ',' + @TableName + '_TableName' + '.'+@ColumnName	 + ' as ' + upper(@TableName+'_'+@ColumnName)
			END

		END
		
		FETCH NEXT FROM attributes_cursor INTO @TableName,@ColumnName
	END
	CLOSE attributes_cursor
	DEALLOCATE attributes_cursor
	
	select @SQL = @SQL + ', Vendor_TableName.CompanyName AS VENDOR_COMPANYNAME '
	select @SQL = @SQL + ', ItemBrand.Brand_Name AS ITEMBRAND_BRAND_NAME '
	select @SQL = @SQL + ', Subteam.Subteam_Name AS SUBTEAM_SUBTEAM_NAME '
	select @SQL = @SQL + ', ItemCategory.Category_Name AS ITEMCATEGORY_CATEGORY_NAME '
	
	IF @Use4LevelHierarchy = 1
	BEGIN
		select @SQL = @SQL + ', ProdHierarchyLevel3.Description AS PRODHIERARCHYLEVEL3_DESCRIPTION '
		select @SQL = @SQL + ', ProdHierarchyLevel4.Description AS PRODHIERARCHYLEVEL4_DESCRIPTION '
		select @SQL = @SQL + ', ProdHierarchyLevel4.ProdHierarchyLevel3_ID AS CALCULATED_PRODHIERARCHYLEVEL3_ID '
	END
	
	IF @LoadFromSLIM = 0
	BEGIN
		select @SQL = @SQL + ', dbo.fn_EIM_GetListOfItemChains(Item_TableName.Item_Key) AS CALCULATED_ITEMCHAINS '
	END
	
	IF (Len(@StoreNos) = 0)
	BEGIN
		select @SQL = @SQL + ', '''' AS STORE_STORE_NAME '
		select @SQL = @SQL + ', NULL AS STORE_STORE_NO '
	END
	ELSE
	BEGIN
		select @SQL = @SQL + ', Store_TableName.Store_Name AS STORE_STORE_NAME '
	END

	IF @UseStoreJurisdictions = 0
	BEGIN
		select @SQL = @SQL + ', CAST(1 as bit) AS ITEM_ISDEFAULTJURISDICTION '
		select @SQL = @SQL + ', '''' AS STOREJURISDICTION_STOREJURISDICTIONDESC '
	END
	ELSE
	BEGIN
		select @SQL = @SQL + ', Item_TableName.IsDefaultJurisdiction AS ITEM_ISDEFAULTJURISDICTION '
		select @SQL = @SQL + ', StoreJurisdiction.StoreJurisdictionDesc AS STOREJURISDICTION_STOREJURISDICTIONDESC '
	END
	
	select @SQL = @SQL + ', 0 AS STICKY '

	IF (Len(@StoreNos) <> 0)
	BEGIN

		-- ************************************ Price and Cost ***********************************************
		select @PriceCost = ', dbo.fn_GetMargin(Price_TableName.Price, Price_TableName.Multiple, ISNULL(VendorCostHistory_TableName.NetCost / (CASE WHEN ISNULL( VendorCostHistory_TableName.Package_Desc1, 1) = 0 THEN 1 ELSE ISNULL( VendorCostHistory_TableName.Package_Desc1, 1) END) , 1)) AS CALCULATED_MARGIN '
		select @SQL = @SQL + @PriceCost
		
		IF @LoadFromSLIM = 0
		BEGIN
			-- ************************************ Discounts and Allowances *************************************
			select @SQL = @SQL + ',' + ' dbo.fn_GetCurrentSumAllowances(Item_TableName.Item_Key, Store_TableName.Store_No) AS CALCULATED_ALLOWANCES '
			select @SQL = @SQL + ',' + ' dbo.fn_GetCurrentSumDiscounts(Item_TableName.Item_Key, Store_TableName.Store_No) AS CALCULATED_DISCOUNTS '
			-- ***************************************************************************************************
		END
		ELSE
		BEGIN
			select @SQL = @SQL + ', Price_TableName.POSPrice AS PRICE_POSPRICE '
			select @SQL = @SQL + ', VendorCostHistory_TableName.unitcost AS VENDORCOSTHISTORY_UNITCOST '
		END
				


		-- ***************************************************************************************************
	End
	Else
	Begin
		select @SQL = @SQL + ', 0.00 AS PRICE_POSPRICE '
		select @SQL = @SQL + ', 0.00 AS VENDORCOSTHISTORY_UNITCOST '
	End


	-- ********* From clause and Joins ************

	SELECT @SQL = @SQL + ' FROM Item_TableName (nolock) '

	-- ************** JOINS ***********************
	-- *******************************
	SELECT @SQL = @SQL + '
					INNER JOIN
						Subteam  (nolock)
						ON Item_TableName.Subteam_No = subteam.Subteam_No
					LEFT JOIN
						ItemCategory  (nolock)
						ON Item_TableName.Category_ID = ItemCategory.Category_ID
					LEFT JOIN
						ProdHierarchyLevel4  (nolock)
						ON Item_TableName.ProdHierarchyLevel4_ID = ProdHierarchyLevel4.ProdHierarchyLevel4_ID
					LEFT JOIN
						ProdHierarchyLevel3  (nolock)
						ON ProdHierarchyLevel4.ProdHierarchyLevel3_ID = ProdHierarchyLevel3.ProdHierarchyLevel3_ID
					LEFT JOIN
						ItemBrand (nolock)
						ON Item_TableName.Brand_ID = itembrand.Brand_ID
					LEFT JOIN
						ItemScale_TableName (nolock)
						ON Item_TableName.Item_Key = ItemScale_TableName.Item_Key '
						
		IF @LoadFromSLIM = 0 AND @UseStoreJurisdictions = 1
		BEGIN
			-- if we are using the jurisdiction view instead of the Item and ItemScale tables
			-- then we need to additionally join on the StoreJurisdictionIds in the views.
			SELECT @SQL = @SQL + '
						AND Item_TableName.StoreJurisdictionId = ItemScale_TableName.StoreJurisdictionId'
		END

		SELECT @SQL = @SQL + ' 
						LEFT JOIN ItemSignAttribute (nolock)
						ON Item_TableName.Item_Key = ItemSignAttribute.Item_Key '
					
		SELECT @SQL = @SQL + '
					LEFT JOIN
						Scale_ExtraText_TableName  (nolock)
						ON ItemScale_TableName.Scale_ExtraText_ID = Scale_ExtraText_TableName.Scale_ExtraText_ID
					LEFT JOIN 
						Scale_StorageData_TableName (nolock)
						ON ItemScale_TableName.Scale_StorageData_ID = Scale_StorageData_TableName.Scale_StorageData_ID
					INNER JOIN
						ItemIdentifier_TableName  (nolock)
						ON Item_TableName.Item_Key = ItemIdentifier_TableName.Item_Key
					LEFT JOIN
						ItemUnit (nolock)
						ON ItemUnit.unit_id = Item_TableName.package_unit_id
					LEFT JOIN
						ItemAttribute_TableName  (nolock)
						on Item_TableName.item_key = ItemAttribute_TableName.item_key '
	
	IF (Len(@StoreNos) <> 0)
	BEGIN
		SELECT @SQL = @SQL +
					' LEFT JOIN
						Price_TableName (nolock)
						on Price_TableName.item_key = Item_TableName.Item_Key
					LEFT JOIN
						ItemIdentifier (nolock) LinkedItemIdentifier
						ON LinkedItemIdentifier.Item_Key = Price_TableName.LinkedItem
					INNER JOIN
						Store_TableName (nolock)
						ON Store_TableName.Store_No = Price_TableName.Store_No
						AND Store_TableName.StoreJurisdictionID = Item_TableName.StoreJurisdictionID						
					LEFT JOIN
						StoreItem_TableName (nolock)
						ON StoreItem_TableName.Store_No = Store_TableName.Store_No
						AND StoreItem_TableName.Item_Key = Item_TableName.Item_Key				
					INNER JOIN
						StoreItemVendor_TableName (nolock)
						ON StoreItemVendor_TableName.Item_Key = Item_TableName.Item_Key
							AND StoreItemVendor_TableName.Store_No = Store_TableName.Store_No
					INNER JOIN
						Vendor_TableName (nolock)
						ON Vendor_TableName.Vendor_ID = StoreItemVendor_TableName.Vendor_ID
					INNER JOIN
						ItemVendor_TableName (nolock)
						ON ItemVendor_TableName.Item_Key = Item_TableName.Item_Key
							AND ItemVendor_TableName.Vendor_ID = Vendor_TableName.Vendor_ID '

		IF @LoadFromSLIM = 1
		BEGIN
								
				SELECT @SQL = @SQL +
						' LEFT JOIN
							VendorCostHistory_TableName (nolock)
							ON VendorCostHistory_TableName.StoreItemVendorID = StoreItemVendor_TableName.StoreItemVendorID
						LEFT JOIN SLIM_VendorDealView  (nolock)
							ON SLIM_VendorDealView.VendorDeal_ID = Item_TableName.Item_Key'
		END
		ELSE
		BEGIN
								
				SELECT @SQL = @SQL +
						' LEFT JOIN
							dbo.fn_VendorCostAll(CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))) VendorCostHistory 
							ON VendorCostHistory.Item_Key = Item_TableName.Item_Key
								and VendorCostHistory.Store_No = Store_TableName.Store_No
								and VendorCostHistory.Vendor_ID = StoreItemVendor_TableName.Vendor_ID'
		END
						
	END
	ELSE
	BEGIN 
		SELECT @SQL = @SQL +
					' 	INNER JOIN
						Store (nolock)
						on Store.StoreJurisdictionID = Item_TableName.StoreJurisdictionID
					
						LEFT JOIN
						StoreItem (nolock)
						ON StoreItem.Store_No = Store.Store_No
						AND StoreItem.Item_Key = Item_TableName.Item_Key				
					
						INNER JOIN
						StoreItemVendor (nolock)
						ON StoreItemVendor.Item_Key = Item_TableName.Item_Key
						AND StoreItemVendor.Store_No = Store.Store_No
					
						INNER JOIN
						Vendor (nolock)
						ON Vendor.Vendor_ID = StoreItemVendor.Vendor_ID
											
						INNER JOIN
						ItemVendor (nolock)
						ON Item_TableName.Item_Key = ItemVendor.Item_Key
						AND ItemVendor.Vendor_ID = StoreItemVendor.Vendor_ID '
	END
	
	IF @UseStoreJurisdictions = 1
	BEGIN
		SELECT @SQL = @SQL +
					' INNER JOIN
						StoreJurisdiction (nolock)
						ON StoreJurisdiction.StoreJurisdictionID = Item_TableName.StoreJurisdictionID '
	END
	
	-- construct the where clause

	SELECT @SQL = @SQL + ' WHERE 1=1 '

	SELECT @SQL = @SQL + ' AND (ItemVendor_TableName.DeleteDate IS NULL OR ItemVendor_TableName.DeleteDate > GetDate()) '
 
	IF (@Item_Description <> '') SELECT @SQL = @SQL + ' AND LOWER(Item_TableName.Item_Description) LIKE ''%' + LOWER(REPLACE(LTRIM(RTRIM(@Item_Description)), '''', '''''')) + '%'' '
	
	IF (@PS_Vendor_ID <> '') SELECT @SQL = @SQL + ' AND LOWER(Vendor_TableName.PS_Vendor_ID) LIKE ''%' + LOWER(REPLACE(LTRIM(RTRIM(@PS_Vendor_ID)), '''', '''''')) + '%'' '
	
	IF (@Vendor_Item_ID <> '') SELECT @SQL = @SQL + ' AND LOWER(ItemVendor_TableName.Item_ID) LIKE ''%' + LOWER(REPLACE(LTRIM(RTRIM(@Vendor_Item_ID)), '''', '''''')) + '%'' '
	
	IF (@Brand_ID > 0) SELECT @SQL = @SQL + ' AND Item_TableName.Brand_ID = ' + CONVERT(VARCHAR(10), @Brand_ID) + ' '
	
	IF @Identifier IS NULL OR @Identifier = ''
		BEGIN
			SELECT @SQL = @SQL + ' AND ItemIdentifier_TableName.Default_Identifier = 1 '
		END
	ELSE
		BEGIN
			SELECT @SQL = @SQL + ' AND LOWER(ItemIdentifier_TableName.Identifier) LIKE ''%' + LOWER(REPLACE(LTRIM(RTRIM(@Identifier)), '''', '''''')) + '%'' '
		END
	
	IF (@Subteam_No > 0)SELECT @SQL = @SQL + ' AND Item_TableName.Subteam_No = ' + CONVERT(VARCHAR(10), @Subteam_No) + ' '
	
	IF (@Category_ID > 0)SELECT @SQL = @SQL + ' AND Item_TableName.Category_ID = ' + CONVERT(VARCHAR(10), @Category_ID) + ' '
	
	IF (@Level3_ID > 0)SELECT @SQL = @SQL + ' AND ProdHierarchyLevel4.ProdHierarchyLevel3_ID = ' + CONVERT(VARCHAR(10), @Level3_ID) + ' '
	
	IF (@Level4_ID > 0)SELECT @SQL = @SQL + ' AND Item_TableName.ProdHierarchyLevel4_ID = ' + CONVERT(VARCHAR(10), @Level4_ID) + ' '
	
	IF @ItemChainID > 0 SELECT @SQL = @SQL + ' AND Item_TableName.Item_Key in (SELECT Item_Key FROM ItemChainItem where ItemChainID=' + CONVERT(VARCHAR(20), @ItemChainID) + ') ' 
    
	IF @DistSubTeam_No > 0 SELECT @SQL = @SQL + ' AND Item_TableName.DistSubTeam_No = ' + CONVERT(VARCHAR(20), @DistSubTeam_No) + ' ' 

	IF @LoadFromSLIM = 0
	BEGIN
		-- filter out the "deleted" relationships
		SELECT @SQL = @SQL + ' AND (ItemVendor_TableName.DeleteDate IS NULL OR ItemVendor_TableName.DeleteDate > GetDate()) '
	
		SELECT @SQL = @SQL + ' AND ItemIdentifier_TableName.Deleted_identifier = 0 '

		IF (@Discontinue_Item = 0) 
		BEGIN
			-- use scalar function to find the Discontinue status to see if all stores for that item are marked discontinue
			IF LEN(@StoreNos) <> 0
				SELECT @SQL = @SQL + ' AND dbo.fn_GetDiscontinueStatus(Item_TableName.Item_Key, Store_TableName.Store_No, NULL) = 0 '
			ELSE
				SELECT @SQL = @SQL + ' AND dbo.fn_GetDiscontinueStatus(Item_TableName.Item_Key, NULL, NULL) = 0 '
		END
		    
		IF (@Exclude_Not_Available = 1) 
			SELECT @SQL = @SQL + ' AND Item_TableName.Not_Available = 0 '
		    
		    
		IF (@IncludeDeletedItems = 0) SELECT @SQL = @SQL + ' AND Item_TableName.Deleted_Item = 0 AND Item_TableName.Remove_Item = 0 ' 
	END
	  
	IF (Len(@StoreNos) <> 0)
	BEGIN
		SELECT @SQL = @SQL + ' AND Price_TableName.Store_No IN (' + @StoreNos + ') '
			
		SELECT @SQL = @SQL + ' AND (LinkedItemIdentifier.Default_Identifier IS NULL OR LinkedItemIdentifier.Default_Identifier = 1)'

		IF (@Vendor_ID <> 0)SELECT @SQL = @SQL + ' AND StoreItemVendor_TableName.Vendor_ID = ' + CONVERT(VARCHAR(10), @Vendor_ID) + ' '
	END
	ELSE
	BEGIN
		IF (@Vendor_ID <> 0)SELECT @SQL = @SQL + ' AND ItemVendor_TableName.Vendor_ID = ' + CONVERT(VARCHAR(10), @Vendor_ID) + ' '
	END
	
	IF @UseStoreJurisdictions = 1
	BEGIN
		IF (Lower(@IsDefaultJurisdiction) <> '-1')SELECT @SQL = @SQL + ' AND Item_TableName.IsDefaultJurisdiction = ' + CONVERT(VARCHAR(10), @IsDefaultJurisdiction) + ' '
		IF (@JurisdictionId <> 0)SELECT @SQL = @SQL + ' AND Item_TableName.StoreJurisdictionID = ' + CONVERT(VARCHAR(10), @JurisdictionId) + ' '
	END
	
	IF @LoadFromSLIM = 1
	BEGIN
		SELECT @SQL = REPLACE(@SQL, 'ItemAttribute_TableName', 'SLIM_ItemAttributeView')
		SELECT @SQL = REPLACE(@SQL, 'ItemIdentifier_TableName', 'SLIM_ItemIdentifierView')
		SELECT @SQL = REPLACE(@SQL, 'ItemScale_TableName', 'SLIM_ItemScaleView')
		SELECT @SQL = REPLACE(@SQL, 'Scale_ExtraText_TableName', 'SLIM_Scale_ExtraTextView')
		SELECT @SQL = REPLACE(@SQL, 'Price_TableName', 'SLIM_PriceView')
		SELECT @SQL = REPLACE(@SQL, 'VendorCostHistory_TableName', 'SLIM_VendorCostHistoryView')
		SELECT @SQL = REPLACE(@SQL, 'StoreItemVendor_TableName', 'SLIM_StoreItemVendorView')
		SELECT @SQL = REPLACE(@SQL, 'ItemVendor_TableName', 'SLIM_ItemVendorView')
		SELECT @SQL = REPLACE(@SQL, 'StoreItem_TableName', 'SLIM_StoreItemView')
		SELECT @SQL = REPLACE(@SQL, 'Item_TableName', 'SLIM_ItemView')
	END
	ELSE
	BEGIN
		SELECT @SQL = REPLACE(@SQL, 'ItemIdentifier_TableName', 'ItemIdentifier')
		SELECT @SQL = REPLACE(@SQL, 'ItemAttribute_TableName', 'ItemAttribute')
		SELECT @SQL = REPLACE(@SQL, 'Scale_ExtraText_TableName', 'Scale_ExtraText')
		SELECT @SQL = REPLACE(@SQL, 'Scale_StorageData_TableName', 'Scale_StorageData')
		SELECT @SQL = REPLACE(@SQL, 'Price_TableName', 'Price')
		SELECT @SQL = REPLACE(@SQL, 'VendorCostHistory_TableName', 'VendorCostHistory')
		SELECT @SQL = REPLACE(@SQL, 'StoreItemVendor_TableName', 'StoreItemVendor')
		SELECT @SQL = REPLACE(@SQL, 'ItemVendor_TableName', 'ItemVendor')
		SELECT @SQL = REPLACE(@SQL, 'StoreItem_TableName', 'StoreItem')

		IF @UseStoreJurisdictions = 1
		BEGIN
			-- use the jurisdiction views instead of the Item and ItemScale tables
			SELECT @SQL = REPLACE(@SQL, 'ItemScale_TableName', 'EIM_Jurisdiction_ItemScaleView')
			SELECT @SQL = REPLACE(@SQL, 'Item_TableName', 'EIM_Jurisdiction_ItemView')
			SELECT @SQL = REPLACE(@SQL, 'itemsignattribute_TableName.signromancetextlong', 'EIM_Jurisdiction_ItemView.signromancetextlong')
			SELECT @SQL = REPLACE(@SQL, 'itemsignattribute_TableName.signromancetextshort', 'EIM_Jurisdiction_ItemView.signromancetextshort')
		END
		ELSE
		BEGIN
			SELECT @SQL = REPLACE(@SQL, 'ItemScale_TableName', 'ItemScale')
			SELECT @SQL = REPLACE(@SQL, 'Item_TableName', 'Item')
			SELECT @SQL = REPLACE(@SQL, 'itemsignattribute_TableName.signromancetextlong', 'ItemSignAttribute.signromancetextlong')
			SELECT @SQL = REPLACE(@SQL, 'itemsignattribute_TableName.signromancetextshort', 'ItemSignAttribute.signromancetextshort')
		END
	END
				
	SELECT @SQL = REPLACE(@SQL, 'Store_TableName', 'Store')
	SELECT @SQL = REPLACE(@SQL, 'Vendor_TableName', 'Vendor')
	SELECT @SQL = REPLACE(@SQL, 'ItemSignAttribute_TableName', 'ItemSignAttribute')
	
	execute(@SQL)
	--select @SQL

	SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_ItemLoadSearch] TO [IRMAClientRole]
    AS [dbo];

