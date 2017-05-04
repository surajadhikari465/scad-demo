IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'Scale_ItemScaleOverrideAdd')
	DROP  TRIGGER  Scale_ItemScaleOverrideAdd
GO

CREATE TRIGGER Scale_ItemScaleOverrideAdd ON ItemScaleOverride
FOR INSERT

AS

-- ****************************************************************************************************************
-- Procedure: Scale_ItemScaleOverrideAdd()
--    Author: unknown
--      Date: unknown
--
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2013-01-25	KM		9382	Add update history template; account for all new ItemScaleOverride values in 4.8;
-- 2013-01-31	KM		9393	Include Nutrifact_ID column;
-- 2013-02-06   MZ      9392    Added a where statement to restrict PLUMCorpChgQueue records to be created only for 
--                              stores in the same jurisdiction.
-- ****************************************************************************************************************

BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    IF @error_no = 0
    BEGIN
		-- CREATE THE INITIAL HISTORY RECORD TO TRACK THE CHANGE
		INSERT INTO ItemScaleOverrideHistory (
			Item_Key, 
			StoreJurisdictionID,
			Scale_Description1,
			Scale_Description2,
			Scale_Description3,
			Scale_Description4,
			Scale_ExtraText_ID,
			Scale_Tare_ID,
			Scale_LabelStyle_ID,
			Scale_ScaleUOMUnit_ID,
			Scale_RandomWeightType_ID,
			Scale_FixedWeight,
			Scale_ByCount,
			ShelfLife_Length,
			ForceTare,
			Scale_Alternate_Tare_ID,
			Scale_EatBy_ID,
			Scale_Grade_ID,
			PrintBlankEatBy,
			PrintBlankPackDate,
			PrintBlankShelfLife,
			PrintBlankTotalPrice,
			PrintBlankUnitPrice,
			PrintBlankWeight,
			Nutrifact_ID			
		) SELECT	
			Inserted.Item_Key, 
			Inserted.StoreJurisdictionID,
			Inserted.Scale_Description1,
			Inserted.Scale_Description2,
			Inserted.Scale_Description3,
			Inserted.Scale_Description4,
			Inserted.Scale_ExtraText_ID,
			Inserted.Scale_Tare_ID,
			Inserted.Scale_LabelStyle_ID,
			Inserted.Scale_ScaleUOMUnit_ID,
			Inserted.Scale_RandomWeightType_ID,
			Inserted.Scale_FixedWeight,
			Inserted.Scale_ByCount,
			Inserted.ShelfLife_Length,
			Inserted.ForceTare,
			Inserted.Scale_Alternate_Tare_ID,
			Inserted.Scale_EatBy_ID,
			Inserted.Scale_Grade_ID,
			Inserted.PrintBlankEatBy,
			Inserted.PrintBlankPackDate,
			Inserted.PrintBlankShelfLife,
			Inserted.PrintBlankTotalPrice,
			Inserted.PrintBlankUnitPrice,
			Inserted.PrintBlankWeight,
			Inserted.Nutrifact_ID
		FROM Inserted
            
		SELECT @error_no = @@ERROR
    END
              
    IF @error_no = 0
    BEGIN
		-- Create a change entry in the PLUMCorpChgQueue table to send the 
		-- updated corporate scale information from IRMA to the scale systems.
		
		-- Note:  The scale data in IRMA is categorized as “Corporate” (same for all stores in the region) or 
		-- “Zone” (store specific) data.  Only prices are treated as “Zone” data.  This means that adding new 
		-- alternate jurisdiction override values for scale information or changing existing jurisdiction overrides 
		-- will communicate a change in corporate scale data to all stores, even if the jurisdiction data for a 
		-- particular store did not change. 
		
		-- Matt U. for bug 8709 - I'm updating this to insert an 'A' instead of a 'C' into the Queue. 
		-- When an item gets added to a jurisdiction, it is also getting assigned a retail price,
		-- and as a new retail for an existing item, the maintenance gets handled as a zone-level price change
		-- instead of a corporate item change.
		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'A', s.Store_No
		FROM 
			Inserted
		CROSS JOIN 
			Store s
		JOIN 
			StoreItem si on si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE
	        s.WFM_Store = 1 AND si.Authorized = 1
	        AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A')
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE  pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')		
			AND inserted.StoreJurisdictionID = s.StoreJurisdictionID

		SELECT @Error_No = @@ERROR
	END
	
    IF @Error_No <> 0
    BEGIN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_ItemScaleOverrideAdd trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO