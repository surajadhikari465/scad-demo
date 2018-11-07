/******************************************************************************
		1. Rebuild Users Table and Drop Extraneous Columns
******************************************************************************/
PRINT N'Status: 1. Rebuild Users Table and Drop Extraneous Columns --- [dbo].[Users] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO	
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[Users] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 1, SYSDATETIME()), 9)
GO
USE ItemCatalog
--USE ItemCatalog_Test
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[Users] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO	
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CoverPage]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_CoverPage]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__AccountEn__7B3C2211]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__AccountEn__7B3C2211]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__SuperUser__7C30464A]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__SuperUser__7C30464A]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Buyer__7E188EBC]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Buyer__7E188EBC]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Accountan__7F0CB2F5]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Accountan__7F0CB2F5]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Distribut__0000D72E]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Distribut__0000D72E]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Approve_T__00F4FB67]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Approve_T__00F4FB67]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Reserved___01E91FA0]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Reserved___01E91FA0]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Reserved___03D16812]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Reserved___03D16812]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Item_Administrator]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_Item_Administrator]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Vendor_Administrator]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_Vendor_Administrator]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Lock_Administrator]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_Lock_Administrator]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Warehouse__6AE38E7C]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Warehouse__6AE38E7C]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_PriceBatchProcessor]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_PriceBatchProcessor]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Inventory_Administrator]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_Inventory_Administrator]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_DCFlatRateAdmin]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_DCFlatRateAdmin]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Cost_Administrator]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_Cost_Administrator]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_CanAdjUploadedPO]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_CanAdjUploadedPO]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__BatchBuil__486EACB1]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__BatchBuil__486EACB1]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__DCAdmin__2B8C4DF3]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__DCAdmin__2B8C4DF3]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_CostAdmin]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_CostAdmin]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_POApprovalAdmin]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF_Users_POApprovalAdmin]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__EInvoicin__7DA71E60]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__EInvoicin__7DA71E60]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__VendorCos__00838B0B]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__VendorCos__00838B0B]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Applicati__6960F9E8]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Applicati__6960F9E8]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__DataAdmin__6A551E21]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__DataAdmin__6A551E21]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__JobAdmini__6B49425A]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__JobAdmini__6B49425A]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__POSInterf__6C3D6693]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__POSInterf__6C3D6693]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__SecurityA__6D318ACC]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__SecurityA__6D318ACC]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__StoreAdmi__6E25AF05]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__StoreAdmi__6E25AF05]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__SystemCon__6F19D33E]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__SystemCon__6F19D33E]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__UserMaint__700DF777]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__UserMaint__700DF777]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Shrink__2D1705B7]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__Shrink__2D1705B7]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__ShrinkAdm__2E0B29F0]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__ShrinkAdm__2E0B29F0]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__POEditor__4EAE9A5A]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__POEditor__4EAE9A5A]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__DeletePO__471930E1]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__DeletePO__471930E1]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__TaxAdmini__2CB67977]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__TaxAdmini__2CB67977]
GO
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__CancelAll__7CCF6099]') AND type = 'D')
	ALTER TABLE [Users] DROP CONSTRAINT [DF__Users__CancelAll__7CCF6099]
GO
/******************************************************************************
		3. Drop Foreign Keys from SO Users
******************************************************************************/
PRINT N'Status: 3. Drop Foreign Keys from SO Users... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Title]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_TelxonStoreLimit]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_TelxonStoreLimit]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Store1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Store1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Store]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_Store]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_AccessLevel]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] DROP CONSTRAINT [FK_Users_AccessLevel]
GO
/******************************************************************************
		4. Drop Indexes from SO Users
******************************************************************************/
PRINT N'Status: 4. Drop Indexes from SO Users... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'idxUserName')
DROP INDEX [idxUserName] ON [dbo].[Users]
GO
/******************************************************************************
		5. Drop Columns from SO Users
******************************************************************************/
PRINT N'Status: 5. Drop Columns from SO Users... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
ALTER TABLE [dbo].[Users]
DROP COLUMN [CanAdjUploadedPO],
	COLUMN [Cost_Administrator],
	COLUMN [DCFlatRateAdmin],
	COLUMN [LastFacilityID];
GO
/******************************************************************************
		6. Add Indexes From FL Users
******************************************************************************/
PRINT N'Status: 6. Add Indexes From FL Users... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND name = N'idxUserName')
CREATE UNIQUE NONCLUSTERED INDEX [idxUserName] ON [dbo].[Users]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		7. Add Foreign Keys From FL Users
******************************************************************************/
PRINT N'Status: 7. Add Foreign Keys From FL Users... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_AccessLevel]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_AccessLevel] FOREIGN KEY([PromoAccessLevel])
REFERENCES [dbo].[UserAccess] ([UserAccessLevel_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_AccessLevel]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_AccessLevel]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Store1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Store1] FOREIGN KEY([Telxon_Store_Limit])
REFERENCES [dbo].[Store] ([Store_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Store1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Store1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_TelxonStoreLimit]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_TelxonStoreLimit] FOREIGN KEY([Telxon_Store_Limit])
REFERENCES [dbo].[Store] ([Store_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_TelxonStoreLimit]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_TelxonStoreLimit]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Title] FOREIGN KEY([Title])
REFERENCES [dbo].[Title] ([Title_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Users_Title]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Title]
GO
/******************************************************************************
		8. Add Constraints From FL Users
******************************************************************************/
PRINT N'Status: 8. Add Constraints From FL Users... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);;
GO		
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_CoverPage]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_CoverPage] DEFAULT ('default')
	FOR [CoverPage]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__AccountEn__7B3C2211]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__AccountEn__7B3C2211] DEFAULT ((1))
	FOR [AccountEnabled]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__SuperUser__7C30464A]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__SuperUser__7C30464A] DEFAULT ((0))
	FOR [SuperUser]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Buyer__7E188EBC]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Buyer__7E188EBC] DEFAULT ((0))
	FOR [PO_Accountant]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Accountan__7F0CB2F5]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Accountan__7F0CB2F5] DEFAULT ((0))
	FOR [Accountant]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Distribut__0000D72E]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Distribut__0000D72E] DEFAULT ((0))
	FOR [Distributor]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Approve_T__00F4FB67]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Approve_T__00F4FB67] DEFAULT ((0))
	FOR [FacilityCreditProcessor]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Reserved___01E91FA0]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Reserved___01E91FA0] DEFAULT ((0))
	FOR [Buyer]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Reserved___03D16812]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Reserved___03D16812] DEFAULT ((0))
	FOR [Coordinator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Item_Administrator]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_Item_Administrator] DEFAULT ((0))
	FOR [Item_Administrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Vendor_Administrator]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_Vendor_Administrator] DEFAULT ((0))
	FOR [Vendor_Administrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Lock_Administrator]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_Lock_Administrator] DEFAULT ((0))
	FOR [Lock_Administrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Warehouse__6AE38E7C]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Warehouse__6AE38E7C] DEFAULT ((0))
	FOR [Warehouse]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_PriceBatchProcessor]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_PriceBatchProcessor] DEFAULT ((0))
	FOR [PriceBatchProcessor]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_Inventory_Administrator]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_Inventory_Administrator] DEFAULT ((0))
	FOR [Inventory_Administrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__BatchBuil__6176F277]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__BatchBuil__6176F277] DEFAULT ((0))
	FOR [BatchBuildOnly]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__DCAdmin__1D841CE2]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__DCAdmin__1D841CE2] DEFAULT ((0))
	FOR [DCAdmin]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_CostAdmin]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_CostAdmin] DEFAULT ((0))
	FOR [CostAdmin]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Users_POApprovalAdmin]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF_Users_POApprovalAdmin] DEFAULT ((0))
	FOR [POApprovalAdmin]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__EInvoicin__06298603]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__EInvoicin__06298603] DEFAULT ((0))
	FOR [EInvoicing_Administrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__VendorCos__1848363E]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__VendorCos__1848363E] DEFAULT ((0))
	FOR [VendorCostDiscrepancyAdmin]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Applicati__51ED29F3]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Applicati__51ED29F3] DEFAULT ((0))
	FOR [ApplicationConfigAdmin]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__DataAdmin__52E14E2C]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__DataAdmin__52E14E2C] DEFAULT ((0))
	FOR [DataAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__JobAdmini__53D57265]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__JobAdmini__53D57265] DEFAULT ((0))
	FOR [JobAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__POSInterf__54C9969E]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__POSInterf__54C9969E] DEFAULT ((0))
	FOR [POSInterfaceAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__SecurityA__55BDBAD7]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__SecurityA__55BDBAD7] DEFAULT ((0))
	FOR [SecurityAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__StoreAdmi__56B1DF10]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__StoreAdmi__56B1DF10] DEFAULT ((0))
	FOR [StoreAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__SystemCon__57A60349]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__SystemCon__57A60349] DEFAULT ((0))
	FOR [SystemConfigurationAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__UserMaint__589A2782]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__UserMaint__589A2782] DEFAULT ((0))
	FOR [UserMaintenance]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__Shrink__0E0213FA]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__Shrink__0E0213FA] DEFAULT ((0))
	FOR [Shrink]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__ShrinkAdm__0EF63833]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__ShrinkAdm__0EF63833] DEFAULT ((0))
	FOR [ShrinkAdmin]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__POEditor__078C829A]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__POEditor__078C829A] DEFAULT ((0))
	FOR [POEditor]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__DeletePO__0C5137B7]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__DeletePO__0C5137B7] DEFAULT ((0))
	FOR [DeletePO]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__TaxAdmini__7035B2A4]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__TaxAdmini__7035B2A4] DEFAULT ((0))
	FOR [TaxAdministrator]
GO
IF NOT EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF__Users__CancelAll__7EF76E63]') AND type = 'D')
	ALTER TABLE [Users] WITH NOCHECK ADD CONSTRAINT [DF__Users__CancelAll__7EF76E63] DEFAULT ((0))
	FOR [CancelAllSales]
GO
/******************************************************************************
		9. Operation Complete
******************************************************************************/
PRINT N'Status: 9. Operation Complete... ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9);
GO
