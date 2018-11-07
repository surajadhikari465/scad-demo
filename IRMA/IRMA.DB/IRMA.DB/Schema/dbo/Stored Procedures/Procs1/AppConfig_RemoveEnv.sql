CREATE PROCEDURE [dbo].[AppConfig_RemoveEnv]

	@EnvironmentID uniqueidentifier,
	@User_ID int
	
AS

BEGIN

SET NOCOUNT ON

DECLARE @Error_No int, @Severity smallint
SELECT @Error_No = 0

BEGIN TRAN

	UPDATE AppConfigValue SET Deleted = 1 WHERE EnvironmentID = @EnvironmentID
	SELECT @Error_No = @@ERROR
	
	IF @Error_No = 0
		BEGIN
			UPDATE AppConfigApp SET Deleted = 1 WHERE EnvironmentID = @EnvironmentID
			SELECT @Error_No = @@ERROR
		END
		
	IF @Error_No = 0
		BEGIN
			UPDATE AppConfigEnv SET Deleted = 1 WHERE EnvironmentID = @EnvironmentID
			SELECT @Error_No = @@ERROR
		END
		
	SET NOCOUNT OFF

	IF @Error_No = 0
		COMMIT TRAN
	ELSE
	BEGIN
		IF @@TRANCOUNT <> 0
			ROLLBACK TRAN
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
		RAISERROR ('Remove Environment Configuration failed with @@ERROR: %d', @Severity, 1, @error_no)
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_RemoveEnv] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_RemoveEnv] TO [IRMAClientRole]
    AS [dbo];

