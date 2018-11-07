/******************************************************************************
		Drop Columns on 6 SO Tables:
		1. DeletedOrder
		2. PriceBatchDetailArchive
		3. PriceHistory
		4. Store
		5. StoreItemVendor
		6. VendorCostHistory
******************************************************************************/
PRINT N'Dropping Columns on 6 SO Tables... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE ItemCatalog
--USE ItemCatalog_Test
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		Deleted Order
******************************************************************************/
PRINT N'Altering [dbo].[DeletedOrder]... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[DeletedOrder]
DROP CONSTRAINT [DF_DeletedOrder_Host_Name]
GO
ALTER TABLE [dbo].[DeletedOrder]
DROP COLUMN [Host_Name];
GO
/******************************************************************************
		PriceBatchDetailArchive
******************************************************************************/
PRINT N'Altering [dbo].[PriceBatchDetailArchive]... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[PriceBatchDetailArchive]
DROP COLUMN [Competitive],
	COLUMN [CompetitiveLastChecked],
	COLUMN [CompetitiveMultiple],
	COLUMN [CompetitivePrice],
	COLUMN [GlutenFreeTag],
	COLUMN [LocalTag],
	COLUMN [Tax_Table_A],
	COLUMN [Tax_Table_B],
	COLUMN [Tax_Table_C],
	COLUMN [Tax_Table_D];
GO
/******************************************************************************
		PriceHistory
******************************************************************************/
PRINT N'Altering [dbo].[PriceHistory]... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[PriceHistory]
DROP COLUMN [Tax_Table_A],
	COLUMN [Tax_Table_B],
	COLUMN [Tax_Table_C],
	COLUMN [Tax_Table_D],
	COLUMN [Competitive],
	COLUMN [CompetitiveMultiple],
	COLUMN [CompetitivePrice],
	COLUMN [CompetitiveLastChecked],
	COLUMN [LocalTag],
	COLUMN [GlutenFreeTag]

GO
/******************************************************************************
		Store
******************************************************************************/
PRINT N'Altering [dbo].[Store]... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[Store]
DROP COLUMN [ShelfTagStockType],
	COLUMN [TelnetPasswordPrevious];
GO
ALTER TABLE [dbo].[Store]
ALTER COLUMN [TelnetPassword] VARCHAR(25) NULL;
GO
/******************************************************************************
		StoreItemVendor
******************************************************************************/
PRINT N'Altering [dbo].[StoreItemVendor]... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[StoreItemVendor]
DROP COLUMN [Distribution_Markup];
GO
/******************************************************************************
		VendorCostHistory
******************************************************************************/
PRINT N'Altering [dbo].[VendorCostHistory]... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[VendorCostHistory]
DROP COLUMN [P2P_Reviewed],
	COLUMN [Source];
GO
/******************************************************************************
		Operation Complete
******************************************************************************/
PRINT N'Status: Operation Complete... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO