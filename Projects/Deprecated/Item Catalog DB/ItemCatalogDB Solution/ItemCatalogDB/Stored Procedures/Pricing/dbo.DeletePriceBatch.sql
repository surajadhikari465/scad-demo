SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.DeletePriceBatch') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.DeletePriceBatch
GO

-- This stored procedure is used to delete a batch AND completely delete all of the pending changes that were
-- associated with the batch.  
CREATE PROCEDURE dbo.DeletePriceBatch
     @PriceBatchHeaderIDs varchar(max)
AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SELECT @error_no = 0

    BEGIN TRAN

    DECLARE @PBD TABLE (PriceBatchDetailID int)

    INSERT INTO @PBD
    SELECT PriceBatchDetailID 
	  FROM PriceBatchDetail pbd
	  JOIN [dbo].[fn_Parse_List] (@PriceBatchHeaderIDs, ',') ids
	    ON ids.Key_Value = pbd.PriceBatchHeaderID

    SELECT @error_no = @@ERROR

    IF @error_no = 0
	-- Delete each of the pending PriceBatchDetail records assigned to the batch.  This process deletes the
	-- batch header when the last PriceBatchDetail record is removed from the batch.
    BEGIN
        DECLARE csPBD CURSOR
        READ_ONLY
        FOR SELECT PriceBatchDetailID FROM @PBD T
        
        DECLARE @PriceBatchDetailID int
        OPEN csPBD
    
        SELECT @error_no = @@ERROR
        
        IF @error_no = 0
        BEGIN
            FETCH NEXT FROM csPBD INTO @PriceBatchDetailID
    
            SELECT @error_no = @@ERROR
        END
    
        WHILE (@error_no = 0) AND (@@fetch_status <> -1)
        BEGIN
        	IF (@@fetch_status <> -2)
        	BEGIN
                EXEC DeletePriceBatchDetail @PriceBatchDetailID
    
                SELECT @error_no = @@ERROR
        	END
    
            IF @error_no = 0
            BEGIN
                FETCH NEXT FROM csPBD INTO @PriceBatchDetailID
        
                SELECT @error_no = @@ERROR
            END
        END
        
        CLOSE csPBD
        DEALLOCATE csPBD
    END

    SET NOCOUNT OFF

    IF @error_no = 0
	    COMMIT TRAN
    ELSE
    BEGIN
        IF @@TRANCOUNT <> 0
            ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('DeletePriceBatch failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

