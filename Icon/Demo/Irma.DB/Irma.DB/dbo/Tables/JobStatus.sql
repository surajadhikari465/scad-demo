CREATE TABLE [dbo].[JobStatus] (
    [Classname]         VARCHAR (50)   NOT NULL,
    [Status]            VARCHAR (50)   NOT NULL,
    [LastRun]           DATETIME       NULL,
    [ServerName]        VARCHAR (50)   NULL,
    [StatusDescription] VARCHAR (255)  NULL,
    [Details]           VARCHAR (2000) NULL,
    CONSTRAINT [PK_JobStatus] PRIMARY KEY CLUSTERED ([Classname] ASC),
    CONSTRAINT [CHK_JobStatus_Status] CHECK ([Status]='Failed' OR [Status]='Complete' OR [Status]='Waiting' OR [Status]='Running' OR [Status]='Queueing')
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[JobStatus] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[JobStatus] TO [IConInterface]
    AS [dbo];

