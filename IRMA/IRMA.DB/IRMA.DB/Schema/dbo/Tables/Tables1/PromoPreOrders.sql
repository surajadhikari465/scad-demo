CREATE TABLE [dbo].[PromoPreOrders] (
    [PromoPreOrderID]   INT          IDENTITY (1, 1) NOT NULL,
    [PriceBatchPromoID] INT          NOT NULL,
    [Item_Key]          INT          NOT NULL,
    [Identifier]        VARCHAR (13) NOT NULL,
    [Store_No]          INT          NOT NULL,
    [OrderQty]          INT          NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromoPreOrders] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[PromoPreOrders] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[PromoPreOrders] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[PromoPreOrders] TO [IRMAPromoRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[PromoPreOrders] TO [IRMAPromoRole]
    AS [dbo];

