CREATE TABLE [dbo].[Title] (
    [Title_ID]   INT          NOT NULL,
    [Title_Desc] VARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Title_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[Title] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Title] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Title] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Title] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Title] TO [IRMAPromoRole]
    AS [dbo];

