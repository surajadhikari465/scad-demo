USE Mammoth
GO

IF NOT EXISTS (
		SELECT 1
		FROM INFORMATION_SCHEMA.TABLES t
		WHERE t.TABLE_SCHEMA = 'dbo'
			AND t.TABLE_NAME = 'tmp_DeletedStores'
		)
	CREATE TABLE dbo.tmp_DeletedStores (
		Region NCHAR(2) NOT NULL
		,LocaleID INT NOT NULL
		,BusinessUnitID INT NOT NULL
		,StoreName NVARCHAR(255) NOT NULL
		,StoreAbbrev NVARCHAR(5) NOT NULL
		,PhoneNumber NVARCHAR(255) NULL
		,LocaleOpenDate DATETIME NULL
		,LocaleCloseDate DATETIME NULL
		,AddedDate DATETIME NOT NULL
		,ModifiedDate DATETIME NULL
		,
		)
ELSE
	TRUNCATE TABLE dbo.tmp_DeletedStores

BEGIN TRY
	BEGIN TRAN;

	WITH Store_CTE
	AS (
		SELECT *
		FROM dbo.Locales_MW l
		WHERE l.BusinessUnitID IN (
				10169
				,10493
				)
		)
	DELETE
	FROM Store_CTE
	OUTPUT deleted.Region
		,deleted.LocaleID
		,deleted.BusinessUnitID
		,deleted.StoreName
		,deleted.StoreAbbrev
		,deleted.PhoneNumber
		,deleted.LocaleOpenDate
		,deleted.LocaleCloseDate
		,deleted.AddedDate
		,deleted.ModifiedDate
	INTO dbo.tmp_DeletedStores

	COMMIT TRAN
END TRY

BEGIN CATCH
	ROLLBACK TRAN;

	THROW;
END CATCH