CREATE TABLE [dbo].[NewItemsLoad] (
    [Identifier]           VARCHAR (13)   NOT NULL,
    [Item_ID]              VARCHAR (20)   NULL,
    [POS_Description]      VARCHAR (26)   NOT NULL,
    [Item_Description]     VARCHAR (60)   NOT NULL,
    [Package_Desc1]        DECIMAL (9, 4) NOT NULL,
    [Package_Desc2]        DECIMAL (9, 4) NOT NULL,
    [Package_Unit_ID]      INT            NOT NULL,
    [SubTeam_No]           INT            NOT NULL,
    [Category_ID]          INT            NOT NULL,
    [Vendor_Unit_ID]       INT            NOT NULL,
    [Freight_Unit_ID]      INT            NULL,
    [RetailUnit_ID]        INT            NOT NULL,
    [Distribution_Unit_ID] INT            NULL,
    [UnitCost]             MONEY          NOT NULL,
    [Cost_Unit_ID]         INT            NULL,
    [MSRP]                 MONEY          NULL,
    [UnitPrice]            MONEY          NOT NULL,
    [PriceMultiple]        TINYINT        NOT NULL,
    [RetailSale]           TINYINT        NOT NULL,
    [Discountable]         TINYINT        NOT NULL,
    [WholeSale_Unit_ID]    INT            NULL,
    [VendorID]             INT            NOT NULL,
    [IdentifierType]       CHAR (1)       NOT NULL,
    [CheckDigit]           CHAR (1)       NULL,
    [UserID]               INT            NOT NULL,
    [StoreList]            VARCHAR (7000) NOT NULL,
    [BrandID]              INT            NOT NULL,
    [Freight]              MONEY          NULL,
    [ClassID]              INT            NULL,
    [PrimVend]             BIT            NULL,
    [FoodStamp]            INT            NULL,
    [LineItemDiscount]     BIT            NULL,
    [RestrictedHours]      BIT            NULL,
    [CostedByWeight]       BIT            NULL,
    [TaxClass_ID]          INT            NOT NULL,
    [LabelType_ID]         INT            NULL,
    [National_Identifier]  BIT            NULL,
    [Vendor_Package_Desc1] DECIMAL (18)   NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT DELETE
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT INSERT
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT UPDATE
    ON OBJECT::[dbo].[NewItemsLoad] TO [IRMAExcelRole]
    AS [dbo];

