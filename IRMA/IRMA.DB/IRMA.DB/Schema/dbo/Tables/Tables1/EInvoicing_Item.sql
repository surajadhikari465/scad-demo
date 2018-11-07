CREATE TABLE [dbo].[EInvoicing_Item] (
    [EInvoice_id]       INT             NOT NULL,
    [line_num]          INT             NOT NULL,
    [upc]               VARCHAR (255)   NULL,
    [vendor_item_num]   VARCHAR (255)   NULL,
    [descrip]           VARCHAR (255)   NULL,
    [brand]             VARCHAR (255)   NULL,
    [lot_num]           VARCHAR (255)   NULL,
    [case_uom]          VARCHAR (10)    NULL,
    [item_qtyper]       DECIMAL (11, 4) NULL,
    [item_uom]          VARCHAR (10)    NULL,
    [case_pack]         DECIMAL (11, 4) NOT NULL,
    [alt_ordering_qty]  DECIMAL (11, 4) NULL,
    [alt_ordering_uom]  VARCHAR (10)    NULL,
    [unit_cost]         MONEY           NULL,
    [qty_shipped]       DECIMAL (11, 4) NULL,
    [ext_cost]          MONEY           NULL,
    [net_ext_cost]      MONEY           NULL,
    [calc_net_ext_cost] MONEY           NULL,
    [Item_Key]          INT             NULL,
    [IsNotIdentifiable] BIT             NULL,
    [IsNotOrdered]      BIT             NULL,
    CONSTRAINT [EInvoicingItem_PK] PRIMARY KEY CLUSTERED ([EInvoice_id] ASC, [line_num] ASC),
    CONSTRAINT [FK_EInvoicing_Item_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
ALTER TABLE [dbo].[EInvoicing_Item] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = OFF);


GO
CREATE NONCLUSTERED INDEX [IX_Einvoicing_Item_UPC]
    ON [dbo].[EInvoicing_Item]([upc] ASC);


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IRMA_Teradata]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [BizTalk]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [IConInterface]
    AS [dbo];


GO
GRANT VIEW CHANGE TRACKING
    ON OBJECT::[dbo].[EInvoicing_Item] TO [iCONReportingRole]
    AS [dbo];

