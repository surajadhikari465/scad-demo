SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetWarehousePurchaseOrders') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetWarehousePurchaseOrders
GO


CREATE PROCEDURE dbo.GetWarehousePurchaseOrders

AS
   -- **************************************************************************
   -- Procedure: GetWarehousePurchaseOrders
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 11/06/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON

    SELECT ISNULL(Store.BusinessUnit_ID % 100, 0) As DistCenter, EXEWarehouse, OrderHeader.OrderHeader_ID, OrderHeader.Vendor_ID,
           LEFT(UserName, 3) As Buyer, ISNULL(Expected_Date, 0) As Expected_Date,
           Identifier, QuantityOrdered, OrderItem.Package_Desc1, OrderHeader.OrderType_ID
    FROM Store
    INNER JOIN
        Vendor VendStore ON VendStore.Store_No = Store.Store_No




    INNER JOIN
        OrderHeader ON OrderHeader.ReceiveLocation_ID = VendStore.Vendor_ID




    INNER JOIN
        Users ON Users.User_ID = OrderHeader.CreatedBy
    INNER JOIN
        OrderItem ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN
        Item ON Item.Item_Key = OrderItem.Item_Key
    INNER JOIN
        ItemIdentifier II (nolock)
        ON II.Item_Key = OrderItem.Item_Key AND Default_Identifier = 1
    WHERE Transfer_SubTeam IS NULL
        AND Expected_Date >= CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))
        AND WarehouseSent = 1 and WarehouseSentDate IS NULL
        AND EXEWarehouse IS NOT NULL
        AND Return_Order = 0
        AND EXISTS (SELECT * 
                    FROM ZoneSubTeam ZST (nolock)
                    WHERE ZST.Supplier_Store_No = Store.Store_No
                        AND ZST.SubTeam_No = (SELECT TOP 1 SubTeam_No FROM OrderItem WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID))
        -- Don't send an order if any of its items have not been added to the warehouse system yet - item adds are delayed because the default identifier can never be changed once sent to the warehouse
        AND NOT EXISTS (SELECT *
                        FROM WarehouseItemChange
                        WHERE Item_Key IN (SELECT Item_Key FROM OrderItem WHERE OrderHeader_ID = OrderHeader.OrderHeader_ID)
                        AND Store_No = Store.Store_No
                        AND ChangeType = 'A')
        AND Item.EXEDistributed = 1
    ORDER BY DistCenter, EXEWarehouse, OrderHeader.OrderHeader_ID


    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO