CREATE PROCEDURE [dbo].[CloseReceiving] 
    @Store_No int,
    @User_ID int
AS 

BEGIN
	/*
	--------------------------------------
	Change History
	--------------------------------------
	Date		Name		TFS		Desc
	--------------------------------------
	7/3/2011	Tom Lux		2280	Fixed issue when declaring LogOrders CURSOR in EXIST clause.
									We do not compare DateReceived <= @ThisRecvLogDate, since @ThisRecvLogDate is a regional time zone value, not Central Time, like the DateReceived field.
	8/17/2011	Tom Lux		2756	I was wrong about the DateReceived field: it's not in Central Time, it's in local receiver-machine time.  So, the issue in RM was apparently with Central-Time-Zone
									stores, because it was comparing date-received time to Mountain Time and therefore missing any closed POs for the last hour.  The change in the previous edit 
									comment above from 7/3/2011 pushed the issue to the East-Coast regions, so they would miss orders closed within 60 minutes of closing receiving.
									We're doing a **TEMP** fix here: we'll branch on the time-zone-offset value...
									- If the offset is <= 0 (Central Time or West of that), use the "new" logic, which isn't perfect, but shouldn't cause POs to be missed.
									- Otherwise (offset is > 0, East of Central Time Zone), use the original logic, which still has flaws, but the "issue" is relative to Central Time
									and completely fixing the issue involves NEVER storing dates in local time zones (always use DB time, this is how IRMA 3.6 worked).
									To do this, we'll use the @CloseReceivingDate var to hold either the Central Time Zone date/time (offset <= 0) or the adjusted date/time (East Coast and greater).
	06/29/2012	Trey D		6664	Changed 'ORDER BY CloseDate' to 'ORDER BY OriginalCloseDate' in conjunction with changing UpdateOrderClosed so that the receiving log
									will now list orders in the same sequence the receiver closes them.
	*/



    DECLARE
		@Error_No int
		,@Affected int
		,@CloseReceivingDate datetime

    SELECT @Error_No = 0

    -- Lock first
    
    BEGIN TRAN
    
    UPDATE Store
    SET RecvLogUser_ID = @User_ID
    WHERE Store_No = @Store_No
    AND DATEDIFF(d, LastRecvLogDate, GETDATE()) > 0
    
    SELECT @Affected = @@ROWCOUNT, @Error_No = @@ERROR
    
    COMMIT TRAN
    
    IF (@Affected = 0) AND (@Error_No = 0)
    BEGIN
            RAISERROR ('Close Receiving has been completed for store: %d today', 16, 1, @Store_No)
        RETURN
    END
    
    -- Get the Last Receiving Log Number and Date and set this receiving log date
    DECLARE
		@LastRecvLog_No int
		,@LastRecvLogDate datetime
		,@ThisRecvLogDate datetime
		,@CentralTimeZoneOffset int
    
    IF @Error_No = 0
    BEGIN
        SELECT @LastRecvLog_No = ISNULL(LastRecvLog_No, 0), @LastRecvLogDate = LastRecvLogDate
        FROM Store 
        WHERE Store_No = @Store_No

        SELECT @Error_No = @@ERROR    
    END
    
    SELECT @CentralTimeZoneOffset = CentralTimeZoneOffset FROM Region
    
    -- Set the date/time we'll use to determine what orders are included in the receiving log group.
    if @CentralTimeZoneOffset <= 0
    begin
		-- Central Time Zone and West of that zone.
		select @CloseReceivingDate = getdate()
	end
	else
	begin
		-- Eastern Time Zone and East of that zone.
		select @CloseReceivingDate = DATEADD(hour,@CentralTimeZoneOffset,CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 20)))
	end
	
    SELECT @ThisRecvLogDate = DATEADD(hour,@CentralTimeZoneOffset,CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 20)))
    
    -- Create cursor to select orders that need to be logged
    IF @Error_No = 0
    BEGIN
        DECLARE LogOrders CURSOR
        READ_ONLY
        FOR 
            SELECT OrderHeader.OrderHeader_ID
            FROM OrderHeader
            INNER JOIN 
                Vendor VendStore (NOLOCK) ON VendStore.Vendor_ID = OrderHeader.ReceiveLocation_ID
            INNER JOIN 
                Vendor (NOLOCK) ON Vendor.Vendor_ID = OrderHeader.Vendor_ID
            LEFT JOIN
                Store (nolock) ON Vendor.Store_No = Store.Store_No
            WHERE VendStore.Store_No = @Store_No
            AND Transfer_SubTeam IS NULL -- DSD orders only
            AND RecvLogDate IS NULL
            AND dbo.fn_VendorType(Vendor.PS_Vendor_ID, Vendor.WFM, Vendor.Store_No, Store.Internal) = 1 -- External 
            AND EXISTS (SELECT * 
                        FROM OrderItem 
                        WHERE OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID 
                        AND (DateReceived > @LastRecvLogDate AND DateReceived <= @CloseReceivingDate)) -- TFS 2280, 2756: Changed 'DateReceived <= @ThisRecvLogDate' statement to use @CloseReceivingDate, which is set above, depending on time zone offset from Central.
            ORDER BY OriginalCloseDate 
        
        DECLARE @OrderHeader_ID int

        OPEN LogOrders

        SELECT @Error_No = @@ERROR
    END
    
    BEGIN TRAN
    
    IF @Error_No = 0
    BEGIN
        FETCH NEXT FROM LogOrders INTO @OrderHeader_ID

        SELECT @Error_No = @@ERROR
    END

    WHILE (@@fetch_status <> -1) AND (@Error_No = 0)
    BEGIN
    	IF (@@fetch_status <> -2)
    	BEGIN
            -- Log the order
            SELECT @LastRecvLog_No = @LastRecvLog_No + 1
            
            UPDATE OrderHeader
            SET RecvLogDate = @ThisRecvLogDate, -- It may not be a good idea to store a time value that has been converted to the regional time zone; this should be done at presentation layer and if done both places, the wrong value will be displayed.
                RecvLog_No = @LastRecvLog_No,
                RecvLogUser_ID = @User_ID
            WHERE OrderHeader_ID = @OrderHeader_ID

            SELECT @Error_No = @@ERROR
    	END

        IF @Error_No = 0
        BEGIN
    	    FETCH NEXT FROM LogOrders INTO @OrderHeader_ID
            SELECT @Error_No = @@ERROR
        END
    END
  
    CLOSE LogOrders
    DEALLOCATE LogOrders
    
    -- Record the Last Receiving info in the Store record
    IF @Error_No = 0
    BEGIN
        UPDATE Store
        SET LastRecvLogDate = @ThisRecvLogDate,
            LastRecvLog_No = @LastRecvLog_No
        WHERE Store_No = @Store_No

        SELECT @Error_No = @@ERROR
    END
    
    IF @Error_No = 0
        COMMIT TRAN
    ELSE
        ROLLBACK TRAN

    -- Unlock receiving regardless of errors
    UPDATE Store
    SET RecvLogUser_ID = NULL
    WHERE Store_No = @Store_No

    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('CloseReceiving failed with @@ERROR %d for store: %d', @Severity, 1, @Error_No, @Store_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseReceiving] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseReceiving] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseReceiving] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CloseReceiving] TO [IRMAReportsRole]
    AS [dbo];

