CREATE TABLE [dbo].[PMOrganizationChgQueue] (
    [PMOrganizationChgID] INT           NULL,
    [HierLevel]           VARCHAR (255) NULL,
    [ItemID]              VARCHAR (255) NULL,
    [ItemDescription]     VARCHAR (255) NULL,
    [ParentID]            VARCHAR (255) NULL,
    [ParentDescription]   VARCHAR (255) NULL,
    [ActionID]            VARCHAR (255) NULL,
    [InsertDate]          DATETIME      NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMOrganizationChgQueue] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMOrganizationChgQueue] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMOrganizationChgQueue] TO [IRMAReportsRole]
    AS [dbo];

