CREATE TABLE dbo.ItemLocale_Supplier_UK
(
    Region                  NCHAR(2)        DEFAULT 'UK' NOT NULL,
    ItemLocaleSupplierID    INT             IDENTITY(1,1) NOT NULL,
    ItemID                  INT             NOT NULL,
    BusinessUnitID          INT             NOT NULL,
    SupplierName            NVARCHAR(255)   NOT NULL,
    SupplierItemID          NVARCHAR(20)    NULL,
    SupplierCaseSize        INT             NULL,
    IrmaVendorKey           NVARCHAR(10)    NULL,
    AddedDateUtc            DATETIME2(7)    DEFAULT SYSUTCDATETIME() NOT NULL,
    ModifiedDateUtc         DATETIME2(7)    NULL,
    CONSTRAINT PK_ItemLocale_Supplier_UK PRIMARY KEY CLUSTERED (Region ASC, ItemLocaleSupplierID ASC) WITH (FILLFACTOR = 100) ON FG_UK,
    CONSTRAINT CK_ItemLocale_Supplier_UK_Region CHECK (Region = 'UK')
) ON FG_UK
GO

CREATE NONCLUSTERED INDEX IX_ItemLocale_Supplier_UK_ItemID_BusinessUnitID 
ON dbo.ItemLocale_Supplier_UK
(   
    ItemID ASC,
    BusinessUnitID ASC
) INCLUDE (
    SupplierName,
    SupplierItemID,
    SupplierCaseSize,
    IrmaVendorKey
) WITH (FILLFACTOR = 100) ON FG_UK
GO