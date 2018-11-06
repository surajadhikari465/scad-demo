/****** Object:  Stored Procedure dbo.msqGetBakeryDistributionInventoryItems    Script Date: 9/19/2005 1:44:37 PM ******/
CREATE PROCEDURE dbo.msqGetBakeryDistributionInventoryItems @StoreNo int, @VendorID int, @SubTeamNo int

AS

DECLARE @CurrDate smalldatetime
SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))

  
    -- Get the Item Unit ID's so we can call CostConverion
    DECLARE @Unit int, @Case int, @Box int, @Shipper int, @Pound int
    
    SELECT @Unit = Unit_ID FROM ItemUnit WHERE UnitSysCode = 'unit'    
    SELECT @Case = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Case'
    SELECT @Box = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Box'
    SELECT @Shipper = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Shipper'
    SELECT @Pound = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Pound'

--Get the last cost paid   
DECLARE @CD TABLE (Item_Key int, VendorCost money)

INSERT INTO @CD (Item_Key, VendorCost)

SELECT I.Item_Key, NULL
FROM ItemVendor IV(nolock)
JOIN Item I(nolock) ON IV.Item_Key = I.Item_Key
JOIN ItemUnit IU(nolock) ON I.Package_Unit_ID = IU.Unit_ID
WHERE IV.Vendor_ID = @VendorID AND ISNULL(I.DistSubTeam_No, I.SubTeam_No) = @SubTeamNo
AND Deleted_Item = 0 AND dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 0

/*
-- Not using last cost because all stores have the same cost. Keeping it just in case they change their mind.

UPDATE @CD
    SET LastCost = (SELECT SUM(ReceivedItemCost + ReceivedItemFreight) / SUM(UnitsReceived)
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
                              WHERE VendStore.Store_No = @StoreNo
                                  AND Return_Order = 0
                                  AND (Item_Key = CD.Item_Key AND DateReceived <= GETDATE())
                                  AND ReceivedItemCost > 0
                                  AND UnitsReceived > 0
                              ORDER BY DateReceived DESC) 
                            AND Item_key = CD.Item_Key)
    FROM @CD CD
*/

--Use vendor cost as all stores have the same cost
UPDATE @CD
    SET VendorCost = (SELECT TOP 1 CASE WHEN UnitCost > 0 THEN UnitCost ELSE NULL END + ISNULL(UnitFreight, 0)
                       FROM dbo.fn_VendorCostAll(@CurrDate) VC
											 --JOIN Vendor V ON V.Vendor_ID = VC.Vendor_ID
                       WHERE VC.Item_Key = CD.Item_Key
											 AND VC.Vendor_ID = @VendorID
                       AND VC.Store_No = @StoreNo)
    FROM @CD CD
    
--Get the inventory list data
SELECT Item_ID AS 'Ripe Order Number', Item_Description AS 'Description', 
			 CONVERT(VARCHAR(5),CONVERT(int, Package_Desc1)) + '/' + CONVERT(VARCHAR(5), CONVERT(int, Package_Desc2))  + ' ' + Unit_Name AS 'Pack',' ' AS ' ', 
			 VendorCost * Package_Desc1 AS 'Cost'
FROM ItemVendor IV(nolock)
JOIN Item I(nolock) ON IV.Item_Key = I.Item_Key
JOIN @CD CD ON CD.Item_Key = I.Item_Key
JOIN ItemUnit IU(nolock) ON I.Package_Unit_ID = IU.Unit_ID
WHERE IV.Vendor_ID = @VendorID AND ISNULL(I.DistSubTeam_No, I.SubTeam_No) = @SubTeamNo
AND Deleted_Item = 0 AND dbo.fn_GetDiscontinueStatus(I.Item_Key, NULL, NULL) = 0
ORDER BY Item_Description