CREATE PROCEDURE [dbo].[AppConfig_AddEnv]

	@EnvironmentID uniqueidentifier OUTPUT,
	@Name varchar(50),
	@Shortname varchar(5),
	@User_ID int
	
AS 

DECLARE @EnvGUID Uniqueidentifier

BEGIN
	IF @EnvironmentID IS NULL
		SET @EnvGUID = NEWID()
	ELSE
		SET @EnvGUID = @EnvironmentID
END

INSERT INTO AppConfigEnv
	(
		EnvironmentID,
		[Name],
		Shortname,
		LastUpdate,
		LastUpdateUserID
	)
VALUES 
	(
		@EnvGUID,
		@Name,
		@Shortname,
		GetDate(),
		@User_ID
	)
	
SELECT @EnvironmentID  = @EnvGUID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddEnv] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddEnv] TO [IRMAClientRole]
    AS [dbo];

