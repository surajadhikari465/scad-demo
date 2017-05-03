CREATE PROCEDURE [dbo].[AppConfig_UpdateKeyValue]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier,
	@KeyID int,
	@Value varchar(350),
	@User_ID int
	
AS 

UPDATE AppConfigValue
	SET
		Value = @Value,
		LastUpdate = GetDate(),
		LastUpdateUserID = @User_ID
	WHERE EnvironmentID = @EnvironmentID
	AND ApplicationID = @ApplicationID
	AND KeyID = @KeyID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_UpdateKeyValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_UpdateKeyValue] TO [IRMAClientRole]
    AS [dbo];

