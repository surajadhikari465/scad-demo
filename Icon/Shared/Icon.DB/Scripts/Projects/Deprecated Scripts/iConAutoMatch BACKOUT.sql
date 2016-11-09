--*******************************************************************************
-- Title: BACKOUT SCRIPT for iCon Auto-Match IRMA Items
-- Author: Benjamin Loving
-- Date: 2013-12-12
-- Purpose: This script will remove any item that was inserted into the Product table by the 
--			 iCon auto-match script.
--*******************************************************************************
DECLARE @runTime DATETIME,
		@runUser VARCHAR(128),
		@runHost VARCHAR(128),
		@runDB VARCHAR(128),
		@CodeLocation VARCHAR(255)

SELECT	@runTime = GETDATE(),
		@runUser = SUSER_NAME(),
		@runHost = HOST_NAME(),
		@runDB = DB_NAME(),
		@CodeLocation = '1.0 Variable Initialization'

PRINT '---------------------------------------------------------------------------------------'
PRINT 'Current System Time: ' + CONVERT(VARCHAR, @runTime, 121)
PRINT 'User Name: ' + @runUser
PRINT 'Running From Host: ' + @runHost
PRINT 'Connected To DB Server: ' + @@SERVERNAME
PRINT 'DB Name: ' + @runDB
PRINT '---------------------------------------------------------------------------------------'
PRINT '-----   Begin [iCon].[dbo].[Product] Auto-Match Backout'
PRINT '---------------------------------------------------------------------------------------'

SET @CodeLocation = '1.1 BEGIN TRY'
BEGIN TRY

	SET @CodeLocation = '1.2 BEGIN TRANSACTION'
	BEGIN TRANSACTION

		SET @CodeLocation = '2. Before DELETE of auto-matched product records'

		----------------------------------------------------------------------
		DELETE FROM [dbo].[Product]
		WHERE [UpdatedBy] = 'AutoMatch'
		----------------------------------------------------------------------
		
		SET @CodeLocation = '3. After DELETE of auto-matched product records'

	IF @@TRANCOUNT > 0
	BEGIN
		SET @CodeLocation = '4. Before transaction commit'
		COMMIT TRANSACTION
		SET @CodeLocation = '5. After transaction commit'
	END

	PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
		+ 'Successfully removed all rows inserted into iCon.dbo.Product by the iCon Auto-Match Script!' + CHAR(13) + CHAR(10)
		+ REPLACE(SPACE(120), SPACE(1), '-')

END TRY

BEGIN CATCH
        
		IF @@TRANCOUNT > 0
				ROLLBACK TRANSACTION

		PRINT REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'Error ' + CONVERT(varchar, ERROR_NUMBER()) + ': ' + ERROR_MESSAGE() + CHAR(13) + CHAR(10)
				+ CHAR(9) + ' at statement  ''' + @CodeLocation + ''' (' + ISNULL(ERROR_PROCEDURE() + ', ', '') + 'line ' + CONVERT(varchar, ERROR_LINE()) + ')' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-') + CHAR(13) + CHAR(10)
				+ 'Database changes were rolled back.' + CHAR(13) + CHAR(10)
				+ REPLACE(SPACE(120), SPACE(1), '-')

	SELECT
		ERROR_NUMBER() AS ErrorNumber,
		ERROR_SEVERITY() AS ErrorSeverity,
		ERROR_STATE() AS ErrorState,
		ERROR_PROCEDURE() AS ErrorProcedure,
		ERROR_LINE() AS ErrorLine,
		ERROR_MESSAGE() AS ErrorMessage
END CATCH
GO