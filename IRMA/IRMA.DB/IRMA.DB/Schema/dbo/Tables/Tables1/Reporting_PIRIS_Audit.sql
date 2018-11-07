CREATE TABLE [dbo].[Reporting_PIRIS_Audit] (
    [Store]           INT            NULL,
    [Barcode]         VARCHAR (14)   NULL,
    [ProductCode]     INT            NULL,
    [Department]      INT            NULL,
    [Section]         INT            NULL,
    [SubSection]      INT            NULL,
    [VATCode]         INT            NULL,
    [LongDescription] VARCHAR (60)   NULL,
    [ItemSize]        VARCHAR (20)   NULL,
    [CaseSize]        DECIMAL (9, 4) NULL,
    [Base]            SMALLMONEY     NULL,
    [Discount]        SMALLMONEY     NULL,
    [Cost]            SMALLMONEY     NULL,
    [UnitPrice1]      SMALLMONEY     NULL,
    [Supplier]        VARCHAR (10)   NULL,
    [Item]            VARCHAR (20)   NULL,
    [CreateDateTime]  DATETIME       CONSTRAINT [DF_Reporting_PIRIS_Audit_CreateDateTime] DEFAULT (getdate()) NULL
);


GO
CREATE CLUSTERED INDEX [CLIX_Reporting_PIRIS_Audit_Store_Barcode]
    ON [dbo].[Reporting_PIRIS_Audit]([Store] ASC, [Barcode] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Reporting_PIRIS_Audit_Barcode_Store]
    ON [dbo].[Reporting_PIRIS_Audit]([Barcode] ASC, [Store] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Reporting_PIRIS_Audit] TO [IRMAReportsRole]
    AS [dbo];

