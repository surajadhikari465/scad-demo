CREATE PROCEDURE dbo.InsertReturnOrderHeader
@OrderHeader_ID int,
@User_ID int
AS

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    
    DECLARE @Vendor_ID int, @PurchaseLocation_ID int, @Transfer_SubTeam int, @Transfer_To_SubTeam int 
	DECLARE @NewOrderHeader_ID int, @OrderType_ID int, @ProductType_ID int

    BEGIN TRAN
    
    SELECT @Vendor_ID = Vendor_ID, @PurchaseLocation_ID = ReceiveLocation_ID, @Transfer_SubTeam = Transfer_SubTeam, 
           @Transfer_To_Subteam = Transfer_To_Subteam, @OrderType_ID = OrderType_ID, @ProductType_ID = ProductType_ID
    FROM OrderHeader
    WHERE OrderHeader_ID = @OrderHeader_ID

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
        IF @OrderType_ID = 2 --Distribution Order 
        BEGIN
            INSERT INTO OrderHeader (Vendor_ID, PurchaseLocation_ID, ReceiveLocation_ID, Transfer_SubTeam, Transfer_To_SubTeam, 
                                     Fax_Order, OrderType_ID, ProductType_ID, Return_Order, CreatedBy, CloseDate,
                                     InvoiceNumber, Sent, SentDate)
            VALUES (@Vendor_ID, @PurchaseLocation_ID, @PurchaseLocation_ID, @Transfer_SubTeam, @Transfer_To_Subteam, 
                    0, @OrderType_ID, @ProductType_ID, 1, @User_ID, CONVERT(char(12), GETDATE(), 1),
                    'CM:' + convert(varchar(10),@OrderHeader_ID), 1, GETDATE())

            SELECT @Error_No = @@ERROR
        END
    END

    IF @Error_No = 0
    BEGIN
        SELECT @NewOrderHeader_ID = SCOPE_IDENTITY()
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        INSERT INTO ReturnOrderList (OrderHeader_ID, ReturnOrderHeader_ID) VALUES (@OrderHeader_ID, @NewOrderHeader_ID)
        SELECT @Error_No = @@ERROR
    END

    IF @Error_No = 0
    BEGIN
        SELECT @NewOrderHeader_ID AS OrderHeader_ID
        COMMIT TRAN
    END
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('InsertReturnOrderHeader failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertReturnOrderHeader] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertReturnOrderHeader] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InsertReturnOrderHeader] TO [IRMAReportsRole]
    AS [dbo];

