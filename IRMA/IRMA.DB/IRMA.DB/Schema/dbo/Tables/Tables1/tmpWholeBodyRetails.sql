CREATE TABLE [dbo].[tmpWholeBodyRetails] (
    [Identifier] VARCHAR (13) NULL,
    [Retail]     MONEY        NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpWholeBodyRetails] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpWholeBodyRetails] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpWholeBodyRetails] TO [IRMAReportsRole]
    AS [dbo];

