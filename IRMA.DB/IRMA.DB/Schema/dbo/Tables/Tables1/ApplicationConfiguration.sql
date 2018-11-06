CREATE TABLE [dbo].[ApplicationConfiguration] (
    [ApplicationId] VARCHAR (255)  NULL,
    [EnvironmentId] VARCHAR (255)  NULL,
    [Key]           VARCHAR (1024) NULL,
    [Value]         VARCHAR (1024) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_ApplicationConfiguration_AppId]
    ON [dbo].[ApplicationConfiguration]([ApplicationId] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ApplicationConfiguration] TO [IRMAReportsRole]
    AS [dbo];

