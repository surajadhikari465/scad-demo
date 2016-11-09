
CREATE PROCEDURE [mammoth].[InsertItemLocaleChangeQueue]
	@Item_Key int,
	@Store_No int = NULL,
	@EventType varchar(50),
	@Identifier varchar(50) = NULL,
	@StoreJurisdictionID int = NULL
AS
BEGIN	
SET NOCOUNT ON
	-- ===============================================
	-- Declare Variables
	-- ===============================================
	DECLARE @eventTypeId int;
	SET @eventTypeId = (SELECT t.EventTypeID FROM mammoth.ItemChangeEventType t WHERE t.EventTypeName = @EventType)

	-- ===============================================
	-- Check configuration values based on EventTypeID
	-- ===============================================
	DECLARE @ItemLocaleConfigValue varchar(350), @ExcludedStoreNo varchar(250);
	SET @ItemLocaleConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client'));
	SET @ExcludedStoreNo = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

	-- Do nothing if the configuration is turned off
	IF @ItemLocaleConfigValue <> 1 OR @ItemLocaleConfigValue IS NULL
		RETURN

	-- Do nothing if the store number is in the list of excluded store numbers
	If EXISTS (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|') WHERE Key_Value = @Store_No)
		return
		
	-- ===============================================
	-- Insert Event into Queue table
	-- ===============================================
	-- If identifier is passed in insert event for that identifier only(happens usually when alternate identifier gets deleted).. otherwise insert events for all identifiers for the passed in item key.
	IF(@Identifier IS NOT NULL)
	BEGIN
		IF (@StoreJurisdictionID IS NULL)
			BEGIN
				INSERT INTO [mammoth].[ItemLocaleChangeQueue]
				(
					Item_Key,
					Store_No,
					Identifier,
					EventTypeID,
					EventReferenceID,
					InsertDate
				)
				SELECT
					@Item_Key			as Item_Key,
					@Store_No			as Store_No,
					@Identifier		    as Identifier,
					@eventTypeId		as EventTypeID,
					NULL				as EventReferenceId,
					GETDATE()	
				FROM
					ValidatedScanCode vsc
				WHERE
					vsc.ScanCode = @Identifier
					AND NOT EXISTS (SELECT 1 
									FROM mammoth.ItemLocaleChangeQueue q (nolock)
									WHERE q.Item_Key = @Item_Key 
										AND ((q.Store_No = @Store_No AND @Store_No IS NOT NULL)
											OR (q.Store_No IS NULL))
										AND q.Identifier = @Identifier 
										AND q.EventTypeID = @eventTypeId
										AND q.InProcessBy IS NULL
										AND q.ProcessFailedDate IS NULL)
			END
		ELSE IF (@StoreJurisdictionID IS NOT NULL)
			BEGIN
				INSERT INTO [mammoth].[ItemLocaleChangeQueue]
				(
					Item_Key,
					Store_No,
					Identifier,
					EventTypeID,
					EventReferenceID,
					InsertDate
				)
				SELECT
					@Item_Key			as Item_Key,
					s.Store_No			as Store_No,
					@Identifier		    as Identifier,
					@eventTypeId		as EventTypeID,
					NULL				as EventReferenceId,
					GETDATE()	
				FROM
					ValidatedScanCode vsc
					JOIN ItemIdentifier ii  on vsc.ScanCode = ii.Identifier
					CROSS APPLY Store s  
				WHERE
					vsc.ScanCode = @Identifier
					AND ii.Deleted_Identifier = 0 
					AND ii.Remove_Identifier = 0
					AND s.StoreJurisdictionID = @StoreJurisdictionID
					AND  (s.WFM_Store = 1 OR s.Mega_Store = 1 )
					AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
					AND s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
					AND NOT EXISTS (SELECT 1 
									FROM mammoth.ItemLocaleChangeQueue q (nolock)
									WHERE q.Item_Key = @Item_Key 
										AND q.Store_No = s.Store_No
										AND q.Identifier = @Identifier 
										AND q.EventTypeID = @eventTypeId
										AND q.InProcessBy IS NULL
										AND q.ProcessFailedDate IS NULL)
			END
	END
	ELSE
	IF(@Identifier IS NULL) 
	BEGIN
		IF (@StoreJurisdictionID IS NULL)
			BEGIN
				INSERT INTO [mammoth].[ItemLocaleChangeQueue]
				(
					Item_Key,
					Store_No,
					Identifier,
					EventTypeID,
					EventReferenceID
				)
				SELECT
					@Item_Key			as Item_Key,
					@Store_No			as Store_No,
					ii.Identifier		as Identifier,
					@eventTypeId		as EventTypeID,
					NULL				as EventReferenceId
				FROM
					dbo.ItemIdentifier	ii
					JOIN dbo.Item		i	on ii.Item_Key = i.Item_Key
					JOIN ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
				WHERE
					ii.Item_Key = @Item_Key
					AND ii.Deleted_Identifier = 0
					AND ii.Remove_Identifier = 0
					AND NOT EXISTS (SELECT 1 
									FROM mammoth.ItemLocaleChangeQueue q (nolock)
									WHERE q.Item_Key = @Item_Key 
										AND ((q.Store_No = @Store_No AND @Store_No IS NOT NULL)
											OR (q.Store_No IS NULL))
										AND q.Identifier = ii.Identifier 
										AND q.EventTypeID = @eventTypeId
										AND q.InProcessBy IS NULL
										AND q.ProcessFailedDate IS NULL)
			END		
		ELSE IF(@StoreJurisdictionID IS NOT NULL)
			BEGIN
				INSERT INTO [mammoth].[ItemLocaleChangeQueue]
				(
					Item_Key,
					Store_No,
					Identifier,
					EventTypeID,
					EventReferenceID
				)
				SELECT
					@Item_Key			as Item_Key,
					s.Store_No			as Store_No,
					ii.Identifier		as Identifier,
					@eventTypeId		as EventTypeID,
					NULL				as EventReferenceId
				FROM
					dbo.ItemIdentifier	ii
					JOIN dbo.Item		i	on ii.Item_Key = i.Item_Key
					JOIN ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
					CROSS APPLY Store			s 
				WHERE
					ii.Item_Key = @Item_Key
					AND ii.Deleted_Identifier = 0
					AND ii.Remove_Identifier = 0
					AND  s.StoreJurisdictionID = @StoreJurisdictionID
					AND  (s.WFM_Store = 1 OR s.Mega_Store = 1 )
					AND (Internal = 1 AND BusinessUnit_ID IS NOT NULL)
					AND s.Store_No not in (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
					AND NOT EXISTS (SELECT 1
									FROM mammoth.ItemLocaleChangeQueue q (nolock)
									WHERE q.Item_Key = @Item_Key 
										AND q.Store_No = s.Store_No
										AND q.Identifier = ii.Identifier
										AND q.EventTypeID = @eventTypeId
										AND q.InProcessBy IS NULL
										AND q.ProcessFailedDate IS NULL)
			END
	END
	SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[InsertItemLocaleChangeQueue] TO [IRMAClientRole]
    AS [dbo];

