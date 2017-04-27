IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_AddEnv]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_AddEnv]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

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