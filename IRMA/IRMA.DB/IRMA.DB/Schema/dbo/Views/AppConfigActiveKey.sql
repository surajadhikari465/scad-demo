CREATE VIEW [dbo].[AppConfigActiveKey]
AS
SELECT     dbo.AppConfigApp.ApplicationID, dbo.AppConfigApp.Name AS Application, dbo.AppConfigApp.EnvironmentID, dbo.AppConfigEnv.Name AS Environment, 
                      dbo.AppConfigKey.Name AS KeyName, dbo.AppConfigValue.KeyID, dbo.AppConfigValue.Value AS KeyValue
FROM         dbo.AppConfigValue INNER JOIN
                      dbo.AppConfigKey ON dbo.AppConfigValue.KeyID = dbo.AppConfigKey.KeyID INNER JOIN
                      dbo.AppConfigApp INNER JOIN
                      dbo.AppConfigEnv ON dbo.AppConfigApp.EnvironmentID = dbo.AppConfigEnv.EnvironmentID ON 
                      dbo.AppConfigValue.EnvironmentID = dbo.AppConfigApp.EnvironmentID AND dbo.AppConfigValue.ApplicationID = dbo.AppConfigApp.ApplicationID
WHERE     (dbo.AppConfigValue.Deleted = 0) AND (dbo.AppConfigKey.Deleted = 0) AND (dbo.AppConfigEnv.Deleted = 0) AND (dbo.AppConfigApp.Deleted = 0)
GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigActiveKey] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppConfigActiveKey] TO [IRMAReportsRole]
    AS [dbo];

