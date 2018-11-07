CREATE PROCEDURE [dbo].[AppConfig_AddKey]

	@KeyID int OUTPUT,
	@Name varchar(150),
	@User_ID int
	
AS

DECLARE @Count As int

SELECT @Count = Count(*) FROM AppConfigKey WHERE [Name] = @Name

BEGIN
	IF @Count > 0
		BEGIN
		
			SELECT @KeyID = KeyID FROM AppConfigKey WHERE [Name] = @Name -- get the current key id
			
			UPDATE AppConfigKey SET Deleted = 0 WHERE KeyID = @KeyID -- go ahead and activate it if it was previously deleted
			
			UPDATE AppConfigValue SET Deleted = 0 WHERE KeyID = @KeyID -- go ahead and activate all key/value pair relationships
			
		END

	ELSE
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
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddKey] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddKey] TO [IRMAClientRole]
    AS [dbo];

