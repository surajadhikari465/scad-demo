IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_AddKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_AddKey]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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