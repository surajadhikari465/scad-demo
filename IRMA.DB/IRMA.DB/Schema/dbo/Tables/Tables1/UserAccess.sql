CREATE TABLE [dbo].[UserAccess] (
    [UserAccessLevel_ID] SMALLINT     IDENTITY (1, 1) NOT NULL,
    [AccessLevel_Name]   VARCHAR (50) NULL,
    CONSTRAINT [PK_UserAccess] PRIMARY KEY CLUSTERED ([UserAccessLevel_ID] ASC)
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserAccess] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[UserAccess] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[UserAccess] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[UserAccess] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[UserAccess] TO [IRMASLIMRole]
    AS [dbo];

