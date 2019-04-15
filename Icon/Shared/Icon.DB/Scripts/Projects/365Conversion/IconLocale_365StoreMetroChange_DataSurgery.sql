USE Icon
GO

-- ====================================================================
-- Data Surgery script to update a 365 store's metro to be in
-- the corresponding WFM metro
-- Example: Toledo store is in the '365_MET_OH' metro
--			and needs to be moved to 'MET_OH' metro as the parent
--
-- NOTE: If we need to convert all 365 stores at once,
-- we can comment the store filter on line 48.
-- If we want to only update some stores at a time,
-- we need to use the store filter on line 48 and list the store BUs
-- as a comma delimited list, e.g. ('10658','10654','10677')
-- ====================================================================

DECLARE @buTraitId int = (SELECT TraitID FROM Trait t WHERE t.traitDesc = 'PS Business Unit ID');

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLES t
		WHERE t.TABLE_SCHEMA = 'dbo'
			AND t.TABLE_NAME = 'tmp_365StoreBackup'
		)
	CREATE TABLE dbo.tmp_365StoreBackup (
		LocaleID INT NOT NULL
		,LocaleName NVARCHAR(100) NOT NULL
		,OldMetroID INT NOT NULL
		,OldMetroName NVARCHAR(100) NOT NULL
		,NewMetroName NVARCHAR(100) NOT NULL
		);
ELSE
	TRUNCATE TABLE dbo.tmp_365StoreBackup

BEGIN TRY
	BEGIN TRANSACTION

	PRINT 'Inserting stores into backup table...' + CONVERT(NVARCHAR(50), GETDATE(), 121)

	-- NOTE:	365 metros are named the same as WFM metros but with '365' prefixed to it.
	-- INSERT data into backup table if rollback is needed
	INSERT INTO dbo.tmp_365StoreBackup
	SELECT s.localeID AS LocaleID
		,s.localeName AS LocaleName
		,s.parentLocaleID AS OldMetroID
		,m.localeName AS OldMetroName
		,REPLACE(m.localeName, '365_', '') AS NewMetroName -- for mapping to new parent ID
	FROM dbo.Locale s
	INNER JOIN dbo.Locale m ON s.parentLocaleID = m.localeID
	INNER JOIN dbo.Locale r ON m.parentLocaleID = r.localeID
	INNER JOIN dbo.Locale c ON r.parentLocaleID = c.localeID
	INNER JOIN dbo.LocaleTrait lt ON s.localeID = lt.localeID
		AND lt.traitID = @buTraitId
	WHERE c.localeName = '365'
		AND lt.traitValue IN ('') -- List specific store BU numbers here if not doing all 365 stores

	PRINT 'Updating 365 stores will the new metro within the WFM chain...' + CONVERT(NVARCHAR(50), GETDATE(), 121)

	-- Map to new metro
	-- OUTPUT for visibility
	UPDATE s
	SET s.parentLocaleID = metro.localeID
	OUTPUT inserted.*
	FROM dbo.Locale s
	INNER JOIN dbo.tmp_365StoreBackup u ON s.localeID = u.LocaleID
	INNER JOIN Locale metro ON u.NewMetroName = metro.localeName

	COMMIT TRANSACTION;
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;

	THROW;
END CATCH