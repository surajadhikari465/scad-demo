CREATE TABLE [dbo].[InventoryServiceExportLoad] (
    [Region]           VARCHAR (2)    NOT NULL,
    [Store_Name]       VARCHAR (50)   NOT NULL,
    [BusinessUnit_ID]  INT            NOT NULL,
    [SubTeam_No]       INT            NOT NULL,
    [SubTeam_Name]     VARCHAR (100)  NOT NULL,
    [Identifier]       VARCHAR (13)   NOT NULL,
    [Item_Description] VARCHAR (65)   NOT NULL,
    [Price]            MONEY          NOT NULL,
    [AvgCost]          MONEY          NOT NULL,
    [Item_Key]         INT            NOT NULL,
    [ICVID]            INT            CONSTRAINT [DF_InventoryServiceExportLoad_ICVID] DEFAULT ((0)) NOT NULL,
    [Period]           INT            NOT NULL,
    [Year]             INT            NOT NULL,
    [InsertDate]       SMALLDATETIME  CONSTRAINT [DF_InventoryServiceExportLoad_InsertDate] DEFAULT (getdate()) NOT NULL,
    [Vendor_ID]        INT            NULL,
    [Package_Desc1]    DECIMAL (9, 4) NULL,
    [RetailUnit]       VARCHAR (5)    NULL
);


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryServiceExportLoad] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryServiceExportLoad] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT SELECT
    ON OBJECT::[dbo].[InventoryServiceExportLoad] TO [IRMAReportsRole]
    AS [dbo];

