CREATE PROCEDURE dbo.DeleteOrderItem 
    @OrderItem_ID int,
    @User_ID int 
AS 
-- **************************************************************************
   -- Procedure: DeleteOrderItem()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from the IRMA client and removes an OrderItem and
   -- all associated FKs and dependent records
   --
   -- Modification History:
   -- Date			Init	Comment
   -- 12/12/2011    MZ      Removed all the references to VendorACKCostHistoryQueue table 
   --                       because the table is deleted in 4.4 
   -- **************************************************************************
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

    DECLARE @OrderStart datetime, @OrderEnd datetime,
            @OrderType_ID int, @Vendor_ID int, @Transfer_From_SubTeam int, 
            @ReceiveLocation_ID int, @Return_Order bit, @Transfer_To_SubTeam int

    SELECT @Vendor_ID = Vendor_ID, @Transfer_From_SubTeam = Transfer_SubTeam, @OrderType_ID = OrderType_ID,
           @ReceiveLocation_ID = ReceiveLocation_ID, @Return_Order = Return_Order, @Transfer_To_SubTeam = Transfer_To_SubTeam
    FROM OrderHeader (NOLOCK) WHERE OrderHeader_ID = (SELECT TOP 1 OrderHeader_ID FROM OrderItem (NOLOCK) WHERE OrderItem_ID = @OrderItem_ID)

    SELECT 
		@OrderStart = OrderStart, 
		@OrderEnd = CASE WHEN @Transfer_From_SubTeam = @Transfer_To_SubTeam
                           THEN OrderEnd -- not transfer
                           ELSE OrderEndTransfers -- is transfer
                         END
    FROM ZoneSubTeam ZST (NOLOCK)
        INNER JOIN 
            Vendor (NOLOCK) 
            ON Vendor.Vendor_ID = @Vendor_ID AND Vendor.Store_No = ZST.Supplier_Store_No 
               AND ZST.SubTeam_No = @Transfer_From_SubTeam
        INNER JOIN 
            Vendor RL (NOLOCK) 
            ON RL.Vendor_ID = @ReceiveLocation_ID
        INNER JOIN 
            Store (NOLOCK) 
            ON Store.Store_No = RL.Store_No AND Store.Zone_ID = ZST.Zone_ID
    WHERE @Return_Order = 0 and @OrderType_ID = 2
          AND NOT EXISTS (SELECT * FROM Users (NOLOCK) WHERE User_ID = @User_ID AND Warehouse = 1)

    IF (DATEDIFF(minute, CONVERT(varchar(255), ISNULL(@OrderStart, CONVERT(smalldatetime, GETDATE())), 108), CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108)) >= 0) OR
        (DATEDIFF(minute, CONVERT(varchar(255), CONVERT(smalldatetime, GETDATE()), 108), CONVERT(varchar(255), ISNULL(@OrderEnd, CONVERT(smalldatetime, GETDATE())), 108)) >= 0)
    BEGIN     

        SELECT @Error_No = @@ERROR

        IF @Error_No = 0
        BEGIN
            DELETE ItemHistory
            FROM ItemHistory 
            WHERE OrderItem_ID = @OrderItem_ID
						   
            DELETE ItemHistoryQueue
            FROM ItemHistoryQueue 
            WHERE OrderItem_ID = @OrderItem_ID
        
            SELECT @Error_No = @@ERROR
        END
        
	     -- suspended avgcost
		if @Error_No = 0        
		begin
			DELETE	SuspendedAvgCost
			WHERE	OrderItem_ID = @OrderItem_ID
		END
        

        
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM OrderItem 
            WHERE OrderItem_ID = @OrderItem_ID 
    
            SELECT @Error_No = @@ERROR
        END
    
        IF @Error_No = 0
            COMMIT TRAN
        ELSE
        BEGIN
            ROLLBACK TRAN
            DECLARE @Severity smallint
            SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
            RAISERROR ('DeleteOrderItem failed with @@ERROR: %d', @Severity, 1, @Error_No)      
        END
    END
    ELSE
    BEGIN
        DECLARE @WindowStart varchar(255), @WindowEnd varchar(255)
        SELECT @WindowStart = CONVERT(varchar(255), @OrderStart, 108), @WindowEnd = CONVERT(varchar(255), @OrderEnd, 108)
        RAISERROR(50002, 16, 1, @WindowStart, @WindowEnd)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItem] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItem] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteOrderItem] TO [IRMAReportsRole]
    AS [dbo];

