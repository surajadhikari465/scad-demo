CREATE TABLE [dbo].[ShelfTag] (
    [ShelfTagID]   INT          IDENTITY (1, 1) NOT NULL,
    [ShelfTagDesc] VARCHAR (50) NOT NULL,
    [CreateDate]   DATETIME     CONSTRAINT [DF_ShelfTag_CreateDate] DEFAULT (getdate()) NOT NULL,
    [ModifyDate]   DATETIME     CONSTRAINT [DF_ShelfTag_ModifyDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_ShelfTag] PRIMARY KEY CLUSTERED ([ShelfTagID] ASC)
);




GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ShelfTag] TO [IRMAReportsRole]
    AS [dbo];

