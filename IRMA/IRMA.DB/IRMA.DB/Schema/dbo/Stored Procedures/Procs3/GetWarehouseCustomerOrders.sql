CREATE PROCEDURE dbo.GetWarehouseCustomerOrders

AS

BEGIN
    SET NOCOUNT ON

    SELECT ISNULL(VStore.BusinessUnit_ID % 100, 0) As DistCenter, VStore.EXEWarehouse, OrderHeader.OrderHeader_ID, OrderHeader.Vendor_ID,
           ReceiveLocation_ID, OrderDate, ISNULL(Expected_Date, 0) As Expected_Date,
           Identifier, ISNULL(SubTeam.SubTeam_Abbreviation, '') As SubTeam_Abbreviation, 
           QuantityAllocated, OrderItem.Package_Desc1, OrderHeader.CreatedBy as Buyer, OrderHeader.OrderType_ID
    FROM Store VStore (nolock)
    INNER JOIN
        Vendor (nolock)
        ON Vendor.Store_No = VStore.Store_No
    INNER JOIN
        OrderHeader 
        ON OrderHeader.Vendor_ID = Vendor.Vendor_ID
    INNER JOIN
        SubTeam 
        ON SubTeam.SubTeam_No = ISNULL(OrderHeader.Transfer_To_SubTeam, OrderHeader.Transfer_SubTeam)
    INNER JOIN
        Vendor RVendor (nolock)
        ON RVendor.Vendor_ID = OrderHeader.ReceiveLocation_ID
    INNER JOIN
        Store RStore (nolock)
        ON RStore.Store_No = RVendor.Store_No
    INNER JOIN
        OrderItem 
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = OrderItem.Item_Key
    INNER JOIN
        ItemIdentifier II (nolock)
        ON II.Item_Key = OrderItem.Item_Key AND Default_Identifier = 1
    INNER JOIN
        ZoneSubTeam ZST (nolock)
        ON ZST.Zone_ID = RStore.Zone_ID AND ZST.SubTeam_No = OrderHeader.Transfer_SubTeam AND ZST.Supplier_Store_No = VStore.Store_No
    WHERE Transfer_SubTeam IS NOT NULL
        AND Expected_Date >= CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))
        AND WarehouseSent = 1 and WarehouseSentDate IS NULL
        AND VStore.EXEWarehouse IS NOT NULL
        AND Return_Order = 0
        -- Don't send an order if any of its items have not been added to the warehouse system yet - item adds are delayed because the default identifier can never be changed once sent to the warehouse
        AND NOT EXISTS (SELECT *
                        FROM WarehouseItemChange
                        WHERE Item_Key IN (SELECT Item_Key FROM OrderItem WHERE OrderHeader_ID = OrderHeader.OrderHeader_ID)
                        AND Store_No = VStore.Store_No
                        AND ChangeType = 'A')
        AND Item.EXEDistributed = 1
    ORDER BY DistCenter, VStore.EXEWarehouse, OrderHeader.OrderHeader_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseCustomerOrders] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseCustomerOrders] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseCustomerOrders] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseCustomerOrders] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWarehouseCustomerOrders] TO [IRMAReportsRole]
    AS [dbo];

