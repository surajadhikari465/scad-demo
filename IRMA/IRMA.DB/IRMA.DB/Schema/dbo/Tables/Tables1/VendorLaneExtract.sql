CREATE TABLE [dbo].[VendorLaneExtract](
	[UPC] [varchar](13) NULL,
    [ITEM_KEY] [int] NULL,
    [VIN] [varchar](20) NULL,
	[STORE_NUMBER] [varchar](20) NULL,
	[VENDOR_NUMBER] [varchar](10) NULL,
	[VENDOR_NAME] [varchar](50) NULL,
	[VENDOR_CASE_SIZE] [varchar](15) NULL,
	[VENDOR_CASE_UOM] [varchar](25) NULL,
	[VENDOR_COST_UOM] [varchar](25) NULL,
	[REG_COST] [varchar](25) NULL,
	[RETAIL_UOM] [varchar](25) NULL,
	[RETAIL_PACK] [varchar](15) NULL,
    [PRIMARY_VENDOR] [bit] NULL,
    [GLOBAL_SUBTEAM] [int] NULL
) ON [PRIMARY];

GO

GRANT ALTER, DELETE, INSERT, SELECT ON [dbo].[VendorLaneExtract] TO [IRMAPDXExtractRole] AS [dbo];
GO

GRANT ALTER, DELETE, INSERT, SELECT ON [dbo].[VendorLaneExtract] TO [IConInterface] AS [dbo];
GO
