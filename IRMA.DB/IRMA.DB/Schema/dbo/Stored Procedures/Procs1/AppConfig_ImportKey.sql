CREATE PROCEDURE [dbo].[AppConfig_ImportKey]

	@KeyID int OUTPUT,
	@Name varchar(50),
	@User_ID int
	
AS

DECLARE @error_no int
SELECT @error_no = 0

SELECT @error_no = COUNT(KeyID)* -1
FROM AppConfigKey WHERE [Name] = @Name

IF (@error_no = 0)
	BEGIN
		INSERT INTO AppConfigKey
			(
				[Name],
				LastUpdate,
				LastUpdateUserID
			)
		VALUES 
			(
				@Name,
				GetDate(),
				@User_ID
			)
			
		SELECT @KeyID  = SCOPE_IDENTITY()
	END
ELSE
	SELECT @KeyID = KeyID FROM AppConfigKey WHERE [Name] = @Name
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_ImportKey] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_ImportKey] TO [IRMAClientRole]
    AS [dbo];

