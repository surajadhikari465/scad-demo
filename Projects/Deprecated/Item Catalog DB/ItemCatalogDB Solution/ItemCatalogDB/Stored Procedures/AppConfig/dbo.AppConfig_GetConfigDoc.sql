IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_GetConfigDoc]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_GetConfigDoc]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AppConfig_GetConfigDoc]

	@ApplicationID uniqueidentifier,
	@EnvironmentID uniqueidentifier
	
AS 

SELECT Configuration 
FROM AppConfigApp 
WHERE EnvironmentID = @EnvironmentID
AND ApplicationID = @ApplicationID
AND Deleted = 0

GO