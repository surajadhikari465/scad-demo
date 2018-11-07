CREATE TABLE [dbo].[App] (
    [AppID]   INT          NOT NULL,
    [AppName] VARCHAR (20) NOT NULL,
    CONSTRAINT [PK_App] PRIMARY KEY CLUSTERED ([AppID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[App] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[App] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[App] TO [IRMAReportsRole]
    AS [dbo];

