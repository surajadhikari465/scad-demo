IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_ImportKey]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_ImportKey]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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