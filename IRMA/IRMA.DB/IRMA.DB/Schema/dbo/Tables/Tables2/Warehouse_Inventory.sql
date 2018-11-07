CREATE TABLE [dbo].[Warehouse_Inventory] (
    [Dist_Center]                INT          NOT NULL,
    [WareHouse]                  INT          NOT NULL,
    [Product_ID]                 VARCHAR (18) NOT NULL,
    [ProductDetail]              INT          NOT NULL,
    [Inv_Status]                 VARCHAR (2)  NULL,
    [Tot_BOH]                    INT          NULL,
    [Tot_Flowthru_BOH]           INT          NULL,
    [Tot_Rcv_Qty]                INT          NULL,
    [Unextract_Rcv_Qty]          INT          NULL,
    [Tot_To_Ship_Qty]            INT          NULL,
    [Tot_Rush_To_Ship_Ord_Qty]   INT          NULL,
    [Tot_UnExtract_Ship_Ord_Qty] INT          NULL,
    [Markout_Qty]                INT          NULL,
    [Tot_UnExtract_Adj_Qty]      INT          NULL,
    [Tot_In_Trans_BOH]           INT          NULL,
    [Tot_Buyer_Resrv_BOH]        INT          NULL,
    [DateCreated]                VARCHAR (10) NULL,
    [TimeCreated]                VARCHAR (8)  NULL,
    [UnitShipCase]               INT          NULL,
    CONSTRAINT [PK_Warehouse_Inventory] PRIMARY KEY CLUSTERED ([Dist_Center] ASC, [WareHouse] ASC, [Product_ID] ASC, [ProductDetail] ASC)
);


GO
GRANT ALTER
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT ALTER
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Warehouse_Inventory] TO [IRMAReportsRole]
    AS [dbo];

