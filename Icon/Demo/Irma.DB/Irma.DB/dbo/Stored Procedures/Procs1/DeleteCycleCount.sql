CREATE PROCEDURE dbo.DeleteCycleCount
	@CycleCountID int 
AS

BEGIN

    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN

        DELETE 
        FROM CycleCountHistory 
        WHERE CycleCountItemID in
			 (SELECT CycleCountItemID 
			FROM CycleCountItems 
			WHERE CycleCountID = @CycleCountID)
        SELECT @Error_No = @@ERROR
        
        DELETE 
        FROM CycleCountItems 
        WHERE CycleCountID = @CycleCountID
        SELECT @Error_No = @@ERROR
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM CycleCountHeader
            WHERE CycleCountID = @CycleCountID
            SELECT @Error_No = @@ERROR
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
        RAISERROR ('Delete Cycle Count failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
            
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCount] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCount] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCount] TO [IRMAReportsRole]
    AS [dbo];

