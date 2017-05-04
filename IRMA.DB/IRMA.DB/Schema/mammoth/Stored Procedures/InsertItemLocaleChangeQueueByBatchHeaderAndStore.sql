
CREATE PROCEDURE [mammoth].[InsertItemLocaleChangeQueueByBatchHeaderAndStore]
	@PriceBatchHeaderID int,
	@Store_No int
AS
BEGIN	

	DECLARE 
		@now datetime,
		@ItemLocaleConfigValue varchar(350),
		@ExcludedStoreNo varchar(250),
		@eventTypeId int;
		
	-- =========================================================
	-- Check configuration values to see if events are turned on
	-- =========================================================
	SET @now = GETDATE();
	SET @eventTypeId = (SELECT EventTypeID FROM mammoth.ItemChangeEventType t WHERE t.EventTypeName = 'ItemLocaleAddOrUpdate')
	SET @ItemLocaleConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client'));
	SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

	-- Do nothing if the configuration is turned off
	IF @ItemLocaleConfigValue <> 1 OR @ItemLocaleConfigValue IS NULL
		RETURN

	-- Do nothing if the store number is in the list of excluded store numbers
	If EXISTS (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|') WHERE Key_Value = @Store_No)
		return

	-- =========================================================
	-- Perform Insert into Mammoth ItemLocale Event Queue Table
	-- =========================================================
	INSERT INTO mammoth.ItemLocaleChangeQueue
	(
		EventTypeID,
		Identifier,
		InsertDate,
		Item_Key,
		Store_No
	)
	SELECT
		@eventTypeId	as EventTypeID,
		ii.Identifier	as Identifier,
		@now			as InsertDate,
		pbd.Item_Key	as Item_Key,
		@Store_No		as Store_No
	FROM
		PriceBatchDetail		pbd
		JOIN ItemChgType		it	on pbd.ItemChgTypeID = it.ItemChgTypeID
		JOIN ItemIdentifier		ii	on pbd.Item_Key = ii.Item_Key
		JOIN ValidatedScanCode	v	on ii.Identifier = v.ScanCode
	WHERE
		pbd.PriceBatchHeaderID = @PriceBatchHeaderID
		AND pbd.Store_No = @Store_No
		AND ii.Deleted_Identifier = 0
		AND ii.Remove_Identifier = 0
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[InsertItemLocaleChangeQueueByBatchHeaderAndStore] TO [IRMAClientRole]
    AS [dbo];

