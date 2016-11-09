CREATE TABLE [dbo].[items2del] (
    [IDENTIFIER]       FLOAT (53)     NULL,
    [ITEM DESCRIPTION] NVARCHAR (255) NULL,
    [DELETE]           FLOAT (53)     NULL
);




GO
GRANT SELECT
    ON OBJECT::[dbo].[items2del] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[items2del] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[items2del] TO [IRMAReportsRole]
    AS [dbo];

