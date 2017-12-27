﻿CREATE TABLE dbo.ItemLocale_Supplier_PN
(
    Region                  NCHAR(2)        DEFAULT 'PN' NOT NULL,
    ItemLocaleSupplierID    INT             IDENTITY(1,1) NOT NULL,
    ItemID                  INT             NOT NULL,
    BusinessUnitID          INT             NOT NULL,
    SupplierName            NVARCHAR(255)   NOT NULL,
    SupplierItemID          NVARCHAR(20)    NULL,
    SupplierCaseSize		DECIMAL(9,4)	NULL,
    IrmaVendorKey           NVARCHAR(10)    NULL,
    AddedDateUtc            DATETIME2(7)    DEFAULT SYSUTCDATETIME() NOT NULL,
    ModifiedDateUtc         DATETIME2(7)    NULL,
    CONSTRAINT PK_ItemLocale_Supplier_PN PRIMARY KEY CLUSTERED (Region ASC, ItemLocaleSupplierID ASC) WITH (FILLFACTOR = 100) ON FG_PN,
    CONSTRAINT CK_ItemLocale_Supplier_PN_Region CHECK (Region = 'PN')
) ON FG_PN
GO

CREATE NONCLUSTERED INDEX IX_ItemLocale_Supplier_PN_ItemID_BusinessUnitID 
ON dbo.ItemLocale_Supplier_PN
(   
    ItemID ASC,
    BusinessUnitID ASC
) INCLUDE (
    SupplierName,
    SupplierItemID,
    SupplierCaseSize,
    IrmaVendorKey
) WITH (FILLFACTOR = 100) ON FG_PN
GO