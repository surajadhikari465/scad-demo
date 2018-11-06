CREATE PROCEDURE [dbo].[GetOrderItemQueueSearch]
	@OrderType_ID			int,
	@VendorID				int,
	@PurchasingVendor_ID	int,
	@FromSubTeam_No			int, 
	@ToSubTeam_No			int,
	@SearchSubTeam_No		int,
	@ProductType_ID			int, 
	@IsFacility				bit, 
	@IsVendorStoreSame		bit, 
	@Discontinue_Item		int, 
    @Credit					bit 

AS 

/*********************************************************************************************
CHANGE LOG
DEV				DATE					TASK				Description
----------------------------------------------------------------------------------------------
DBS				20110211				13748				Add Discontinue Item for grid
TTL				20110317				1584				Changed from using vendor name to using vendor ID, so we get an exact match and
															do not inadvertently pull in extra vendors that match on vendor name but differ by a prefix or suffix in their name.
															Change @Vendor param to @VendorID and updated screen to pass vendor ID, not name.
BAS				01/14/2013				8755				Updated Item.Discontinue_Item to dbo.fn_GetDiscontinueStatus to account for schema change
KM				2013-02-12				9204				Update references to GetDiscontinueItem so that it properly calls GetDiscontinueStatus instead;
***********************************************************************************************/

BEGIN
	SET NOCOUNT ON
	

	DECLARE @SQL					varchar(2500)
	DECLARE @IsSubTeamRestricted	bit
	DECLARE @Store_No				int
	DECLARE @Transfer				bit
	
	DECLARE @PackagingSubteam int = (SELECT TOP 1 SubTeam_NO FROM SubTeam where SubTeam_Name = 'Packaging')

	IF @PurchasingVendor_ID > 0 BEGIN SELECT @Store_No = Store_No FROM Vendor WHERE Vendor_ID = @PurchasingVendor_ID END
	IF @OrderType_ID = 3 SET @Transfer = 1 ELSE SET @Transfer = 0

	SELECT @SQL = 'SELECT DISTINCT TOP 1001 
						  Q.OrderItemQueue_ID 
						, Item.Item_Key 
						, Item_Description 
						, Identifier = 
							(SELECT TOP 1 Identifier 
			            	FROM ItemIdentifier (nolock) 
			              	WHERE ItemIdentifier.Item_Key = Item.Item_Key 
			              	ORDER BY Default_Identifier DESC) 
						, Item.SubTeam_No 
						, S.SubTeam_Name AS SubTeamName
						, CASE WHEN Item.Not_Available = 1 THEN ''NA'' ELSE '''' END as NotAvailable 
						, Q.Quantity 
						, Q.Unit_ID 
						, IU.Unit_Name AS QuantityUnitName
						, U.UserName 
						, ISNULL(Q.CreditReason_ID, 0) as CreditReason_ID 
						, ISNULL(CR.CreditReason, ''Enter Reason'') as CreditReason 
						, CONVERT(varchar(255), Q.Insert_Date, 121) As Insert_Date
						, dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) As Discontinue_Item
						, Item.Package_Desc2
						, Item.Package_Unit_ID '

    IF @OrderType_ID <> 3 -- Not transfer
		SELECT @SQL = @SQL + ', CASE WHEN(SIV.PrimaryVendor = 1) THEN ''*'' ELSE '''' END as PrimaryVendor '
	ELSE
		-- Transfers don't have SIV records
		SELECT @SQL = @SQL + ', '''' as PrimaryVendor '
	
	-- Initiate the FROM clause for all cases
	SELECT @SQL = @SQL + 'FROM Item (nolock) 
				INNER JOIN OrderItemQueue Q (nolock) 
				    ON Item.Item_Key = Q.Item_Key 
					AND Q.Transfer = ' + CAST(@Transfer as varchar(10)) + ' 
					AND Q.Credit = ' + CAST(@Credit as varchar(10)) + ' 
					AND Q.TransferToSubTeam_No = ' + CAST(@ToSubTeam_No as varchar(10)) + ' 
				INNER JOIN ItemUnit IU (nolock)
				    ON Q.Unit_ID = IU.Unit_ID 
				INNER JOIN SubTeam S (nolock)
				    ON Item.SubTeam_No = S.SubTeam_No 
				INNER JOIN Users U (nolock)
				    ON Q.User_ID = U.User_ID 
				LEFT OUTER JOIN CreditReasons CR (nolock) 
				    ON Q.CreditReason_ID = CR.CreditReason_ID '

	IF @OrderType_ID = 1 -- Purchase Order
		BEGIN
	        SELECT @SQL = @SQL + 'INNER JOIN ItemVendor IV (nolock) 
	                                    ON Item.Item_Key = IV.Item_Key '									
			IF (@SearchSubTeam_No > 0)
				BEGIN
					SELECT @SQL = @SQL + 'AND Item.SubTeam_No = ' + CAST(@SearchSubTeam_No as varchar(10)) + ' '
				END			
		END
	ELSE IF @OrderType_ID = 2 -- Distribution Order
		BEGIN
		-- Use Item.DistSubTeam_No (ZoneSubTeam) (if exists)
	        SELECT @SQL = @SQL + 'INNER JOIN ItemVendor IV (nolock) 
	                                    ON Item.Item_Key = IV.Item_Key 
					AND ISNULL(Item.DistSubTeam_No, Item.SubTeam_No) = ' + CAST(@FromSubTeam_No as varchar(10)) + ' '
			
            IF ((@SearchSubTeam_No > 0) AND (@SearchSubTeam_No <> @FromSubTeam_No))
				BEGIN
					-- Retail subteam
					SELECT @SQL = @SQL + 'AND Item.SubTeam_No = ' + CAST(@SearchSubTeam_No as varchar(10)) + ' '
				END
									
		END

	-- Purchase and Distribution orders always have vendor and store
    IF (@OrderType_ID <> 3)
	    BEGIN
	        SELECT @SQL = @SQL + 'INNER JOIN 
	                                    StoreItemVendor SIV (nolock) 
	                                    ON Item.Item_Key = SIV.Item_Key 
	                                    AND SIV.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' 
                                        AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > GETDATE())
	                                INNER JOIN 
	                                    Vendor (nolock) 
	                                    ON SIV.Vendor_ID = Vendor.Vendor_ID '
	        SELECT @SQL = @SQL + 'WHERE Q.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ' AND Vendor.Vendor_ID = ' + cast(@VendorID as varchar) + ' '
	    END 
	ELSE  	-- @OrderType_ID = 3  Transfer order
		BEGIN
	        
			SELECT @SQL = @SQL + 'WHERE Q.Store_No = ' + CONVERT(VARCHAR(10), @Store_No) 
	
			IF ((@IsFacility = 1) AND (@IsVendorStoreSame = 0) AND (ISNULL(@Store_No, 0) > 0))
				-- Limit the items to those not distributed by this supplier's zonesubteams
				BEGIN
					SELECT @SQL = @SQL + 'AND (ISNULL(Item.DistSubTeam_No, Item.SubTeam_No) NOT IN  
										(SELECT ZoneSubTeam.SubTeam_No 
										FROM ZoneSubTeam 
										WHERE ZoneSubTeam.Supplier_Store_No = ' + CONVERT(VARCHAR(10), @Store_No) + ')) '
				END

		    IF (@SearchSubTeam_No <> 0) 
				BEGIN
					SELECT @SQL = @SQL + 'AND Item.SubTeam_No = ' + CONVERT(VARCHAR(10), @SearchSubTeam_No) + ' '	
				END
		END
    
	IF(@ProductType_ID = 2)
	BEGIN
		SELECT @SQL = @SQL + 'AND Item.SubTeam_No = ' +  CAST(@PackagingSubteam as varchar(10))  + ' '
	END	
	ELSE 
	BEGIN
		SELECT @SQL = @SQL + 'AND Item.SubTeam_No NOT IN (' +  CAST(@PackagingSubteam as varchar(10)) +') ' 
	END

	IF (@Discontinue_Item = 0) SELECT @SQL = @SQL + 'AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0 '
    SELECT @SQL = @SQL + 'AND Item.Deleted_Item = 0 AND Item.Remove_Item = 0 '

    SELECT @SQL = @SQL + 'ORDER BY Item_Description'

	--**********************************************************
	-- Insert @SQL into temp table
	--**********************************************************
	DECLARE @CostDate smalldatetime
	SELECT @CostDate = GETDATE() + dbo.fn_GetLeadTimeDays(@VendorID);

	CREATE TABLE #items 
	(
		OrderItemQueue_ID int,
		Item_Key int,
		Item_Description varchar(60), 
		Identifier varchar(13),
		SubTeam_No int,
		SubTeamName varchar(100),
		NotAvailable varchar(2), 
		Quantity decimal(18,4), 
		Unit_ID int, 
		QuantityUnitName varchar(25),
		UserName varchar(25),
		CreditReason_ID int,
		CreditReason varchar(25),
		Insert_Date datetime,
		Discontinue_Item bit,
		Package_Desc2 decimal(9,4),
		Package_Unit_ID int,
		PrimaryVendor varchar(1)
	)

	INSERT INTO #items EXEC(@SQL)

	--**********************************************************
	-- Main Query - adding 'Cost' column
	--**********************************************************
	-- For non-transfer orders - we have a vendor and store
	IF @OrderType_ID <> 3
	BEGIN
		SELECT
			i.OrderItemQueue_ID,
			i.Item_Key,
			i.Item_Description,
			i.Identifier,
			i.SubTeam_No,
			i.SubTeamName,
			i.NotAvailable,
			i.Quantity,
			i.Unit_ID,
			i.QuantityUnitName,
			i.UserName,
			i.CreditReason_ID,
			i.CreditReason,
			i.Insert_Date,
			i.Discontinue_Item,
			i.PrimaryVendor,
			(vca.NetCost * i.Quantity) as Cost
		FROM
			#items i
			INNER JOIN dbo.fn_VendorCostAll(@CostDate)	vca on	i.Item_Key = vca.Item_Key
																AND vca.Store_No = @Store_No
																AND vca.Vendor_ID = @VendorID
	END
	ELSE -- transfer orders leave cost out
	BEGIN
		SELECT
			i.OrderItemQueue_ID,
			i.Item_Key,
			i.Item_Description,
			i.Identifier,
			i.SubTeam_No,
			i.SubTeamName,
			i.NotAvailable,
			i.Quantity,
			i.Unit_ID,
			i.QuantityUnitName,
			i.UserName,
			i.CreditReason_ID,
			i.CreditReason,
			i.Insert_Date,
			i.Discontinue_Item,
			i.PrimaryVendor,
			0 as Cost
		FROM
			#items i
	END
    
    SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueSearch] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueSearch] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueSearch] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderItemQueueSearch] TO [IRMAReportsRole]
    AS [dbo];