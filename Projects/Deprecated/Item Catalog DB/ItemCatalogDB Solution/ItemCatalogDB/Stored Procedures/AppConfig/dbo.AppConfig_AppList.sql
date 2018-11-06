IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AppConfig_AppList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[AppConfig_AppList]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[AppConfig_AppList]

	@EnvironmentID As uniqueidentifier

AS

SELECT 
	AppConfigApp.ApplicationID, 
	AppConfigApp.EnvironmentID,
	AppConfigApp.[Name],
	AppConfigType.[Name] As [Type]
FROM AppConfigApp
INNER JOIN AppConfigType ON AppConfigType.TypeID = AppConfigApp.TypeID
WHERE AppConfigApp.EnvironmentID = @EnvironmentID 
AND AppConfigApp.Deleted = 0
ORDER BY AppConfigApp.[Name]

GO