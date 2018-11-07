SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateCycleCountMasterClosed]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateCycleCountMasterClosed]
GO


CREATE PROCEDURE dbo.UpdateCycleCountMasterClosed
	@MasterCountID int
	,@ClosedDate datetime
	,@ResetInventoryCnt bit
	,@SetNonCountedToZero bit
AS
BEGIN
    SET NOCOUNT ON

    DECLARE @error_no int
    SET @error_no = 0

    BEGIN TRAN
    
    UPDATE 
    	CycleCountMaster 
    SET 
    	ClosedDate = @ClosedDate
    	,UpdateIH = @ResetInventoryCnt
    	,SetNonCountedToZero = @SetNonCountedToZero
     
    WHERE 
    	MasterCountID = @MasterCountID

    SELECT @error_no = @@ERROR

    IF (@error_no = 0) AND (@ResetInventoryCnt = 1)
    BEGIN
        IF EXISTS (SELECT * 
                   FROM CycleCountMaster M 
                   INNER JOIN Store ON Store.Store_No = M.Store_No
                   INNER JOIN SubTeam ON SubTeam.SubTeam_No = M.SubTeam_No
                   WHERE M.MasterCountID = @MasterCountID
                       AND Store.EXEWarehouse IS NOT NULL
                       AND SubTeam.EXEDistributed = 1)
        BEGIN
            EXEC InsertItemHistoryCycleCount @MasterCountID
            SELECT @error_no = @@ERROR
        END
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
        RAISERROR ('UpdateCycleCountMasterClosed failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


