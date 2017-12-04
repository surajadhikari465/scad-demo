CREATE TABLE dbo.ItemLocale_Supplier_MA
(
    Region                  NCHAR(2)        DEFAULT 'MA' NOT NULL,
    ItemLocaleSupplierID    INT             IDENTITY(1,1) NOT NULL,
    ItemID                  INT             NOT NULL,
    BusinessUnitID          INT             NOT NULL,
    SupplierName            NVARCHAR(255)   NOT NULL,
    SupplierItemID          NVARCHAR(20)    NULL,
    SupplierCaseSize        INT             NULL,
    IrmaVendorKey           NVARCHAR(10)    NULL,
    AddedDateUtc            DATETIME2(7)    DEFAULT SYSUTCDATETIME() NOT NULL,
    ModifiedDateUtc         DATETIME2(7)    NULL,
    CONSTRAINT PK_ItemLocale_Supplier_MA PRIMARY KEY CLUSTERED (Region ASC, ItemLocaleSupplierID ASC) WITH (FILLFACTOR = 100) ON FG_MA,
    CONSTRAINT CK_ItemLocale_Supplier_MA_Region CHECK (Region = 'MA')
) ON FG_MA
GO

CREATE NONCLUSTERED INDEX IX_ItemLocale_Supplier_MA_ItemID_BusinessUnitID 
ON dbo.ItemLocale_Supplier_MA
(   
    ItemID ASC,
    BusinessUnitID ASC
) INCLUDE (
    SupplierName,
    SupplierItemID,
    SupplierCaseSize,
    IrmaVendorKey
) WITH (FILLFACTOR = 100) ON FG_MA
GO