CREATE TABLE [irma].[StoreItemVendor] (
    [Region]				NCHAR(2)	  NOT NULL,
	[StoreItemVendorID]     INT           NOT NULL,
    [Store_No]              INT           NOT NULL,
    [Item_Key]              INT           NOT NULL,
    [Vendor_ID]             INT           NOT NULL,
    [AverageDelivery]       SMALLINT      NULL,
    [PrimaryVendor]         BIT           CONSTRAINT [DF_StoreItemVendor_PrimaryVendor] DEFAULT ((0)) NOT NULL,
    [DeleteDate]            SMALLDATETIME NULL,
    [DeleteWorkStation]     VARCHAR (255) NULL,
    [LastCostAddedDate]     DATETIME      NULL,
    [LastCostRefreshedDate] DATETIME      CONSTRAINT [DF_StoreItemVendor_LastCostRefreshedDate] DEFAULT (getdate()) NOT NULL,
    [DiscontinueItem]       BIT           CONSTRAINT [DF_StoreItemVendor_DiscontinueItem] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK__StoreItemVender] PRIMARY KEY CLUSTERED ([Region] ASC, [StoreItemVendorID] ASC) WITH (FILLFACTOR = 100)
);

