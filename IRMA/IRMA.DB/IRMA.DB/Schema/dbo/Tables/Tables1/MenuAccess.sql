CREATE TABLE [dbo].[MenuAccess] (
    [MenuAccessID] INT            IDENTITY (1, 1) NOT NULL,
    [MenuName]     VARCHAR (5000) NOT NULL,
    [Visible]      BIT            NOT NULL,
    CONSTRAINT [PK_MenuAccess] PRIMARY KEY CLUSTERED ([MenuAccessID] ASC)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[MenuAccess] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[MenuAccess] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[MenuAccess] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[MenuAccess] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[MenuAccess] TO [IRMAReportsRole]
    AS [dbo];

