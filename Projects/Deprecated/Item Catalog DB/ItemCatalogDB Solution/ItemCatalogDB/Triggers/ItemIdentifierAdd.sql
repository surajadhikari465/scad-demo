IF EXISTS (SELECT name 
	   FROM   sysobjects 
	   WHERE  name = N'ItemIdentifierAdd' 
	   AND 	  type = 'TR')
    DROP TRIGGER ItemIdentifierAdd
GO

CREATE TRIGGER ItemIdentifierAdd
ON ItemIdentifier
FOR INSERT 
AS 
BEGIN
    BEGIN TRY
		-- 365 non-scale PLUs which are marked Send to Scale should generate Add maintenance for 365 stores only.
		declare @Is365NonScalePlu bit =		case 
												when exists (select icfs.Item_Key from ItemCustomerFacingScale icfs join inserted on icfs.Item_Key = inserted.Item_Key where icfs.SendToScale = 1) then 1
												else 0
											end

		DELETE 
			PLUMCorpChgQueue
		FROM 
			PLUMCorpChgQueue
			INNER JOIN Inserted ON Inserted.Item_Key = PLUMCorpChgQueue.Item_Key
		WHERE 
			Inserted.Scale_Identifier = 1
			OR @Is365NonScalePlu = 1

		INSERT INTO 
			PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
		SELECT 
			Inserted.Item_Key, 'A', s.Store_No
		FROM 
			Inserted
			CROSS JOIN Store s
			JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
		WHERE 
			Inserted.Scale_Identifier = 1 
			AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
			AND si.Authorized = 1 
			AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
			AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
		
		if @Is365NonScalePlu = 1
			begin
				INSERT INTO 
					PLUMCorpChgQueue (Item_Key, ActionCode, Store_No)
				SELECT 
					Inserted.Item_Key, 'A', s.Store_No
				FROM 
					Inserted
					CROSS JOIN Store s
					INNER JOIN StoreItem si ON si.Item_Key = Inserted.Item_Key AND si.Store_No = s.Store_No
				WHERE 
					(s.WFM_Store = 0 AND s.Mega_Store = 1)
					AND si.Authorized = 1 
					AND NOT EXISTS (SELECT pq.item_key FROM PlumCorpChgQueue pq (NOLOCK) WHERE pq.Item_Key = Inserted.Item_Key AND pq.store_no = si.store_no AND pq.ActionCode = 'A') 
					AND NOT EXISTS (SELECT pqt.item_key FROM PlumCorpChgQueueTmp pqt (NOLOCK) WHERE pqt.Item_Key = Inserted.Item_Key AND pqt.store_no = si.store_no AND pqt.ActionCode = 'A')
			end
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT <> 0 ROLLBACK TRAN

		DECLARE 
			@ErrorMessage NVARCHAR(MAX) = 'ItemIdentifierAdd trigger failed with error: ' + ERROR_MESSAGE(),
			@ErrorSeverity INT = ERROR_SEVERITY(),
			@ErrorState INT = ERROR_STATE()

		RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
	END CATCH
END
GO
