/******************************************************************************
		SO [dbo].[OrderHeader]
		Rollback
******************************************************************************/
PRINT N'Status: Begin [dbo].[OrderHeader] ROLLBACK --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[OrderHeader]  --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 5, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable FL Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable FL Change Tracking --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderHeader] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop FL Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop FL Defaults (Manually Generated) --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF_OrderHeader_OrderDate]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Syste__64ECEE3F]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHeade__Sent__67C95AEA]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Fax_O__68BD7F23]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Quant__6AA5C795]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Disco__6B99EBCE]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF_OrderHeader_Return_Order]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF_OrderHeader_WarehouseSend]
GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF_OrderHeader_FromOrderQueue]
GO
--ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Email__572AA745]  -- Not a valid constraint
--GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF_OrderHeader_OverrideTransmissionMethod]
GO
--ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Elect__4FCA47F6]  -- Not a valid constraint
--GO
--ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__IsDro__7D944002]  -- Not a valid constraint
--GO
ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF_OrderHeader_PayByAgreedCost]
GO
--ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__InRev__38FDB9AF]  -- Not a valid constraint
--GO
--ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__Parti__2208731A]  -- Not a valid constraint
--GO
--ALTER TABLE [OrderHeader] DROP CONSTRAINT [DF__OrderHead__DSDOr__3332FF1C]  -- Not a valid constraint
--GO
/******************************************************************************
		3. Drop FL Triggers
******************************************************************************/
PRINT N'Status: 3. Drop FL Triggers --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderUpdate]'))
DROP TRIGGER [dbo].[OrderHeaderUpdate]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderDel]'))
DROP TRIGGER [dbo].[OrderHeaderDel]
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderAdd]'))
DROP TRIGGER [dbo].[OrderHeaderAdd]
GO
/******************************************************************************
		4. Drop FL Foreign Keys
******************************************************************************/
PRINT N'Status: 4. Drop FL Foreign Keys --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_RefuseReceivingReasonID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderItem_RefuseReceivingReasonID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_Users1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_Users]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SubTeam1]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SubTeam]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_ReasonCodeDetail]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_OrderRefreshCostSource]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_OrderRefreshCostSource]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_MatchingValidationCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_MatchingValidationCode]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_MatchingUser_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_MatchingUser_ID]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ClosedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_ClosedBy]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_Currency_OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Vendo__07220AB2]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__User___75235608]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__User___75235608]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__RecvL__6A02E22E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__RecvL__6A02E22E]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Recei__04459E07]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Purch__025D5595]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Order__2D435324]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Order__2D435324]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Creat__63F8CA06]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Creat__63F8CA06]
GO
/******************************************************************************
		5. Drop FL Indexes
******************************************************************************/
PRINT N'Status: 5. Drop FL Indexes --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'OH_ExtSrcID_ExtSrcOrdID')
DROP INDEX [OH_ExtSrcID_ExtSrcOrdID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'IX_OH_OHID_ExternalOrderId')
DROP INDEX [IX_OH_OHID_ExternalOrderId] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxVendorOrder_ID')
DROP INDEX [idxVendorOrder_ID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxApprovedCloseDate')
DROP INDEX [idxApprovedCloseDate] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID')
DROP INDEX [_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID] ON [dbo].[OrderHeader]
GO
/******************************************************************************
		6. Rename FL PK
******************************************************************************/
PRINT N'Status: 6. Rename FL PK --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'PK_OrderHeader_OrderHeader_ID')
EXECUTE sp_rename N'[dbo].[PK_OrderHeader_OrderHeader_ID]', N'PK_OrderHeader_OrderHeader_ID_Rollback';
GO
/******************************************************************************
		7. Rename SO PK
******************************************************************************/
PRINT N'Status: 7. Rename SO PK --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader_Unaligned]') AND name = N'PK_OrderHeader_OrderHeader_ID_Unaligned')
EXECUTE sp_rename N'[dbo].[PK_OrderHeader_OrderHeader_ID_Unaligned]', N'PK_OrderHeader_OrderHeader_ID';
GO
/******************************************************************************
		8. Rename FL Table
******************************************************************************/
PRINT N'Status: 8. Rename FL Table --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[OrderHeader]', N'OrderHeader_Rollback';
GO
/******************************************************************************
		9. Rename SO Table
******************************************************************************/
PRINT N'Status: 9. Rename SO Table --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader_Unaligned]') AND type in (N'U'))
EXECUTE sp_rename N'[dbo].[OrderHeader_Unaligned]', N'OrderHeader';
GO
/******************************************************************************
		10. Create SO Defaults (manually generated)
******************************************************************************/
PRINT N'Status: 10. Create SO Defaults --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_OrderDate] DEFAULT (convert(varchar(12),getdate(),101)) FOR [OrderDate]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Syste__64ECEE3F] DEFAULT (0) FOR [SystemGenerated]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHeade__Sent__67C95AEA] DEFAULT (0) FOR [Sent]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Fax_O__68BD7F23] DEFAULT (1) FOR [Fax_Order]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Quant__6AA5C795] DEFAULT (0) FOR [QuantityDiscount]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Disco__6B99EBCE] DEFAULT (0) FOR [DiscountType]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_Return_Order] DEFAULT (0) FOR [Return_Order]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_WarehouseSend] DEFAULT (0) FOR [WarehouseSent]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_FromOrderQueue] DEFAULT (0) FOR [FromQueue]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_AccrualException] DEFAULT ((0)) FOR [AccrualException]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_IsDropShipment] DEFAULT ((0)) FOR [IsDropShipment]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Elect__2223AAA1] DEFAULT ((0)) FOR [Electronic_Order]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__PayBy__2317CEDA] DEFAULT ((0)) FOR [PayByAgreedCost]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Email__2500174C] DEFAULT ((0)) FOR [Email_Order]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF_OrderHeader_OverrideTransmissionMethod] DEFAULT ((0)) FOR [OverrideTransmissionMethod]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__InRev__6289459A] DEFAULT ((0)) FOR [InReview]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__Parti__09F12431] DEFAULT ((0)) FOR [PartialShipment]
GO
ALTER TABLE [OrderHeader]WITH NOCHECK ADD CONSTRAINT [DF__OrderHead__DSDOr__1B1BB033] DEFAULT ((0)) FOR [DSDOrder]
GO
/******************************************************************************
		11. Enable SO Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable SO Change Tracking --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderHeader] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Grant SO Perms
******************************************************************************/
PRINT N'Status: 12. Grant SO Perms --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [BizTalk] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [IConInterface] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [iCONReportingRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [IRMA_Teradata] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [IRMAAdminRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [IRMAClientRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [IRMAPDXExtractRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[OrderHeader] TO [IRMAReportsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [IRMAReportsRole] AS [dbo]
GO
GRANT UPDATE ON [dbo].[OrderHeader] TO [IRMAReportsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [IRMAReportsRole] AS [dbo]
GO
GRANT INSERT ON [dbo].[OrderHeader] TO [IRMASchedJobs] AS [dbo]
GO
GRANT UPDATE ON [dbo].[OrderHeader] TO [IRMASchedJobs] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [IRMASchedJobsRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [IRMASupportRole] AS [dbo]
GO
GRANT VIEW CHANGE TRACKING ON [dbo].[OrderHeader] TO [IRMASupportRole] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [IRSUser] AS [dbo]
GO
GRANT SELECT ON [dbo].[OrderHeader] TO [SOAppsUserAdmin] AS [dbo]
GO
/******************************************************************************
		13. Create SO Indexes
******************************************************************************/
PRINT N'Status: 13. Create SO Indexes --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderDate')
CREATE CLUSTERED INDEX [idxOrderDate] ON [dbo].[OrderHeader]
(
	[OrderDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID] ON [dbo].[OrderHeader]
(
	[Vendor_ID] ASC,
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[InvoiceNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxApprovedCloseDate')
CREATE NONCLUSTERED INDEX [idxApprovedCloseDate] ON [dbo].[OrderHeader]
(
	[CloseDate] ASC,
	[ApprovedDate] ASC
)
INCLUDE ( 	[OrderHeader_ID],
	[InvoiceNumber],
	[Vendor_ID],
	[PurchaseLocation_ID],
	[OrderDate],
	[Transfer_To_SubTeam],
	[InvoiceDate],
	[eInvoice_Id],
	[ResolutionCodeID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAdjustedCost')
CREATE NONCLUSTERED INDEX [idxOrderHeaderAdjustedCost] ON [dbo].[OrderHeader]
(
	[Transfer_To_SubTeam] ASC,
	[Vendor_ID] ASC,
	[CloseDate] ASC,
	[ReceiveLocation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAdjustedCost2')
CREATE NONCLUSTERED INDEX [idxOrderHeaderAdjustedCost2] ON [dbo].[OrderHeader]
(
	[Transfer_To_SubTeam] ASC,
	[CloseDate] ASC,
	[ReceiveLocation_ID] ASC,
	[Vendor_ID] ASC,
	[OrderHeader_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc1')
CREATE NONCLUSTERED INDEX [idxOrderHeaderAlloc1] ON [dbo].[OrderHeader]
(
	[CloseDate] ASC,
	[Vendor_ID] ASC,
	[OrderHeader_ID] ASC,
	[Transfer_SubTeam] ASC,
	[WarehouseSent] ASC,
	[OrderType_ID] ASC,
	[ReceiveLocation_ID] ASC
)
INCLUDE ( 	[Expected_Date],
	[SentDate],
	[Transfer_To_SubTeam],
	[AllocatingDate],
	[AllocatingUserID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc3')
CREATE NONCLUSTERED INDEX [idxOrderHeaderAlloc3] ON [dbo].[OrderHeader]
(
	[AllocatingUserID] ASC,
	[User_ID] ASC
)
INCLUDE ( 	[OrderHeader_ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
ALTER INDEX [idxOrderHeaderAlloc3] ON [dbo].[OrderHeader] DISABLE
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc4')
CREATE NONCLUSTERED INDEX [idxOrderHeaderAlloc4] ON [dbo].[OrderHeader]
(
	[CloseDate] ASC,
	[Transfer_To_SubTeam] ASC,
	[ReceiveLocation_ID] ASC,
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[Expected_Date],
	[SentDate]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc5')
CREATE NONCLUSTERED INDEX [idxOrderHeaderAlloc5] ON [dbo].[OrderHeader]
(
	[OrderHeader_ID] ASC,
	[Transfer_To_SubTeam] ASC,
	[ReceiveLocation_ID] ASC
)
INCLUDE ( 	[Transfer_SubTeam]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderCreditReasonReport1')
CREATE NONCLUSTERED INDEX [idxOrderHeaderCreditReasonReport1] ON [dbo].[OrderHeader]
(
	[ReceiveLocation_ID] ASC,
	[CloseDate] ASC,
	[OrderHeader_ID] ASC,
	[Vendor_ID] ASC
)
INCLUDE ( 	[Transfer_To_SubTeam]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderID')
CREATE UNIQUE NONCLUSTERED INDEX [idxOrderHeaderID] ON [dbo].[OrderHeader]
(
	[OrderHeader_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderItemInvoiceDiscrepanciesReport1')
CREATE NONCLUSTERED INDEX [idxOrderHeaderItemInvoiceDiscrepanciesReport1] ON [dbo].[OrderHeader]
(
	[OrderHeader_ID] ASC,
	[ReceiveLocation_ID] ASC,
	[Vendor_ID] ASC,
	[InvoiceNumber] ASC,
	[InvoiceDate] ASC,
	[CloseDate] ASC
)
INCLUDE ( 	[QuantityDiscount],
	[DiscountType]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderItemInvoiceDiscrepanciesReport2')
CREATE NONCLUSTERED INDEX [idxOrderHeaderItemInvoiceDiscrepanciesReport2] ON [dbo].[OrderHeader]
(
	[OrderHeader_ID] ASC,
	[ReceiveLocation_ID] ASC,
	[Vendor_ID] ASC,
	[InvoiceNumber] ASC,
	[InvoiceDate] ASC,
	[CloseDate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
ALTER INDEX [idxOrderHeaderItemInvoiceDiscrepanciesReport2] ON [dbo].[OrderHeader] DISABLE
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderOrderTypeID')
CREATE NONCLUSTERED INDEX [idxOrderHeaderOrderTypeID] ON [dbo].[OrderHeader]
(
	[OrderType_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderScanGunOrderInfo')
CREATE NONCLUSTERED INDEX [idxOrderHeaderScanGunOrderInfo] ON [dbo].[OrderHeader]
(
	[CloseDate] ASC,
	[ReceiveLocation_ID] ASC,
	[Return_Order] ASC,
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[Transfer_To_SubTeam]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderScanGunOrderInfo2')
CREATE NONCLUSTERED INDEX [idxOrderHeaderScanGunOrderInfo2] ON [dbo].[OrderHeader]
(
	[ReceiveLocation_ID] ASC,
	[CloseDate] ASC,
	[Return_Order] ASC,
	[Transfer_To_SubTeam] ASC,
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[SentDate]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderUpdateOrderApplyVendorCost')
CREATE NONCLUSTERED INDEX [idxOrderHeaderUpdateOrderApplyVendorCost] ON [dbo].[OrderHeader]
(
	[UploadedDate] ASC,
	[ReceiveLocation_ID] ASC,
	[Vendor_ID] ASC,
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[SentDate],
	[OrderDate]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxVendorOrder_ID')
CREATE NONCLUSTERED INDEX [idxVendorOrder_ID] ON [dbo].[OrderHeader]
(
	[InvoiceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'IX_OH_OHID_ExternalOrderId')
CREATE NONCLUSTERED INDEX [IX_OH_OHID_ExternalOrderId] ON [dbo].[OrderHeader]
(
	[OrderHeader_ID] ASC,
	[OrderExternalSourceOrderID] ASC
)
INCLUDE ( 	[SentDate],
	[ReceiveLocation_ID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'IX_OrderHeaderReceiveLocation_ID')
CREATE NONCLUSTERED INDEX [IX_OrderHeaderReceiveLocation_ID] ON [dbo].[OrderHeader]
(
	[ReceiveLocation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'OH_ExtSrcID_ExtSrcOrdID')
CREATE NONCLUSTERED INDEX [OH_ExtSrcID_ExtSrcOrdID] ON [dbo].[OrderHeader]
(
	[OrderExternalSourceID] ASC,
	[OrderExternalSourceOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/******************************************************************************
		14. Create SO Foreign Keys
******************************************************************************/
PRINT N'Status: 14. Create SO Foreign Keys --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Creat__63F8CA06]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Creat__63F8CA06] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Creat__63F8CA06]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Creat__63F8CA06]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Order__73EBA58E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD FOREIGN KEY([OrderExternalSourceID])
REFERENCES [dbo].[OrderExternalSource] ([ID])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Purch__025D5595] FOREIGN KEY([PurchaseLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Purch__025D5595]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Recei__04459E07] FOREIGN KEY([ReceiveLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Recei__04459E07]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__RecvL__6A02E22E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__RecvL__6A02E22E] FOREIGN KEY([RecvLogUser_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__RecvL__6A02E22E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__RecvL__6A02E22E]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__User___75235608]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__User___75235608] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__User___75235608]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__User___75235608]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Vendo__07220AB2] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Vendo__07220AB2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_Currency_OrderHeader] FOREIGN KEY([CurrencyID])
REFERENCES [dbo].[Currency] ([CurrencyID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_Currency_OrderHeader]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ClosedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderHeader_ClosedBy] FOREIGN KEY([ClosedBy])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ClosedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_ClosedBy]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_MatchingUser_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_MatchingUser_ID] FOREIGN KEY([MatchingUser_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_MatchingUser_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_MatchingUser_ID]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_MatchingValidationCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_MatchingValidationCode] FOREIGN KEY([MatchingValidationCode])
REFERENCES [dbo].[ValidationCode] ([ValidationCode])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_MatchingValidationCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_MatchingValidationCode]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_OrderRefreshCostSource]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderHeader_OrderRefreshCostSource] FOREIGN KEY([OrderRefreshCostSource_ID])
REFERENCES [dbo].[OrderRefreshCostSource] ([OrderRefreshCostSource_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_OrderRefreshCostSource]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_OrderRefreshCostSource]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_ReasonCodeDetail] FOREIGN KEY([ReasonCodeDetailID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ReasonCodeDetail]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_ReasonCodeDetail]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam] FOREIGN KEY([Transfer_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_SubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam1] FOREIGN KEY([Transfer_To_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_SubTeam1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam] FOREIGN KEY([SupplyTransferToSubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderHeader_Users] FOREIGN KEY([Accounting_In_UserID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderHeader_Users1] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_Users1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_RefuseReceivingReasonID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_RefuseReceivingReasonID] FOREIGN KEY([RefuseReceivingReasonID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_RefuseReceivingReasonID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderItem_RefuseReceivingReasonID]
GO
/******************************************************************************
		15. Create SO Triggers
******************************************************************************/
PRINT N'Status: 15. Create SO Triggers --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderAdd]'))
EXEC dbo.sp_executesql @statement = N'CREATE TRIGGER [dbo].[OrderHeaderAdd]
ON [dbo].[OrderHeader]
AFTER INSERT
AS
/*
	This trigger will populate Orderheader.PayByAgreedCost with the correct value for every order that is created.
	Added for bug 10586
*/
BEGIN
	UPDATE 
		OrderHeader	
	SET 
		PayByAgreedCost = dbo.fn_IsPayByAgreedCostStoreVendor(vs.Store_No, oh.Vendor_ID, GETDATE())
	FROM
		OrderHeader		oh
		JOIN Vendor		vs	ON	oh.PurchaseLocation_ID	= vs.Vendor_ID
		JOIN Inserted	i	ON	oh.OrderHeader_ID		= i.OrderHeader_ID
END' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderDel]'))
EXEC dbo.sp_executesql @statement = N'CREATE Trigger [dbo].[OrderHeaderDel] 
ON [dbo].[OrderHeader]
FOR DELETE
AS
BEGIN
    DECLARE @Error_No int
    SELECT @Error_No = 0
	UPDATE OrderExportDeletedQueue
	SET QueueInsertedDate = GetDate(), DeliveredToStoreOpsDate = NULL
	WHERE OrderHeader_ID in (SELECT OrderHeader_ID FROM Deleted)
	IF @@ROWCOUNT=0
	BEGIN
	    INSERT INTO OrderExportDeletedQueue
		SELECT OrderHeader_ID, GetDate(), NULL FROM Deleted
	END
    SELECT @Error_No = @@ERROR
    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR (''OrderHeaderDel trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
    END
END' 
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderUpdate]'))
EXEC dbo.sp_executesql @statement = N'
CREATE Trigger [dbo].[OrderHeaderUpdate] 
ON [dbo].[OrderHeader]
FOR UPDATE
AS
-- **************************************************************************
-- Trigger: OrderHeaderUpdate
--    Author: n/a
--      Date: n/a
--
-- Description:
-- Trigger for whenever OrderHeader is updated
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 11/26/2012	BAS		8133	Added OrderRefreshCostSource_ID for tracking
--								where UpdateOrderRefreshCost is called on
--								an order
-- 08/17/2017   MZ      20620   Register an order for a WFM ordering banner store to
--                              the [infor].[OrderExpectedDateChangeQueue] table when
--                              its expected date changes before the order is closed.
-- **************************************************************************
BEGIN
	DECLARE @Error_No int
	SELECT @Error_No = 0
	-- Log changes
	INSERT INTO	OrderHeaderHistory (OrderHeader_ID, InvoiceNumber, OrderHeaderDesc, Vendor_ID, PurchaseLocation_ID, ReceiveLocation_ID, CreatedBy, OrderDate, CloseDate, OriginalCloseDate, SystemGenerated, Sent, Fax_Order, Expected_Date, SentDate, QuantityDiscount, DiscountType, Transfer_SubTeam, Transfer_To_SubTeam, Return_Order, User_ID, Temperature, Accounting_In_DateStamp, Accounting_In_UserID, InvoiceDate, ApprovedDate, ApprovedBy, UploadedDate, RecvLogDate, RecvLog_No, RecvLogUser_ID, VendorDoc_ID, VendorDocDate, WarehouseSent, WarehouseSentDate, OrderType_ID, ProductType_ID, FromQueue, SentToFaxDate, [Host_Name], ClosedBy, MatchingValidationCode, MatchingUser_ID, MatchingDate, Freight3Party_OrderCost, PayByAgreedCost, OrderRefreshCostSource_ID)
	SELECT		Inserted.OrderHeader_ID, Inserted.InvoiceNumber, Inserted.OrderHeaderDesc, Inserted.Vendor_ID, Inserted.PurchaseLocation_ID, Inserted.ReceiveLocation_ID, Inserted.CreatedBy, Inserted.OrderDate, Inserted.CloseDate, Inserted.OriginalCloseDate, Inserted.SystemGenerated, Inserted.Sent, Inserted.Fax_Order, Inserted.Expected_Date, Inserted.SentDate, Inserted.QuantityDiscount, Inserted.DiscountType, Inserted.Transfer_SubTeam, Inserted.Transfer_To_SubTeam, Inserted.Return_Order, Inserted.User_ID, Inserted.Temperature, Inserted.Accounting_In_DateStamp, Inserted.Accounting_In_UserID, Inserted.InvoiceDate, Inserted.ApprovedDate, Inserted.ApprovedBy, Inserted.UploadedDate, Inserted.RecvLogDate, Inserted.RecvLog_No, Inserted.RecvLogUser_ID, Inserted.VendorDoc_ID, Inserted.VendorDocDate, Inserted.WarehouseSent, Inserted.WarehouseSentDate, Inserted.OrderType_ID, Inserted.ProductType_ID, Inserted.FromQueue, Inserted.SentToFaxDate, HOST_NAME(), Inserted.ClosedBy, Inserted.MatchingValidationCode, Inserted.MatchingUser_ID, Inserted.MatchingDate, Inserted.Freight3Party_OrderCost, Inserted.PayByAgreedCost, Inserted.OrderRefreshCostSource_ID
	FROM			INSERTED		INNER JOIN	DELETED	ON	Deleted.OrderHeader_ID = Inserted.OrderHeader_ID
	WHERE		(Inserted.Sent <> Deleted.SENT 
				OR Inserted.Fax_Order <> Deleted.Fax_Order
				OR ISNULL(Inserted.SentDate, 0) <> ISNULL(Deleted.SentDate, 0)
				OR ISNULL(Inserted.ClosedBy, -1) <> ISNULL(Deleted.ClosedBy, -1)
				OR ISNULL(Inserted.ApprovedDate, 0) <> ISNULL(Deleted.ApprovedDate, 0)
				OR ISNULL(Inserted.ApprovedBy, -1) <> ISNULL(Deleted.ApprovedBy, -1)
				OR ISNULL(Inserted.UploadedDate, 0) <> ISNULL(Deleted.UploadedDate, 0)
				OR ISNULL(Inserted.MatchingValidationCode, -1) <> ISNULL(Deleted.MatchingValidationCode, -1)
				OR ISNULL(Inserted.MatchingDate, 0) <> ISNULL(Deleted.MatchingDate, 0)
				OR ISNULL(Inserted.MatchingUser_ID, -1) <> ISNULL(Deleted.MatchingUser_ID, -1)
				OR ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0)
				OR Inserted.PayByAgreedCost <> Deleted.PayByAgreedCost
				OR Inserted.OrderRefreshCostSource_ID <> Deleted.OrderRefreshCostSource_ID
				)
	UNION	
	SELECT		Deleted.OrderHeader_ID, Deleted.InvoiceNumber, Deleted.OrderHeaderDesc, Deleted.Vendor_ID, Deleted.PurchaseLocation_ID, Deleted.ReceiveLocation_ID, Deleted.CreatedBy, Deleted.OrderDate, Deleted.CloseDate, Deleted.OriginalCloseDate, Deleted.SystemGenerated, Deleted.Sent, Deleted.Fax_Order, Deleted.Expected_Date, Deleted.SentDate, Deleted.QuantityDiscount, Deleted.DiscountType, Deleted.Transfer_SubTeam, Deleted.Transfer_To_SubTeam, Deleted.Return_Order, Deleted.User_ID, Deleted.Temperature, Deleted.Accounting_In_DateStamp, Deleted.Accounting_In_UserID, Deleted.InvoiceDate, Deleted.ApprovedDate, Deleted.ApprovedBy, Deleted.UploadedDate, Deleted.RecvLogDate, Deleted.RecvLog_No, Deleted.RecvLogUser_ID, Deleted.VendorDoc_ID, Deleted.VendorDocDate, Deleted.WarehouseSent, Deleted.WarehouseSentDate, Deleted.OrderType_ID, Deleted.ProductType_ID, Deleted.FromQueue, Deleted.SentToFaxDate, HOST_NAME(), Deleted.ClosedBy, Deleted.MatchingValidationCode, Deleted.MatchingUser_ID, Deleted.MatchingDate, Deleted.Freight3Party_OrderCost, Deleted.PayByAgreedCost, Deleted.OrderRefreshCostSource_ID
	FROM			DELETED	INNER JOIN	INSERTED					ON Inserted.OrderHeader_ID = Deleted.OrderHeader_ID
						LEFT JOIN	OrderHeaderHistory OHH		ON Deleted.OrderHeader_ID = OHH.OrderHeader_ID
	WHERE		OHH.OrderHeader_ID IS NULL
				AND (Inserted.Sent <> Deleted.Sent
				OR Inserted.Fax_Order <> Deleted.Fax_Order
				OR ISNULL(Inserted.SentDate, 0) <> ISNULL(Deleted.SentDate, 0)
				OR ISNULL(Inserted.CloseDate, 0) <> ISNULL(Deleted.CloseDate, 0)
				OR ISNULL(Inserted.ClosedBy, -1) <> ISNULL(Deleted.ClosedBy, -1) 
				OR ISNULL(Inserted.ApprovedDate, 0) <> ISNULL(Deleted.ApprovedDate, 0)
				OR ISNULL(Inserted.ApprovedBy, -1) <> ISNULL(Deleted.ApprovedBy, -1)
				OR ISNULL(Inserted.UploadedDate, 0) <> ISNULL(Deleted.UploadedDate, 0)
				OR ISNULL(Inserted.MatchingValidationCode, -1) <> ISNULL(Deleted.MatchingValidationCode, -1)
				OR ISNULL(Inserted.MatchingDate, 0) <> ISNULL(Deleted.MatchingDate, 0)
				OR ISNULL(Inserted.MatchingUser_ID, -1) <> ISNULL(Deleted.MatchingUser_ID, -1)
				OR ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0)
				OR Inserted.PayByAgreedCost <> Deleted.PayByAgreedCost
				OR Inserted.OrderRefreshCostSource_ID <> Deleted.OrderRefreshCostSource_ID
				)
	SELECT @Error_No = @@ERROR
	--
	-- StoreOps Export 
	--
	IF @Error_No = 0
	BEGIN
  
			DECLARE @OrdersToExport TABLE (OrderHeader_Id int NOT NULL PRIMARY KEY) 
			DECLARE @InsertDate datetime
			
			INSERT INTO @OrdersToExport
			SELECT DISTINCT Inserted.OrderHeader_ID
			FROM		Inserted	INNER JOIN	Deleted		ON Inserted.OrderHeader_ID = Deleted.OrderHeader_ID
			WHERE 
					(Inserted.SentDate IS NOT NULL)
					AND
					((Deleted.SentDate IS NULL) 
					OR (ISNULL(Inserted.CloseDate, 0) <> ISNULL(Deleted.CloseDate, 0))
					OR (Inserted.Vendor_ID <> Deleted.Vendor_ID)
					OR (Inserted.ReceiveLocation_ID <> Deleted.ReceiveLocation_ID)
					OR (ISNULL(Inserted.Transfer_To_SubTeam, 0) <> ISNULL(Deleted.Transfer_To_SubTeam, 0)) 
					OR (ISNULL(Inserted.Transfer_SubTeam, 0) <> ISNULL(Deleted.Transfer_SubTeam, 0))
					OR (ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0))
					OR (ISNULL(Inserted.RecvLogDate, 0) <> ISNULL(Deleted.RecvLogDate, 0))
					OR (ISNULL(Inserted.UploadedDate, 0) <> ISNULL(Deleted.UploadedDate, 0))
					OR (ISNULL(Inserted.ApprovedDate, 0) <> ISNULL(Deleted.ApprovedDate, 0))
					OR (ISNULL(Inserted.OrderHeaderDesc, '''') <> ISNULL(Deleted.OrderHeaderDesc, ''''))
					OR (Inserted.Return_Order <> Deleted.Return_Order)
					OR (ISNULL(Inserted.InvoiceNumber, '''') <> ISNULL(Deleted.InvoiceNumber, ''''))
					OR (ISNULL(Inserted.InvoiceDate, 0) <> ISNULL(Deleted.InvoiceDate, 0))
					)
				
			DELETE oeq
			FROM		OrderExportQueue oeq	INNER JOIN	@OrdersToExport ote		ON oeq.orderHeader_id = ote.OrderHeader_Id
			
			SET @InsertDate = GETDATE()
			INSERT INTO	OrderExportQueue
			SELECT		OrderHeader_ID, @InsertDate, NULL 
			FROM			@OrdersToExport
	
			SELECT @Error_No = @@ERROR
	END
	--
	-- Infor Export 
	--
	IF @Error_No = 0
	BEGIN
		INSERT INTO	infor.OrderExpectedDateChangeQueue (OrderHeader_ID, InsertDate)
		SELECT		Inserted.OrderHeader_ID, GETDATE()
		FROM		INSERTED		
		INNER JOIN	DELETED	ON	Deleted.OrderHeader_ID = Inserted.OrderHeader_ID
		INNER JOIN  Vendor psl on Inserted.PurchaseLocation_ID = psl.Vendor_ID
		INNER JOIN  Store s on s.Store_No = psl.Store_no 
		WHERE       ISNULL(Inserted.Expected_Date, 0) <> ISNULL(Deleted.Expected_Date, 0)
		AND         Inserted.Sent = 1
		AND         Inserted.OrderType_ID <> 3
		AND         Inserted.OriginalCloseDate is null
		AND         (s.mega_store = 1 
		 OR			 s.BusinessUnit_ID in (SELECT Key_Value FROM [dbo].[fn_Parse_List]([dbo].[fn_GetAppConfigValue](''WFMBannerStoresForOrdering'', ''IRMA CLIENT''), ''|'')))	
		SELECT @Error_No = @@ERROR
	END
	IF @Error_No <> 0
	BEGIN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
		RAISERROR (''OrderHeaderUpdate trigger failed with @@ERROR: %d'', @Severity, 1, @Error_No)
	END
END
' 
GO
/******************************************************************************
		16. Create SO Extended Properties
******************************************************************************/
PRINT N'Status: 16. Create SO Extended Properties --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderHeader_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order header id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderHeader_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'InvoiceNumber'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vendor Order id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'InvoiceNumber'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderHeaderDesc'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Order description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderHeaderDesc'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Vendor_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vender id refer to the vendor table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Vendor_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'PurchaseLocation_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Purchase location id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'PurchaseLocation_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'ReceiveLocation_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Receive location id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'ReceiveLocation_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'CreatedBy'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Who created the order refer to the user table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date the order was created' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'CloseDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date the order was closed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'CloseDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OriginalCloseDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The original close date' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OriginalCloseDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'SystemGenerated'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Was the order system generated  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'SystemGenerated'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Sent'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Was the order sent  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Sent'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Fax_Order'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Is this a fax order  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Fax_Order'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Expected_Date'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Expected date ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Expected_Date'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'SentDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Send date of the order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'SentDate'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'QuantityDiscount'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contains how much has to be bought for a discount' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'QuantityDiscount'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'DiscountType'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0=No Discount;1=Cash Discount;2=Percent Discount;4=Landed Percent' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'DiscountType'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Return_Order'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Return order  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Return_Order'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'User_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'User refer to user table' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'User_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Temperature'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Temperature requirements  1 = Yes or 0 = No' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Temperature'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Accounting_In_DateStamp'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Date accouting received order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Accounting_In_DateStamp'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Accounting_In_UserID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Person in accounting who received the order' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Accounting_In_UserID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderType_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1 = Purchase, 2 = Distribution, 3 = Transfer' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderType_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'ProductType_ID'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1=Product, 2 = Packaging, 3 = Supplies' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'ProductType_ID'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'CostEffectiveDate'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Allows the user to set the cost effective date for an order.  This overrides the usage of "current date" when referencing the item cost records.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'CostEffectiveDate'
GO
/******************************************************************************
		17. Finish Up
******************************************************************************/
PRINT N'Status: **** Operation Complete ****: --- [dbo].[OrderHeader] Rollback --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
