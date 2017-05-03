IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'Scale_ItemScale_Add')
	BEGIN
		PRINT 'Dropping Trigger Scale_ItemScale_Add'
		DROP  Trigger Scale_ItemScale_Add
	END
GO

PRINT 'Creating Trigger Scale_ItemScale_Add'
GO
CREATE Trigger Scale_ItemScale_Add 
ON ItemScale
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0

    -- Queue for PlumCorpChg
    INSERT INTO 
		PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
    SELECT 
		Inserted.Item_Key, 'A', s.Store_No
    FROM 
		Inserted
	CROSS JOIN 
		Store s
	JOIN 
		StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
	WHERE 
		s.WFM_Store = 1 AND si.Authorized = 1
		AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND ActionCode = 'A')
		AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND ActionCode = 'A')

		

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('Scale_ItemScale_Add trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO