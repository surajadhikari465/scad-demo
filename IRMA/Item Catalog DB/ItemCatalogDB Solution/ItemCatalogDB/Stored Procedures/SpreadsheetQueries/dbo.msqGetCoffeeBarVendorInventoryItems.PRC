SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[msqGetCoffeeBarVendorInventoryItems]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[msqGetCoffeeBarVendorInventoryItems]
GO

CREATE PROCEDURE dbo.msqGetCoffeeBarVendorInventoryItems @StoreNo int

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

DECLARE @Vendor TABLE (VendorID int)
INSERT INTO @Vendor
   
SELECT DISTINCT V.Vendor_ID
FROM OrderInvoice OIN(nolock)
	JOIN OrderHeader OH( nolock) ON OIN.OrderHeader_ID = OH.OrderHeader_ID
	JOIN Vendor V( nolock) ON V.Vendor_ID = OH.Vendor_ID
	JOIN Vendor C(nolock) ON C.Vendor_ID = OH.Receivelocation_ID
WHERE OIN.SubTeam_No = OH.Transfer_To_SubTeam 
AND OH.Transfer_To_SubTeam = 4800
	AND C.Store_No = @StoreNo
	AND OH.CloseDate >= GETDATE() - 60




DECLARE @CD TABLE (Item_Key int, LastCost money)

INSERT INTO @CD (Item_Key, LastCost)

SELECT DISTINCT I.Item_Key, NULL
FROM ItemVendor IV
	JOIN ItemIdentifier II ON IV.Item_Key = II.Item_Key
	JOIN Item I ON I.Item_Key = II.Item_Key
	JOIN @Vendor V on V.Vendorid = IV.Vendor_ID
WHERE (Deleted_Item = 0 or Remove_Item = 0)
	AND Deleted_Identifier = 0 AND Default_Identifier = 1
	AND I.SubTeam_NO = 4800  


UPDATE @CD
    SET  LastCost = (SELECT SUM(ReceivedItemCost + ReceivedItemFreight) / SUM(UnitsReceived)
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
                                  AND (Item_Key = CD.Item_Key AND DateReceived >= GETDATE()- 60)
                                  AND ReceivedItemCost > 0
                                  AND UnitsReceived > 0
                              ORDER BY DateReceived DESC) 
                            AND Item_key = CD.Item_Key)
FROM @CD CD
  


SELECT  II.Identifier, I.Item_Description, Vendor.CompanyName AS 'Vendor' , CONVERT(VARCHAR(5),CONVERT(int, Package_Desc1)) + '/' + CONVERT(VARCHAR(5), CONVERT(int, Package_Desc2))  + ' ' + Unit_Name AS 'Pack',
ISNULL(LastCost,0) * Package_Desc1 AS 'LastCost'
FROM ItemVendor IV
	JOIN ItemIdentifier II(nolock) ON IV.Item_Key = II.Item_Key AND Default_Identifier = 1
	JOIN Item I(nolock) ON I.Item_Key = II.Item_Key
	JOIN @Vendor V ON V.VendorID = IV.Vendor_ID
	JOIN Vendor(nolock) ON Vendor.Vendor_ID = IV.Vendor_ID
	JOIN @CD CD ON CD.Item_Key = I.Item_Key
	JOIN ItemUnit IU(nolock) ON I.Package_Unit_ID = IU.Unit_ID
WHERE (Deleted_Item = 0 or Remove_Item = 0)
	AND Deleted_Identifier = 0 AND LastCost > 0
ORDER BY Item_Description



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

