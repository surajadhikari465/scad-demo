USE Icon
GO

PRINT 'Rolling back 365 store metro change using the dbo.tmp_365StoreBackup table...' + CONVERT(NVARCHAR(50), GETDATE(), 121)

BEGIN TRY
	BEGIN TRANSACTION

	UPDATE s
	SET s.parentLocaleID = b.OldMetroID
	OUTPUT inserted.*
	FROM dbo.Locale s
	INNER JOIN dbo.tmp_365StoreBackup b ON s.localeID = b.LocaleID

	COMMIT TRANSACTION
END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION;

	THROW;
END CATCH

