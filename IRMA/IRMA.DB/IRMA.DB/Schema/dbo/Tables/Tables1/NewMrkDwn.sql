CREATE TABLE [dbo].[NewMrkDwn] (
    [StoreNo]   INT            NULL,
    [SubTeamNo] INT            NULL,
    [MrkDwn]    DECIMAL (9, 4) NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewMrkDwn] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewMrkDwn] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewMrkDwn] TO [IRMAReportsRole]
    AS [dbo];

