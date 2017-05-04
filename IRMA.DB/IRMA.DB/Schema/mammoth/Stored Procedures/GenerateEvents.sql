
CREATE PROCEDURE [mammoth].[GenerateEvents]
	@IdentifiersType dbo.IdentifiersType READONLY,
	@EventTypeName varchar(100),
	@StoreJurisdictionID int = NULL
AS
BEGIN

	/* =============================================
	Author:		Blake Jones
	Create date: 2016-02-09
	Description:	Generates Mammoth Item Locale and Price
					events. Created specifically for refreshing 
					items through the Mammoth Web Support application.
	CHANGE LOG
	DEV		DATE		TASK			Description
	JA-BJ   08-12-2016  20520:17619		Added price batch detail information for pending price events
	BJ		09-08-2016	VSTS 18173		Filtered out current prices if the item is not authorized for a store
	MZ      09-30-2015  21175:18423     When generating price events for batches in sent status, restrict to price batches only
	BS		12-15-2016  VSTS 19477		Added @StoreJurisdictionID parameter in order to consolidate
										some mammoth event creation stored procedures, namely being able to remove
										mammoth.IconGenerateMammothEvents which is no longer used by the Icon Global Controller
	=============================================*/

	-- ===============================================
	-- Insert TVP into Temp Table
	-- ===============================================
	IF OBJECT_ID('tempdb..#Identifiers') IS NOT NULL
		TRUNCATE TABLE #Identifiers
	ELSE
	BEGIN
		CREATE TABLE #Identifiers (Identifier nvarchar(13));
	END

	INSERT INTO #Identifiers SELECT * FROM @IdentifiersType;

	-- ===============================================
	-- Check configuration values based on EventTypeID
	-- ===============================================
	DECLARE @ItemLocaleConfigValue varchar(350) = (SELECT dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client')), 
			@PriceConfigValue varchar(350) = (SELECT dbo.fn_GetAppConfigValue('MammothPriceChanges','IRMA Client')), 
			@ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client')),
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
		AND ((s.StoreJurisdictionID = @StoreJurisdictionID)
			OR (@StoreJurisdictionID IS NULL))

	DECLARE @eventTypeId INT = (SELECT EventTypeID FROM mammoth.ItemChangeEventType WHERE EventTypeName = @EventTypeName)

	If @EventTypeName = 'ItemLocaleAddOrUpdate'
		BEGIN
			INSERT INTO mammoth.ItemLocaleChangeQueue(Item_Key, Store_No, Identifier, EventTypeID)
		SELECT 
			ii.Item_Key,
			s.Store_No,
			it.Identifier,
			@eventTypeId
		FROM 
			#Identifiers it
			JOIN ValidatedScanCode vsc ON vsc.ScanCode = it.Identifier
			JOIN ItemIdentifier ii ON it.Identifier = ii.Identifier
			JOIN Item i ON ii.Item_Key = i.Item_Key
			JOIN Price p ON i.Item_Key = p.Item_Key
			JOIN #ActiveStores s ON p.Store_No = s.Store_No
			JOIN StoreItem si ON p.Item_Key = si.Item_Key
								AND p.Store_No = si.Store_No
		WHERE
			ii.Remove_Identifier = 0
			AND ii.Deleted_Identifier = 0
			AND i.Deleted_Item = 0
			AND NOT EXISTS 
				(SELECT 1 
				FROM mammoth.ItemLocaleChangeQueue q (nolock)
				WHERE q.Item_Key = ii.Item_Key 
					AND q.Store_No IS NULL
					AND q.Identifier = it.Identifier 
					AND q.EventTypeID = @eventTypeId
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
				it.Identifier,
				NULL,
				@eventTypeId
			FROM 
				#Identifiers it
				JOIN ValidatedScanCode vsc ON vsc.ScanCode = it.Identifier
				JOIN ItemIdentifier ii ON it.Identifier = ii.Identifier
				JOIN Price p ON p.Item_Key = ii.Item_Key
				JOIN #ActiveStores s ON s.Store_No = p.Store_No 
				JOIN StoreItem si on p.Item_Key = si.Item_Key
					AND p.Store_No = si.Store_No
			 WHERE 
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND si.Authorized = 1
			UNION
			SELECT 
				ii.Item_Key,
				s.Store_No,
				it.Identifier,
				pbd.PriceBatchDetailID,
				@eventTypeId
			FROM 
				#Identifiers it
				JOIN ValidatedScanCode vsc ON vsc.ScanCode = it.Identifier
				JOIN ItemIdentifier ii ON it.Identifier = ii.Identifier
				JOIN PriceBatchDetail pbd ON pbd.Item_Key = ii.Item_Key
				JOIN PriceBatchHeader pbh ON pbd.PriceBatchHeaderID = pbh.PriceBatchHeaderID
				JOIN PriceChgType PCT ON PBD.PriceChgTypeID = PCT.PriceChgTypeID
				JOIN #ActiveStores s on pbd.Store_No = s.Store_No
			 WHERE 
				ii.Remove_Identifier = 0
				AND ii.Deleted_Identifier = 0
				AND pbh.PriceBatchStatusID = @sentPriceBatchStatus
				AND ((pbh.PriceChgTypeID is not null)
					OR (pbh.ItemChgTypeID = @NewItemChangeTypeId AND pbd.PriceChgTypeID is not null))
				AND ((pbd.AutoGenerated <> 1 AND pbd.InsertApplication <> 'Sale Off')
					OR (pbd.InsertApplication = 'Sale Off' AND PCT.PriceChgTypeDesc <> 'REG'))
		END

	DROP TABLE #ActiveStores
	DROP TABLE #Identifiers
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[GenerateEvents] TO [IConInterface]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[GenerateEvents] TO [MammothRole]
    AS [dbo];

