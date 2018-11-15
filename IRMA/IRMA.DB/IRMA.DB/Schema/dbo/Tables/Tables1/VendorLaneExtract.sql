CREATE TABLE [dbo].[VendorLaneExtract](
	[UPC] [varchar](13) NULL,
	[STORE_NUMBER] [varchar](20) NULL,
	[VENDOR_NUMBER] [varchar](10) NULL,
	[VENDOR_NAME] [varchar](50) NULL,
	[CASE_SIZE] [varchar](15) NULL,
	[CASE_UOM] [varchar](25) NULL,
	[VENDOR_COST_UOM] [varchar](25) NULL,
	[REG_COST] [varchar](25) NULL,
	[RETAIL_UOM] [varchar](25) NULL,
	[RETAIL_PACK] [varchar](15) NULL
) ON [PRIMARY];

GO

GRANT ALTER, DELETE, INSERT, SELECT ON [dbo].[VendorLaneExtract] TO [IRMAPDXExtractRole]
Go
