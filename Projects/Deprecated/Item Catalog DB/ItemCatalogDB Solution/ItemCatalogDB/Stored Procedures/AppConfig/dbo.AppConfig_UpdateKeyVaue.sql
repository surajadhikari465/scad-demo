IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_UpdateKeyValue]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_UpdateKeyValue]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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
