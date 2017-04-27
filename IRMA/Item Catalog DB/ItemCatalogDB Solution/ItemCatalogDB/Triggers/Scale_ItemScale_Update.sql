IF EXISTS (SELECT name FROM sysobjects WHERE name = N'Scale_ItemScale_Update' AND type = 'TR')
    DROP TRIGGER [Scale_ItemScale_Update]
GO

CREATE Trigger [dbo].[Scale_ItemScale_Update] 
ON [dbo].[ItemScale]
FOR UPDATE
AS
BEGIN
    DECLARE @Error_No int = 0
    DECLARE @UseSmartX INT = (SELECT dbo.fn_InstanceDataValue('UseSmartXPriceData', NULL))

	-- 365 non-scale PLU maintenance is not handled here.  Check GenerateCustomerFacingScaleMaintenance.
    DECLARE @Is365NonScalePlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key) then 1 else 0 end
	DECLARE @Is365CFSPlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key and icfs.SendToScale = 1) then 1 else 0 end
    
    IF @UseSmartX = 0
        -- For regions not using SmartX scale writer, all scale item changes
        -- should be captured.
        BEGIN
            INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        Inserted.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
		    CROSS JOIN 
				Store s
		    JOIN 
				StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
	        WHERE
		        ((@Is365NonScalePlu = 0 AND (s.WFM_Store = 1 OR s.Mega_Store = 1))
				OR (@Is365CFSPlu = 1 AND (s.Mega_Store = 1)))
				AND si.Authorized = 1 
				AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode like '[AC]')
        END
    ELSE
        BEGIN
            INSERT INTO 
		        PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
            SELECT 
		        Inserted.Item_Key, 'C', s.Store_No
            FROM 
		        Inserted
            INNER JOIN 
				Deleted ON Deleted.Item_Key = Inserted.Item_Key
            CROSS JOIN 
				Store s
            JOIN 
				StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
	        WHERE
               (ISNULL(Inserted.Scale_Tare_ID,-1)             <> ISNULL(Deleted.Scale_Tare_ID,-1)             OR
	            ISNULL(Inserted.Scale_Alternate_Tare_ID,-1)   <> ISNULL(Deleted.Scale_Alternate_Tare_ID,-1)   OR
	            ISNULL(Inserted.Scale_LabelStyle_ID,-1)       <> ISNULL(Deleted.Scale_LabelStyle_ID,-1)       OR
	            ISNULL(Inserted.Scale_EatBy_ID,-1)            <> ISNULL(Deleted.Scale_EatBy_ID,-1)            OR
	            ISNULL(Inserted.Scale_Grade_ID,-1)            <> ISNULL(Deleted.Scale_Grade_ID,-1)            OR
	            ISNULL(Inserted.Scale_RandomWeightType_ID,-1) <> ISNULL(Deleted.Scale_RandomWeightType_ID,-1) OR
	            ISNULL(Inserted.Scale_ScaleUOMUnit_ID,-1)     <> ISNULL(Deleted.Scale_ScaleUOMUnit_ID,-1)     OR
	            ISNULL(Inserted.Scale_FixedWeight,-1)         <> ISNULL(Deleted.Scale_FixedWeight,-1)         OR
	            ISNULL(Inserted.Scale_ByCount,-1)             <> ISNULL(Deleted.Scale_ByCount,-1)             OR
       			ISNULL(Inserted.Scale_ExtraText_ID,-1)        <> ISNULL(Deleted.Scale_ExtraText_ID,-1)        OR
       			ISNULL(Inserted.Nutrifact_ID,-1)              <> ISNULL(Deleted.Nutrifact_ID,-1)              OR
				ISNULL(Inserted.Scale_Ingredient_ID,-1)       <> ISNULL(Deleted.Scale_Ingredient_ID,-1)       OR
				ISNULL(Inserted.Scale_Allergen_ID,-1)         <> ISNULL(Deleted.Scale_Allergen_ID,-1)         OR
	            ISNULL(Inserted.ForceTare,-1)                 <> ISNULL(Deleted.ForceTare,-1)                 OR
	            ISNULL(Inserted.PrintBlankShelfLife,-1)       <> ISNULL(Deleted.PrintBlankShelfLife,-1)       OR
	            ISNULL(Inserted.PrintBlankEatBy,-1)           <> ISNULL(Deleted.PrintBlankEatBy,-1)           OR
	            ISNULL(Inserted.PrintBlankPackDate,-1)        <> ISNULL(Deleted.PrintBlankPackDate,-1)        OR
	            ISNULL(Inserted.PrintBlankWeight,-1)          <> ISNULL(Deleted.PrintBlankWeight,-1)          OR
	            ISNULL(Inserted.PrintBlankUnitPrice,-1)       <> ISNULL(Deleted.PrintBlankUnitPrice,-1)       OR
	            ISNULL(Inserted.PrintBlankTotalPrice,-1)      <> ISNULL(Deleted.PrintBlankTotalPrice,-1)      OR
	            ISNULL(Inserted.Scale_Description1,-1)        <> ISNULL(Deleted.Scale_Description1,-1)        OR
	            ISNULL(Inserted.Scale_Description2,-1)        <> ISNULL(Deleted.Scale_Description2,-1)        OR
	            ISNULL(Inserted.Scale_Description3,-1)        <> ISNULL(Deleted.Scale_Description3,-1)        OR
	            ISNULL(Inserted.Scale_Description4,-1)        <> ISNULL(Deleted.Scale_Description4,-1)        OR
	            ISNULL(Inserted.ShelfLife_Length,-1)          <> ISNULL(Deleted.ShelfLife_Length,-1))        AND
		        
				(@Is365NonScalePlu = 0 AND (s.WFM_Store = 1 OR s.Mega_Store = 1))
				AND si.Authorized = 1
		        AND NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode like '[AC]') 
				AND NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode like '[AC]')
        END

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('Scale_ItemScale_Update trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
END
