/******************************************************************************
		SO [dbo].[Vendor]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[Vendor] ROLLBACK  --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[Vendor]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 30, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[Vendor] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor1__Custome__1C873BEC]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor1__Interna__1D7B6025]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor1__ActiveV__1E6F845E]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor1__User4__2334397B]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor1__User5__24285DB4]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_PS_Location_Code]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__vendor__WFM__2E82F504]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF__vendor__Non_Prod__7CAD38FC]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_EFT]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_InStoreManufacturedProducts]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_EXEWarehouseVendSent]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_EXEWarehouseCustSent]
GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_AddVendor]
GO
--ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor__EInvoici__5171CDEF]
--GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_LeadTimeDays]
GO
--ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor__Einvoice__4B1C69EA]
--GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_AllowReceiveAll]
GO
--ALTER TABLE [Vendor] DROP CONSTRAINT [DF__Vendor__Shortpay__55881720]
--GO
ALTER TABLE [Vendor] DROP CONSTRAINT [DF_Vendor_AllowBarcodePOReport]
GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[VendorUpdate]'))
DROP TRIGGER [dbo].[VendorUpdate]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[VendorInsert]'))
DROP TRIGGER [dbo].[VendorInsert]
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_Store1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] DROP CONSTRAINT [FK_Vendor_Store1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_POTransmissionType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] DROP CONSTRAINT [FK_Vendor_POTransmissionType]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] DROP CONSTRAINT [FK_Vendor_1__13]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] DROP CONSTRAINT [FK_Currency_Vendor]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorStoreNo')
DROP INDEX [idxVendorStoreNo] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorCompany')
DROP INDEX [idxVendorCompany] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxOrganizationUserID')
DROP INDEX [idxOrganizationUserID] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxBatchHeaderVendor')
DROP INDEX [idxBatchHeaderVendor] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Vendor_StoreNo_VendorID')
DROP INDEX [_dta_IX_Vendor_StoreNo_VendorID] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Vendor_ID_Store_no')
DROP INDEX [_dta_IX_Vendor_ID_Store_no] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Vendor_CompanyName')
DROP INDEX [_dta_IX_Vendor_CompanyName] ON [dbo].[Vendor]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Store_no_CompanyName')
DROP INDEX [_dta_IX_Store_no_CompanyName] ON [dbo].[Vendor]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'PK_Vendor_Vendor_ID')
EXECUTE sp_rename N'[dbo].[PK_Vendor_Vendor_ID]', N'PK_Vendor_Vendor_ID_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor_Unaligned]') AND name = N'PK_Vendor_Vendor_ID_Unaligned')
EXECUTE sp_rename N'[dbo].[PK_Vendor_Vendor_ID_Unaligned]', N'PK_Vendor_Vendor_ID';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[Vendor]', N'Vendor_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Vendor_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[Vendor_Unaligned]', N'Vendor';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor1__Custome__1C873BEC] DEFAULT (0) FOR [Customer]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor1__Interna__1D7B6025] DEFAULT (0) FOR [InternalCustomer]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor1__ActiveV__1E6F845E] DEFAULT (0) FOR [ActiveVendor]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor1__User4__2334397B] DEFAULT (0) FOR [Order_By_Distribution]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_PS_Location_Code] DEFAULT ('DEFAULT') FOR [PS_Location_Code]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__vendor__WFM__2E82F504] DEFAULT (0) FOR [WFM]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__vendor__Non_Prod__7CAD38FC] DEFAULT (0) FOR [Non_Product_Vendor]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_EFT] DEFAULT (0) FOR [EFT]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_InStoreManufacturedProducts] DEFAULT (0) FOR [InStoreManufacturedProducts]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_EXEWarehouseVendSent] DEFAULT (0) FOR [EXEWarehouseVendSent]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_EXEWarehouseCustSent] DEFAULT (0) FOR [EXEWarehouseCustSent]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_P2PIntegrator_ISA_InterchangeControlNumber] DEFAULT ((1)) FOR [EDI_ISA_InterchangeControlNumber]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_AcceptsFaxOrders] DEFAULT ((0)) FOR [AcceptsFaxOrders]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_DASVendor] DEFAULT ((0)) FOR [DASVendor]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_AddVendor] DEFAULT ((0)) FOR [AddVendor]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor__Electron__60C3C281] DEFAULT ((0)) FOR [Electronic_Transfer]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor__EInvoici__4B25C034] DEFAULT ((0)) FOR [EInvoicing]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_LeadTimeDays] DEFAULT ((0)) FOR [LeadTimeDays]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor__Einvoice__74A7F5D5] DEFAULT ((0)) FOR [EinvoiceRequired]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_AllowReceiveAll] DEFAULT ((0)) FOR [AllowReceiveAll]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF__Vendor__Shortpay__3D70C837] DEFAULT ((0)) FOR [ShortpayProhibited]
GO
ALTER TABLE [Vendor] WITH NOCHECK ADD CONSTRAINT [DF_Vendor_AllowBarcodePOReport] DEFAULT ((0)) FOR [AllowBarcodePOReport]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[Vendor] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_PADDING ON
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [BizTalk] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [ExtractRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [iCONReportingRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IMHARole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IRMA_Teradata] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAAdminRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAAVCIRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAExcelRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAPromoRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IRMAReports] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT DELETE ON [dbo].[Vendor] TO [IRMASLIMRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[Vendor] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMASLIMRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[Vendor] TO [IRMASLIMRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRMASupportRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [MammothRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [SOAppsUserAdmin] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [sobluesky] AS [dbo]
GO
GRANT SELECT ON [dbo].[Vendor] TO [SODataControl] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[Vendor] TO [spice_user] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Store_no_CompanyName')
CREATE NONCLUSTERED INDEX [_dta_IX_Store_no_CompanyName] ON [dbo].[Vendor]
(
	[Store_no] ASC,
	[CompanyName] ASC
)
INCLUDE ( 	[Vendor_ID],
	[Vendor_Key]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Vendor_CompanyName')
CREATE NONCLUSTERED INDEX [_dta_IX_Vendor_CompanyName] ON [dbo].[Vendor]
(
	[CompanyName] ASC
)
INCLUDE ( 	[Vendor_ID],
	[Store_no],
	[PS_Vendor_ID],
	[WFM]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Vendor_ID_Store_no')
CREATE NONCLUSTERED INDEX [_dta_IX_Vendor_ID_Store_no] ON [dbo].[Vendor]
(
	[Vendor_ID] ASC,
	[Store_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'_dta_IX_Vendor_StoreNo_VendorID')
CREATE NONCLUSTERED INDEX [_dta_IX_Vendor_StoreNo_VendorID] ON [dbo].[Vendor]
(
	[Store_no] ASC,
	[Vendor_ID] ASC
)
INCLUDE ( 	[State]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxOrganizationUserID')
CREATE NONCLUSTERED INDEX [idxOrganizationUserID] ON [dbo].[Vendor]
(
	[User_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorCompany')
CREATE NONCLUSTERED INDEX [idxVendorCompany] ON [dbo].[Vendor]
(
	[CompanyName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorIDCompanyName')
CREATE NONCLUSTERED INDEX [idxVendorIDCompanyName] ON [dbo].[Vendor]
(
	[Vendor_ID] ASC
)
INCLUDE ( 	[CompanyName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorOrderAlloc')
CREATE NONCLUSTERED INDEX [idxVendorOrderAlloc] ON [dbo].[Vendor]
(
	[Store_no] ASC,
	[Vendor_ID] ASC
)
INCLUDE ( 	[CompanyName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorScanGunOrderInfo')
CREATE NONCLUSTERED INDEX [idxVendorScanGunOrderInfo] ON [dbo].[Vendor]
(
	[Vendor_ID] ASC
)
INCLUDE ( 	[Vendor_Key],
	[CompanyName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Vendor]') AND name = N'idxVendorStoreNo')
CREATE NONCLUSTERED INDEX [idxVendorStoreNo] ON [dbo].[Vendor]
(
	[Store_no] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor]  WITH CHECK ADD  CONSTRAINT [FK_Currency_Vendor] FOREIGN KEY([CurrencyID])
REFERENCES [dbo].[Currency] ([CurrencyID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] CHECK CONSTRAINT [FK_Currency_Vendor]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor]  WITH NOCHECK ADD  CONSTRAINT [FK_Vendor_1__13] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_1__13]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] CHECK CONSTRAINT [FK_Vendor_1__13]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_POTransmissionType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor]  WITH CHECK ADD  CONSTRAINT [FK_Vendor_POTransmissionType] FOREIGN KEY([POTransmissionTypeID])
REFERENCES [dbo].[POTransmissionType] ([POTransmissionTypeID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_POTransmissionType]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] CHECK CONSTRAINT [FK_Vendor_POTransmissionType]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_Store1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor]  WITH NOCHECK ADD  CONSTRAINT [FK_Vendor_Store1] FOREIGN KEY([Store_no])
REFERENCES [dbo].[Store] ([Store_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Vendor_Store1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Vendor]'))
ALTER TABLE [dbo].[Vendor] CHECK CONSTRAINT [FK_Vendor_Store1]
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[VendorInsert]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[VendorInsert]
ON [dbo].[Vendor]
FOR INSERT 
AS 
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    -- Add to queue for EXE for Vendors who are stores in an EXE warehouse''s zone
    INSERT INTO WarehouseVendorChange (Store_No, Vendor_ID, ChangeType, Customer)
	SELECT Supplier_Store_No, Inserted.Vendor_ID, ''A'', 1
    FROM Inserted
    INNER JOIN
        Store CustStore ON CustStore.Store_No = Inserted.Store_No
    INNER JOIN
        ZoneSubTeam ON ZoneSubTeam.Zone_ID = CustStore.Zone_ID
    INNER JOIN
        Store VendStore ON VendStore.Store_No = ZoneSubTeam.Supplier_Store_No
    WHERE VendStore.EXEWarehouse IS NOT NULL
    SELECT @Error_No = @@ERROR
    IF @Error_No = 0
    BEGIN
		UPDATE VendorExportQueue
		SET QueueInsertedDate = GetDate(), 
			DeliveredToStoreOpsDate = NULL,
			Old_PS_Vendor_ID = (Select PS_Vendor_ID From Deleted)
		WHERE Vendor_ID in
			(
			SELECT DISTINCT Inserted.Vendor_ID
			FROM 
				Inserted 
			INNER JOIN
				Deleted 
				ON Inserted.Vendor_ID = Deleted.Vendor_ID
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 AND
				(Inserted.PS_Vendor_ID IS NOT NULL OR Inserted.PS_Export_Vendor_ID IS NOT NULL)
			)
		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO VendorExportQueue 
			SELECT Inserted.Vendor_ID, GetDate(), NULL, Inserted.PS_Vendor_ID
			FROM Inserted
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 AND 
				(PS_Vendor_ID IS NOT NULL OR PS_Export_Vendor_ID IS NOT NULL)
		END    
        SELECT @Error_No = @@ERROR
    END
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''VendorInsert trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[VendorUpdate]'))
EXEC dbo.sp_executesql @statement = N'
CREATE Trigger [dbo].[VendorUpdate]
ON [dbo].[Vendor]
FOR UPDATE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
    -- Add vendor to EXE vendor change queue table if they supply a warehouse that has the EXE system installed
    -- The first part is for Vendors associated with the warehouse''s items
    INSERT INTO WarehouseVendorChange (Store_No, Vendor_ID, ChangeType, Customer)
    SELECT DISTINCT Store.Store_No, Inserted.Vendor_ID, CASE WHEN Inserted.EXEWarehouseVendSent = 1 THEN ''M'' ELSE ''A'' END, 0
    FROM Inserted
    INNER JOIN
        Deleted ON Deleted.Vendor_ID = Inserted.Vendor_ID
    INNER JOIN
        ItemVendor ON ItemVendor.Vendor_ID = Inserted.Vendor_ID
    INNER JOIN
        Item ON Item.Item_Key = ItemVendor.Item_Key
    INNER JOIN
        ZoneSubTeam ON ZoneSubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        Store ON Store.Store_No = ZoneSubTeam.Supplier_Store_No
    WHERE (Inserted.CompanyName <> Deleted.CompanyName
        OR ISNULL(Inserted.Address_Line_1, '''') <> ISNULL(Deleted.Address_Line_1, '''')
        OR ISNULL(Inserted.Address_Line_2, '''') <> ISNULL(Deleted.Address_Line_2, '''')
        OR ISNULL(Inserted.City, '''') <> ISNULL(Deleted.City, '''')
        OR ISNULL(Inserted.State, '''') <> ISNULL(Deleted.State, '''')
        OR ISNULL(Inserted.Zip_Code, '''') <> ISNULL(Deleted.Zip_Code, '''')
        OR ISNULL(Inserted.Country, '''') <> ISNULL(Deleted.Country, '''')
        OR ISNULL(Inserted.Phone, '''') <> ISNULL(Deleted.Phone, ''''))
        AND Store.EXEWarehouse IS NOT NULL
        AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = Store.Store_No AND WVC.Vendor_ID = Inserted.Vendor_ID AND WVC.Customer = 0 AND WVC.ChangeType = ''A'' AND Inserted.EXEWarehouseVendSent = 0)
    UNION
    -- This part is for Vendors who are stores in the warehouse''s zone
    SELECT Supplier_Store_No, Inserted.Vendor_ID, CASE WHEN Inserted.EXEWarehouseCustSent = 1 THEN ''M'' ELSE ''A'' END, 1
    FROM Inserted
    INNER JOIN
        Deleted ON Deleted.Vendor_ID = Inserted.Vendor_ID
    INNER JOIN
        Store CustStore ON CustStore.Store_No = Inserted.Store_No
    INNER JOIN
        ZoneSubTeam ON ZoneSubTeam.Zone_ID = CustStore.Zone_ID
    INNER JOIN
        Store VendStore ON VendStore.Store_No = ZoneSubTeam.Supplier_Store_No
    WHERE (Inserted.CompanyName <> Deleted.CompanyName
        OR ISNULL(Inserted.Address_Line_1, '''') <> ISNULL(Deleted.Address_Line_1, '''')
        OR ISNULL(Inserted.Address_Line_2, '''') <> ISNULL(Deleted.Address_Line_2, '''')
        OR ISNULL(Inserted.City, '''') <> ISNULL(Deleted.City, '''')
        OR ISNULL(Inserted.State, '''') <> ISNULL(Deleted.State, '''')
        OR ISNULL(Inserted.Zip_Code, '''') <> ISNULL(Deleted.Zip_Code, '''')
        OR ISNULL(Inserted.Country, '''') <> ISNULL(Deleted.Country, '''')
        OR ISNULL(Inserted.Phone, '''') <> ISNULL(Deleted.Phone, '''')
        OR ISNULL(Inserted.Store_No, 0) <> ISNULL(Deleted.Store_No, 0))
        AND VendStore.EXEWarehouse IS NOT NULL
        AND NOT EXISTS (SELECT * FROM WarehouseVendorChange WVC WHERE WVC.Store_No = ZoneSubTeam.Supplier_Store_No AND WVC.Vendor_ID = Inserted.Vendor_ID AND WVC.Customer = 1 AND WVC.ChangeType = ''A'' AND Inserted.EXEWarehouseCustSent = 0)
    SELECT @Error_No = @@ERROR
    
	IF @Error_No = 0 
    BEGIN
		UPDATE VendorExportQueue
		SET QueueInsertedDate = GetDate(), 
			DeliveredToStoreOpsDate = NULL,
			Old_PS_Vendor_ID = (Select PS_Vendor_ID from Deleted)
		WHERE Vendor_ID in
			(
			SELECT DISTINCT Inserted.Vendor_ID
			FROM 
				Inserted 
			INNER JOIN
				Deleted 
				ON Inserted.Vendor_ID = Deleted.Vendor_ID
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 
			)
		IF @@ROWCOUNT = 0
		BEGIN
			INSERT INTO VendorExportQueue
			SELECT Inserted.Vendor_ID, GetDate(), NULL, Inserted.PS_Vendor_ID
			FROM Inserted
			INNER JOIN
				Deleted
				ON Deleted.Vendor_ID = Inserted.Vendor_ID
			WHERE dbo.fn_IsInternalVendor(Inserted.Vendor_ID) = 0 
				-- AND (Inserted.CompanyName <> Deleted.CompanyName
				--OR ISNULL(Inserted.PS_Vendor_ID, '''') <> ISNULL(Deleted.PS_Vendor_ID, '''')
				--OR ISNULL(Inserted.PS_Export_Vendor_ID, '''') <> ISNULL(Deleted.PS_Export_Vendor_ID, ''''))
		END
        SELECT @Error_No = @@ERROR
    END
	--IF @Error_No = 0
	--BEGIN
	--	DECLARE @mammothUpdates dbo.ItemKeyAndStoreNoType
	--	INSERT INTO @mammothUpdates(
	--		Item_Key, 
	--		Store_No)
	--	SELECT 
	--		siv.Item_Key, 
	--		siv.Store_no
	--	FROM inserted i
	--	JOIN deleted d on i.Vendor_ID = d.Vendor_ID
	--	JOIN StoreItemVendor siv ON i.Vendor_ID = siv.Vendor_ID
	--	WHERE (i.CompanyName <> d.CompanyName
	--		OR i.Vendor_Key <> d.Vendor_Key)
	--		AND siv.PrimaryVendor = 1
	--		AND siv.DeleteDate IS NULL
	--	IF EXISTS (SELECT TOP 1 1 FROM @mammothUpdates)
	--		EXEC mammoth.GenerateEventsByItemKeyAndStoreNoType @mammothUpdates, ''ItemLocaleAddOrUpdate''
	--	SELECT @Error_No = @@ERROR
	--END
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''VendorUpdate trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END
' 
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Vendor_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Unique vendor id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Vendor_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Vendor_Key'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Outside vendor key' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Vendor_Key'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'CompanyName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vendor company name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'CompanyName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Address_Line_1'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vendor address 1' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Address_Line_1'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Address_Line_2'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vendor address 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Address_Line_2'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'City'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'City' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'City'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'State'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'State' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'State'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Zip_Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Postal code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Zip_Code'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Country'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Country' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Country'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Phone'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phone number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Phone'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Fax'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fax number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Fax'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_CompanyName'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Company''s pay to name' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_CompanyName'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Attention'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay attention to' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Attention'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Address_Line_1'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to address 1 ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Address_Line_1'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Address_Line_2'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to address 2' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Address_Line_2'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_City'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to city' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_City'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_State'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to state' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_State'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Zip_Code'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to postal code' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Zip_Code'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Country'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to country' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Country'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Phone'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to phone number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Phone'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Fax'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to fax number' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Fax'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Comment'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Comment' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Comment'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'InternalCustomer'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Internal customer 1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'InternalCustomer'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'ActiveVendor'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Active vendor  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'ActiveVendor'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Store_no'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Store_no'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Order_By_Distribution'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order by distribution  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Order_By_Distribution'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'User_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User that entered the vendor record' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'User_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'Phone_Ext'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phone extension' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'Phone_Ext'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'PayTo_Phone_Ext'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pay to phone extension' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'PayTo_Phone_Ext'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'EDI_Notification_Email'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'If the vendor wants to be notified when we send an EDI 850, enter the email address here.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'EDI_Notification_Email'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'Vendor', N'COLUMN',N'CaseDistMarkupDollars'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The monetary amount of markup per case by a distribution center' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Vendor', @level2type=N'COLUMN',@level2name=N'CaseDistMarkupDollars'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: 17. Finish Up --- [dbo].[Vendor] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: **** Operation Complete ****: --- [dbo].[Vendor] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
