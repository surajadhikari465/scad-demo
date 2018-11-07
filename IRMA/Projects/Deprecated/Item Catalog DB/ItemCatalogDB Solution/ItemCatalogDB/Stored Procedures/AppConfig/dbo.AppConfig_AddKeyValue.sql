IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_AddKeyValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_AddKeyValue]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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