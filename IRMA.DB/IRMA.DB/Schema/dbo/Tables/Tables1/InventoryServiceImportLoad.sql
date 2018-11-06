CREATE TABLE [dbo].[InventoryServiceImportLoad] (
    [REGION]              VARCHAR (5)    NULL,
    [STORE_NAME]          VARCHAR (50)   NULL,
    [PS_BU]               INT            NULL,
    [PS_PROD_SUBTEAM]     INT            NULL,
    [PS_PROD_DESCRIPTION] VARCHAR (100)  NULL,
    [UPC]                 VARCHAR (14)   NULL,
    [COUNT]               NUMERIC (9, 4) NULL,
    [EFF_PRICE]           MONEY          NULL,
    [EFF_PRICE_EXTENDED]  MONEY          NULL,
    [FLAG]                VARCHAR (1)    NULL,
    [COST]                MONEY          NULL,
    [INV_LOC_INDEX]       VARCHAR (4)    NULL,
    [AREA]                VARCHAR (4)    NULL,
    [SECTION]             VARCHAR (4)    NULL,
    [DESCRIPTION]         VARCHAR (30)   NULL,
    [SKU]                 INT            NULL,
    [ICVID]               INT            NULL,
    [Period]              INT            NOT NULL,
    [Year]                INT            NOT NULL,
    [InsertDate]          DATETIME       CONSTRAINT [DF_InventoryServiceImportLoad_InsertDate] DEFAULT (getdate()) NOT NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryServiceImportLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryServiceImportLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryServiceImportLoad] TO [IRMAReportsRole]
    AS [dbo];

