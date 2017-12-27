CREATE TABLE dbo.ItemLocale_Supplier_NE
(
    Region                  NCHAR(2)        DEFAULT 'NE' NOT NULL,
    ItemLocaleSupplierID    INT             IDENTITY(1,1) NOT NULL,
    ItemID                  INT             NOT NULL,
    BusinessUnitID          INT             NOT NULL,
    SupplierName            NVARCHAR(255)   NOT NULL,
    SupplierItemID          NVARCHAR(20)    NULL,
    SupplierCaseSize		DECIMAL(9,4)	NULL,
    IrmaVendorKey           NVARCHAR(10)    NULL,
    AddedDateUtc            DATETIME2(7)    DEFAULT SYSUTCDATETIME() NOT NULL,
    ModifiedDateUtc         DATETIME2(7)    NULL,
    CONSTRAINT PK_ItemLocale_Supplier_NE PRIMARY KEY CLUSTERED (Region ASC, ItemLocaleSupplierID ASC) WITH (FILLFACTOR = 100) ON FG_NE,
    CONSTRAINT CK_ItemLocale_Supplier_NE_Region CHECK (Region = 'NE')
) ON FG_NE
GO

CREATE NONCLUSTERED INDEX IX_ItemLocale_Supplier_NE_ItemID_BusinessUnitID 
ON dbo.ItemLocale_Supplier_NE
(   
    ItemID ASC,
    BusinessUnitID ASC
) INCLUDE (
    SupplierName,
    SupplierItemID,
    SupplierCaseSize,
    IrmaVendorKey
) WITH (FILLFACTOR = 100) ON FG_NE
GO