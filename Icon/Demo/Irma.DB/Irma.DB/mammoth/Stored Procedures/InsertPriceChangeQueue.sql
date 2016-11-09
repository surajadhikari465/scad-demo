
CREATE PROCEDURE [mammoth].[InsertPriceChangeQueue]
	@PriceBatchHeaderID int,
    @PriceBatchStatusID tinyint,
	@Identifier varchar(50) = null
AS
--**************************************************************************************
-- Modification History:
-- Date       	Init  	TFS   		Comment
-- 2016-01-12	DN		18115		Added a JOIN to ValidatedScanCode table to filter out
--									non-validated items.
--**************************************************************************************

BEGIN
    SET NOCOUNT ON

	DECLARE @EventTypeID int = 0
	DECLARE @IsRollback	BIT = 0
	DECLARE @PriceConfigValue VARCHAR(350);
	SET @PriceConfigValue = (SELECT dbo.fn_GetAppConfigValue('MammothPriceChanges','IRMA Client'));
	DECLARE @ExcludedStoreNo varchar(250) = (SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

	---- If identifier is passed in insert event for that identifier only(happens usually when alternate identifier gets added and there wont be any PDB information)..
	-- Otherwise insert events for all identifiers for the passed in PDB Header info (as part of price batching..identifier will be blank)

	IF CONVERT(BIT,@PriceConfigValue) = 1
		BEGIN
		-- Rollback to Package status
			IF	@PriceBatchStatusID = 2 AND 
				EXISTS 
				(SELECT * 
				FROM PriceBatchHeader pbh (NOLOCK) 
				WHERE pbh.PriceBatchHeaderID = @PriceBatchHeaderID AND 
				pbh.PriceBatchStatusID = 5) 
		
				BEGIN
					SET @EventTypeID = (SELECT EventTypeID FROM mammoth.ItemChangeEventType (NOLOCK) WHERE EventTypeName = 'PriceRollback')
					SET @IsRollback = 1
				END

			-- Sent status
			IF @PriceBatchStatusID = 5 
				SET @EventTypeID = (SELECT EventTypeID FROM mammoth.ItemChangeEventType (NOLOCK) WHERE EventTypeName = 'Price')

			IF	@PriceBatchStatusID = 5 OR 
					(@PriceBatchStatusID = 2 AND @IsRollback = 1)
					BEGIN
						INSERT INTO mammoth.PriceChangeQueue 
						(	Item_Key,
							Store_No,
							Identifier,
							EventTypeID,
							EventReferenceID,
							InsertDate
						)
						SELECT 
							PBD.Item_Key,
							PBD.Store_No,
							PBD.Identifier,
							@EventTypeID AS EventTypeID,
							PBD.PriceBatchDetailID,
							GETDATE() AS InsertDate
						FROM PriceBatchDetail PBD (NOLOCK) INNER JOIN PriceBatchHeader PBH (NOLOCK)
						ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
						INNER JOIN ValidatedScanCode VSC (NOLOCK)
						ON PBD.Identifier = VSC.ScanCode 
						WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID 
							AND PBH.PriceChgTypeID IS NOT NULL
							AND PBD.Store_No not in (SELECT Key_Value FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|'))
					END			
		END

		 SET NOCOUNT OFF
END

print '[' + convert(nvarchar, getdate(), 121) + '] ' + 'Finish: [mammoth.InsertPriceChangeQueue.sql]'

GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[InsertPriceChangeQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[InsertPriceChangeQueue] TO [IRSUser]
    AS [dbo];

