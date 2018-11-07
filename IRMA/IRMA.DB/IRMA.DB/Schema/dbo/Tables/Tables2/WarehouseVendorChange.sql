CREATE TABLE [dbo].[WarehouseVendorChange] (
    [WarehouseVendorChangeID] INT      IDENTITY (1, 1) NOT NULL,
    [Store_No]                INT      NOT NULL,
    [Vendor_ID]               INT      NOT NULL,
    [ChangeType]              CHAR (1) NOT NULL,
    [Customer]                BIT      NOT NULL,
    [InsertDate]              DATETIME CONSTRAINT [DF_WarehouseVendorChange_InsertDate] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_WarehouseVendorChange] PRIMARY KEY CLUSTERED ([WarehouseVendorChangeID] ASC) WITH (FILLFACTOR = 80),
    CONSTRAINT [FK_WarehouseVendorChange_Store] FOREIGN KEY ([Store_No]) REFERENCES [dbo].[Store] ([Store_No]),
    CONSTRAINT [FK_WarehouseVendorChange_Vendor] FOREIGN KEY ([Vendor_ID]) REFERENCES [dbo].[Vendor] ([Vendor_ID])
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[WarehouseVendorChange] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[WarehouseVendorChange] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[WarehouseVendorChange] TO [IRMAReportsRole]
    AS [dbo];

