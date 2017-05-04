SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
if not exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTransferOrderItemSearch]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
begin
	EXEC ('CREATE PROCEDURE [dbo].[GetTransferOrderItemSearch] AS SELECT 1')
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

ALTER PROCEDURE [dbo].[GetTransferOrderItemSearch] 
	@FromSubTeam_No		int,
	@ToSubTeam_No		int,
	@SearchSubTeam_No	int,
	@DistSubTeam_No		int,
	@Category_ID		int,
	@Vendor				varchar(50),
	@Store_No			int,
	@Item_Description	varchar(60),
	@Identifier			varchar(13),
	@Discontinue_Item	int,
	@Not_Available		int,
	@Brand_ID			int,
    @Pre_Order			bit,
    @EXEDistributed		bit,
    @ProductType_ID		int,
    @VIN				varchar(20) = null 
 AS

/*********************************************************************************************
CHANGE LOG
DEV		DATE			TASK	Description
----------------------------------------------------------------------------------------------
MZ      02/11/13        9948    
MZ		05/23/13		9684    Modified the Vendor checking code in the end to find an authorized 
								vendor. Since currently an item can be transferred between any stores 
                                so long as the vendor/item is actively authorized to one store, this 
								piece of code may need to be modified again for enhancement.
BL		10/17/2013		14151 	Pre-select item keys to improve performance, added nolock hints, 
								reordered joins. Use Intersect to filter through Vendor and Identifier
								and ItemDescription search criteria. Look at discontinueitem
								directly from storeitemvendor as opposed to the function call.
BL		2013/10/13		14320	Changed the vendor query to check for exact match before partial
FA	    12/5/2013       14528   Added parameter VIN
***********************************************************************************************/

BEGIN
    SET NOCOUNT ON
	DECLARE
		@WFM_Store				BIT
		,@HFM_Store				BIT
		,@VendorID				INT
		,@StoreJurisdictionID	INT
		,@date					SMALLDATETIME

	SELECT @date = GETDATE()
	SELECT @StoreJurisdictionID = (SELECT StoreJurisdictionID FROM Store (nolock) WHERE Store_No = @Store_No)

	----------------------------------------------------------------------
	-- Only one record should be returned from the followong SELECT 
	-- statement, because the Client code calls fn_IsMultipleMatchedVendors
	-- first to make sure only one vendor will be returned from the vendor
	-- name wild search. fn_IsMultipleMatchedVendors was modified to allow
	-- for exact match check. so performa an exact match and only if no
	-- vendor ID is found then proceed to do a partial match.
	SELECT	@VendorID = Vendor_ID FROM Vendor WHERE CompanyName = @Vendor
	IF @Vendor <> '' AND @VendorID IS NULL
		SELECT	@VendorID = Vendor_ID FROM Vendor WHERE CompanyName LIKE '%' + @Vendor + '%'
	
	IF @Store_No IS NOT NULL
		SELECT @WFM_Store = WFM_Store, @HFM_Store = Mega_Store FROM Store (nolock) WHERE Store_No = @Store_No
		
	DECLARE @SubTeamList TABLE (SubTeam_No INT PRIMARY KEY) 		
	
	IF @ProductType_ID = 1 			
		INSERT @SubTeamList 
		SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID IN (1,2,3,4) 
	IF @ProductType_ID = 2 
		INSERT @SubTeamList
		SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 5 
	IF @ProductType_ID = 3 
		INSERT @SubTeamList
		SELECT SubTeam_No FROM SubTeam (nolock) WHERE SubTeamType_ID = 6 
	
	IF (@SearchSubTeam_No <> 0)
		DELETE FROM @SubTeamList
		WHERE SubTeam_No <> @SearchSubTeam_No

	----------------------------------------------------------------------
	-- Use an intersection of queries using the three filter criteria on 
	-- Vendor, Identifier and Item Description, filtered by subteam
	CREATE TABLE #Items (Item_Key INT PRIMARY KEY)

	INSERT INTO #Items (Item_Key)

	SELECT I.Item_Key
	FROM Item I (NOLOCK)
	INNER JOIN @SubTeamList S ON I.SubTeam_No =  S.SubTeam_No
	JOIN 
		(SELECT Item.Item_Key
		FROM Item (NOLOCK)	
		LEFT JOIN ItemOverride (NOLOCK) ON ItemOverride.Item_Key = Item.Item_Key
		WHERE (@Item_Description = ''
		OR ItemOverride.Item_Description LIKE '%' + @Item_Description + '%'
		OR Item.Item_Description LIKE '%' + @Item_Description + '%') 
		
		INTERSECT
	
		SELECT I.Item_Key
		FROM Item I (NOLOCK)
		LEFT JOIN ItemOverride (NOLOCK) ON ItemOverride.Item_Key = I.Item_Key
		WHERE @Brand_ID IS NULL 
			OR ItemOverride.Brand_ID = @Brand_ID
			OR I.Brand_ID = @Brand_ID
	
		INTERSECT
			
		SELECT II.Item_Key
		FROM ItemIdentifier II (NOLOCK)
		WHERE (@Identifier = '' OR II.Identifier LIKE @Identifier + '%')
		GROUP BY II.Item_Key 

		INTERSECT 
		
		SELECT IV.Item_Key
		FROM ItemVendor IV (NOLOCK)
		WHERE ((@Vendor = '' AND @VendorID IS NULL) OR IV.Vendor_ID = @VendorID)
		GROUP BY IV.Item_Key) AS itemIntersect ON itemIntersect.Item_Key = I.Item_Key

	----------------------------------------------------------------------
	-- Limit Item_Keys to valid Vendor_IDs, wether defined in the 
	-- parameters or through picking a primary vendor of the item from a 
	-- store where the item is authorized.
    -- 
	-- The following comment is from the original GetOrderItemSearch query...
	/* /* 
	For transfer orders, we want to include all vendors and all stores when 
	retrieving the item list, but we do not want to show duplicates in the 
	result set when, for example, there are two different vendors that supply
	the item.
	So, we use a subquery, keying off each item in the result set, which finds 
	the first store-item-vendor entry (again, we don't care what vendor or store) 
	where the item is authorized for the store.
	We use this single vendor ID for the results to ensure we only get one row 
	per item.
	Bug 2983: Fixed the issues introduced by bug fix for 2384, limited the logic 
	to Transfer Orders only with appropriate conditions.
	*/ */

	CREATE TABLE #itemVendorTemp (Item_Key INT NOT NULL,
							  Vendor_ID INT NULL,
							  Identifier VARCHAR(13) NULL)

	INSERT INTO #itemVendorTemp (Item_Key, Vendor_ID)
	SELECT
		i.Item_Key,
		MIN(SIV.Vendor_ID) AS Vendor_ID
	FROM #Items i
	INNER JOIN StoreItem SI (NOLOCK)		ON SI.Item_Key = I.Item_Key
											AND (@Store_No IS NULL OR @Store_No = 0 OR SI.Store_No = @Store_No)
											AND si.Authorized = 1
	INNER JOIN StoreItemVendor SIV (NOLOCK) ON SI.Item_Key = SIV.Item_Key 
											AND	SIV.Store_No = SI.Store_No
											AND siv.PrimaryVendor = 1
											AND (@VendorID IS NULL OR SIV.Vendor_ID = @VendorID)
											AND (SIV.DeleteDate IS NULL OR SIV.DeleteDate > @date)
											AND (@Discontinue_Item = 1 OR SIV.DiscontinueItem = 0)
	GROUP BY i.Item_Key
	
	CREATE NONCLUSTERED INDEX idx_itemVendorTemp
	ON #itemVendorTemp (item_Key)
	INCLUDE (vendor_id)
	

	----------------------------------------------------------------------
	-- Update the list of items with the appropriate identifier 
	IF (@Identifier = '')
	BEGIN
		UPDATE #itemVendorTemp
		SET Identifier = II.Identifier
		FROM #itemVendorTemp ivt
		INNER JOIN ItemIdentifier II (NOLOCK) ON IVT.Item_Key = II.Item_Key 
		WHERE II.Default_Identifier = 1
	END
	ELSE
	BEGIN
		UPDATE #itemVendorTemp
		SET Identifier = II.Identifier
		FROM #itemVendorTemp ivt
		INNER JOIN ItemIdentifier II (NOLOCK) ON IVT.Item_Key = II.Item_Key 
		WHERE II.Identifier like @Identifier + '%'
											
	END	

	----------------------------------------------------------------------
	-- Return the results
	SELECT DISTINCT TOP 1001 
		items.Item_Key,	
		ISNULL(ItemOverride.Item_Description, Item.Item_Description) AS Item_Description, 
		items.Identifier,			   
		Item.SubTeam_No AS SubTeam_No,
		Pre_Order, 
		EXEDistributed,
		ISNULL(IBO.Brand_Name, IB.Brand_Name) AS Brand,
		CASE WHEN @Store_No IS NULL OR @Store_No = 0 THEN NULL ELSE dbo.fn_GetCurrentNetCost(Item.Item_Key, @Store_No) END AS Cost,
		ISNULL(ItemOverride.Not_Available, Item.Not_Available) AS Not_Available,    
		ISNULL(ItemOverride.Not_AvailableNote, Item.Not_AvailableNote) AS Not_AvailableNote,   
		ISNULL(ItemOverride.Package_Desc2, Item.Package_Desc2) As Package_Desc2,
		VIS.StatusCode AS VendorItemStatus,
		VIS.StatusName AS VendorItemStatusFull,
		IUnit.Unit_Abbreviation AS Package_Unit,
		NULL AS VendorItemID,
		NULL AS Package_Desc1
	FROM #itemVendorTemp items (NOLOCK)
	INNER JOIN ItemVendor IV (NOLOCK)				ON Items.Item_Key = IV.Item_Key
													AND items.Vendor_ID = IV.Vendor_ID
													AND	IV.Item_ID like '%' + ISNULL(@VIN, IV.Item_ID) + '%'
	INNER JOIN Item (NOLOCK)						ON Item.Item_Key = IV.Item_Key
													  AND Item.Deleted_Item = 0
													  AND Item.Remove_Item = 0 
	INNER JOIN ItemIdentifier (NOLOCK)				ON ItemIdentifier.Item_Key = Item.Item_Key 
	INNER JOIN ItemBrand IB (NOLOCK)				ON IB.Brand_Id = Item.Brand_Id 
	 LEFT JOIN ItemOverride (NOLOCK)				ON ItemOverride.Item_Key = Item.Item_Key 
													AND ItemOverride.StoreJurisdictionID = @StoreJurisdictionID 
     LEFT JOIN ItemBrand IBO (NOLOCK)				ON ItemOverride.Brand_ID = IBO.Brand_ID  
	 LEFT JOIN VendorItemStatuses VIS (NOLOCK)		ON IV.VendorItemStatus = VIS.StatusID 
	 LEFT JOIN ItemUnit IUnit (NOLOCK)				ON IUnit.Unit_ID = Item.Package_Unit_ID

	WHERE (@WFM_Store IS NULL OR @WFM_Store <> 1 OR Item.WFM_Item = @WFM_Store)
	  AND (@HFM_Store IS NULL OR @HFM_Store <> 1 OR Item.HFM_Item = @HFM_Store)
	  AND (@Category_ID = 0 OR Item.Category_ID = @Category_ID)
	  AND (@Pre_Order IS NULL OR Item.Pre_Order = @Pre_Order)
	  AND (@EXEDistributed IS NULL OR Item.EXEDistributed = @EXEDistributed)
      AND (@Not_Available = 1 OR ISNULL(ItemOverride.Not_Available, Item.Not_Available) = 0)
      AND (@Brand_ID IS NULL OR ISNULL(ItemOverride.Brand_ID, Item.Brand_ID) = @Brand_ID)
	ORDER BY Item_Description

	DROP TABLE #Items, #itemVendorTemp
  
	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
