CREATE TABLE [dbo].[Version] (
    [ApplicationName] VARCHAR (20) NOT NULL,
    [Version]         VARCHAR (20) NOT NULL,
    [Environment]     VARCHAR (20) NOT NULL,
    [UsedByRegion]    BIT          NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Version] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Version] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Version] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Version] TO [IRMASchedJobs]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Version] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Version] TO [IConInterface]
    AS [dbo];

