IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_EnvList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_EnvList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AppConfig_EnvList]

AS

SELECT 
	EnvironmentID,
	[Name],
	Shortname
FROM AppConfigEnv
WHERE Deleted = 0
ORDER BY [Name]

GO