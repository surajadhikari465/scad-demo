IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'Scale_NutriFact_Update')
	BEGIN
		PRINT 'Dropping Trigger Scale_NutriFact_Update'
		DROP  Trigger Scale_NutriFact_Update
	END
GO

PRINT 'Creating Trigger Scale_NutriFact_Update'
GO
CREATE Trigger Scale_NutriFact_Update
ON [dbo].[NutriFacts]
FOR UPDATE
AS
-- **************************************************************************
-- Trigger: Scale_NutriFact_Update()
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
-- 07/13/2016   MZ       20166(15920)   Added queuing for PlumCorpChgQueue
-- **************************************************************************
BEGIN
    DECLARE @Error_No int 
    SELECT @Error_No = 0

	-- DETERMINE IF CURRENT REGION PUSHES SCALE NUTRIFACT DATA --
    DECLARE @PushScaleNutrifactData bit
    SELECT @PushScaleNutrifactData = FlagValue FROM InstanceDataFlags WHERE FlagKey = 'PushScaleNutrifactData'

    IF @Error_No = 0 AND @PushScaleNutrifactData = 1
	BEGIN
		INSERT INTO 
			NutriFactsChgQueue (NutriFactsID, ActionCode)
		SELECT 
			Inserted.NutriFactsID, 'C'
		FROM 
			Inserted
		WHERE
			NOT EXISTS (SELECT 1 FROM NutriFactsChgQueue NQ WHERE NQ.NutriFactsID = Inserted.NutriFactsID)
		AND NOT EXISTS (SELECT 1 FROM NutriFactsChgQueueTmp NQT WHERE NQT.NutriFact_ID = Inserted.NutriFactsID)

		SELECT @Error_No = @@ERROR
	END

	-- Queue for PlumCorpChgQueue
	IF @Error_No = 0
	BEGIN
		INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.NutriFactsID = Inserted.NutriFactsID
			INNER JOIN ItemScale isc ON isc.Nutrifact_ID = Inserted.NutriFactsID
            CROSS JOIN 
				Store s
            JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			WHERE 
				(s.WFM_Store = 1 OR s.Mega_Store = 1)
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
				AND NOT EXISTS (SELECT icfs.Item_Key from ItemCustomerFacingScale icfs (NOLOCK) WHERE icfs.Item_Key = si.Item_Key)
			UNION
			SELECT 
		        isc.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.NutriFactsID = Inserted.NutriFactsID
			INNER JOIN ItemScale isc ON isc.Nutrifact_ID = Inserted.NutriFactsID
            CROSS JOIN 
				Store s
            INNER JOIN 
				StoreItem si ON si.Item_Key = isc.Item_Key AND si.Store_No = s.Store_No
			INNER JOIN
				ItemCustomerFacingScale icfs on icfs.Item_Key = si.Item_Key and icfs.SendToScale = 1
			WHERE
				s.Mega_Store = 1
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = isc.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = isc.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
			SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_NutriFact_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END