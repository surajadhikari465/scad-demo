CREATE TABLE [dbo].[Sales] (
    [Identifier]  VARCHAR (13) NULL,
    [Description] VARCHAR (60) NULL,
    [Sales]       INT          NULL,
    [Purchases]   INT          NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Sales] TO [IRMAReportsRole]
    AS [dbo];

