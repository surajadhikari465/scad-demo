CREATE PROCEDURE dbo.Replenishment_ScalePush_InsertScaleExtraTextChgQueue (
	@ScaleExtraTextID INT,
	@ActionCode CHAR(1),
	@Store_No INT
)
AS
-- **************************************************************************
-- Procedure: Replenishment_ScalePush_InsertScaleExtraTextChgQueue()
--    Author: Benjamin Loving
--      Date: 05/25/2012
--
-- Description:
-- This procedure is called to place a ScaleExtraText record into the
-- ScaleExtraTextChgQueue to be sent down to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 05/25/2012	BJL   			4945	Initial creation
-- **************************************************************************
BEGIN
   DECLARE @Error_No int
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE EXTRA TEXT DATA --
    DECLARE @PushScaleScaleExtraTextData bit
    SELECT @PushScaleScaleExtraTextData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleExtraTextData'

    IF @Error_No = 0
		AND @PushScaleScaleExtraTextData = 1
		AND @ScaleExtraTextID IS NOT NULL
		AND NOT EXISTS (SELECT 1 FROM Scale_ExtraTextChgQueue SETQ WHERE SETQ.Scale_ExtraText_ID = @ScaleExtraTextID AND SETQ.ActionCode = @ActionCode)
		AND NOT EXISTS (SELECT 1 FROM Scale_ExtraTextChgQueueTmp SETQT WHERE SETQT.Scale_ExtraText_ID = @ScaleExtraTextID AND SETQT.ActionCode = @ActionCode)
		AND EXISTS (SELECT 1 FROM Scale_ExtraText WHERE Scale_ExtraText_ID = @ScaleExtraTextID)
		AND (@Store_No IS NULL OR EXISTS (SELECT 1 FROM Store WHERE Store_No = @Store_No))
	BEGIN
		-- Queue for ScaleExtraTextChg
		INSERT INTO 
			Scale_ExtraTextChgQueue (Scale_ExtraText_ID, ActionCode, Store_No)
		VALUES
			(@ScaleExtraTextID, @ActionCode, @Store_No)

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Replenishment_ScalePush_InsertScaleExtraTextChgQueue failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_InsertScaleExtraTextChgQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_InsertScaleExtraTextChgQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_InsertScaleExtraTextChgQueue] TO [IRMASchedJobsRole]
    AS [dbo];

