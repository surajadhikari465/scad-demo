SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AutoCloseReceiving]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AutoCloseReceiving]
GO


CREATE PROCEDURE dbo.AutoCloseReceiving
	@User_ID int = 0
AS
BEGIN
    SET NOCOUNT ON
    
    DECLARE @Error_No int
    SELECT @Error_No = 0

    DECLARE @Date datetime
    -- Get current date without the time
    SELECT @Date = CONVERT(datetime, CONVERT(varchar(10), GETDATE(), 102))

    DECLARE ReceivingStores CURSOR
    READ_ONLY
    FOR SELECT Store_No 
        FROM Store
        WHERE (Mega_Store = 1 OR Distribution_Center = 1 OR Manufacturer = 1 OR WFM_Store = 1)
        AND (LastRecvLogDate IS NOT NULL) AND (ISNULL(LastRecvLogDate, @Date) < @Date)
    

    DECLARE @Store_No int, @Errors varchar(255)

    OPEN ReceivingStores

    SELECT @Error_No = @@ERROR
    
    IF @Error_No = 0
    BEGIN
        FETCH NEXT FROM ReceivingStores INTO @Store_No
        SELECT @Error_No = @@ERROR
    END

    WHILE (@@fetch_status <> -1) AND (@Error_No = 0)
    BEGIN
        IF (@@fetch_status <> -2)
        BEGIN
            EXEC CloseReceiving @Store_No, @User_ID
            
            SELECT @Error_No = @@ERROR
            IF @Error_No = 50000 -- User is closing receiving or lock did not clear
            BEGIN
                SELECT @Errors = CASE WHEN @Errors IS NOT NULL THEN @Errors + ',' ELSE '' END + CONVERT(varchar(255), @Store_No) + '-' + CONVERT(varchar(255), @Error_No)
                SELECT @Error_No = 0
            END
        END 
        
        IF @Error_No = 0
        BEGIN
            FETCH NEXT FROM ReceivingStores INTO @Store_No
            SELECT @Error_No = @@ERROR
        END
    END
    
    CLOSE ReceivingStores
    DEALLOCATE ReceivingStores

    IF (@Error_No <> 0)
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (@Error_No, @Severity, 1)
        SET NOCOUNT OFF
        RETURN
    END
    
    SET NOCOUNT OFF
    
    IF @Errors IS NOT NULL
        RAISERROR ('AutoCloseReceiving failed for one or more stores: %s', 16, 1, @Errors)

END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


