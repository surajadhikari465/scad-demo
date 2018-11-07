CREATE TABLE [dbo].[PMOrganizationChg] (
    [PMOrganizationChgID] INT           IDENTITY (1, 1) NOT NULL,
    [HierLevel]           VARCHAR (255) NULL,
    [ItemID]              VARCHAR (255) NULL,
    [ItemDescription]     VARCHAR (255) NULL,
    [ParentID]            VARCHAR (255) NULL,
    [ParentDescription]   VARCHAR (255) NULL,
    [ActionID]            VARCHAR (255) NULL,
    [InsertDate]          DATETIME      CONSTRAINT [DF_PMOrganizationChg_InsertDate] DEFAULT (getdate()) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMOrganizationChg] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMOrganizationChg] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMOrganizationChg] TO [IRMAReportsRole]
    AS [dbo];

