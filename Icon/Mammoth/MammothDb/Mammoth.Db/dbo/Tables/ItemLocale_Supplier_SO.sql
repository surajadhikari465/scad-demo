CREATE TABLE dbo.ItemLocale_Supplier_SO
(
    Region                  NCHAR(2)        DEFAULT 'SO' NOT NULL,
    ItemLocaleSupplierID    INT             IDENTITY(1,1) NOT NULL,
    ItemID                  INT             NOT NULL,
    BusinessUnitID          INT             NOT NULL,
    SupplierName            NVARCHAR(255)   NOT NULL,
    SupplierItemID          NVARCHAR(20)    NULL,
    SupplierCaseSize		DECIMAL(9,4)	NULL,
    IrmaVendorKey           NVARCHAR(10)    NULL,
    AddedDateUtc            DATETIME2(7)    DEFAULT SYSUTCDATETIME() NOT NULL,
    ModifiedDateUtc         DATETIME2(7)    NULL,
    CONSTRAINT PK_ItemLocale_Supplier_SO PRIMARY KEY CLUSTERED (Region ASC, ItemLocaleSupplierID ASC) WITH (FILLFACTOR = 100) ON FG_SO,
    CONSTRAINT CK_ItemLocale_Supplier_SO_Region CHECK (Region = 'SO')
) ON FG_SO
GO

CREATE NONCLUSTERED INDEX IX_ItemLocale_Supplier_SO_ItemID_BusinessUnitID 
ON dbo.ItemLocale_Supplier_SO
(   
    ItemID ASC,
    BusinessUnitID ASC
) INCLUDE (
    SupplierName,
    SupplierItemID,
    SupplierCaseSize,
    IrmaVendorKey
) WITH (FILLFACTOR = 100) ON FG_SO
GO