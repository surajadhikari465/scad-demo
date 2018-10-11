CREATE TABLE [app].[PostDeploymentScriptHistory] (
    [ScriptKey] VARCHAR (128) NOT NULL,
    [RunTime]   DATETIME      NOT NULL,
    CONSTRAINT [PK_PostDeploymentScriptHistory] PRIMARY KEY CLUSTERED ([ScriptKey] ASC)
);




