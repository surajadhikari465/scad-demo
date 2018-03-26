﻿CREATE PROCEDURE [mammoth].[InsertPriceChangeQueue] @PriceBatchHeaderID INT
	,@PriceBatchStatusID TINYINT
	,@Identifier VARCHAR(50) = NULL
AS
--**************************************************************************************
-- Modification History:
-- Date       	Init  	TFS   		Comment
-- 2016-01-12	DN		18115		Added a JOIN to ValidatedScanCode table to filter out
--									non-validated items.
-- 2016-02-26	BS		14119		Filtered out Auto Generated Sale Off
-- 2017-09-25	BJ		VSTS 23436	No longer generate price events if GlobalPriceManagement is active
--**************************************************************************************
BEGIN
	SET NOCOUNT ON

	DECLARE @EventTypeID INT = 0
	DECLARE @IsRollback BIT = 0
	DECLARE @PriceConfigValue VARCHAR(350);

	SET @PriceConfigValue = (
			SELECT dbo.fn_GetAppConfigValue('MammothPriceChanges', 'IRMA Client')
			);

	DECLARE @ExcludedStoreNo VARCHAR(250) = (
			SELECT dbo.fn_GetAppConfigValue('LabAndClosedStoreNo', 'IRMA Client')
			);
	DECLARE @NewItemChangeTypeId INT = (
			SELECT ItemChgTypeID
			FROM ItemChgType
			WHERE ItemChgTypeDesc = 'New'
			);
	DECLARE @GlobalPriceManagementIdfKey NVARCHAR(21) = 'GlobalPriceManagement'
	DECLARE @MammothPriceEventTypeID INT = (SELECT EventTypeID
											FROM mammoth.ItemChangeEventType(NOLOCK)
											WHERE EventTypeName = 'Price')

	---- If identifier is passed in insert event for that identifier only(happens usually when alternate identifier gets added and there wont be any PDB information)..
	-- Otherwise insert events for all identifiers for the passed in PDB Header info (as part of price batching..identifier will be blank)
	IF CONVERT(BIT, @PriceConfigValue) = 1
	BEGIN
		DECLARE @GlobalPriceManagementActive BIT

		SELECT DISTINCT @GlobalPriceManagementActive = idf.FlagValue
		FROM PriceBatchDetail pbd 
		JOIN fn_GetInstanceDataFlagStoreValues(@GlobalPriceManagementIdfKey) idf ON pbd.Store_No = idf.Store_No
		WHERE pbd.PriceBatchHeaderID = @PriceBatchHeaderID

		IF @GlobalPriceManagementActive = 0
		BEGIN
			-- Rollback to Package status
			IF @PriceBatchStatusID = 2
				AND EXISTS (
					SELECT *
					FROM PriceBatchHeader pbh(NOLOCK)
					WHERE pbh.PriceBatchHeaderID = @PriceBatchHeaderID
						AND pbh.PriceBatchStatusID = 5
					)
			BEGIN
				SET @EventTypeID = (
						SELECT EventTypeID
						FROM mammoth.ItemChangeEventType(NOLOCK)
						WHERE EventTypeName = 'PriceRollback'
						)
				SET @IsRollback = 1
			END

			-- Sent status
			IF @PriceBatchStatusID = 5
				SET @EventTypeID = @MammothPriceEventTypeID

			IF @PriceBatchStatusID = 5
				OR (
					@PriceBatchStatusID = 2
					AND @IsRollback = 1
					)
			BEGIN
				--If rolling back a CancelAllSales PBD then insert a Price Mammoth Event with no Event Reference ID
				--so that the sale price is sent down to Mammoth
				INSERT INTO mammoth.PriceChangeQueue (
					Item_Key
					,Store_No
					,Identifier
					,EventTypeID
					,EventReferenceID
					,InsertDate
					)
				SELECT PBD.Item_Key
					,PBD.Store_No
					,PBD.Identifier
					,CASE 
						WHEN PBD.CancelAllSales = 1 AND @PriceBatchStatusID = 2 AND @IsRollback = 1 THEN @MammothPriceEventTypeID 
						ELSE @EventTypeID 
					 END AS EventTypeID
					,CASE WHEN PBD.CancelAllSales = 1 AND @PriceBatchStatusID = 2 AND @IsRollback = 1 THEN NULL ELSE PBD.PriceBatchDetailID END 
					,GETDATE() AS InsertDate
				FROM PriceBatchDetail PBD(NOLOCK)
				INNER JOIN PriceBatchHeader PBH(NOLOCK) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
				INNER JOIN ValidatedScanCode VSC(NOLOCK) ON PBD.Identifier = VSC.ScanCode
				INNER JOIN PriceChgType PCT(NOLOCK) ON PBD.PriceChgTypeID = PCT.PriceChgTypeID
				WHERE PBD.PriceBatchHeaderID = @PriceBatchHeaderID
					AND (
						(PBH.PriceChgTypeID IS NOT NULL)
						OR (
							PBH.ItemChgTypeID = @NewItemChangeTypeId
							AND PBD.PriceChgTypeID IS NOT NULL
							)
						)
					AND PBD.Store_No NOT IN (
						SELECT Key_Value
						FROM dbo.fn_Parse_List(@ExcludedStoreNo, '|')
						)
					AND (
						(
							PBD.AutoGenerated <> 1
							AND PBD.InsertApplication <> 'Sale Off'
							)
						OR (
							PBD.InsertApplication = 'Sale Off'
							AND PCT.PriceChgTypeDesc <> 'REG'
							)
						)
			END
		END
	END

	SET NOCOUNT OFF
END
GO

GRANT EXECUTE
	ON OBJECT::[mammoth].[InsertPriceChangeQueue]
	TO [IRMAClientRole] AS [dbo];
GO
