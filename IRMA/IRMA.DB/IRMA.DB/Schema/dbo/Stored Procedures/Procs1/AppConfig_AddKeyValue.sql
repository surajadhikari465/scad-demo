CREATE PROCEDURE [dbo].[AppConfig_AddKeyValue]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier,
	@UpdateExistingKeyValue bit,
	@KeyID int,
	@Value varchar(350),
	@User_ID int
	
AS 

DECLARE @error_no int
SELECT @error_no = 0

SELECT @error_no = COUNT(KeyID)* -1
FROM AppConfigValue 
WHERE ApplicationID = @ApplicationID
AND EnvironmentID = @EnvironmentID
AND KeyID = @KeyID

IF (@error_no = 0)
	BEGIN
		INSERT INTO AppConfigValue
			(
				ApplicationID,
				EnvironmentID,
				KeyID,
				Value,
				LastUpdate,
				LastUpdateUserID
			)
		VALUES 
			(
				@ApplicationID,
				@EnvironmentID,
				@KeyID,
				@Value,
				GetDate(),
				@User_ID
			)
	END
			
IF (@error_no = -1 AND @UpdateExistingKeyValue = 1)
	BEGIN
		EXEC AppConfig_UpdateKeyValue @ApplicationID, @EnvironmentID, @KeyID, @Value, @User_ID
	END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddKeyValue] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AppConfig_AddKeyValue] TO [IRMAClientRole]
    AS [dbo];

