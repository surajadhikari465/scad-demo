CREATE TABLE [dbo].[PMProductChg] (
    [PMProductChgID]    INT           IDENTITY (1, 1) NOT NULL,
    [HierLevel]         VARCHAR (255) NULL,
    [Item_Key]          INT           NULL,
    [ItemID]            VARCHAR (255) NULL,
    [ItemDescription]   VARCHAR (255) NULL,
    [ParentID]          VARCHAR (255) NULL,
    [ParentDescription] VARCHAR (255) NULL,
    [ActionID]          VARCHAR (255) NULL,
    [Status]            VARCHAR (255) NULL,
    [ItemType]          VARCHAR (255) NULL,
    [InsertDate]        DATETIME      CONSTRAINT [DF_PMProductChg_InsertDate] DEFAULT (getdate()) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMProductChg] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMProductChg] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PMProductChg] TO [IRMAReportsRole]
    AS [dbo];

