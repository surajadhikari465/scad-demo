﻿CREATE TABLE [dbo].[AppLogHist] (
    [Id]            INT              IDENTITY (1, 1) NOT NULL,
    [LogDate]       DATETIME         NOT NULL,
    [ApplicationID] UNIQUEIDENTIFIER NOT NULL,
    [HostName]      VARCHAR (64)     NOT NULL,
    [UserName]      VARCHAR (64)     NOT NULL,
    [Thread]        VARCHAR (255)    NOT NULL,
    [Level]         VARCHAR (50)     NOT NULL,
    [Logger]        VARCHAR (255)    NOT NULL,
    [Message]       VARCHAR (4000)   NOT NULL,
    [Exception]     VARCHAR (2000)   NULL,
    [InsertDate]    DATETIME         NOT NULL,
    CONSTRAINT [PK_AppLog] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 80)
);


GO
CREATE NONCLUSTERED INDEX [idxAppIdAndDates]
    ON [dbo].[AppLogHist]([LogDate] ASC, [ApplicationID] ASC, [InsertDate] ASC) WITH (FILLFACTOR = 80);


GO
GRANT DELETE
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[AppLogHist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[AppLogHist] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[AppLogHist] TO [IRMAReportsRole]
    AS [dbo];

