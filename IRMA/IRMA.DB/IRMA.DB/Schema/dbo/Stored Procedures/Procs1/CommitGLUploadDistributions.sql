/* 03.30.2009 - TFS#9223 - Robert Shurbet
	-Mark all order headers as uploaded
	11/17/2009 - TFS#11482 - BSR - added '=' to MIN(DateReceived) <= @EndDate
*/

CREATE PROCEDURE [dbo].[CommitGLUploadDistributions]
    @Store_No int,
    @CurrDate datetime = null,
	@StartDate datetime = null,
	@EndDate datetime = null
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

	IF @StartDate IS NULL OR @EndDate IS NULL
		BEGIN
			-- previous Monday thru Sunday
			SELECT @StartDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 1 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())) - 6, ISNULL(@CurrDate, GETDATE())), 101)),
				   @EndDate = CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 2 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())), ISNULL(@CurrDate, GETDATE())), 101))
		END

    DECLARE @Orders TABLE (OrderHeader_ID int PRIMARY KEY)

    INSERT INTO @Orders
    SELECT OrderHeader.OrderHeader_ID
    FROM OrderHeader (NOLOCK)
    INNER JOIN
		OrderItem (NOLOCK)
        --OrderItem WITH(INDEX(idxOrderItemHeaderID, NOLOCK))
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN
        Vendor (NOLOCK) 
        ON OrderHeader.Vendor_ID = Vendor.Vendor_ID
    INNER JOIN
        Store As VStore (NOLOCK)
        ON VStore.Store_No = Vendor.Store_No
    INNER JOIN
        Vendor As RVend (NOLOCK)
        ON OrderHeader.ReceiveLocation_ID = RVend.Vendor_ID
    INNER JOIN
        Store As RStore (NOLOCK)
        ON RVend.Store_No = RStore.Store_No 
    WHERE 
		OrderHeader.OrderType_ID = 2 	-- Distribution Order Type
		AND OrderHeader.AccountingUploadDate IS NULL
	    -- Customer is not external and Vendor is Regional
	    AND dbo.fn_GetCustomerType(RStore.Store_No, RStore.Internal, RStore.BusinessUnit_ID) <> 1 
		AND dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, VStore.Internal) = 3
	    AND Vendor.Store_No = ISNULL(@Store_No, Vendor.Store_No)
	    AND OrderHeader.CloseDate IS NOT NULL
    GROUP BY OrderHeader.OrderHeader_ID
    HAVING MIN(DateReceived) >= @StartDate AND MIN(DateReceived) <= @EndDate
    
    BEGIN TRAN

    UPDATE OrderHeader SET OrderHeader.AccountingUploadDate = GetDate()
    WHERE OrderHeader_ID IN (SELECT OrderHeader_ID FROM @Orders)
    
    SELECT @Error_No = @@ERROR
    
    IF @Error_No = 0
        COMMIT TRAN
    ELSE
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('CommitGLUploadDistributions failed with @@ERROR: %d', @Severity, 1, @Error_No)       
    END

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitGLUploadDistributions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitGLUploadDistributions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitGLUploadDistributions] TO [IRMASchedJobsRole]
    AS [dbo];

