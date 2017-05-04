CREATE TABLE [dbo].[Purchases] (
    [Identifier]  VARCHAR (13) NULL,
    [Description] VARCHAR (60) NULL,
    [Purchases]   INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Purchases] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Purchases] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Purchases] TO [IRMAReportsRole]
    AS [dbo];

