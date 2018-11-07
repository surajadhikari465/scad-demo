CREATE PROCEDURE [dbo].[GetVendorItemsInStores_All] 
	-- Add the parameters for the stored procedure here
	@Vendor_ID	int
AS
   -- **************************************************************************
   -- Procedure: GetVendorItemsInStores_All()
   --    Author: Hussain Hashim
   --      Date: 8/02/2007
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures. Gets all items in all Stores for a Vendor
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 08/11/2009  BBB	Added calls to ItemOverride for Item_Size.Unit_Abbreviation
   -- **************************************************************************
BEGIN

SELECT     
	VendorCost.Item_Key
	, dbo.ItemVendor.Item_ID
	, VendorCost.Store_No
	, dbo.Store.Store_Name
	, VendorCost.Vendor_ID
	, VendorCost.UnitCost AS CaseCost
	, CONVERT(FLOAT, VendorCost.UnitCost / VendorCost.Package_Desc1) AS UnitCost
	, CONVERT(FLOAT, dbo.fn_GetCurrentCost(VendorCost.Item_Key, VendorCost.Store_No) / VendorCost.Package_Desc1) AS CurrentUnitCost
	, CONVERT(FLOAT, dbo.fn_GetCurrentNetCost(VendorCost.Item_Key, VendorCost.Store_No) / VendorCost.Package_Desc1) AS CurrentNetCost
	, CONVERT(FLOAT, VendorCost.Package_Desc1) AS CaseSize
	, VendorCost.Package_Desc1 AS TotalInnerPacks
	, dbo.fn_ItemOverride_Package_Desc2(VendorCost.Item_Key,dbo.Price.Store_No) AS InnerPackSize
	, CONVERT	(Varchar(10), 
			CONVERT	(numeric(6, 2), 
					dbo.fn_ItemOverride_Package_Desc2(VendorCost.Item_Key,dbo.Price.Store_No)
					)
				)  
		+ ' ' + ISNULL(iu2.Unit_Abbreviation, ItemUnit.Unit_Abbreviation) AS Item_Size
	, dbo.Price.Price, dbo.Price.Multiple, dbo.Price.POSPrice
	, dbo.fn_GetCurrentMarginPercent(dbo.Price.POSPrice, dbo.Price.Multiple, VendorCost.UnitCost, VendorCost.Package_Desc1) AS CurrentMarginPercent
	, dbo.fn_GetCurrentMarkupPercent(dbo.Price.POSPrice, dbo.Price.Multiple, VendorCost.UnitCost, VendorCost.Package_Desc1) AS CurrentMarkupPercent
	, dbo.ItemIdentifier.Identifier
	, dbo.fn_ItemOverride_ItemDescription(VendorCost.Item_Key,dbo.Price.Store_No) AS Item_Description
	, dbo.SubTeam.SubTeam_Name
	, dbo.Item.SubTeam_No
FROM 
	dbo.fn_VendorCostItemsStores(@Vendor_ID, GETDATE()) AS VendorCost 
	INNER JOIN dbo.ItemVendor
		ON VendorCost.Item_Key = dbo.ItemVendor.Item_Key AND
		VendorCost.Vendor_ID = dbo.ItemVendor.Vendor_ID
	INNER JOIN dbo.Item 
		ON VendorCost.Item_Key = dbo.Item.Item_Key 
	INNER JOIN dbo.ItemIdentifier
		ON dbo.Item.Item_Key = dbo.ItemIdentifier.Item_Key
	INNER JOIN dbo.SubTeam 
		ON dbo.Item.DistSubTeam_No = dbo.SubTeam.SubTeam_No AND 
		dbo.Item.SubTeam_No = dbo.SubTeam.SubTeam_No 
	INNER JOIN dbo.ItemUnit 
		ON dbo.Item.Package_Unit_ID = dbo.ItemUnit.Unit_ID 
	INNER JOIN dbo.Price 
		ON VendorCost.Item_Key = dbo.Price.Item_Key AND VendorCost.Store_No = dbo.Price.Store_No 
	INNER JOIN dbo.Store 
		ON dbo.Price.Store_No = dbo.Store.Store_No
	LEFT JOIN dbo.ItemOverride iov (nolock)
					 on Item.Item_Key = iov.Item_Key AND iov.StoreJurisdictionID = Store.StoreJurisdictionID
	LEFT JOIN dbo.ItemUnit iu2 ON iov.Package_Unit_ID = iu2.Unit_ID
		
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItemsInStores_All] TO [IRMAReportsRole]
    AS [dbo];

