CREATE PROCEDURE [dbo].[Reporting_Movement_BySubteam]
    @BeginDate varchar(10),
    @EndDate varchar(10),
    @SubTeam_No int,
    @Category_ID int,
    @Vendor_ID int
WITH RECOMPILE
AS

-- **************************************************************************
-- Procedure: Reporting_Movement_BySubteam()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from multiple RDL files and generates reports consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 01/11/2013  BAS	Update i.Discontinue_Item reference to
--					account for schema change. 
-- **************************************************************************

BEGIN  -- Begin Stored Procedure

SET NOCOUNT ON

/* Build a temp table dynamically off of the number of stores */

-- Build a temp table w/ just an item key field. This will be the item list
-- created for one subteam
	if exists (select * from dbo.sysobjects where id = object_id(N'#ItemList') and OBJECTPROPERTY(id, N'IsTable') = 1)
	drop table #ItemList

	CREATE TABLE #ItemList (Item_Key int PRIMARY KEY CLUSTERED, upcno varchar(13) NULL,  code varchar(25) NULL,  dept int NULL,
	 cat varchar(35) NULL, family varchar(35) NULL, category varchar(35) NULL,  class varchar(35) NULL,  createdate varchar(10) NULL,
	 brand varchar(35) NULL, description varchar(60) NULL, pk decimal(14,4) NULL, size decimal(14,4) NULL, uom varchar(25) NULL,
	tagtype varchar(4) NULL, 	rpt varchar(25) NULL, vendor varchar(50) NULL, vendornum varchar(10) NULL, case_size decimal(14,4) NULL,
	cost decimal(14,4) NULL, ytd_sold decimal(14,4) NULL, ytd_sales decimal(14,4) NULL, promo varchar(2) NULL, blank_one varchar(5) NULL)
	
-- create variable(s) to hold the output of the cursors
	DECLARE @Store_No int

-- There will be some dynamic sql created to make variable names
-- specific to each store number, create a variable to hold the SQL string
	DECLARE @SQL nvarchar(max), @SQL2 nvarchar(max), @SQL3 nvarchar(max), @StoreString varchar(20)

-- clear out any data in the SQL string variable 
	SET @SQL = ('')
	SET @SQL2 = ('')
	SET @SQL3 = ('')
	
-- Create a store list cursor, for each store add a column as
-- '<store_no>(Unit)', datatype as decimal(14,4)
-- create the cursor
	DECLARE itr_alterunit CURSOR READ_ONLY
	FOR
		SELECT s.store_no
		FROM store s (nolock) INNER JOIN zone z (nolock) ON s.zone_id=z.zone_id
	
-- open the cursor
	OPEN itr_alterunit
	
-- grab the next record in the result set and drop the values into 
-- the variables provided; note: acts like a parameter signature
	FETCH NEXT FROM itr_alterunit INTO @Store_No
	-- @@fetch_status = -1 means fetch failed row beyond the result set
	WHILE (@@fetch_status <> -1)
	BEGIN
--			exec (@SQL) --must be in parentheses, otherwise it compiles but doesn't execute; go figure
			set @StoreString = (CAST(@Store_No as varchar(6)))
			-- add a column
			set @SQL = (@SQL + ' alter table #ItemList add s' + @StoreString + '_Unit decimal(14,4) NULL')
			-- add a column
			set @SQL2 = (@SQL2 + ' alter table #ItemList add s' + @StoreString + '_Sales decimal(14,4) NULL')
			-- add a column
			set @SQL3 = (@SQL3 + ' alter table #ItemList add s' + @StoreString + '_Margin decimal(14,4) NULL')
			
		-- grab the next record for processing
		FETCH NEXT FROM itr_alterunit INTO @Store_No
	END
	
-- close the cursor
	CLOSE itr_alterunit

			EXEC sp_executesql 
				@stmt = @SQL
			EXEC sp_executesql 
				@stmt = @SQL2
			EXEC sp_executesql 
				@stmt = @SQL3
	
-- clear out any data in the SQL string variable 
	SET @SQL = ('')

-- Add one column for the top 1 vendor_id, and hold onto a storeitemvendor_id
-- solely just to grab the first available primary vendor, for reference only
	ALTER TABLE #ItemList ADD vendor_id int NULL
	ALTER TABLE #ItemList ADD storeitemvendor_id int NULL
	
--DaveStacey - 20071025 - moved this from above
-- Now drop in the item keys for the selected subteam
	INSERT INTO #ItemList (Item_Key) --must reference the field otherwise t-sql screams
	SELECT i.item_key
	FROM item i (nolock)
		join itemidentifier ii (nolock) on ii.item_key = i.item_key
		left join price p (nolock) on p.item_key = i.item_key
		left join storesubteam sst (nolock) on sst.store_no = p.store_no and sst.subteam_no = p.ExceptionSubTeam_No
	WHERE (p.ExceptionSubTeam_No = ISNULL(@SubTeam_No, p.ExceptionSubTeam_No) or  i.SubTeam_No = ISNULL(@SubTeam_No, i.SubTeam_No) )
		and	dbo.fn_GetDiscontinueStatus(i.Item_Key, NULL, NULL) = 0 and I.Deleted_Item = 0
		and ii.Default_Identifier = 1
	GROUP BY I.Item_Key 
	ORDER BY  I.Item_Key
	
/* At this point the temp table has the full structure needed, 
	ready to go to populate the table */

-- Populate all the item attribute information, take note of the columns
-- that can be null

	UPDATE #ItemList
	SET #ItemList.upcno = ItemIdentifier.Identifier
	FROM #ItemList
		INNER JOIN ItemIdentifier (nolock)
			ON #ItemList.Item_Key = ItemIdentifier.Item_Key
				AND ItemIdentifier.Default_Identifier = 1
			
	UPDATE #ItemList
	SET 
		#ItemList.dept = Item.Subteam_No, 
		#ItemList.description = Item.Item_Description, 
		#ItemList.createdate = CAST(MONTH(Item.Insert_Date) AS varchar(2)) + '/' + CAST(DAY(Item.Insert_Date) AS varchar(2)) + '/' + CAST(YEAR(Item.Insert_Date) AS varchar(4)),
		#ItemList.cat = ItemCategory.Category_Name,
		#ItemList.family = NatItemFamily.NatFamilyName,
		#ItemList.category = NatItemCat.NatCatName,
		#ItemList.class = NatItemClass.ClassName,
		#ItemList.pk = Item.Package_Desc1,
		#ItemList.size = Item.Package_Desc2,
		#ItemList.uom = ItemUnit.Unit_Abbreviation,
		#ItemList.brand = ItemBrand.Brand_Name
	FROM #ItemList
		INNER JOIN Item (nolock)
			ON #ItemList.Item_Key = Item.Item_Key
		LEFT JOIN ItemCategory (nolock)
			ON Item.Category_ID = ItemCategory.Category_ID
		LEFT JOIN ItemUnit (nolock)
			ON Item.Package_Unit_ID = ItemUnit.Unit_ID
		LEFT JOIN ItemBrand (nolock)
			ON Item.Brand_ID = ItemBrand.Brand_ID
		LEFT JOIN NatItemClass (nolock)
			ON Item.ClassID = NatItemClass.ClassID
		LEFT JOIN NatItemCat (nolock)
			ON NatItemClass.NatCatID = NatItemCat.NatCatID
		LEFT JOIN NatItemFamily (nolock)
			ON NatItemCat.NatFamilyID = NatItemFamily.NatFamilyID
						
-- Grab data out of the ItemAttribute table, needs some dynamic SQL

-- create variable(s) to use as query parameters
	DECLARE @Field_Type varchar(50)
	
-- update #ItemList.rpt
	SELECT @Field_Type = (SELECT RIGHT(RTRIM(ai.field_type),1) FROM attributeidentifier ai (nolock) WHERE ai.screen_text = 'Report Codes')
	--select @Field_Type as Field_Type
	
	IF @Field_Type IS NOT NULL
	BEGIN
		SET @SQL = ('UPDATE #ItemList SET #ItemList.rpt = ia.Text_' + @Field_Type + ' FROM #ItemList INNER JOIN ItemAttribute ia (nolock) ON #ItemList.Item_Key = ia.Item_Key')
		EXEC sp_executesql 
		@stmt = @SQL
	END
	
-- update #ItemList.tagtype
	SELECT @Field_Type = (SELECT RIGHT(RTRIM(ai.field_type),1) FROM attributeidentifier ai (nolock) WHERE ai.screen_text = 'Tag UOM')
	
	IF @Field_Type IS NOT NULL
	BEGIN
		SET @SQL = ('UPDATE #ItemList SET #ItemList.tagtype = ia.Text_' + @Field_Type + ' FROM #ItemList INNER JOIN ItemAttribute ia (nolock) ON #ItemList.Item_Key = ia.Item_Key')
		--PRINT @SQL -- print the SQL statement for debugging
		EXEC sp_executesql 
		@stmt = @SQL
	END
	
-- Create a variable to hold the output of the cursor
	DECLARE @Item_Key int
	
-- Create the cursor
	DECLARE itr_vendor CURSOR READ_ONLY
	FOR 
		SELECT #ItemList.Item_Key FROM #ItemList
		
-- open the cursor
	OPEN itr_vendor
	
-- grab the next record in the result set and drop the values into 
-- the variables provided
	FETCH NEXT FROM itr_vendor INTO @Item_Key
	-- @@fetch_status = -1 means fetch failed row beyond the result set
	WHILE (@@fetch_status <> -1)
	BEGIN
			UPDATE #ItemList
			SET 
				#ItemList.vendor_id = Result.vendor_id,
				#ItemList.storeitemvendor_id = Result.storeitemvendorid
			FROM (
				SELECT TOP 1 siv.vendor_id, siv.storeitemvendorid, siv.Item_Key 
				FROM StoreItemVendor siv (nolock)
					INNER JOIN #ItemList B
						ON siv.item_key = B.Item_key
				WHERE B.Item_Key = @Item_Key
					AND siv.primaryvendor = 1
				ORDER BY siv.Store_No) as Result
			WHERE #ItemList.Item_Key = Result.Item_Key
		-- grab the next record for processing
		FETCH NEXT FROM itr_vendor INTO @Item_Key
	END	
	
-- close the cursor
	CLOSE itr_vendor

-- release the cursor from memory
	DEALLOCATE itr_vendor

-- TODO: These couple of vendor updates should probably be consolidated into a cleaner statement
-- Now update the vendor information for the items that returned a vendor_id
	UPDATE #ItemList
	SET 
		#ItemList.vendor = Vendor.CompanyName,
		#ItemList.vendornum = Vendor.Vendor_Key
	FROM #ItemList
		INNER JOIN Vendor (nolock)
			ON #ItemList.vendor_id = Vendor.vendor_id
			
-- Update the case size off of VendorCostHistory
	UPDATE #ItemList
	SET
		#ItemList.case_size = VCH.Package_Desc1
	FROM #ItemList
		INNER JOIN VendorCostHistory VCH (nolock)
			ON #ItemList.storeitemvendor_id = VCH.storeitemvendorid 
	WHERE VCH.VendorCostHistoryID = (select top 1 VendorCostHistoryID FROM VendorCostHistory VCH (nolock)
			WHERE #ItemList.storeitemvendor_id = VCH.storeitemvendorid order by VCH.VendorCostHistoryID Desc)
		
--------------------------------------------------------------------------------
-- DEBUG (DELETE BEFORE PRD): Check the table to ensure proper fields created
	--SELECT * FROM #ItemList
--------------------------------------------------------------------------------

-- Create a store list cursor, for each store retrieve the movement data and 
-- update the appropriate unit, sales and margin data columns
	
-- open the cursor
	OPEN itr_alterunit
	
-- grab the next record in the result set and drop the values into 
-- the variables provided
	FETCH NEXT FROM itr_alterunit INTO @Store_No
	-- @@fetch_status = -1 means fetch failed row beyond the result set
	WHILE (@@fetch_status <> -1)
	BEGIN
			SET @StoreString = (CAST(@Store_No as varchar(6)))
			-- clear out any data in the SQL string variable 
				SET @SQL = ('')
			-- build the SQL string for the update - set units, cost and margin for each store
			SET @SQL = (@SQL + 'UPDATE #ItemList SET #ItemList.s' + @StoreString + '_Unit = Result.Unit, #ItemList.s' + @StoreString + '_Sales = Result.Sales FROM #ItemList JOIN (SELECT B.Item_Key, SUM(dbo.fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, SSBI.Price_Level, SSBI.Sales_Quantity, SSBI.Return_Quantity, Item.Package_Desc1, Weight)) As Unit, SUM(Sales_Amount - Return_Amount - Markdown_Amount - Promotion_Amount) As Sales FROM #ItemList B LEFT JOIN Sales_SumByItem SSBI (nolock) ON B.Item_Key = SSBI.Item_Key LEFT JOIN Item (nolock) ON B.Item_Key = Item.Item_Key LEFT JOIN ItemIdentifier (nolock) ON Item.Item_Key = ItemIdentifier.Item_Key LEFT JOIN ItemUnit (nolock) ON Item.Retail_Unit_ID = ItemUnit.Unit_ID WHERE SSBI.Store_No = '+ @StoreString + ' AND (SSBI.Date_Key >= CONVERT(smalldatetime, ''' + @BeginDate + ''') AND SSBI.Date_Key < CONVERT(smalldatetime, ''' + @EndDate + ''')) GROUP BY B.Item_Key) Result ON #ItemList.Item_Key = Result.Item_Key')
			EXEC sp_executesql 
				@stmt = @SQL
			-- clear out any data in the SQL string variable 
				SET @SQL = ('')
			
			SET @SQL = (@SQL + 'UPDATE #ItemList SET #ItemList.cost = (dbo.fn_GetCurrentNetCost(#ItemList.Item_Key, ' + @StoreString + '))') 
			EXEC sp_executesql 
				@stmt = @SQL
			-- clear out any data in the SQL string variable 
				SET @SQL = ('')
			
			SET @SQL = (@SQL + 'UPDATE #ItemList SET #ItemList.s' + @StoreString + '_Margin = CASE WHEN #ItemList.s' + @StoreString + '_Sales > 0 THEN (#ItemList.s' + @StoreString + '_Sales - (#ItemList.s' + @StoreString + '_Unit * (#ItemList.cost / CASE WHEN #ItemList.case_size > 0 Then #ItemList.case_size ELSE 1 END))) ELSE NULL END')
			EXEC sp_executesql 
				@stmt = @SQL
			
		-- grab the next record for processing
		FETCH NEXT FROM itr_alterunit INTO @Store_No
	END
	
-- close the cursor
	CLOSE itr_alterunit
	
-- release the cursor from memory
	DEALLOCATE itr_alterunit

--------------------------------------------------------------------------------
-- return final resultset
	SELECT * FROM #ItemList order by item_key
--------------------------------------------------------------------------------

-- The stored procedure is done, drop the temp table
	DROP TABLE #ItemList
	
	SET NOCOUNT OFF
END  -- End Stored Procedure
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_BySubteam] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_BySubteam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_BySubteam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_BySubteam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_BySubteam] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_Movement_BySubteam] TO [ExtractRole]
    AS [dbo];

