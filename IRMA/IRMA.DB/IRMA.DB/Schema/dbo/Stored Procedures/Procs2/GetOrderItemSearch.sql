
CREATE PROCEDURE [dbo].[GetOrderItemSearch] 
	@OrderType_ID		int,
	@FromSubTeam_No		int,
	@ToSubTeam_No		int,
	@SearchSubTeam_No	int,
	@DistSubTeam_No		int,
	@Category_ID		int,
	@Vendor				varchar(50),
	@Store_No			int,
	@Vendor_ID			varchar(20),
	@Item_Description	varchar(60),
	@Identifier			varchar(13),
	@Discontinue_Item	int,
	@Not_Available		int,
	@Brand_ID			int,
	@IsFacility			bit,
	@IsVendorStoreSame	bit,
    @Pre_Order			bit,
    @EXEDistributed		bit,
    @ProductType_ID		int
 
 AS

/*********************************************************************************************
CHANGE LOG
DEV		DATE			TASK	Description
----------------------------------------------------------------------------------------------
BSR		070510			12793	Replace all calls (4 occurrences) from dbo.GetCurrentCost to 
								dbo.GetCurrentNetCost. 
TTL		01/24/2011		759		Changed @Now var to @CostDate and updated to include any vendor
								lead-time in the date used to pull vendor cost attributes.
AM/TL   07/29/2011      2384    Fix for  Items Duplicating When Creating Transfers
MD      09/14/2011      2983    Fixed the issues introduced by bug fix for 2384, limited the 
								logic to Transfer Orders only with appropriate conditions.
KM		2012/01/30		3639	Check if item is authorized for the ordering store;
KM		2012/02/03		3639	Add join to StoreItem to prevent transfer order crash;
KM		2012/02/07		3639	Minor modifications to StoreItem joins;
KM		2012/12/25		8780	Update jurisdictional search to reflect new ItemOverride columns;
FA      2013/29/01      9952    Modified code to do partial vendor name matching
FA      2013/02/06      10814   Modified code to handle blank vendor name in order item search
FA      2013/02/08      10223   Modified code to handle blank vendor name in order item search
MZ      2013/06/19      12767   Fixed an error occurred when retrieving items for distribution 
								orders due to the introduction of the ItemOverride table
MZ      2013/07/01      12961   Added () in the where statement as "(Item.Brand_ID = @Brand_ID 
								OR ItemOverride.Brand_ID = @Brand_ID)"
BL		2013/10/02		14151	Added nolock hints, removed extraneous code for transfers, 
								pre-select item keys to improve performance, added nolock hints, 
								reordered joins. Use Intersect to filter through Vendor and 
								Identifier and ItemDescription search criteria. Leveraged
								the AutomaticOrderList cost logic and look at storeitemvendor
								discontinueitem as opposed to the scalar function call.
BL		2013/10/13		14320	Changed the vendor query to check for exact match before partial
BL		2013/11/15		14499	Modified alias for Item.DistSubteam_no to I.DistSubteam_no
***********************************************************************************************/

BEGIN
    SET NOCOUNT ON

	DECLARE
		@WFM_Store			BIT
		,@HFM_Store			BIT
		,@ExceptionSubTeam	INT
		,@VendorID			INT
		,@CostDate			SMALLDATETIME
		,@SQL				VARCHAR(8000)

	----------------------------------------------------------------------------------------
	--Get the vendor name first
	SELECT @VendorID = Vendor_ID FROM Vendor WHERE CompanyName = @Vendor
	
	IF @Vendor <> '' AND @OrderType_ID <> 1 AND @VendorID IS NULL
	BEGIN
		SELECT @VendorID = Vendor_ID FROM Vendor WHERE CompanyName LIKE '%' + @Vendor + '%'
	END

	----------------------------------------------------------------------------------------
	-- Cost Date
	SELECT @CostDate = GETDATE() + dbo.fn_GetLeadTimeDays(ISNULL(@VendorID, @Vendor_ID))
	
	----------------------------------------------------------------------------------------
	-- Store number
    IF @Store_No IS NOT NULL
	BEGIN
		SELECT @WFM_Store = WFM_Store, @HFM_Store = Mega_Store FROM Store (nolock) WHERE Store_No = @Store_No
	END
	ELSE
	BEGIN

		SELECT @Store_No = Store_No FROM Vendor WHERE CompanyName = @Vendor
		
		IF @Vendor <> '' AND @OrderType_ID <> 1 AND @Store_No IS NULL
			SELECT @Store_No = Store_No FROM Vendor WHERE CompanyName LIKE '%' + @Vendor + '%'
	END

    SELECT @Vendor_ID = RTRIM(@Vendor_ID)

	----------------------------------------------------------------------------------------
	-- Replace any single-quotes with 2 single-quotes
	SELECT @Vendor = REPLACE(RTRIM(@Vendor),'''','''''')
	SELECT @Item_Description = REPLACE(RTRIM(@Item_Description),'''','''''')


	----------------------------------------------------------------------------------------    
	SELECT @SQL = 'DECLARE @date SMALLDATETIME ' 
	SELECT @SQL = @SQL + 'SELECT @date = CONVERT(SMALLDATETIME, GETDATE()) '

	----------------------------------------------------------------------------------------    
    SELECT @SQL = @SQL + 'DECLARE @SubTeamList TABLE (SubTeam_No INT Primary Key) '
	
	IF @ProductType_ID = 1 
			SELECT @SQL = @SQL + 'INSERT @SubTeamList				
									SELECT SubTeam_No FROM SubTeam (NOLOCK) WHERE SubTeamType_ID IN (1,2,3,4) '
	IF @ProductType_ID = 2 
			SELECT @SQL = @SQL + 'INSERT @SubTeamList
									SELECT SubTeam_No FROM SubTeam (NOLOCK) WHERE SubTeamType_ID = 5 '
	IF @ProductType_ID = 3 
			SELECT @SQL = @SQL + 'INSERT @SubTeamList
									SELECT SubTeam_No FROM SubTeam (NOLOCK) WHERE SubTeamType_ID = 6 '

	SELECT @SQL = @SQL + 'CREATE TABLE #Items (Item_Key INT PRIMARY KEY, StoreItemVendorID INT, Vendor_ID INT, DiscontinueItem BIT) 
								
	INSERT INTO #Items (Item_Key, StoreItemVendorID, Vendor_ID, DiscontinueItem) 
								
	SELECT I.Item_Key, SIV.StoreItemVendorID, SIV.Vendor_ID, SIV.DiscontinueItem 
	FROM Item I (NOLOCK) 
	INNER JOIN @SubTeamList S ON I.SubTeam_No =  S.SubTeam_No 
	INNER JOIN StoreItemVendor SIV (NOLOCK) ON SIV.Item_Key = I.Item_Key 
								AND SIV.Vendor_ID = ' + convert(varchar(16), @VendorID) + ' ' +
								'AND SIV.Store_No = ' + convert(varchar(10),ISNULL(@Store_No,0)) + ' ' + 
								'AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > @date)  '

	----------------------------------------------------------------------------------------
	-- Use an intersection of queries using the three filter criteria on 
	-- Vendor, Identifier and Item Description, filtered by subteam
	IF (@Item_Description <> '' OR 
		@Brand_ID IS NOT NULL OR 
	   (@Identifier IS NOT NULL AND @Identifier <> '') OR
	   (@VendorID IS NOT NULL AND @VendorID <> 0))
	BEGIN
		SELECT @SQL = @SQL +  'INNER JOIN (  '
		
		IF (@Item_Description <> '')
		BEGIN

			SELECT @SQL = @SQL + '(SELECT Item.Item_Key 
										FROM Item (NOLOCK) '

			IF @Store_No IS NULL
				SELECT @SQL = @SQL + 'WHERE Item.Item_Description LIKE ''%' + @Item_Description + '%'') '
			ELSE
				SELECT @SQL = @SQL + 'LEFT JOIN ItemOverride (NOLOCK) ON ItemOverride.Item_Key = Item.Item_Key  
									  WHERE Item.Item_Description LIKE ''%' + @Item_Description + '%'' OR ItemOverride.Item_Description LIKE ''%' + @Item_Description + '%'') '
		END
		
		IF (@Brand_ID IS NOT NULL)
		BEGIN
			IF (@Item_Description <> '')
				SELECT @SQL = @SQL + ' INTERSECT '


			SELECT @SQL = @SQL + '(SELECT Item.Item_Key 
										FROM Item (NOLOCK) '

			IF (@Store_No IS NULL)
				SELECT @SQL = @SQL + 'WHERE Item.Brand_ID = ' + CONVERT(VARCHAR(20), @Brand_ID) + ') '
			ELSE
				SELECT @SQL = @SQL + 'LEFT JOIN ItemOverride (NOLOCK) ON ItemOverride.Item_Key = Item.Item_Key  
									  WHERE Item.Brand_ID = ' + CONVERT(VARCHAR(20), @Brand_ID) + ' OR ItemOverride.Brand_ID = ' + CONVERT(VARCHAR(20), @Brand_ID) + ') '
		END
		
		IF (@Identifier IS NOT NULL AND @Identifier <> '')
		BEGIN
			IF (@Item_Description <> '' OR @Brand_ID IS NOT NULL)
				SELECT @SQL = @SQL + ' INTERSECT '


			SELECT @SQL = @SQL + '(SELECT II.Item_Key 
								   FROM ItemIdentifier II (NOLOCK) 
								   WHERE II.Identifier LIKE ''' + @Identifier + '%'' ' + 
								   'GROUP BY II.Item_Key) '
		END
		
		IF (@Vendor <> '''' AND @VendorID IS NOT NULL AND @VendorID <> 0)
		BEGIN
			IF (@Item_Description <> '' OR @Brand_ID IS NOT NULL OR (@Identifier IS NOT NULL AND @Identifier <> '')) 
				SELECT @SQL = @SQL + ' INTERSECT '


			SELECT @SQL = @SQL + '(SELECT IV.Item_Key 
									FROM ItemVendor IV (NOLOCK) 
									WHERE IV.Vendor_ID = ' + convert(varchar(16), @VendorID) + ' ' +
									'GROUP BY IV.Item_Key )'
		END	
				
		SELECT @SQL = @SQL + ') AS itemIntersect ON itemIntersect.Item_Key = I.Item_Key '
	END

	SELECT @SQL = @SQL + 'WHERE 1 = 1 '

	IF (@Discontinue_Item = 0)
		SELECT @SQL = @SQL + 'AND SIV.DiscontinueItem = 0 '

    IF (@DistSubTeam_No > 0)
		BEGIN
			SELECT @SQL = @SQL + 'AND I.DistSubTeam_No = ' + CAST(@DistSubTeam_No as varchar(10)) + ' '
		END

--****************************************************************************	
	-- Manually get cost information (this is faster than using fn_VendorCostAll)
	--****************************************************************************
	-------------------------------------------------------
	-- Get Max VendorCostHistoryIDs for latest cost record
	-------------------------------------------------------
	SELECT @SQL = @SQL + 'SELECT 
		i.StoreItemVendorID		 as StoreItemVendorID,  
		MAX(vch.VendorCostHistoryID) as VendorCostHistoryID 
	INTO #costIDs 
	FROM 
		#items						i
		INNER JOIN VendorCostHistory	vch (nolock) on i.StoreItemVendorID = vch.StoreItemVendorID 
	WHERE 
		(('+CHAR(39)+convert(varchar(20),@CostDate,120)+CHAR(39)+' >= vch.StartDate) AND ('+CHAR(39)+convert(varchar(20),@CostDate,120)+CHAR(39)+' <= vch.EndDate)) 
	GROUP BY 
		i.StoreItemVendorID 

	create clustered index idx_costIDs_VendorCostHistoryID on #costIDs (VendorCostHistoryID) 
	create nonclustered index idx_costIDs_StoreItemVendorID on #costIDs (StoreItemVendorID) INCLUDE (VendorCostHistoryID) '

	--------------------------------------------------
	-- Get Promo Insert Dates from VendorDealHistory
	--------------------------------------------------
	SELECT @SQL = @SQL + 'select 
		cid.StoreItemVendorID	as StoreItemVendorID, 
		max(vdh.insertdate)		as InsertDate 
	into #promoDates 
	from 
		#costIDs					cid	
		left join vendordealhistory vdh (nolock) on cid.StoreItemVendorID	= vdh.StoreItemVendorID 

	where 
		'+CHAR(39)+convert(varchar(20),@CostDate,120)+CHAR(39)+' between vdh.StartDate and vdh.EndDate 
	group by 
		cid.StoreItemVendorID 

	create clustered index idx_promoDates_StoreItemVendorID on #promoDates (StoreItemVendorID) 
	create nonclustered index ix_promoDates_InsertDate on #promoDates (InsertDate) include (StoreItemVendorID) '

	----------------------------------------------------------
	-- Get promo amounts based on Insert Dates in #promoDates
	----------------------------------------------------------
	SELECT @SQL = @SQL + 'select 
		cd.StoreItemVendorID	as StoreItemVendorID, 
		cd.VendorCostHistoryID	as VendorCostHistoryID, 
		ISNULL(SUM(	CASE  
						WHEN vdt.CaseAmtType = ''%'' THEN (vdh.CaseAmt / 100) * ISNULL(vch.UnitCost,0) 
						ELSE vdh.CaseAmt 
					END),0)		as PromoAmount 
	into #promos 
	from 
		#costIDs cd 
		inner join VendorCostHistory vch (nolock) on cd.VendorCostHistoryID = vch.VendorCostHistoryID 
		left join #promoDates		 prd (nolock) on cd.StoreItemVendorID = prd.StoreItemVendorID 
		left join vendordealhistory vdh (nolock) on cd.StoreItemVendorID	= vdh.StoreItemVendorID 
		left join VendorDealType	vdt	(nolock) on vdh.VendorDealTypeID	= vdt.VendorDealTypeID 
	where 
		prd.InsertDate = vdh.InsertDate 
		and '+CHAR(39)+convert(varchar(20),@CostDate,120)+CHAR(39)+' BETWEEN vdh.StartDate and vdh.EndDate 
	group by 
		cd.StoreItemVendorID, 
		cd.VendorCostHistoryID 
	
	create nonclustered index idx_promos_StoreItemVendorID on #promos (StoreItemVendorID) include (PromoAmount) '

	--------------------------------------------------
	-- Put All Needed Cost Fields into it''s own table
	--------------------------------------------------
	SELECT @SQL = @SQL + 'select 
		cid.StoreItemVendorID	as StoreItemVendorID, 
		cid.VendorCostHistoryID as VendorCostHistoryID, 
		vch.UnitCost			as UnitCost, 
		vch.Package_Desc1		as Package_Desc1, 
		ISNULL(vch.UnitCost,0) - ISNULL(pr.PromoAmount,0) + ISNULL(vch.UnitFreight,0)	as NetCost 
	into #vendorCost 
	from 
		#costIDs					cid 
		left join VendorCostHistory vch (nolock) on cid.VendorCostHistoryID		= vch.VendorCostHistoryID 
		left join #promos			pr	(nolock) on cid.StoreItemVendorID		= pr.StoreItemVendorID 

	create nonclustered index idx_vendorCost_StoreItemVendorID on #vendorCost (StoreItemVendorID) include (VendorCostHistoryID, UnitCost, Package_Desc1, NetCost) '

	----------------------------------------------------------------------------------------
    IF (@Identifier <> '') 
    BEGIN
		IF (@Store_No IS NOT NULL)
		BEGIN
			-- Include ItemOverride logic if the store is input to the query
			SELECT @SQL = @SQL + 'SELECT DISTINCT TOP 1001 
											Item.Item_Key, 
											ISNULL(ItemOverride.Item_Description, Item.Item_Description) As Item_Description,
											Identifier, 
											CASE
												WHEN Price.ExceptionSubteam_No IS NOT NULL THEN 
													Price.ExceptionSubteam_No
												ELSE 
													Item.SubTeam_No 
											END AS SubTeam_No, 
											Pre_Order, 
											EXEDistributed, 
											ISNULL(IBO.Brand_Name, IB.Brand_Name) as Brand, 
											vca.NetCost as Cost, 
											ISNULL(ItemOverride.Not_Available, Item.Not_Available) as Not_Available, 
											ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote) as Not_AvailableNote,
											ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) as Package_Desc2,     
											VIS.StatusCode as VendorItemStatus,
											VIS.StatusName as VendorItemStatusFull,
											IUnit.Unit_Abbreviation AS Package_Unit ' 
  
			SELECT @SQL = @SQL + ', IV.Item_ID AS VendorItemID, VCA.Package_Desc1 AS Package_Desc1 '
		END
		ELSE
		BEGIN
			-- Skip ItemOverride logic when the store is not specified
			SELECT @SQL = @SQL + 'SELECT DISTINCT TOP 1001 
									Item.Item_Key, 
									Item.Item_Description As Item_Description,
									Identifier, 
									CASE
										WHEN Price.ExceptionSubteam_No IS NOT NULL 
											THEN Price.ExceptionSubteam_No
										ELSE Item.SubTeam_No 
									END AS SubTeam_No, 
									Pre_Order, 
									EXEDistributed,
									IB.Brand_Name as Brand, 
									vca.NetCost as Cost, 
									Item.Not_Available,    
									Item.Not_AvailableNote,   
									Item.Package_Desc2 As Package_Desc2,  
									VIS.StatusCode as VendorItemStatus,
									VIS.StatusName as VendorItemStatusFull,
									IUnit.Unit_Abbreviation AS Package_Unit '
   
			SELECT @SQL = @SQL + ', IV.Item_ID AS VendorItemID, VCA.Package_Desc1 AS Package_Desc1 '
		END
	END				
    ELSE
    BEGIN
		IF (@Store_No IS NOT NULL)
		BEGIN
			-- Include ItemOverride logic if the store is input to the query
			SELECT @SQL = @SQL + 'SELECT DISTINCT TOP 1001 
									Item.Item_Key, 
									ISNULL(ItemOverride.Item_Description, Item.Item_Description) As Item_Description, 
									Identifier = 
										(SELECT TOP 1 Identifier 
										FROM ItemIdentifier (nolock) 
										WHERE ItemIdentifier.Item_Key = Item.Item_Key 
										ORDER BY Default_Identifier DESC),
									CASE
										WHEN Price.ExceptionSubteam_No IS NOT NULL THEN
											Price.ExceptionSubteam_No
										ELSE 
											Item.SubTeam_No 
									END AS SubTeam_No, 
									Pre_Order, 
									EXEDistributed,
									ISNULL(IBO.Brand_Name, IB.Brand_Name) as Brand, 
									vca.NetCost as Cost, 
									ISNULL(ItemOverride.Not_Available, Item.Not_Available) as Not_Available,   
									ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote) as Not_AvailableNote,  
									ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) As Package_Desc2,     
									VIS.StatusCode as VendorItemStatus,
									VIS.StatusName as VendorItemStatusFull,
									IUnit.Unit_Abbreviation AS Package_Unit '
               
			SELECT @SQL = @SQL + ', IV.Item_ID AS VendorItemID, VCA.Package_Desc1 AS Package_Desc1 '
		END
		ELSE
		BEGIN
			-- Skip ItemOverride logic when the store is not specified
			SELECT @SQL = @SQL + 'SELECT DISTINCT TOP 1001 
									Item.Item_Key, 
									Item.Item_Description As Item_Description, 
									Identifier = 
										(SELECT TOP 1 Identifier 
										FROM ItemIdentifier (nolock) 
										WHERE ItemIdentifier.Item_Key = Item.Item_Key 
										ORDER BY Default_Identifier DESC),
									CASE
										WHEN Price.ExceptionSubteam_No IS NOT NULL THEN
											Price.ExceptionSubteam_No
										ELSE 
											Item.SubTeam_No 
									END AS SubTeam_No, 
									Pre_Order, 
									EXEDistributed,
									IB.Brand_Name as Brand, 
									vca.NetCost as Cost, 
									Item.Not_Available,    
									Item.Not_AvailableNote,   
									Item.Package_Desc2 As Package_Desc2,     
									VIS.StatusCode as VendorItemStatus,
									VIS.StatusName as VendorItemStatusFull,
									IUnit.Unit_Abbreviation AS Package_Unit '
               
			SELECT @SQL = @SQL + ', IV.Item_ID AS VendorItemID, VCA.Package_Desc1 AS Package_Desc1 '
		END
	END				

	IF @OrderType_ID = 1 OR @OrderType_ID = 4 -- Purchase Order or Flowthru Order
		BEGIN
	        SELECT @SQL = @SQL + 'FROM #Items I
								 INNER JOIN Item (NOLOCK) ON I.Item_Key = Item.Item_Key '

	        -- make sure we join the override table to get the correct values by jurisdiction,
	        -- skipping the ItemOverride logic when the store is not specified
	        IF @Store_No IS NOT NULL
	            BEGIN
		            SELECT @SQL = @SQL + ' LEFT JOIN ItemOverride (nolock) ON ItemOverride.Item_Key = Item.Item_Key AND ItemOverride.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store WHERE Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ') '
		            SELECT @SQL = @SQL + ' LEFT JOIN ItemBrand (nolock) IBO ON ItemOverride.Brand_ID = IBO.Brand_ID '
					SELECT @SQL = @SQL + ' LEFT JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key AND Price.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' AND Price.ExceptionSubteam_No IS NOT NULL ' 
		        END
	        ELSE
	            SELECT @SQL = @SQL + ' LEFT JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key AND Price.ExceptionSubteam_No IS NOT NULL '
			
			SELECT @SQL = @SQL + 'INNER JOIN 
										ItemVendor IV (nolock) 
	                                    ON I.Item_Key = IV.Item_Key 
										AND (IV.DeleteDate IS NULL OR IV.DeleteDate > @date) '	
										
			IF (@Vendor <> '') SELECT @SQL = @SQL + ' AND IV.Vendor_ID = ' + CAST(@VendorID AS varchar(20)) + ' '

			IF (@SearchSubTeam_No > 0)
                BEGIN
                    SELECT @SQL = @SQL + 'AND ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No) = ' + CAST(@SearchSubTeam_No as varchar(10)) + ' '
                END	
						
			SELECT @SQL = @SQL + ' LEFT JOIN VendorItemStatuses VIS (nolock) ON IV.VendorItemStatus = VIS.StatusID '
		END

	ELSE IF @OrderType_ID = 2 -- Distribution Order
		BEGIN
			-- Use Item.DistSubTeam_No (ZoneSubTeam) (if exists)
	        SELECT @SQL = @SQL + 'FROM #Items I
								 INNER JOIN Item (NOLOCK) ON I.Item_Key = Item.Item_Key '
	        
			-- make sure we join the override table to get the correct values by jurisdiction,
	        -- skipping the ItemOverride logic when the store is not specified
	        IF @Store_No IS NOT NULL
	            BEGIN
		            SELECT @SQL = @SQL + ' LEFT JOIN ItemOverride (nolock) ON ItemOverride.Item_Key = Item.Item_Key AND ItemOverride.StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store (nolock) WHERE Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ') '
		            SELECT @SQL = @SQL + ' LEFT JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key AND Price.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' AND Price.ExceptionSubteam_No IS NOT NULL ' 
					SELECT @SQL = @SQL + ' LEFT JOIN ItemBrand (nolock) IBO ON ItemOverride.Brand_ID = IBO.Brand_ID '
		        END
	        ELSE
	            SELECT @SQL = @SQL + ' LEFT JOIN Price (nolock) ON Price.Item_Key = Item.Item_Key AND Price.ExceptionSubteam_No IS NOT NULL '
				SELECT @SQL = @SQL + 'INNER JOIN 
										ItemVendor IV (nolock) 
	                                    ON Item.Item_Key = IV.Item_Key 
										AND (IV.DeleteDate IS NULL OR IV.DeleteDate > @date) '

										-- TFS#8673 - limit the results to only the vendor being searched on
										IF (@Vendor <> '') SELECT @SQL = @SQL + ' AND IV.Vendor_ID = ' + CAST(@VendorID AS varchar(20)) + ' '

										IF ((@SearchSubTeam_No > 0) AND (@SearchSubTeam_No <> @FromSubTeam_No))
										BEGIN
										-- Retail subteam
											SELECT @SQL = @SQL + 'AND ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No) = ' + CAST(@SearchSubTeam_No as varchar(10)) + ' '
										END

									SELECT @SQL = @SQL + '	AND ISNULL(Item.DistSubTeam_No, ISNULL(Price.ExceptionSubteam_No, Item.SubTeam_No)) = ' + CAST(@FromSubTeam_No as varchar(10)) + ' '
									SELECT @SQL = @SQL + ' LEFT JOIN VendorItemStatuses VIS (nolock) ON IV.VendorItemStatus = VIS.StatusID '
		END

	SELECT @SQL = @SQL + 'INNER JOIN 
								Vendor (nolock) 
								ON i.Vendor_ID = Vendor.Vendor_ID '

	IF (@Identifier <> '') SELECT @SQL = @SQL + 'INNER JOIN 
													ItemIdentifier (nolock) 
													ON Item.Item_Key = ItemIdentifier.Item_Key '

	SELECT @SQL = @SQL + '
				INNER JOIN ItemBrand IB  (nolock)
				ON	IB.Brand_Id = Item.Brand_Id 
				LEFT JOIN #vendorCost		vca			 on I.StoreItemVendorID = vca.StoreItemVendorID 
				LEFT JOIN ItemUnit IUnit (nolock)
				ON (IUnit.Unit_ID = Item.Package_Unit_ID)
				INNER JOIN StoreItem (nolock) si ON item.Item_Key = si.Item_Key AND ' + CONVERT(VARCHAR(10), ISNULL(@Store_No, 0)) + ' = si.Store_No AND si.Authorized = 1 '

	IF @Vendor = '' or @OrderType_ID = 1
		SELECT @SQL = @SQL + 'WHERE Vendor.CompanyName = ''' + @Vendor + ''' '
	ELSE
		SELECT @SQL = @SQL + 'WHERE Vendor.CompanyName LIKE ''%' + @Vendor + '%'' '

	-- If we've joined with ItemOverride (@Store_No IS NOT NULL), then compare the appropriate parameters with the ItemOverride values.
    IF @Store_No IS NOT NULL
    BEGIN
        IF @WFM_Store = 1 SELECT @SQL = @SQL + 'AND Item.WFM_Item = 1 '	
        IF @HFM_Store = 1 SELECT @SQL = @SQL + 'AND Item.HFM_Item = 1 '
		IF @Not_Available = 0 SELECT @SQL = @SQL + 'AND ISNULL(ItemOverride.Not_Available, Item.Not_Available) = 0 '
    END

    IF (@Category_ID <> 0) SELECT @SQL = @SQL + 'AND Item.Category_ID = ' + CONVERT(VARCHAR(10), @Category_ID) + ' ' 
    IF (@Not_Available = 0 AND @Store_No IS NULL) SELECT @SQL = @SQL + 'AND Item.Not_Available = 0 '
    IF (@Identifier <> '') SELECT @SQL = @SQL + 'AND Identifier LIKE ''' + @Identifier + '%'' '
    IF @Pre_Order IS NOT NULL SELECT @SQL = @SQL + 'AND Item.Pre_Order = ' + CAST(@Pre_Order as char(1)) + ' '
    IF @EXEDistributed IS NOT NULL SELECT @SQL = @SQL + 'AND Item.EXEDistributed = ' + CAST(@EXEDistributed as char(1)) + ' ' 

    IF (@Vendor_ID <> '') 
	    BEGIN
	        IF @Vendor <> '' and @Store_no is not null
	            SELECT @SQL = @SQL + 'AND Item.Item_Key IN (SELECT Item_Key FROM ItemVendor (nolock) WHERE Item_ID like ''%' + @Vendor_ID + '%'' AND Vendor_ID = ' + CAST(@VendorID as varchar(20)) + ')'   
	        ELSE
	            SELECT @SQL = @SQL + 'AND Item_ID like ''%' + @Vendor_ID + '%'' '
	    END

    SELECT @SQL = @SQL + 'AND Item.Deleted_Item = 0 AND Item.Remove_Item = 0 '
	
	SELECT @SQL = @SQL + 'AND iv.DeleteDate is null '

    SELECT @SQL = @SQL + 'ORDER BY Item_Description '
	
	SELECT @SQL = @SQL + 'DROP TABLE #Items '

	EXECUTE(@SQL)
    
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemSearch] TO [IRMAReportsRole]
    AS [dbo];

