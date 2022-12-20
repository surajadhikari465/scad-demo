CREATE TABLE [dbo].[PostDeploymentScriptHistory]
(
	[ScriptKey] VARCHAR(128) NOT NULL PRIMARY KEY, 
    [RunTime] DATETIME NOT NULL
)
