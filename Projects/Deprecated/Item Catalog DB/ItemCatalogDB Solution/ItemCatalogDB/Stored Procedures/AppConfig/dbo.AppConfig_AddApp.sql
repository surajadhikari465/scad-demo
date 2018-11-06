IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_AddApp]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_AddApp]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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