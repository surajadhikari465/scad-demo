IF EXISTS (SELECT name FROM sysobjects WHERE name = N'Scale_ItemScale_Add' AND type = 'TR')
    DROP TRIGGER [Scale_ItemScale_Add]
GO

CREATE Trigger [dbo].[Scale_ItemScale_Add] 
ON [dbo].[ItemScale]
FOR INSERT
AS
BEGIN
    DECLARE @Error_No int = 0
	
	-- 365 non-scale PLU maintenance is not handled here.  Check GenerateCustomerFacingScaleMaintenance.
    DECLARE @Is365NonScalePlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key) then 1 else 0 end
	DECLARE @Is365CFSPlu bit = case when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key and icfs.SendToScale = 1) then 1 else 0 end
    
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
		(@Is365NonScalePlu = 0 AND (s.WFM_Store = 1 OR s.Mega_Store = 1))
		AND si.Authorized = 1 
		AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
		AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
    UNION
	SELECT 
		Inserted.Item_Key, 'A', s.Store_No
    FROM 
		Inserted
	CROSS JOIN 
		Store s
	JOIN 
		StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
	WHERE 
		(@Is365CFSPlu = 1 AND s.Mega_Store = 1)
		AND si.Authorized = 1 
		AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
		AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')

    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
		BEGIN
			ROLLBACK TRAN
			DECLARE @Severity smallint
			SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
			RAISERROR ('Scale_ItemScale_Add trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
		END
END