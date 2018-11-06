﻿CREATE PROCEDURE dbo.DeleteCycleCountMaster
	@MasterCountID int 
AS

BEGIN

    SET NOCOUNT ON

    DECLARE @Error_No int
    SELECT @Error_No = 0

    BEGIN TRAN
        
        DELETE 
        FROM CycleCountHistory 
        WHERE CycleCountItemID in 
			(SELECT CycleCountItems.CycleCountItemID 
                                        FROM CycleCountItems
                                        WHERE CycleCountID in 
				(SELECT CycleCountHeader.CycleCountID 
                                        	FROM CycleCountHeader
                                       		 WHERE CycleCountHeader.MasterCountID = @MasterCountID))
        SELECT @Error_No = @@ERROR
        
        IF @Error_No = 0
        BEGIN
        	DELETE 
       	 FROM CycleCountItems 
       	 WHERE CycleCountID in (SELECT CycleCountHeader.CycleCountID 
                                        	FROM CycleCountHeader
                                       		 WHERE CycleCountHeader.MasterCountID = @MasterCountID)
            SELECT @Error_No = @@ERROR
        END

        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM CycleCountHeader
            WHERE MasterCountID = @MasterCountID
            SELECT @Error_No = @@ERROR
        END
        
        IF @Error_No = 0
        BEGIN
            DELETE 
            FROM CycleCountMaster
            WHERE MasterCountID = @MasterCountID
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
        RAISERROR ('Delete Master Cycle Count failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
            
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCountMaster] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCountMaster] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DeleteCycleCountMaster] TO [IRMAReportsRole]
    AS [dbo];

