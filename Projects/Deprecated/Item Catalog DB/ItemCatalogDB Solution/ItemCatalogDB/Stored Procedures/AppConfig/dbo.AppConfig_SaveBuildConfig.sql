IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_SaveBuildConfig]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_SaveBuildConfig]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AppConfig_SaveBuildConfig]

	@ApplicationID As uniqueidentifier,
	@EnvironmentID As uniqueidentifier,
	@Configuration As xml,
	@User_ID As int

AS


UPDATE AppConfigApp 
	SET Configuration = @Configuration,
		LastUpdateUserID = @User_ID,
		LastUpdate = GetDate()
WHERE ApplicationID = @ApplicationID
AND EnvironmentID = @EnvironmentID


GO