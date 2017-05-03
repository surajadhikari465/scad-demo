CREATE TABLE [dbo].[Shipper] (
    [Shipper_Key] INT             NOT NULL,
    [Item_Key]    INT             NOT NULL,
    [Quantity]    DECIMAL (18, 4) CONSTRAINT [DF__Shipper__Quantit__740F363E] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Shipper_ShipperKey_ItemKey] PRIMARY KEY NONCLUSTERED ([Shipper_Key] ASC, [Item_Key] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_Shipper_Item2] FOREIGN KEY ([Shipper_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_Shipper_Item3] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [idxShipperID]
    ON [dbo].[Shipper]([Shipper_Key] ASC, [Item_Key] ASC) WITH (FILLFACTOR = 80);


GO
ALTER INDEX [idxShipperID]
    ON [dbo].[Shipper] DISABLE;


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[Shipper] TO [IRMAPromoRole]
    AS [dbo];

