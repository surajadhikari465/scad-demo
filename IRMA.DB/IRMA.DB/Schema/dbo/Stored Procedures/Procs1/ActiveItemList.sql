create   PROCEDURE dbo.ActiveItemList
@SubTeam_No int,
@WFM_Item tinyint
AS
--**************************************************************************
-- Procedure: ActiveItemList
--
-- Revision:
-- 01/10/2013  MZ    TFS 8755 - Replace Item.Discontinue_Item with a function call to 
--                   dbo.fn_GetDiscontinueStatus(Item_Key, Store_No, Vendor_Id)
-- 09/18/2013  MZ    TFS 13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
--**************************************************************************
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT 
	Item.Item_Key, 
	ItemIdentifier.Identifier,
	Item.Item_Description, 
	Item.Package_Desc1, 
	Item.Package_Desc2, 
	ItemUnit.Unit_Name, 
	Vendor.CompanyName,
	VIS.StatusCode,
	VIS.StatusName
FROM ItemIdentifier (NOLOCK) INNER JOIN (
       ItemUnit (NOLOCK) RIGHT JOIN (
         Item (NOLOCK) LEFT JOIN (
           OrderHeader (NOLOCK) INNER JOIN Vendor (NOLOCK) ON (OrderHeader.Vendor_ID = Vendor.Vendor_ID) INNER JOIN (
             SELECT Item.Item_Key, MAX(OrderHeader.OrderHeader_ID) AS OrderHeader_ID
             FROM OrderHeader (NOLOCK) INNER JOIN (
                    OrderItem (NOLOCK) INNER JOIN Item (NOLOCK) ON (OrderItem.Item_Key = Item.Item_Key)
                  ) ON (OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID)
             WHERE 
				OrderHeader.OrderType_ID = 1	-- Purchase orders 
				AND Item.SubTeam_No = @SubTeam_No 
				AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, OrderHeader.Vendor_ID) = 0
             GROUP BY Item.Item_Key
           ) T1 ON (OrderHeader.OrderHeader_ID = T1.OrderHeader_ID)
         ) ON (Item.Item_Key = T1.Item_Key) 
       ) ON (ItemUnit.Unit_ID = Item.Package_Unit_ID)
     ) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
	 INNER JOIN ItemVendor IV on IV.Item_Key= Item.Item_Key and iv.Vendor_ID = vendor.Vendor_ID
	 LEFT  JOIN VendorItemStatuses VIS ON VIS.StatusID = IV.VendorItemStatus
WHERE 
	Item.SubTeam_No = @SubTeam_No 
	AND CAST(Item.WFM_Item AS tinyint) >= @WFM_Item 
	AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, IV.Vendor_ID) = 0 
	AND Retail_Sale = 1
    AND Item.Deleted_Item = 0
	AND VIS.StatusCode in ('A','N','S', null) -- include Active, NotAvailable, and Seasonal (and null) values.
	AND VIS.StatusCode not in ('V','M')		  -- exclude Vendor Disco and MFG Disco
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ActiveItemList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ActiveItemList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ActiveItemList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ActiveItemList] TO [IRMAReportsRole]
    AS [dbo];

