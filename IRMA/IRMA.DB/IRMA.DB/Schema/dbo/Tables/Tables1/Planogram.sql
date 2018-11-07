CREATE TABLE [dbo].[Planogram] (
    [Planogram_ID]         INT           IDENTITY (1, 1) NOT NULL,
    [Store_No]             INT           NOT NULL,
    [Item_Key]             INT           NOT NULL,
    [ProductGroup]         VARCHAR (3)   NULL,
    [ShelfIdentifier]      VARCHAR (3)   NULL,
    [ProductPlacement]     VARCHAR (3)   NULL,
    [MaxUnits]             VARCHAR (3)   NULL,
    [ProductFacings]       INT           NULL,
    [ProductPlanogramCode] VARCHAR (8)   NULL,
    [InsertDate]           DATETIME      CONSTRAINT [DF_Planogram_InsertDate] DEFAULT (getdate()) NOT NULL,
    [InsertWorkstation]    VARCHAR (255) CONSTRAINT [DF_Planogram_InsertWorkstation] DEFAULT (host_name()) NOT NULL,
    CONSTRAINT [PK_Planogram] PRIMARY KEY CLUSTERED ([Planogram_ID] ASC),
    CONSTRAINT [FK_Planogram_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_Planogram_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
ALTER TABLE [dbo].[Planogram] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_Planogram]
    ON [dbo].[Planogram]([Store_No] ASC, [Item_Key] ASC, [ProductFacings] ASC, [ProductGroup] ASC, [ProductPlacement] ASC, [ProductPlanogramCode] ASC, [ShelfIdentifier] ASC);


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Planogram] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Planogram] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Planogram] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IRMAReports]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[Planogram] TO [iCONReportingRole]
    AS [dbo];

