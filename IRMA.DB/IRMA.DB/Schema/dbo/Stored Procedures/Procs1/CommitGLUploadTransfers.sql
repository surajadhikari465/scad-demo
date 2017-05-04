
CREATE PROCEDURE dbo.CommitGLUploadTransfers
	@Region_Code varchar(3)
AS
-- **************************************************************************
-- Procedure: CommitGLUploadTransfers

-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/28/2013   FA      11384   Modified the parameter list by removing start
--                              date and end date. These values will be calculated
--                              inside stored procedure to return the transfers
--                              received last week (Prev. Monday through Sunday)
-- 03/07/2016   FA      14545   Added region parameter to filter transfers by region as RM database now hosts both RM and TS regions

-- **************************************************************************

BEGIN
    SET NOCOUNT ON
    
	DECLARE		@StartDate			DateTime
	DECLARE		@EndDate			DateTime
    DECLARE		@CurrDate			DateTime 
    
    SELECT	@CurrDate = GETDATE()

 	-- previous monday through Sunday  
	SELECT
		@StartDate = CONVERT ( datetime,CONVERT ( varchar(255),DATEADD ( "day" , 1 - DATEPART ( dw , ISNULL ( @CurrDate , GETDATE ( ) ) ) - 6 , ISNULL ( @CurrDate , GETDATE ( ) ) ),101 ) ) ,
		@EndDate = CONVERT ( datetime,CONVERT ( varchar(255),DATEADD ( "day" , 2 - DATEPART ( dw , ISNULL ( @CurrDate , GETDATE ( ) ) ) , ISNULL ( @CurrDate , GETDATE ( ) ) ),101 ) )
	
		
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @Orders TABLE (OrderHeader_ID int PRIMARY KEY)

    INSERT INTO @Orders
		SELECT 
			OrderHeader.OrderHeader_ID
		FROM 
			OrderHeader							(NOLOCK)
			INNER JOIN	OrderItem				(NOLOCK)	ON OrderItem.OrderHeader_ID			= OrderHeader.OrderHeader_ID
			INNER JOIN	Vendor					(NOLOCK)	ON OrderHeader.Vendor_ID			= Vendor.Vendor_ID
			INNER JOIN	Store VStore			(NOLOCK)	ON VStore.Store_No					= Vendor.Store_No
			INNER JOIN  StoreRegionMapping srm	(NOLOCK)	ON VStore.Store_No					= srm.Store_no AND @Region_Code = srm.Region_Code	
			INNER JOIN	Vendor RVend			(NOLOCK)	ON OrderHeader.ReceiveLocation_ID	= RVend.Vendor_ID
			INNER JOIN	Store RStore			(NOLOCK)	ON RVend.Store_No					= RStore.Store_No
		WHERE 	
			OrderHeader.OrderType_ID = 3 	-- Transfer Order Type
			AND OrderHeader.AccountingUploadDate IS NULL 
			AND dbo.fn_GetCustomerType(RStore.Store_No, RStore.Internal, RStore.BusinessUnit_ID) = 3  -- Customer and Vendor are regional
			AND dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, VStore.Internal) = 3
			AND DATEDIFF("day", @StartDate, DateReceived) >= 0
			AND DATEDIFF("day", DateReceived, @EndDate) > 0
			AND CloseDate is not null
		GROUP BY OrderHeader.OrderHeader_ID
 
	--select * from @Orders
 
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
        RAISERROR ('CommitGLUploadTransfers failed with @@ERROR: %d', @Severity, 1, @Error_No)       
    END

    SET NOCOUNT OFF
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitGLUploadTransfers] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitGLUploadTransfers] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CommitGLUploadTransfers] TO [IRMASchedJobsRole]
    AS [dbo];

