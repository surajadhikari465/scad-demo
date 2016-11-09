CREATE PROCEDURE dbo.GetStoreItemOrderInfo 
	@Store_No int,
    @TransferToSubTeam_No int,
    @Item_Key int,
    @QtyOnOrder decimal(18,4) OUTPUT,
    @QtyOnQueue decimal(18,4) OUTPUT,
    @QtyOnQueueCredit decimal(18,4) OUTPUT,
    @QtyOnQueueTransfer decimal(18,4) OUTPUT,
    @PrimaryVendorKey varchar(255) OUTPUT,
    @PrimaryVendorName varchar(255) OUTPUT,
    @LastReceivedDate datetime OUTPUT,
    @LastReceived decimal(18,4) OUTPUT

AS
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SET @error_no = 0

    DECLARE @SubTeam_No int

    IF @TransferToSubTeam_No IS NULL
    BEGIN
        SELECT @SubTeam_No = SubTeam_No FROM Item (nolock) WHERE Item_Key = @Item_Key
        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        SELECT @QtyOnOrder = SUM(ISNULL(QuantityAllocated, QuantityOrdered))
        FROM OrderHeader (NOLOCK)
        INNER JOIN
            Vendor RL (NOLOCK)
            ON RL.Vendor_ID = OrderHeader.ReceiveLocation_ID
        INNER JOIN
            OrderItem (NOLOCK)
            ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
        WHERE CloseDate IS NULL 
        AND DateReceived IS NULL 
        AND RL.Store_No = @Store_No 
        AND OrderItem.Item_Key = @Item_Key
        AND Transfer_To_SubTeam = ISNULL(@TransferToSubTeam_No, @SubTeam_No)
        AND Return_Order = 0

        SELECT @error_no = @@ERROR
    END

    IF @error_no = 0
    BEGIN
        SELECT @QtyOnQueue = SUM(CASE WHEN Credit = 0 AND Transfer = 0 THEN Quantity ELSE 0 END),
               @QtyOnQueueCredit = SUM(CASE WHEN Credit = 1 THEN Quantity ELSE 0 END),
               @QtyOnQueueTransfer = SUM(CASE WHEN Transfer = 1 THEN Quantity ELSE 0 END)
        FROM OrderItemQueue Q (nolock)
        WHERE Store_No = @Store_No 
            AND TransferToSubTeam_No = ISNULL(@TransferToSubTeam_No, @SubTeam_No)
            AND Item_Key = @Item_Key

        SELECT @error_no = @@ERROR
    END
    
    IF @error_no = 0
    BEGIN
		SELECT @LastReceivedDate = DateReceived, @LastReceived = QuantityReceived
		FROM OrderItem (nolock)
		WHERE OrderItem_ID = (SELECT TOP 1 OrderItem_ID
								FROM ItemHistory (nolock)
								WHERE Item_Key = @Item_Key
									AND Store_No = @Store_No
									AND SubTeam_No = ISNULL(@TransferToSubTeam_No, @SubTeam_No)
									AND Adjustment_ID = 5
								ORDER BY DateStamp DESC)


        SELECT @error_no = @@ERROR
    END
         
    IF @error_no = 0
    BEGIN                       
		SELECT @PrimaryVendorKey = Vendor.Vendor_Key, @PrimaryVendorName = Vendor.CompanyName
		FROM StoreItemVendor SIV (nolock)
		INNER JOIN Vendor (nolock) ON Vendor.Vendor_ID = SIV.Vendor_ID
		WHERE PrimaryVendor = 1
			AND SIV.Store_No = @Store_No
			AND SIV.Item_Key = @Item_Key

        SELECT @error_no = @@ERROR
    END

    SELECT @QtyOnOrder = ISNULL(@QtyOnOrder, 0), 
           @QtyOnQueue = ISNULL(@QtyOnQueue, 0), 
           @QtyOnQueueCredit = ISNULL(@QtyOnQueueCredit, 0),
           @QtyOnQueueTransfer = ISNULL(@QtyOnQueueTransfer, 0),
           @PrimaryVendorKey = ISNULL(@PrimaryVendorKey, ''),
           @PrimaryVendorName = ISNULL(@PrimaryVendorName, ''),
           @LastReceivedDate = ISNULL(@LastReceivedDate, 0),
           @LastReceived = ISNULL(@LastReceived, 0)

    IF @error_no <> 0
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('GetStoreItemOrderInfo failed with @@ERROR: %d', @Severity, 1, @error_no)
    END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemOrderInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemOrderInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemOrderInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetStoreItemOrderInfo] TO [IRMAReportsRole]
    AS [dbo];

