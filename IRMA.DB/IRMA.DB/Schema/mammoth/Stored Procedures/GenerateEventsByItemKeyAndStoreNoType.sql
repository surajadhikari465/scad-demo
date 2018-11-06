CREATE PROCEDURE mammoth.GenerateEventsByItemKeyAndStoreNoType
	@ItemKeyAndStoreNoType dbo.ItemKeyAndStoreNoType READONLY,
	@EventTypeName varchar(100)
AS
BEGIN

	/* =============================================
	Author:		Blake Jones
	Create date: 2017-12-14
	Description:	Generates Mammoth Item Locale and Price
					events .
	=============================================*/

	-- ===============================================
	-- Insert TVP into Temp Table
	-- ===============================================
	IF OBJECT_ID('tempdb..#ItemKeyAndStoreNoType') IS NOT NULL
		TRUNCATE TABLE #ItemKeyAndStoreNoType
	ELSE
	BEGIN
		CREATE TABLE #ItemKeyAndStoreNoType (
			Item_Key INT NOT NULL,
			Store_No INT NOT NULL);
	END

	INSERT INTO #ItemKeyAndStoreNoType 
	SELECT Item_Key,
		Store_No
	FROM @ItemKeyAndStoreNoType

	-- ===============================================
	-- Check configuration values based on EventTypeID
	-- ===============================================
	DECLARE @ItemLocaleConfigValue varchar(350) = (SELECT dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client')), 
			@PriceConfigValue varchar(350) = (SELECT dbo.fn_GetAppConfigValue('MammothPriceChanges','IRMA Client')), 
			@ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client')),
			@GlobalPriceManagementIdfKey nvarchar(21) = 'GlobalPriceManagement',
			@NewItemChangeTypeId int = (select ItemChgTypeID from ItemChgType where ItemChgTypeDesc = 'New');

	-- Do nothing if the configuration is turned off
	IF (@ItemLocaleConfigValue <> 1 OR @ItemLocaleConfigValue IS NULL) AND @EventTypeName = 'ItemLocaleAddOrUpdate'
		RETURN

	IF (@PriceConfigValue <> 1 OR @PriceConfigValue IS NULL) AND @EventTypeName = 'Price'
		RETURN

	-- ===============================================
	-- Populate store numbers into ActiveStores table
	-- ===============================================
	IF OBJECT_ID('tempdb..#ActiveStores') IS NOT NULL
		TRUNCATE TABLE #ActiveStores
	ELSE
	BEGIN
		CREATE TABLE #ActiveStores
		(
			Store_No int PRIMARY KEY
		)
	END

	INSERT INTO #ActiveStores
	SELECT Store_No
	FROM Store s
	WHERE
		s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
		AND (s.WFM_Store = 1 OR s.Mega_Store = 1 )
		AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)

	DECLARE @eventTypeId INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = @EventTypeName)

	If @EventTypeName = 'ItemLocaleAddOrUpdate'
		BEGIN
			INSERT INTO mammoth.ItemLocaleChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
			SELECT 
				ii.Item_Key,
				s.Store_No,
				ii.Identifier,
				@eventTypeId
			FROM 
				#ItemKeyAndStoreNoType t
				JOIN Item i ON t.Item_Key = i.Item_Key
				JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
				JOIN StoreItem si ON t.Item_Key = si.Item_Key
					AND t.Store_No = si.Store_No
				JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
				JOIN #ActiveStores s ON si.Store_No = s.Store_No
			WHERE
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND i.Deleted_Item = 0
				AND NOT EXISTS 
					(SELECT 1 
					FROM mammoth.ItemLocaleChangeQueue q (nolock)
					WHERE q.Item_Key = ii.Item_Key 
						AND (q.Store_No IS NULL OR q.Store_No = s.Store_No)
						AND q.Identifier = ii.Identifier
						AND q.InProcessBy IS NULL
						AND q.ProcessFailedDate IS NULL)
		END
	ELSE If @EventTypeName = 'Price'
		BEGIN
			DECLARE @sentPriceBatchStatus INT = (SELECT PriceBatchStatusID FROM PriceBatchStatus WHERE PriceBatchStatusDesc = 'Sent')

			INSERT INTO mammoth.PriceChangeQueue(Item_Key, Store_No, Identifier, EventReferenceID, EventTypeID)
			SELECT 
				ii.Item_Key,
				s.Store_No,
				ii.Identifier,
				NULL,
				@eventTypeId
			FROM 
				#ItemKeyAndStoreNoType t
				JOIN #ActiveStores s ON t.Store_No = s.Store_No
				JOIN Price p ON t.Item_Key = p.Item_Key
					and t.Store_No = p.Store_No 
				JOIN StoreItem si on p.Item_Key = si.Item_Key
					AND p.Store_No = si.Store_No
				JOIN ItemIdentifier ii ON t.Item_Key = ii.Item_Key 
				JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
				JOIN fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf ON si.Store_No = idf.Store_No
			WHERE 
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND si.Authorized = 1
				AND NOT EXISTS 
					(SELECT 1 
					FROM mammoth.PriceChangeQueue q (nolock)
					WHERE q.Item_Key = ii.Item_Key 
						AND q.Store_No = s.Store_No
						AND q.Identifier = ii.Identifier 
						AND q.EventTypeID = @eventTypeId
						AND q.InProcessBy IS NULL
						AND q.ProcessFailedDate IS NULL)
				AND idf.FlagValue = 0
			UNION
			SELECT 
				ii.Item_Key,
				s.Store_No,
				ii.Identifier,
				pbd.PriceBatchDetailID,
				@eventTypeId
			FROM 
				#ItemKeyAndStoreNoType t
				JOIN #ActiveStores s on t.Store_No = s.Store_No
				JOIN ItemIdentifier ii ON t.Item_Key = ii.Item_Key
				JOIN ValidatedScanCode vsc ON vsc.ScanCode = ii.Identifier
				JOIN PriceBatchDetail pbd ON pbd.Item_Key = t.Item_Key
					AND pbd.Store_No = t.Store_No
				JOIN PriceBatchHeader pbh ON pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
				JOIN PriceChgType PCT ON PBD.PriceChgTypeID = PCT.PriceChgTypeID
				JOIN fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf ON s.Store_No = idf.Store_No
			WHERE 
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND pbh.PriceBatchStatusID = @sentPriceBatchStatus
				AND ((pbh.PriceChgTypeID is not null)
					OR (pbh.ItemChgTypeID = @NewItemChangeTypeId AND pbd.PriceChgTypeID is not null))
				AND ((pbd.AutoGenerated <> 1 AND pbd.InsertApplication <> 'Sale Off')
					OR (pbd.InsertApplication = 'Sale Off' AND PCT.PriceChgTypeDesc <> 'REG'))
				AND NOT EXISTS 
					(SELECT 1 
					FROM mammoth.PriceChangeQueue q (nolock)
					WHERE q.Item_Key = ii.Item_Key 
						AND q.Store_No = s.Store_No
						AND q.Identifier = ii.Identifier 
						AND q.EventTypeID = @eventTypeId
						AND q.InProcessBy IS NULL
						AND q.ProcessFailedDate IS NULL)
				AND idf.FlagValue = 0
		END

	DROP TABLE #ActiveStores
	DROP TABLE #ItemKeyAndStoreNoType
END

GO
GRANT EXECUTE
    ON OBJECT::mammoth.GenerateEventsByItemKeyAndStoreNoType TO IRMAClientRole
    AS dbo;