CREATE PROCEDURE dbo.msqBakeryInventoryCobb

AS

BEGIN
    SET NOCOUNT ON

DECLARE @ItemKey table (Item_Key int, Cost smallmoney)

INSERT INTO @ItemKey

SELECT Item_Key, Null
FROM Item
WHERE Deleted_Item = 0
AND SubTeam_No = 4200


-- Get the last ordered cost 
    UPDATE @ItemKey
    SET Cost = (SELECT SUM(ReceivedItemCost + ReceivedItemFreight) / SUM(UnitsReceived)
                       FROM OrderItem (nolock)
                       WHERE OrderHeader_ID = 
                             (SELECT TOP 1 OrderHeader.OrderHeader_ID
                              FROM OrderHeader (nolock)
                              INNER JOIN
                                  Vendor VendStore (nolock)
                                  ON VendStore.Vendor_ID = OrderHeader.ReceiveLocation_ID
                              INNER JOIN
                                  OrderItem (nolock)
                                  ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
                              WHERE VendStore.Store_No = 108
                                  AND Return_Order = 0
                                  AND (Item_Key = IK.Item_Key AND DateReceived < getdate())
                                  AND ReceivedItemCost > 0
                                  AND UnitsReceived > 0
                              ORDER BY DateReceived DESC) 
                            AND Item_key = IK.Item_Key)
    FROM @ItemKey IK
    WHERE Cost IS NULL

--
SELECT CompanyName, Item_Description , Package_Desc1, Package_Desc2, Unit_Name, ISNULL(Cost,0) AS 'LastCost'       
FROM dbo.ItemVendor IV(nolock)      	    
INNER JOIN dbo.Item(nolock) ON IV.Item_Key = Item.Item_Key      	    
INNER JOIN dbo.ItemIdentifier IID(nolock) ON Item.Item_Key = IID.Item_key      	    
INNER JOIN dbo.Vendor(nolock) ON IV.Vendor_ID = Vendor.Vendor_ID      	    
INNER JOIN dbo.Price(nolock) ON Item.Item_Key = Price.Item_Key      	    
JOIN @ItemKey IK ON IK.Item_Key = Item.Item_Key
LEFT JOIN dbo.ItemUnit IU(nolock) ON Item.Package_Unit_ID = IU.Unit_ID          
WHERE Vendor.Vendor_ID IN (5319,6422,6354,3654,5004,5139,5572,203,5596,5116,390,5281,6352,6427,586,5058,5793,6240)  
AND SubTeam_No = 4200 AND Price.Store_No = 108  	      
AND Deleted_Item = 0 AND Deleted_identifier = 0    
AND Default_Identifier = 1  --AND Retail_Sale = 1      
ORDER BY  CompanyName, Item_Description      

    SET NOCOUNT OFF
END