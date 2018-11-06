SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.InsertItemHistoryCycleCountCursor') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.InsertItemHistoryCycleCountCursor
GO

CREATE PROCEDURE dbo.InsertItemHistoryCycleCountCursor

AS

BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SET @error_no = 0

    DECLARE CCM CURSOR
    READ_ONLY
    FOR SELECT MasterCountID 
        FROM CycleCountMaster
        WHERE ClosedDate IS NOT NULL AND UpdateIH = 1 AND ISNULL(IHUpdated, 0) = 0
        ORDER BY ClosedDate
    
    DECLARE @MasterCountID int
    OPEN CCM
    
    FETCH NEXT FROM CCM INTO @MasterCountID
    WHILE (@@fetch_status <> -1)
    BEGIN
    	IF (@@fetch_status <> -2)
    	BEGIN
            EXEC InsertItemHistoryCycleCount @MasterCountID

            SELECT @error_no = @@ERROR

            IF @error_no <> 0
            BEGIN
                IF @@TRANCOUNT <> 0
                    ROLLBACK TRAN
                DECLARE @Severity smallint
                SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
                RAISERROR ('InsertItemHistoryCycleCountCursor failed with @@ERROR: %d', @Severity, 1, @error_no)
                RETURN
            END
    	END
    	FETCH NEXT FROM CCM INTO @MasterCountID
    END
    
    CLOSE CCM
    DEALLOCATE CCM

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

