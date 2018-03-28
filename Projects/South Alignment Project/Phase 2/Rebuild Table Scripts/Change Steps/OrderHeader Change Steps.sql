/******************************************************************************
		SO [dbo].[OrderHeader]
		Change Steps
******************************************************************************/
PRINT N'Status: BEGIN FL [dbo].[OrderHeader] ChangeSteps (takes about 7 minutes in TEST):'+ ' ---  [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for Script to Complete: --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 7, SYSDATETIME()), 9)
GO
USE [ItemCatalog]
--USE [ItemCatalog_Test]
GO
SET NOCOUNT ON;
GO
/******************************************************************************
		1. Disable SO Change Tracking
******************************************************************************/
PRINT N'Status: 1. Disable SO Change Tracking --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderHeader] DISABLE CHANGE_TRACKING
GO
/******************************************************************************
		2. Drop SO Defaults (Manually Generated)
******************************************************************************/
PRINT N'Status: 2. Drop SO Defaults (Manually Generated) --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_Return_Order];
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Parti__09F12431];
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__DSDOr__1B1BB033];
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Elect__2223AAA1];
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__PayBy__2317CEDA];
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Email__2500174C];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_AccrualException];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__InRev__6289459A];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_WarehouseSend];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_OverrideTransmissionMethod];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_OrderDate];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Syste__64ECEE3F];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHeade__Sent__67C95AEA];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Fax_O__68BD7F23];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Quant__6AA5C795];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_FromOrderQueue];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF_OrderHeader_IsDropShipment];  -- Not a valid constraint, not a part of the create table script
GO
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [DF__OrderHead__Disco__6B99EBCE];  -- Not a valid constraint, not a part of the create table script
GO
/******************************************************************************
		3. Drop SO Extended Properties
******************************************************************************/
PRINT N'Status: 3. Drop SO Extended Properties --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'CostEffectiveDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'CostEffectiveDate'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'ProductType_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'ProductType_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderType_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderType_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Accounting_In_UserID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Accounting_In_UserID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Accounting_In_DateStamp'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Accounting_In_DateStamp'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Temperature'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Temperature'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'User_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'User_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Return_Order'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Return_Order'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'DiscountType'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'DiscountType'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'QuantityDiscount'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'QuantityDiscount'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'SentDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'SentDate'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Expected_Date'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Expected_Date'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Fax_Order'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Fax_Order'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Sent'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Sent'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'SystemGenerated'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'SystemGenerated'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OriginalCloseDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OriginalCloseDate'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'CloseDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'CloseDate'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderDate'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderDate'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'CreatedBy'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'CreatedBy'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'ReceiveLocation_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'ReceiveLocation_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'PurchaseLocation_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'PurchaseLocation_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'Vendor_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'Vendor_ID'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderHeaderDesc'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderHeaderDesc'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'InvoiceNumber'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'InvoiceNumber'
GO
IF  EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'OrderHeader', N'COLUMN',N'OrderHeader_ID'))
EXEC sys.sp_dropextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'OrderHeader', @level2type=N'COLUMN',@level2name=N'OrderHeader_ID'
GO
/******************************************************************************
		4. Drop SO Triggers
******************************************************************************/
PRINT N'Status: 4. Drop SO Triggers --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		5. Drop SO Foreign Keys
******************************************************************************/
PRINT N'Status: 5. Drop SO Foreign Keys --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Order__73EBA58E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Order__73EBA58E]
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Creat__63F8CA06]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Creat__63F8CA06]
GO
/******************************************************************************
		6. Drop SO Indexes
******************************************************************************/
PRINT N'Status: 6. Drop SO Indexes --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'OH_ExtSrcID_ExtSrcOrdID')
DROP INDEX [OH_ExtSrcID_ExtSrcOrdID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'IX_OrderHeaderReceiveLocation_ID')
DROP INDEX [IX_OrderHeaderReceiveLocation_ID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'IX_OH_OHID_ExternalOrderId')
DROP INDEX [IX_OH_OHID_ExternalOrderId] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxVendorOrder_ID')
DROP INDEX [idxVendorOrder_ID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderUpdateOrderApplyVendorCost')
DROP INDEX [idxOrderHeaderUpdateOrderApplyVendorCost] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderScanGunOrderInfo2')
DROP INDEX [idxOrderHeaderScanGunOrderInfo2] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderScanGunOrderInfo')
DROP INDEX [idxOrderHeaderScanGunOrderInfo] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderOrderTypeID')
DROP INDEX [idxOrderHeaderOrderTypeID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderItemInvoiceDiscrepanciesReport2')
DROP INDEX [idxOrderHeaderItemInvoiceDiscrepanciesReport2] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderItemInvoiceDiscrepanciesReport1')
DROP INDEX [idxOrderHeaderItemInvoiceDiscrepanciesReport1] ON [dbo].[OrderHeader]
GO
--IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderID')
--DROP INDEX [idxOrderHeaderID] ON [dbo].[OrderHeader]  -- causes error
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderCreditReasonReport1')
DROP INDEX [idxOrderHeaderCreditReasonReport1] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc5')
DROP INDEX [idxOrderHeaderAlloc5] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc4')
DROP INDEX [idxOrderHeaderAlloc4] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc3')
DROP INDEX [idxOrderHeaderAlloc3] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAlloc1')
DROP INDEX [idxOrderHeaderAlloc1] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAdjustedCost2')
DROP INDEX [idxOrderHeaderAdjustedCost2] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderHeaderAdjustedCost')
DROP INDEX [idxOrderHeaderAdjustedCost] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxApprovedCloseDate')
DROP INDEX [idxApprovedCloseDate] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID')
DROP INDEX [_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID] ON [dbo].[OrderHeader]
GO
IF  EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxOrderDate')
DROP INDEX [idxOrderDate] ON [dbo].[OrderHeader] WITH ( ONLINE = OFF )
GO
/******************************************************************************
		7. Rename SO PK 
******************************************************************************/
PRINT N'Status: 7. Rename SO PK  --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[PK_OrderHeader_OrderHeader_ID]', N'PK_OrderHeader_OrderHeader_ID_Unaligned';
GO
/******************************************************************************
		8. Rename SO Table
******************************************************************************/
PRINT N'Status: 8. Rename SO Table --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
EXECUTE sp_rename N'[dbo].[OrderHeader]', N'OrderHeader_Unaligned';
GO
/******************************************************************************
		9. Create FL Table
******************************************************************************/
PRINT N'Status: 9. Create FL Table --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderHeader](
	[OrderHeader_ID] [int] IDENTITY(1,1) NOT NULL,
	[InvoiceNumber] [varchar](20) NULL,
	[OrderHeaderDesc] [varchar](4000) NULL,
	[Vendor_ID] [int] NOT NULL,
	[PurchaseLocation_ID] [int] NOT NULL,
	[ReceiveLocation_ID] [int] NOT NULL,
	[CreatedBy] [int] NOT NULL,
	[OrderDate] [smalldatetime] NOT NULL CONSTRAINT [DF_OrderHeader_OrderDate]  DEFAULT (CONVERT([varchar](12),getdate(),(101))),
	[CloseDate] [datetime] NULL,
	[OriginalCloseDate] [datetime] NULL,
	[SystemGenerated] [bit] NOT NULL CONSTRAINT [DF__OrderHead__Syste__64ECEE3F]  DEFAULT ((0)),
	[Sent] [bit] NOT NULL CONSTRAINT [DF__OrderHeade__Sent__67C95AEA]  DEFAULT ((0)),
	[Fax_Order] [bit] NOT NULL CONSTRAINT [DF__OrderHead__Fax_O__68BD7F23]  DEFAULT ((1)),
	[Expected_Date] [smalldatetime] NULL,
	[SentDate] [smalldatetime] NULL,
	[QuantityDiscount] [decimal](9, 2) NOT NULL CONSTRAINT [DF__OrderHead__Quant__6AA5C795]  DEFAULT ((0)),
	[DiscountType] [int] NOT NULL CONSTRAINT [DF__OrderHead__Disco__6B99EBCE]  DEFAULT ((0)),
	[Transfer_SubTeam] [int] NULL,
	[Transfer_To_SubTeam] [int] NOT NULL,
	[Return_Order] [bit] NOT NULL CONSTRAINT [DF_OrderHeader_Return_Order]  DEFAULT ((0)),
	[User_ID] [int] NULL,
	[Temperature] [tinyint] NULL,
	[Accounting_In_DateStamp] [smalldatetime] NULL,
	[Accounting_In_UserID] [int] NULL,
	[InvoiceDate] [smalldatetime] NULL,
	[ApprovedDate] [smalldatetime] NULL,
	[ApprovedBy] [int] NULL,
	[UploadedDate] [smalldatetime] NULL,
	[RecvLogDate] [datetime] NULL,
	[RecvLog_No] [int] NULL,
	[RecvLogUser_ID] [int] NULL,
	[VendorDoc_ID] [varchar](16) NULL,
	[VendorDocDate] [smalldatetime] NULL,
	[WarehouseSent] [bit] NOT NULL CONSTRAINT [DF_OrderHeader_WarehouseSend]  DEFAULT ((0)),
	[WarehouseSentDate] [smalldatetime] NULL,
	[SentToFaxDate] [smalldatetime] NULL,
	[OrderType_ID] [int] NOT NULL,
	[ProductType_ID] [int] NOT NULL,
	[FromQueue] [bit] NOT NULL CONSTRAINT [DF_OrderHeader_FromOrderQueue]  DEFAULT ((0)),
	[ClosedBy] [int] NULL,
	[MatchingValidationCode] [int] NULL,
	[MatchingUser_ID] [int] NULL,
	[MatchingDate] [datetime] NULL,
	[Freight3Party_OrderCost] [smallmoney] NULL,
	[DVOOrderID] [varchar](10) NULL,
	[eInvoice_Id] [int] NULL,
	[Email_Order] [bit] NOT NULL DEFAULT ((0)),
	[SentToEmailDate] [smalldatetime] NULL,
	[OverrideTransmissionMethod] [bit] NOT NULL CONSTRAINT [DF_OrderHeader_OverrideTransmissionMethod]  DEFAULT ((0)),
	[SupplyTransferToSubTeam] [int] NULL,
	[AccountingUploadDate] [datetime] NULL,
	[Electronic_Order] [bit] NOT NULL DEFAULT ((0)),
	[SentToElectronicDate] [smalldatetime] NULL,
	[IsDropShipment] [bit] NOT NULL DEFAULT ((0)),
	[InvoiceDiscrepancy] [bit] NULL,
	[InvoiceDiscrepancySentDate] [smalldatetime] NULL,
	[InvoiceProcessingDiscrepancy] [bit] NULL,
	[WarehouseCancelled] [datetime] NULL,
	[PayByAgreedCost] [bit] NOT NULL CONSTRAINT [DF_OrderHeader_PayByAgreedCost]  DEFAULT ((0)),
	[PurchaseAccountsTotal] [money] NULL,
	[CurrencyID] [int] NULL,
	[APUploadedCost] [money] NULL,
	[OrderExternalSourceID] [int] NULL,
	[OrderExternalSourceOrderID] [int] NULL,
	[QtyShippedProvided] [bit] NULL,
	[POCostDate] [datetime] NULL,
	[AdminNotes] [varchar](5000) NULL,
	[ResolutionCodeID] [int] NULL,
	[InReview] [bit] NOT NULL DEFAULT ((0)),
	[InReviewUser] [int] NULL,
	[ReasonCodeDetailID] [int] NULL,
	[RefuseReceivingReasonID] [int] NULL,
	[OrderedCost] [money] NULL,
	[OriginalReceivedCost] [money] NULL,
	[TotalPaidCost] [money] NULL,
	[AdjustedReceivedCost] [money] NULL,
	[OrderRefreshCostSource_ID] [int] NULL,
	[PartialShipment] [bit] NOT NULL DEFAULT ((0)),
	[DSDOrder] [bit] NOT NULL DEFAULT ((0)),
	[TotalRefused] [money] NULL,
 CONSTRAINT [PK_OrderHeader_OrderHeader_ID] PRIMARY KEY CLUSTERED 
(
	[OrderHeader_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/******************************************************************************
		10. Populate FL Table in Batches
******************************************************************************/
PRINT N'Status: 10. Populate FL Table in Batches (takes about 6 minutes in TEST) --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
PRINT N'Status: Approximate Target End Time for this Step to Complete: --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), DATEADD(MINUTE, 6, SYSDATETIME()), 9)
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;
IF EXISTS (SELECT TOP 1 1 FROM [dbo].[OrderHeader_Unaligned])
    BEGIN
        DECLARE @RowsToLoad BIGINT;
		DECLARE @RowsPerBatch INT = 10000;
		DECLARE @LeftBoundary BIGINT = 0;
		DECLARE @RightBoundary BIGINT = @RowsPerBatch;
		SELECT @RowsToLoad = MAX([OrderHeader_ID]) FROM [dbo].[OrderHeader_Unaligned]
		WHILE @LeftBoundary < @RowsToLoad
		BEGIN
			SET IDENTITY_INSERT [dbo].[OrderHeader] ON;
			INSERT INTO [dbo].[OrderHeader] ([OrderHeader_ID], [InvoiceNumber], [OrderHeaderDesc], [Vendor_ID], [PurchaseLocation_ID], [ReceiveLocation_ID], [CreatedBy], [OrderDate], [CloseDate], [OriginalCloseDate], [SystemGenerated], [Sent], [Fax_Order], [Expected_Date], [SentDate], [QuantityDiscount], [DiscountType], [Transfer_SubTeam], [Transfer_To_SubTeam], [Return_Order], [User_ID], [Temperature], [Accounting_In_DateStamp], [Accounting_In_UserID], [InvoiceDate], [ApprovedDate], [ApprovedBy], [UploadedDate], [RecvLogDate], [RecvLog_No], [RecvLogUser_ID], [VendorDoc_ID], [VendorDocDate], [WarehouseSent], [WarehouseSentDate], [SentToFaxDate], [OrderType_ID], [ProductType_ID], [FromQueue], [ClosedBy], [IsDropShipment], [OrderExternalSourceID], [OrderExternalSourceOrderID], [MatchingValidationCode], [MatchingUser_ID], [MatchingDate], [Freight3Party_OrderCost], [DVOOrderID], [eInvoice_Id], [Electronic_Order], [PayByAgreedCost], [Email_Order], [SentToEmailDate], [OverrideTransmissionMethod], [SupplyTransferToSubTeam], [AccountingUploadDate], [SentToElectronicDate], [InvoiceDiscrepancy], [InvoiceDiscrepancySentDate], [InvoiceProcessingDiscrepancy], [WarehouseCancelled], [PurchaseAccountsTotal], [CurrencyID], [APUploadedCost], [QtyShippedProvided], [POCostDate], [AdminNotes], [ResolutionCodeID], [InReview], [InReviewUser], [ReasonCodeDetailID], [RefuseReceivingReasonID], [OrderedCost], [OriginalReceivedCost], [TotalPaidCost], [AdjustedReceivedCost], [OrderRefreshCostSource_ID], [PartialShipment], [DSDOrder], [TotalRefused])
			SELECT   src.[OrderHeader_ID],
					 src.[InvoiceNumber],
					 src.[OrderHeaderDesc],
					 src.[Vendor_ID],
					 src.[PurchaseLocation_ID],
					 src.[ReceiveLocation_ID],
					 src.[CreatedBy],
					 src.[OrderDate],
					 src.[CloseDate],
					 src.[OriginalCloseDate],
					 src.[SystemGenerated],
					 src.[Sent],
					 src.[Fax_Order],
					 src.[Expected_Date],
					 src.[SentDate],
					 CAST (src.[QuantityDiscount] AS DECIMAL (9, 2)),
					 src.[DiscountType],
					 src.[Transfer_SubTeam],
					 src.[Transfer_To_SubTeam],
					 src.[Return_Order],
					 src.[User_ID],
					 CASE WHEN src.[Temperature] > 255 THEN 255 WHEN src.[Temperature] < 0 THEN 0 END as [Temperature],  -- adjusting for new data type in FL
					 src.[Accounting_In_DateStamp],
					 src.[Accounting_In_UserID],
					 src.[InvoiceDate],
					 src.[ApprovedDate],
					 src.[ApprovedBy],
					 src.[UploadedDate],
					 src.[RecvLogDate],
					 src.[RecvLog_No],
					 src.[RecvLogUser_ID],
					 src.[VendorDoc_ID],
					 src.[VendorDocDate],
					 src.[WarehouseSent],
					 src.[WarehouseSentDate],
					 src.[SentToFaxDate],
					 src.[OrderType_ID],
					 src.[ProductType_ID],
					 src.[FromQueue],
					 src.[ClosedBy],
					 src.[IsDropShipment],
					 src.[OrderExternalSourceID],
					 src.[OrderExternalSourceOrderID],
					 src.[MatchingValidationCode],
					 src.[MatchingUser_ID],
					 src.[MatchingDate],
					 src.[Freight3Party_OrderCost],
					 src.[DVOOrderID],
					 src.[eInvoice_Id],
					 src.[Electronic_Order],
					 src.[PayByAgreedCost],
					 src.[Email_Order],
					 src.[SentToEmailDate],
					 src.[OverrideTransmissionMethod],
					 src.[SupplyTransferToSubTeam],
					 src.[AccountingUploadDate],
					 src.[SentToElectronicDate],
					 src.[InvoiceDiscrepancy],
					 src.[InvoiceDiscrepancySentDate],
					 src.[InvoiceProcessingDiscrepancy],
					 src.[WarehouseCancelled],
					 src.[PurchaseAccountsTotal],
					 src.[CurrencyID],
					 src.[APUploadedCost],
					 src.[QtyShippedProvided],
					 src.[POCostDate],
					 src.[AdminNotes],
					 src.[ResolutionCodeID],
					 src.[InReview],
					 src.[InReviewUser],
					 src.[ReasonCodeDetailID],
					 src.[RefuseReceivingReasonID],
					 src.[OrderedCost],
					 src.[OriginalReceivedCost],
					 src.[TotalPaidCost],
					 src.[AdjustedReceivedCost],
					 src.[OrderRefreshCostSource_ID],
					 src.[PartialShipment],
					 src.[DSDOrder],
					 src.[TotalRefused]
			FROM     [dbo].[OrderHeader_Unaligned] src
			WHERE
				src.[OrderHeader_ID] > @LeftBoundary
				AND src.[OrderHeader_ID] <= @RightBoundary
			ORDER BY [OrderHeader_ID] ASC;
			SET @LeftBoundary = @LeftBoundary + @RowsPerBatch;
			SET @RightBoundary = @RightBoundary + @RowsPerBatch;
			SET IDENTITY_INSERT [dbo].[OrderHeader] OFF;
		END
    END
	SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO
/******************************************************************************
		11. Enable FL Change Tracking
******************************************************************************/
PRINT N'Status: 11. Enable FL Change Tracking --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderHeader] ENABLE CHANGE_TRACKING WITH(TRACK_COLUMNS_UPDATED = OFF)
GO
/******************************************************************************
		12. Create FL Indexes
******************************************************************************/
PRINT N'Status: 12. Create FL Indexes --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID')
CREATE NONCLUSTERED INDEX [_dta_IX_OrderHeader_Vendor_ID_OrderHeader_ID] ON [dbo].[OrderHeader]
(
	[Vendor_ID] ASC,
	[OrderHeader_ID] ASC
)
INCLUDE ( 	[InvoiceNumber]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
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
	[ResolutionCodeID]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'idxVendorOrder_ID')
CREATE NONCLUSTERED INDEX [idxVendorOrder_ID] ON [dbo].[OrderHeader]
(
	[InvoiceNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 80) ON [PRIMARY]
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
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeader]') AND name = N'OH_ExtSrcID_ExtSrcOrdID')
CREATE NONCLUSTERED INDEX [OH_ExtSrcID_ExtSrcOrdID] ON [dbo].[OrderHeader]
(
	[OrderExternalSourceID] ASC,
	[OrderExternalSourceOrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
GO
/******************************************************************************
		13. Create FL Foreign Keys
******************************************************************************/
PRINT N'Status: 13. Create FL Foreign Keys --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Creat__63F8CA06]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Creat__63F8CA06] FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Creat__63F8CA06]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK__OrderHead__Creat__63F8CA06]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Order__2D435324]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD FOREIGN KEY([OrderExternalSourceID])
REFERENCES [dbo].[OrderExternalSource] ([ID])
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Purch__025D5595] FOREIGN KEY([PurchaseLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK__OrderHead__Purch__025D5595]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Recei__04459E07] FOREIGN KEY([ReceiveLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK__OrderHead__Recei__04459E07]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__RecvL__6A02E22E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__RecvL__6A02E22E] FOREIGN KEY([RecvLogUser_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__RecvL__6A02E22E]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK__OrderHead__RecvL__6A02E22E]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__User___75235608]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__User___75235608] FOREIGN KEY([User_ID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__User___75235608]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK__OrderHead__User___75235608]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Vendo__07220AB2] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK__OrderHead__Vendo__07220AB2]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_Currency_OrderHeader] FOREIGN KEY([CurrencyID])
REFERENCES [dbo].[Currency] ([CurrencyID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Currency_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_Currency_OrderHeader]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ClosedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_ClosedBy] FOREIGN KEY([ClosedBy])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_ClosedBy]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_ClosedBy]
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
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam] FOREIGN KEY([Transfer_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_SubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam1] FOREIGN KEY([Transfer_To_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_SubTeam1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam] FOREIGN KEY([SupplyTransferToSubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_Users] FOREIGN KEY([Accounting_In_UserID])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_Users]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_Users1] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Users] ([User_ID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderHeader_Users1]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_RefuseReceivingReasonID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_RefuseReceivingReasonID] FOREIGN KEY([RefuseReceivingReasonID])
REFERENCES [dbo].[ReasonCodeDetail] ([ReasonCodeDetailID])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_RefuseReceivingReasonID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] CHECK CONSTRAINT [FK_OrderItem_RefuseReceivingReasonID]
GO
/******************************************************************************
		14. Create FL Triggers
******************************************************************************/
PRINT N'Status: 14. Create FL Triggers --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		15. Grant SO Perms
******************************************************************************/
PRINT N'Status: 15. Grant SO Perms --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
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
		16. Check FL Checks (generated from VS schema compare)
******************************************************************************/
PRINT N'Status: 16. Check FL Checks --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
ALTER TABLE [dbo].[OrderHeader] WITH CHECK CHECK CONSTRAINT [FK_OrderHeader_OrderRefreshCostSource]
GO
/******************************************************************************
		17. Compare SO and FL Tables
******************************************************************************/
PRINT N'Status: 17. Compare SO and FL Tables (takes about 5 minutes in TEST):'+ ' --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
GO
DECLARE @old BIGINT
DECLARE	@new BIGINT

SELECT @old = count(*)
FROM [dbo].[OrderHeader_Unaligned](NOLOCK)

SELECT @new = count(*)
FROM [dbo].[OrderHeader](NOLOCK)

PRINT N'[table here]_Unaligned] Row Count:	' + CONVERT(NVARCHAR(30), @old)
PRINT N'[table here] Aligned Row Count:	' + CONVERT(NVARCHAR(30), @new)
IF @old = @new
BEGIN
	PRINT N'**** SUCCESS!!**** New Table Row Count Matches Old Row Count.' + ' --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
ELSE
BEGIN
	PRINT N'**** OPERATION FAILED **** New Table Row Count Does Not Match Old Row Count.' + ' --- [dbo].[OrderHeader] --- ' + CONVERT(NVARCHAR(MAX), SYSDATETIME(), 9)
END
