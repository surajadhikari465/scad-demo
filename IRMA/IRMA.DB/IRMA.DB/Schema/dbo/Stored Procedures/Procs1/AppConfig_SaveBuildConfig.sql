CREATE PROCEDURE [dbo].[AppConfig_SaveBuildConfig]

	@ApplicationID As uniqueidentifier,
	@EnvironmentID As uniqueidentifier,
	@Configuration As xml,
	@User_ID As int

AS


UPDATE AppConfigApp 
	SET Configuration = @Configuration,
		LastUpdateUserID = @User_ID,
		LastUpdate = GetDate()
WHERE ApplicationID = @ApplicationID
AND EnvironmentID = @EnvironmentID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_SaveBuildConfig] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_SaveBuildConfig] TO [IRMAClientRole]
    AS [dbo];

