-- This script is specific for the Icon Locale data surgery script.
-- This will need to be executed in an isolated CHG request
-- This will not be part of a release.

USE Mammoth
GO

CREATE TABLE dbo.tmp_DeletedStores
(
	Region nchar(2) NOT NULL,
	LocaleID int NOT NULL,
	BusinessUnitID int NOT NULL,
	StoreName nvarchar(255) NOT NULL,
	StoreAbbrev nvarchar(5) NOT NULL,
	PhoneNumber nvarchar(255) NULL,
	LocaleOpenDate datetime NULL,
	LocaleCloseDate datetime NULL,
	AddedDate datetime NOT NULL,
	ModifiedDate datetime NULL,
)

BEGIN TRY
BEGIN TRAN
	;WITH Store_CTE AS
	(SELECT *
	FROM dbo.Locales_TS l
	WHERE l.BusinessUnitID IN ()) -- input specific businessUnitIDs here for CHG request

	DELETE FROM Store_CTE
	OUTPUT
		deleted.Region,
		deleted.LocaleID,
		deleted.BusinessUnitID,
		deleted.StoreName,
		deleted.StoreAbbrev,
		deleted.PhoneNumber,
		deleted.LocaleOpenDate,
		deleted.LocaleCloseDate,
		deleted.AddedDate,
		deleted.ModifiedDate
		INTO dbo.tmp_DeletedStores

	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN;
	THROW;
END CATCH