CREATE PROCEDURE dbo.Replenishment_ScalePush_InsertNutriFactsChgQueue (
	@NutriFactsID INT,
	@ActionCode CHAR(1),
	@Store_No INT
)
AS
-- **************************************************************************
-- Procedure: Replenishment_ScalePush_InsertNutriFactsChgQueue()
--    Author: Benjamin Loving
--      Date: 05/25/2012
--
-- Description:
-- This procedure is called to place a NutriFacts record into the
-- NutrifactsChgQueue to be sent down to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 06.06.2012	BBB				6583	Removed actioncode from where clause
-- 05/25/2012	BJL   			4945	Initial creation
-- **************************************************************************
BEGIN
   DECLARE @Error_No int
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE NUTRIFACT DATA --
    DECLARE @PushScaleNutrifactData bit
    SELECT @PushScaleNutrifactData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleNutrifactData'
           
    IF @Error_No = 0
		AND @PushScaleNutrifactData = 1
		AND @NutriFactsID IS NOT NULL
		AND NOT EXISTS (SELECT 1 FROM NutrifactsChgQueue NQ WHERE NQ.NutriFactsID = @NutriFactsID)
		AND NOT EXISTS (SELECT 1 FROM NutrifactsChgQueueTmp NQT WHERE NQT.Nutrifact_ID = @NutriFactsID)
		AND EXISTS (SELECT 1 FROM Nutrifacts WHERE NutriFactsID = @NutriFactsID)
		AND (@Store_No IS NULL OR EXISTS (SELECT 1 FROM Store WHERE Store_No = @Store_No))
	BEGIN
		-- Queue for NutriFactsChg
		INSERT INTO
			NutriFactsChgQueue (NutriFactsID, ActionCode, Store_No)
		VALUES
			(@NutriFactsID, @ActionCode, @Store_No)

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Replenishment_ScalePush_InsertNutriFactsChgQueue failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_InsertNutriFactsChgQueue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_InsertNutriFactsChgQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_ScalePush_InsertNutriFactsChgQueue] TO [IRMASchedJobsRole]
    AS [dbo];

