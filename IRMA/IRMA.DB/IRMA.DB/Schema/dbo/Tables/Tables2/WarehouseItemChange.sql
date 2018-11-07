CREATE TABLE [dbo].[WarehouseItemChange] (
    [WarehouseItemChangeID] INT      IDENTITY (1, 1) NOT NULL,
    [Store_No]              INT      NOT NULL,
    [Item_Key]              INT      NOT NULL,
    [ChangeType]            CHAR (1) NOT NULL,
    [InsertDate]            DATETIME CONSTRAINT [DF_WarehouseItemChange_InsertDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WarehouseItemChange] PRIMARY KEY CLUSTERED ([WarehouseItemChangeID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_WarehouseItemChange_Item] FOREIGN KEY ([Item_Key]) REFERENCES [dbo].[Item] ([Item_Key]),
    CONSTRAINT [FK_WarehouseItemChange_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[WarehouseItemChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[WarehouseItemChange] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[WarehouseItemChange] TO [IRMAReportsRole]
    AS [dbo];

