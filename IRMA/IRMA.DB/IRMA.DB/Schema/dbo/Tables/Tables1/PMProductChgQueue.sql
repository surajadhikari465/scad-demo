CREATE TABLE [dbo].[PMProductChgQueue] (
    [PMProductChgID]    INT           NULL,
    [HierLevel]         VARCHAR (255) NULL,
    [Item_Key]          INT           NULL,
    [ItemID]            VARCHAR (255) NULL,
    [ItemDescription]   VARCHAR (255) NULL,
    [ParentID]          VARCHAR (255) NULL,
    [ParentDescription] VARCHAR (255) NULL,
    [ActionID]          VARCHAR (255) NULL,
    [Status]            VARCHAR (255) NULL,
    [ItemType]          VARCHAR (255) NULL,
    [InsertDate]        DATETIME      NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMProductChgQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMProductChgQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMProductChgQueue] TO [IRMAReportsRole]
    AS [dbo];

