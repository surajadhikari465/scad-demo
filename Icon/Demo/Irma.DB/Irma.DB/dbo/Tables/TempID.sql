CREATE TABLE [dbo].[TempID] (
    [Identifier] VARCHAR (13) NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[TempID] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempID] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[TempID] TO [IRMAReportsRole]
    AS [dbo];

