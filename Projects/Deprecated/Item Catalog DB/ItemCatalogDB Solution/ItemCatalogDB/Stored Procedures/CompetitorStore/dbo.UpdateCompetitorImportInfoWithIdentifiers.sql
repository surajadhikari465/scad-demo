if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateCompetitorImportInfoWithIdentifiers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateCompetitorImportInfoWithIdentifiers]
GO

CREATE PROCEDURE [dbo].[UpdateCompetitorImportInfoWithIdentifiers] 
	@CompetitorImportSessionID int
AS
BEGIN

/*
	This procedure updates previously imported CompetitorImportInfo records from a given
	session with IDs from any existing records found with matching string identifiers.
	The match to each table is done separately to simplify the false-positive detection logic.
*/

-- Match Competitor Locations
UPDATE
	CompetitorImportInfo
SET
	CompetitorLocationID = LC.CompetitorLocationID
FROM
	CompetitorImportInfo CII
	INNER JOIN CompetitorLocation LC ON CII.Location = LC.Name
WHERE
	CompetitorImportSessionID = @CompetitorImportSessionID

-- Match Competitors
UPDATE
	CompetitorImportInfo
SET
	CompetitorID = C.CompetitorID
FROM
	CompetitorImportInfo CII
	INNER JOIN Competitor C ON CII.Competitor = C.Name
WHERE
	CompetitorImportSessionID = @CompetitorImportSessionID

-- Match Competitor Stores
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
	CompetitorImportSessionID = @CompetitorImportSessionID

-- Match Items
UPDATE
	CompetitorImportInfo
SET
	Item_Key = II.Item_Key
FROM
	CompetitorImportInfo CII
	INNER JOIN ItemIdentifier II ON CII.UPCCode = II.Identifier
WHERE
	CompetitorImportSessionID = @CompetitorImportSessionID
	
UPDATE
	CompetitorImportInfo
SET
	Item_Key = CSII.Item_Key
FROM
	CompetitorImportInfo CII
	INNER JOIN CompetitorStoreItemIdentifier CSII ON CII.UPCCode = CSII.Identifier
WHERE
	CompetitorImportSessionID = @CompetitorImportSessionID
	AND
	CII.CompetitorStoreID = CSII.CompetitorStoreID
	
END 
go