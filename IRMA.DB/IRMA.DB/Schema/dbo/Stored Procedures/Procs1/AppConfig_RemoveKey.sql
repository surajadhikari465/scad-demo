CREATE PROCEDURE [dbo].[AppConfig_RemoveKey]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier,
	@KeyID int,
	@User_ID int
	
AS

BEGIN

SET NOCOUNT ON

DECLARE @Error_No int, @Severity smallint
SELECT @Error_No = 0

BEGIN TRAN

	IF (@ApplicationID IS NOT NULL AND @EnvironmentID IS NOT NULL)
		--remove the key/value pair from a specific application and environment by flipping the deleted flag
		BEGIN
			UPDATE AppConfigValue 
				SET 
					Deleted = 1,
					LastUpdate = GetDate(),
					LastUpdateUserID = @User_ID
			WHERE KeyID = @KeyID 
			AND ApplicationID = @ApplicationID 
			AND EnvironmentID = @EnvironmentID
			
			SELECT @Error_No = @@ERROR
		END
	
	IF (@ApplicationID IS NULL AND @EnvironmentID IS NOT NULL)
		--remove the key/value pair from all applications in a single environment by flipping the deleted flag
		BEGIN
			UPDATE AppConfigValue 
				SET 
					Deleted = 1, 
					LastUpdate = GetDate(),
					LastUpdateUserID = @User_ID
			WHERE KeyID = @KeyID 
			AND EnvironmentID = @EnvironmentID
			
			SELECT @Error_No = @@ERROR
		END

	IF (@ApplicationID IS NULL AND @EnvironmentID IS NULL)
		-- remove the key/value pair from all applications and all environments by flipping the deleted flag
		BEGIN
			UPDATE AppConfigValue 
				SET 
					Deleted = 1,
					LastUpdate = GetDate(),
					LastUpdateUserID = @User_ID 
			WHERE KeyID = @KeyID
			
			SELECT @Error_No = @@ERROR

			IF @Error_No = 0
				UPDATE AppConfigKey
					SET 
					Deleted = 1,
					LastUpdate = GetDate(),
					LastUpdateUserID = @User_ID
				WHERE KeyID = @KeyID
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
		RAISERROR ('Remove Application Configuration Key failed with @@ERROR: %d', @Severity, 1, @error_no)
	END
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_RemoveKey] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_RemoveKey] TO [IRMAClientRole]
    AS [dbo];

