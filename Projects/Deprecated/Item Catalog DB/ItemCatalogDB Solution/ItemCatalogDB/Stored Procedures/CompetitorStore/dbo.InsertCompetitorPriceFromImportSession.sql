if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertCompetitorPriceFromImportSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertCompetitorPriceFromImportSession]
GO

CREATE PROCEDURE [dbo].[InsertCompetitorPriceFromImportSession] 
	@CompetitorImportSessionID int,
	@OverwriteExistingPrices bit
AS
BEGIN

/*
	This procedure creates new CompetitorPrice records from previously imported CompetitorImportInfo 
	records and either updates or prevents collisions with existing CompetitorPrice records, depending
	on the OverwiteExistingPrices parameter. It also creates new Competitor, CompetitorLocation, 
	CompetitorStore, and CompetitorStoreItemIdentifier records where appropriate.
*/

DECLARE @UserID int
DECLARE @UpdateDateTime smalldatetime
DECLARE @ErrorCode int

SET @ErrorCode = 0
SET @UpdateDateTime = GETDATE()

SELECT
	@UserID = [User_ID]
FROM
	CompetitorImportSession
WHERE
	CompetitorImportSessionID = @CompetitorImportSessionID

BEGIN TRANSACTION

	-- Create New Competitors
	INSERT INTO Competitor
		SELECT DISTINCT
			CII.Competitor
		FROM
			CompetitorImportInfo CII
			LEFT OUTER JOIN Competitor C ON CII.Competitor = C.Name
		WHERE
			@CompetitorImportSessionID = CII.CompetitorImportSessionID
			AND
			CII.CompetitorID IS NULL
			AND
			C.CompetitorID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Update the staging table
	UPDATE 
		CompetitorImportInfo
	SET 
		CompetitorID = C.CompetitorID
	FROM
		CompetitorImportInfo CII
		INNER JOIN Competitor C ON CII.Competitor = C.Name
	WHERE
		CompetitorImportSessionID = @CompetitorImportSessionID
		AND
		CII.CompetitorID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Create New CompetitorLocations
	INSERT INTO CompetitorLocation
		SELECT DISTINCT
			CII.Location
		FROM
			CompetitorImportInfo CII
			LEFT OUTER JOIN CompetitorLocation CL ON CII.Location = CL.Name
		WHERE
			@CompetitorImportSessionID = CII.CompetitorImportSessionID
			AND
			CII.CompetitorLocationID IS NULL
			AND
			CL.CompetitorLocationID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Update the staging table
	UPDATE
		CompetitorImportInfo
	SET
		CompetitorLocationID = LC.CompetitorLocationID
	FROM
		CompetitorImportInfo CII
		INNER JOIN CompetitorLocation LC ON CII.Location = LC.Name
	WHERE
		CII.CompetitorImportSessionID = @CompetitorImportSessionID
		AND
		CII.CompetitorLocationID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Create New CompetitorStores (a trigger will create the corresponding identifiers)
	INSERT INTO CompetitorStore
		SELECT DISTINCT
			CII.CompetitorID, 
			CII.CompetitorLocationID,
			CII.CompetitorStore,
			@UserID AS UpdateUserID,
			@UpdateDateTime AS UpdateDateTime
		FROM
			CompetitorImportInfo CII
			LEFT OUTER JOIN CompetitorStore CS ON 
				CII.CompetitorID = CS.CompetitorID
				AND
				CII.CompetitorLocationID = CS.CompetitorLocationID
				AND
				CII.CompetitorStore = CS.Name
		WHERE
			@CompetitorImportSessionID = CII.CompetitorImportSessionID
			AND
			CII.CompetitorStoreID IS NULL
			AND
			CS.CompetitorStoreID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Update the staging table
	UPDATE
		CompetitorImportInfo
	SET
		CompetitorStoreID = CSI.CompetitorStoreID
	FROM
		CompetitorImportInfo CII
		INNER JOIN CompetitorStoreIdentifier CSI ON CII.CompetitorStore = CSI.Identifier
		INNER JOIN CompetitorStore CS ON 
			CSI.CompetitorStoreID = CS.CompetitorStoreID 
			AND
			-- No store match if the locations don't also match
			CII.CompetitorLocationID = CS.CompetitorLocationID
			AND
			-- No store match if the competitors don't also match
			CII.CompetitorID = CS.CompetitorID
	WHERE
		CII.CompetitorImportSessionID = @CompetitorImportSessionID
		AND
		CII.CompetitorStoreID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Create New CompetitorStoreItemIdentifiers
	INSERT INTO CompetitorStoreItemIdentifier
		SELECT DISTINCT
			CII.CompetitorStoreID, 
			CII.UPCCode, 
			CII.Item_Key
		FROM
			CompetitorImportInfo CII
			LEFT OUTER JOIN ItemIdentifier II ON 
				CII.Item_Key = II.Item_Key 
				AND 
				CII.UPCCode = II.Identifier
			LEFT OUTER JOIN CompetitorStoreItemIdentifier CSII ON 
				CII.CompetitorStoreID = CSII.CompetitorStoreID
				AND
				CII.UPCCode = CSII.Identifier
				AND
				CII.Item_Key = CSII.Item_Key
		WHERE
			@CompetitorImportSessionID = CII.CompetitorImportSessionID
			AND
			-- Disallow overlap with WFM Identifiers
			II.Identifier_ID IS NULL
			AND
			-- Ensure that the identifier we're attempting to create doesn't already exist
			CSII.CompetitorStoreID IS NULL

	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

	-- Finally, create/update the CompetitorPrice table
	IF @OverwriteExistingPrices = 1
	BEGIN
		-- Update existing CompetitorPrice records where overlap occurs with this import session
		UPDATE
			CompetitorPrice
		SET
			Description = CII.Description,
			PriceMultiple = CII.PriceMultiple,
			Price = CII.Price,
			SaleMultiple = CII.SaleMultiple,
			Sale = CII.Sale, 
			Size = CII.Size,
			Unit_ID = CII.Unit_ID,
			CheckDate = CII.CheckDate,
			UpdateUserID = @UserID,
			UpdateDateTime = @UpdateDateTime
		FROM
			CompetitorImportInfo CII
			INNER JOIN CompetitorPrice CP ON
				CII.Item_Key = CP.Item_Key
				AND
				CII.CompetitorStoreID = CP.CompetitorStoreID
				AND
				CII.FiscalYear = CP.FiscalYear
				AND
				CII.FiscalPeriod = CP.FiscalPeriod
				AND
				CII.PeriodWeek = CP.PeriodWeek
		WHERE
			CII.CompetitorImportSessionID = @CompetitorImportSessionID

		SELECT @ErrorCode = @@ERROR
		IF (@ErrorCode <> 0) GOTO ERROR
	END

	-- Insert new CompetitorPrice records, making sure to avoid dups, which are either 
	-- ignored or updated a few lines above this
	INSERT INTO CompetitorPrice
		(Item_Key,
		CompetitorStoreID,
		FiscalYear,
		FiscalPeriod,
		PeriodWeek,
		UPCCode,
		Description,
		PriceMultiple,
		Price,
		SaleMultiple,
		Sale,
		Size,
		Unit_ID,
		CheckDate,
		UpdateUserID,
		UpdateDateTime)		
	SELECT
		CII.Item_Key,
		CII.CompetitorStoreID,
		CII.FiscalYear,
		CII.FiscalPeriod,
		CII.PeriodWeek,
		CII.UPCCode,
		CII.Description,
		CII.PriceMultiple,
		CII.Price,
		CII.SaleMultiple,
		CII.Sale, 
		CII.Size,
		CII.Unit_ID,
		CII.CheckDate,
		@UserID,
		@UpdateDateTime
	FROM
		CompetitorImportInfo CII
		LEFT OUTER JOIN CompetitorPrice CP ON
			CII.Item_Key = CP.Item_Key
			AND
			CII.CompetitorStoreID = CP.CompetitorStoreID
			AND
			CII.FiscalYear = CP.FiscalYear
			AND
			CII.FiscalPeriod = CP.FiscalPeriod
			AND
			CII.PeriodWeek = CP.PeriodWeek
	WHERE
		CII.CompetitorImportSessionID = @CompetitorImportSessionID
		AND
		-- No overlap
		CP.Item_Key IS NULL
	
	SELECT @ErrorCode = @@ERROR
	IF (@ErrorCode <> 0) GOTO ERROR

ERROR:
IF(@ErrorCode = 0)
	COMMIT TRANSACTION
ELSE
BEGIN
	ROLLBACK TRANSACTION
    DECLARE @Severity smallint
    SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @ErrorCode), 16)
    RAISERROR ('InsertCompetitorPriceFromImportSession failed with @@ERROR: %d', @Severity, 1, @ErrorCode)       
END
	
END
GO