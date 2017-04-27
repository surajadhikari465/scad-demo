IF EXISTS (SELECT * FROM dbo.sysobjects WHERE ID = OBJECT_ID(N'[dbo].[AppConfigActive]') AND OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW AppConfigActive
GO
CREATE VIEW [dbo].[AppConfigActive]
AS
SELECT     dbo.AppConfigApp.ApplicationID, dbo.AppConfigApp.EnvironmentID, dbo.AppConfigEnv.Name AS EnvironmentName, 
                      dbo.AppConfigApp.Name AS ApplicationName, dbo.AppConfigType.Name AS TypeName, dbo.AppConfigApp.Configuration, dbo.AppConfigApp.LastUpdate, 
                      dbo.AppConfigApp.LastUpdateUserID
FROM         dbo.AppConfigApp INNER JOIN
                      dbo.AppConfigEnv ON dbo.AppConfigApp.EnvironmentID = dbo.AppConfigEnv.EnvironmentID INNER JOIN
                      dbo.AppConfigType ON dbo.AppConfigApp.TypeID = dbo.AppConfigType.TypeID
WHERE     (dbo.AppConfigApp.Deleted = 0)

GO
