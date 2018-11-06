CREATE PROCEDURE dbo.UpdateOrderInfo
    @OrderHeader_ID int,
    @Temperature int, 
    @QuantityDiscount decimal(18,4),
    @Expected_Date smalldatetime, 
    @DiscountType int, 
	@ReasonCodeDetailID varchar(4),
    @PurchaseLocation_ID int, 
    @ReceiveLocation_ID int, 
    @Fax_Order bit, 
    @Email_Order bit,
    @Electronic_Order bit,
    @Return_Order bit,
    @OrigOrderHeader_ID int,
    @User_ID int,
    @DropShip int
AS 

 -- **************************************************************************
   -- Procedure: UpdateOrderInfo()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 7/20/2011		MD		2095	Added ReasonCodeDetailID to save off Reason Code
   -- 01/07/2013	DN		9244	Changed the parameter ReasonCodeDetailID from nchar(3) to varchar(4) and updated
   --                               code to assign a null value to the varible ReasonCodeDetail_ID
   -- **************************************************************************

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

	DECLARE @ReasonCodeDetail_ID INT
	BEGIN
	IF @ReasonCodeDetailID = 'NULL'
		SET @ReasonCodeDetail_ID = NULL
	ELSE
		SET @ReasonCodeDetail_ID = CONVERT(INT, @ReasonCodeDetailID)
	END

    BEGIN TRAN

    DECLARE @OrderStart datetime, @OrderEnd datetime,
            @OrderType_ID int, @Vendor_ID int, @Transfer_From_SubTeam int, @Transfer_To_SubTeam int

    SELECT @Vendor_ID = Vendor_ID, @Transfer_From_SubTeam = Transfer_SubTeam, @OrderType_ID = OrderType_ID, 
           @Transfer_To_SubTeam = Transfer_To_SubTeam 
    FROM OrderHeader (NOLOCK) 
    WHERE OrderHeader_ID = @OrderHeader_ID

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
        UPDATE OrderHeader
        SET Temperature = @Temperature, 
            QuantityDiscount = @QuantityDiscount,
            Expected_Date = @Expected_Date, 
            DiscountType = @DiscountType, 
			ReasonCodeDetailID = @ReasonCodeDetail_ID,
            PurchaseLocation_ID = @PurchaseLocation_ID, 
            ReceiveLocation_ID = @ReceiveLocation_ID, 
            Fax_Order = @Fax_Order, 
            Email_Order = @Email_Order,
            Electronic_Order = @Electronic_Order,
            Return_Order = @Return_Order,
            User_ID = NULL,
            isDropShipment = @DropShip
        FROM OrderHeader 
        WHERE OrderHeader_ID = @OrderHeader_ID
    
        SELECT @Error_No = @@ERROR
    
        IF (@Error_No = 0) AND (@Return_Order = 1)
        BEGIN
            DELETE FROM ReturnOrderList 
            WHERE ReturnOrderHeader_ID = @OrderHeader_ID
    
            SELECT @Error_No = @@ERROR
    
            IF (@Error_No = 0) AND (@OrigOrderHeader_ID IS NOT NULL)
            BEGIN
                INSERT INTO ReturnOrderList (OrderHeader_ID, ReturnOrderHeader_ID)
                VALUES (@OrigOrderHeader_ID, @OrderHeader_ID)
    
                SELECT @Error_No = @@ERROR
            END
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
            RAISERROR ('UpdateOrderInfo failed with @@ERROR: %d', @Severity, 1, @Error_No)
        END
    END
    ELSE
    BEGIN
        DECLARE @WindowStart varchar(255), @WindowEnd varchar(255)
        SELECT @WindowStart = CONVERT(varchar(255), @OrderStart, 108), @WindowEnd = CONVERT(varchar(255), @OrderEnd, 108)
        RAISERROR(50002, 16, 1, @WindowStart, @WindowEnd)
    END
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateOrderInfo] TO [IRMAReportsRole]
    AS [dbo];

