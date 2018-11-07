CREATE PROCEDURE dbo.UpdateOrderWarehouseSend
    @OrderHeader_ID int
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    -- Default allocations do not apply to orders to external vendors
    IF (SELECT OrderType_ID FROM OrderHeader (NOLOCK) WHERE OrderHeader_ID = @OrderHeader_ID) <> 1 
    BEGIN
        UPDATE OrderItem
        SET QuantityAllocated = QuantityOrdered
        WHERE OrderHeader_ID = @OrderHeader_ID AND QuantityAllocated IS NULL
        
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        UPDATE OrderHeader
        SET WarehouseSent = 1
        FROM OrderHeader
        INNER JOIN
            Vendor
            ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
        LEFT JOIN
            Store VendStore
            ON Vendor.Store_No = VendStore.Store_No
        INNER JOIN
            Vendor RecvVend
            ON OrderHeader.ReceiveLocation_ID = RecvVend.Vendor_ID
        --TFS# 12346 - Robert S. 3/30/2010
        --With 3.6, we can now do inter-regional shipping so we need external region locations with no store record
        --to go to EXE. Making this a left join and adding additional OR criteria.
        --Proper fix would be to have an EXEWarehouse flag on the Vendor table, but fow now
        --we'll just detect the combination of flags that makes this an external region location
        -- for shipping.
        LEFT JOIN
            Store RecvStore
            ON RecvVend.Store_No = RecvStore.Store_No
        WHERE OrderHeader_ID = @OrderHeader_ID
            AND (ISNULL(VendStore.EXEWarehouse, 0) = 1 OR RecvStore.EXEWarehouse = 1 OR (RecvVend.Customer = 1 AND RecvVend.InternalCustomer = 0 AND RecvVend.WFM = 1))

        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        COMMIT TRAN
        SET NOCOUNT OFF
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('UpdateOrderWarehouseSend failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSend] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSend] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderWarehouseSend] TO [IRMAReportsRole]
    AS [dbo];

