CREATE PROCEDURE [dbo].[AppConfig_AddApp]

	@ApplicationID uniqueidentifier OUTPUT,
	@EnvironmentID uniqueidentifier,
	@TypeID int,
	@Name varchar(50),
	@User_ID int
	
AS 

DECLARE @AppGUID Uniqueidentifier
DECLARE @HistoryEntry Varchar(MAX)

BEGIN
	IF @ApplicationID IS NULL
		SET @AppGUID = NEWID()
	ELSE
		SET @AppGUID = @ApplicationID
END

INSERT INTO AppConfigApp
	(
		ApplicationID,
		EnvironmentID,
		TypeID,
		[Name],
		LastUpdate,
		LastUpdateUserID
	)
VALUES 
	(
		@AppGUID,
		@EnvironmentID,
		@TypeID,
		@Name,
		GetDate(),
		@User_ID
	)
	
SELECT @ApplicationID  = @AppGUID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddApp] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddApp] TO [IRMAClientRole]
    AS [dbo];

