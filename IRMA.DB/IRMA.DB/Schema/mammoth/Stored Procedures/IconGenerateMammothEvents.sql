CREATE PROCEDURE mammoth.IconGenerateMammothEvents
	@IdentifiersType dbo.IdentifiersType READONLY,
	@EventTypeName varchar(100),
	@StoreJurisdictionID int = NULL
AS
BEGIN
	-- ===============================================
	-- Check configuration values based on EventTypeID
	-- ===============================================
	DECLARE @ItemLocaleConfigValue varchar(350), @PriceConfigValue varchar(350), @ExcludedStoreNo varchar(250);
	SET @ItemLocaleConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client'));
	SET @PriceConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothPriceChanges','IRMA Client'));
	SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));
	DECLARE @GlobalPriceManagementIdfKey nvarchar(21) = 'GlobalPriceManagement'

	-- Do nothing if the configuration is turned off
	IF (@ItemLocaleConfigValue <> 1 OR @ItemLocaleConfigValue IS NULL) AND @EventTypeName = 'ItemLocaleAddOrUpdate'
		RETURN

	IF (@PriceConfigValue <> 1 OR @PriceConfigValue IS NULL) AND @EventTypeName = 'Price'
		RETURN

	DECLARE @eventTypeId INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = @EventTypeName)

	If (@EventTypeName = 'ItemLocaleAddOrUpdate' OR  @EventTypeName = 'ItemDeauthorization')
		BEGIN
	IF @StoreJurisdictionID is NULL
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
			JOIN Item i ON ii.Item_Key = i.Item_Key
		WHERE
			ii.Remove_Identifier = 0
			AND ii.Deleted_Identifier = 0
			AND i.Deleted_Item = 0
	END
	ELSE
	BEGIN
		INSERT INTO mammoth.ItemLocaleChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
		SELECT 
			ii.Item_Key,
			s.Store_No,
			it.Identifier,
			@eventTypeId
		FROM 
			@IdentifiersType it
			JOIN ValidatedScanCode vsc ON vsc.ScanCode = it.Identifier
			JOIN ItemIdentifier ii ON it.Identifier = ii.Identifier
			JOIN Item i ON ii.Item_Key = i.Item_Key
			CROSS APPLY Store s 
		 WHERE 
			ii.Remove_Identifier = 0
			AND ii.Deleted_Identifier = 0
			AND i.Deleted_Item = 0
			AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
			AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
			AND s.StoreJurisdictionID = @StoreJurisdictionID
			AND s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
	END
END
	ELSE
	If @EventTypeName = 'Price'
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
				JOIN fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf ON s.Store_No = idf.Store_No
			WHERE 
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND (s.WFM_Store = 1 OR s.Mega_Store = 1)
				AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
				AND s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
				AND idf.FlagValue = 0
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[IconGenerateMammothEvents] TO [IConInterface]
    AS [dbo];

GO

