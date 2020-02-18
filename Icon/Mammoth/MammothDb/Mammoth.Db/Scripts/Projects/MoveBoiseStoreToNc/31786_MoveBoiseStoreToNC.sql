USE Mammoth
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLES t
		WHERE t.TABLE_SCHEMA = 'dbo'
			AND t.TABLE_NAME = 'tmp_DeletedStoresBoise'
		)

CREATE TABLE dbo.tmp_DeletedStoresBoise
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

ELSE
	TRUNCATE TABLE dbo.tmp_DeletedStoresBoise

BEGIN TRY
BEGIN TRAN
	;WITH Store_CTE AS
	(SELECT *
	FROM dbo.Locales_RM l
	WHERE l.BusinessUnitID = 10284)

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
		INTO dbo.tmp_DeletedStoresBoise

	COMMIT TRAN
END TRY
BEGIN CATCH
	ROLLBACK TRAN;
	THROW;
END CATCH

