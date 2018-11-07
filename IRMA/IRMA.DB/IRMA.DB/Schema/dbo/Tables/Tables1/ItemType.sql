CREATE TABLE [dbo].[ItemType] (
    [ItemType_ID]   INT          NOT NULL,
    [ItemType_Name] VARCHAR (25) NOT NULL,
    CONSTRAINT [PK_ItemType_ItemType_ID] PRIMARY KEY CLUSTERED ([ItemType_ID] ASC) WITH (FILLFACTOR = 80)
);


GO
GRANT DELETE
    ON OBJECT::[dbo].[ItemType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[ItemType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[ItemType] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemType] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemType] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[ItemType] TO [IRMAReportsRole]
    AS [dbo];

