IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'Scale_Nutrifact_Add')
	BEGIN
		PRINT 'Dropping Trigger Scale_Nutrifact_Add'
		DROP  Trigger Scale_Nutrifact_Add
	END
GO

PRINT 'Creating Trigger Scale_Nutrifact_Add'
GO
CREATE Trigger Scale_Nutrifact_Add 
ON NutriFacts
FOR INSERT
AS
-- **************************************************************************
-- Trigger: Scale_Nutrifact_Add()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This trigger is called to put nutrifact records in the queue to be sent to the scales.
--
-- Modification History:
-- Date       	Init  			TFS   	Comment
-- 06/05/2012	BJL   			6583	Added check against the NutriFactsChgQueue and
--										NutriFactsChgQueueTmp tables to prevent duplicate entry.
-- **************************************************************************
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE NUTRIFACT DATA --
    DECLARE @PushScaleNutrifactData bit
    SELECT @PushScaleNutrifactData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleNutrifactData'
           
    IF @Error_No = 0 AND @PushScaleNutrifactData = 1
	BEGIN
		-- Queue for NutriFactsChg
		INSERT INTO 
			NutriFactsChgQueue (NutriFactsID, ActionCode)
		SELECT 
			Inserted.NutriFactsID, 'A'
		FROM 
			Inserted
		WHERE
			NOT EXISTS (SELECT 1 FROM NutriFactsChgQueue NQ WHERE NQ.NutriFactsID = Inserted.NutriFactsID)
		AND NOT EXISTS (SELECT 1 FROM NutriFactsChgQueueTmp NQT WHERE NQT.NutriFact_ID = Inserted.NutriFactsID)

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_Nutrifact_Add trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO

