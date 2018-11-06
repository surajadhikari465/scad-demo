IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'Scale_ItemScale_Update')
	BEGIN
		PRINT 'Dropping Trigger Scale_ItemScale_Update'
		DROP  Trigger Scale_ItemScale_Update
	END
GO

PRINT 'Creating Trigger Scale_ItemScale_Update'
GO
CREATE Trigger Scale_ItemScale_Update 
ON ItemScale
FOR UPDATE
AS
BEGIN
    DECLARE @Error_No int
    DECLARE @UseSmartX INT
        
    SELECT @Error_No = 0
    
    SELECT @UseSmartX = dbo.fn_InstanceDataValue('UseSmartXPriceData', NULL)
    
    -- Queue for PlumCorpChg
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
		        s.WFM_Store = 1 AND si.Authorized = 1 AND
		        NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.item_key = si.store_no AND pq.ActionCode = 'A') AND
				NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
        END
    ELSE
        -- For regions using SmartX (MA), merely adding, changing, or removing an existing Extra Text association
        -- should not trigger a scale item change; all other scale item changes should.
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
		        s.WFM_Store = 1 AND si.Authorized = 1 AND
		        NOT EXISTS (SELECT * FROM PlumCorpChgQueue WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C') AND
				NOT EXISTS (SELECT * FROM PlumCorpChgQueueTmp WHERE Item_Key = Inserted.Item_Key AND ActionCode = 'C')
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

GO