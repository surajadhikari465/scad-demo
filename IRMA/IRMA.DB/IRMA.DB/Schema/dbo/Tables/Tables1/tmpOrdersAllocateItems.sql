CREATE TABLE [dbo].[tmpOrdersAllocateItems] (
    [RowID]            INT             IDENTITY (1, 1) NOT NULL,
    [Store_No]         INT             NULL,
    [SubTeam_No]       INT             NULL,
    [UserName]         VARCHAR (35)    NULL,
    [Item_Key]         INT             NULL,
    [Identifier]       VARCHAR (13)    NULL,
    [Item_Description] VARCHAR (60)    NULL,
    [Category_Name]    VARCHAR (35)    NULL,
    [Pre_Order]        BIT             NULL,
    [PackSize]         DECIMAL (18, 4) NULL,
    [BOH]              DECIMAL (18, 4) NULL,
    [WOO]              DECIMAL (18, 4) NULL,
    [SOO]              DECIMAL (18, 4) NULL,
    [FIFODateTime]     DATETIME        NULL,
    CONSTRAINT [PK_tmpOrdersAllocItems] PRIMARY KEY CLUSTERED ([RowID] ASC)
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[tmpOrdersAllocateItems] TO PUBLIC
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[tmpOrdersAllocateItems] TO PUBLIC
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[tmpOrdersAllocateItems] TO PUBLIC
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateItems] TO PUBLIC
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[tmpOrdersAllocateItems] TO PUBLIC
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[tmpOrdersAllocateItems] TO [IRMAReportsRole]
    AS [dbo];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'The date of the earliest receipt of this item on a PO where inventory is still available.', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'tmpOrdersAllocateItems', @level2type = N'COLUMN', @level2name = N'FIFODateTime';

