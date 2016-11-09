﻿
CREATE PROCEDURE mammoth.GenerateEvents
	@IdentifiersType dbo.IdentifiersType READONLY,
	@EventTypeName varchar(100)
AS
BEGIN
	-- ===============================================
	-- Check configuration values based on EventTypeID
	-- ===============================================
	DECLARE @ItemLocaleConfigValue varchar(350), @PriceConfigValue varchar(350), @ExcludedStoreNo varchar(250);
	SET @ItemLocaleConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client'));
	SET @PriceConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothPriceChanges','IRMA Client'));
	SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

	-- Do nothing if the configuration is turned off
	IF (@ItemLocaleConfigValue <> 1 OR @ItemLocaleConfigValue IS NULL) AND @EventTypeName = 'ItemLocaleAddOrUpdate'
		RETURN

	IF (@PriceConfigValue <> 1 OR @PriceConfigValue IS NULL) AND @EventTypeName = 'Price'
		RETURN

	DECLARE @eventTypeId INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = @EventTypeName)

	If @EventTypeName = 'ItemLocaleAddOrUpdate'
		BEGIN
			INSERT INTO mammoth.ItemLocaleChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
		SELECT 
			ii.Item_Key,
			NULL,
			it.Identifier,
			@eventTypeId
		FROM 
			@IdentifiersType it
			JOIN ValidatedScanCode vsc ON vsc.ScanCode = it.Identifier
			JOIN ItemIdentifier ii ON it.Identifier = ii.Identifier
		WHERE
			ii.Remove_Identifier = 0
			AND ii.Deleted_Identifier = 0
			AND NOT EXISTS 
				(SELECT 1 
				FROM mammoth.ItemLocaleChangeQueue q
				WHERE q.Item_Key = ii.Item_Key 
					AND q.Store_No IS NULL
					AND q.Identifier = it.Identifier 
					AND q.EventTypeID = @eventTypeId
					AND q.InProcessBy IS NULL
					AND q.ProcessFailedDate IS NULL)
		END
	ELSE If @EventTypeName = 'Price'
		BEGIN
			INSERT INTO mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
			SELECT 
				ii.Item_Key,
				s.Store_No,
				it.Identifier,
				@eventTypeId
			FROM 
				@IdentifiersType it
				JOIN ValidatedScanCode vsc ON vsc.ScanCode = it.Identifier
				JOIN ItemIdentifier ii ON it.Identifier = ii.Identifier
				JOIN Price p on p.Item_key = ii.Item_key
				JOIN Store s on s.Store_No = p.Store_No 
			 WHERE 
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
				AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
				AND s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
		END
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [mammoth.GenerateEvents.sql]'
GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[GenerateEvents] TO [MammothRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[GenerateEvents] TO [IConInterface]
    AS [dbo];

