CREATE TABLE [dbo].[EInvoicing_ItemData] (
    [EInvoice_Id]  INT           NOT NULL,
    [ItemId]       INT           NOT NULL,
    [ElementName]  VARCHAR (255) NOT NULL,
    [ElementValue] VARCHAR (255) NULL
);


GO
CREATE NONCLUSTERED INDEX [IX_EInvoicingItemData_Id]
    ON [dbo].[EInvoicing_ItemData]([EInvoice_Id] ASC)
    INCLUDE([ElementName]) WITH (FILLFACTOR = 90);


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ItemData] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_ItemData] TO [IRMAReportsRole]
    AS [dbo];

