/******************************************************************************
-- This output is generated to be run as part of the rollback process.
Status: SO - Point FKs to Aligned Tables (takes about 15 minutes in TEST): --- Apr 24 2018 12:10:37.5085617PM
Status: Approximate Target End Time for Script to Complete: --- Apr 24 2018 12:25:37.5114912PM
Status: 1. Manually Handle FKs with Mulitple Columns --- Apr 24 2018 12:10:37.5241857PM
******************************************************************************/
/******************************************************************************
		Altering Table #176 --- Apr 24 2018 12:10:39.2340372PM
******************************************************************************/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_StoreSubTeam] FOREIGN KEY([Store_No], [TransferToSubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])

GO
/******************************************************************************
		Altering Table #175 --- Apr 24 2018 12:10:39.5201517PM
******************************************************************************/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_StoreSubTeam] FOREIGN KEY([Store_No], [TransferToSubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])

GO
/******************************************************************************
		Altering Table #174 --- Apr 24 2018 12:10:39.7623237PM
******************************************************************************/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ScanGunStoreSubTeam_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ScanGunStoreSubTeam]'))
ALTER TABLE [dbo].[ScanGunStoreSubTeam] DROP CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ScanGunStoreSubTeam_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ScanGunStoreSubTeam]'))
ALTER TABLE [dbo].[ScanGunStoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam] FOREIGN KEY([Store_No], [SubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])

GO
/******************************************************************************
Status: 2. Dynamically Generate SQL to Point to Aligned Tables --- Apr 24 2018 12:10:39.8814567PM
Status: Approximate Target End Time for this Step to Complete: --- [dbo].[OrderItem] --- Apr 24 2018 12:56:39.8834097PM
******************************************************************************/
/******************************************************************************
		Altering Table #173 --- Apr 24 2018 12:10:40.4273202PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DSDVendorStore_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[DSDVendorStore]'))
ALTER TABLE [dbo].[DSDVendorStore] DROP CONSTRAINT [FK_DSDVendorStore_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DSDVendorStore_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[DSDVendorStore]'))
ALTER TABLE [dbo].[DSDVendorStore]  WITH CHECK ADD  CONSTRAINT [FK_DSDVendorStore_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DSDVendorStore_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[DSDVendorStore]'))
ALTER TABLE [dbo].[DSDVendorStore] NOCHECK CONSTRAINT [FK_DSDVendorStore_Vendor]


GO
/******************************************************************************
		Altering Table #172 --- Apr 24 2018 12:10:40.5337587PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Purch]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK__DeletedOrder__Purch]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Purch]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrder__Purch] FOREIGN KEY([PurchaseLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Purch]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Purch]


GO
/******************************************************************************
		Altering Table #171 --- Apr 24 2018 12:10:40.6245732PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Recei]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK__DeletedOrder__Recei]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Recei]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrder__Recei] FOREIGN KEY([ReceiveLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Recei]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Recei]


GO
/******************************************************************************
		Altering Table #170 --- Apr 24 2018 12:10:40.7124582PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Vendo]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK__DeletedOrder__Vendo]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Vendo]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrder__Vendo] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Vendo]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK__DeletedOrder__Vendo]


GO
/******************************************************************************
		Altering Table #169 --- Apr 24 2018 12:10:40.8003432PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Purch__025D5595]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Purch__025D5595] FOREIGN KEY([PurchaseLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Purch__025D5595]


GO
/******************************************************************************
		Altering Table #168 --- Apr 24 2018 12:10:43.7874567PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Recei__04459E07]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Recei__04459E07] FOREIGN KEY([ReceiveLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Recei__04459E07]


GO
/******************************************************************************
		Altering Table #167 --- Apr 24 2018 12:10:44.3626152PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Vendo__07220AB2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Vendo__07220AB2] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK__OrderHead__Vendo__07220AB2]


GO
/******************************************************************************
		Altering Table #166 --- Apr 24 2018 12:10:44.3997222PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_3__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor] DROP CONSTRAINT [FK_ItemVendor_3__16]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_3__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemVendor_3__16] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_3__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor] NOCHECK CONSTRAINT [FK_ItemVendor_3__16]


GO
/******************************************************************************
		Altering Table #165 --- Apr 24 2018 12:10:44.4915132PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorDocTransfer_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorDocTransfer]'))
ALTER TABLE [dbo].[VendorDocTransfer] DROP CONSTRAINT [FK_VendorDocTransfer_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorDocTransfer_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorDocTransfer]'))
ALTER TABLE [dbo].[VendorDocTransfer]  WITH CHECK ADD  CONSTRAINT [FK_VendorDocTransfer_Vendor] FOREIGN KEY([VendorID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_VendorDocTransfer_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorDocTransfer]'))
ALTER TABLE [dbo].[VendorDocTransfer] NOCHECK CONSTRAINT [FK_VendorDocTransfer_Vendor]


GO
/******************************************************************************
		Altering Table #164 --- Apr 24 2018 12:10:44.5774452PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreVendor]'))
ALTER TABLE [dbo].[StoreVendor] DROP CONSTRAINT [FK_StoreVendor_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreVendor]'))
ALTER TABLE [dbo].[StoreVendor]  WITH CHECK ADD  CONSTRAINT [FK_StoreVendor_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_StoreVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreVendor]'))
ALTER TABLE [dbo].[StoreVendor] NOCHECK CONSTRAINT [FK_StoreVendor_Vendor]


GO
/******************************************************************************
		Altering Table #163 --- Apr 24 2018 12:10:44.7053667PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party] DROP CONSTRAINT [FK_OrderInvoice_Freight3Party_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_Freight3Party_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party] NOCHECK CONSTRAINT [FK_OrderInvoice_Freight3Party_Vendor_ID]


GO
/******************************************************************************
		Altering Table #162 --- Apr 24 2018 12:10:44.8772307PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PayOrderedCost_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayOrderedCost]'))
ALTER TABLE [dbo].[PayOrderedCost] DROP CONSTRAINT [FK_PayOrderedCost_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PayOrderedCost_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayOrderedCost]'))
ALTER TABLE [dbo].[PayOrderedCost]  WITH CHECK ADD  CONSTRAINT [FK_PayOrderedCost_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PayOrderedCost_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayOrderedCost]'))
ALTER TABLE [dbo].[PayOrderedCost] NOCHECK CONSTRAINT [FK_PayOrderedCost_Vendor]


GO
/******************************************************************************
		Altering Table #161 --- Apr 24 2018 12:10:45.0071052PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupInvoice]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupInvoice] DROP CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupInvoice]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupInvoice]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupInvoice]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupInvoice] NOCHECK CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]


GO
/******************************************************************************
		Altering Table #160 --- Apr 24 2018 12:10:45.0969432PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupLog_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupLog]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupLog] DROP CONSTRAINT [FK_OrderInvoice_ControlGroupLog_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupLog_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupLog]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_ControlGroupLog_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupLog_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupLog]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupLog] NOCHECK CONSTRAINT [FK_OrderInvoice_ControlGroupLog_Vendor_ID]


GO
/******************************************************************************
		Altering Table #159 --- Apr 24 2018 12:10:45.1809222PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceMatchingTolerance_VendorOverride]'))
ALTER TABLE [dbo].[InvoiceMatchingTolerance_VendorOverride] DROP CONSTRAINT [FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceMatchingTolerance_VendorOverride]'))
ALTER TABLE [dbo].[InvoiceMatchingTolerance_VendorOverride]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceMatchingTolerance_VendorOverride]'))
ALTER TABLE [dbo].[InvoiceMatchingTolerance_VendorOverride] NOCHECK CONSTRAINT [FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]


GO
/******************************************************************************
		Altering Table #158 --- Apr 24 2018 12:10:45.3518097PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemManger_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemManager]'))
ALTER TABLE [dbo].[ItemManager] DROP CONSTRAINT [FK_ItemManger_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemManger_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemManager]'))
ALTER TABLE [dbo].[ItemManager]  WITH CHECK ADD  CONSTRAINT [FK_ItemManger_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemManger_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemManager]'))
ALTER TABLE [dbo].[ItemManager] NOCHECK CONSTRAINT [FK_ItemManger_Vendor]


GO
/******************************************************************************
		Altering Table #157 --- Apr 24 2018 12:10:45.4758252PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue] DROP CONSTRAINT [FK_VendorItemDiscrepancyQueue_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue]  WITH CHECK ADD  CONSTRAINT [FK_VendorItemDiscrepancyQueue_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue] NOCHECK CONSTRAINT [FK_VendorItemDiscrepancyQueue_Vendor]


GO
/******************************************************************************
		Altering Table #156 --- Apr 24 2018 12:10:45.6193707PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseVendorChange_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseVendorChange]'))
ALTER TABLE [dbo].[WarehouseVendorChange] DROP CONSTRAINT [FK_WarehouseVendorChange_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseVendorChange_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseVendorChange]'))
ALTER TABLE [dbo].[WarehouseVendorChange]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseVendorChange_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_WarehouseVendorChange_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseVendorChange]'))
ALTER TABLE [dbo].[WarehouseVendorChange] NOCHECK CONSTRAINT [FK_WarehouseVendorChange_Vendor]


GO
/******************************************************************************
		Altering Table #155 --- Apr 24 2018 12:10:45.7238562PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_1__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contact]'))
ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [FK_Contact_1__16]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_1__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contact]'))
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_1__16] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Contact_1__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contact]'))
ALTER TABLE [dbo].[Contact] NOCHECK CONSTRAINT [FK_Contact_1__16]


GO
/******************************************************************************
		Altering Table #154 --- Apr 24 2018 12:10:45.8449422PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadHeaderVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadHeaderVendor]'))
ALTER TABLE [dbo].[ItemUploadHeaderVendor] DROP CONSTRAINT [FK_ItemUploadHeaderVendor_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadHeaderVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadHeaderVendor]'))
ALTER TABLE [dbo].[ItemUploadHeaderVendor]  WITH CHECK ADD  CONSTRAINT [FK_ItemUploadHeaderVendor_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadHeaderVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadHeaderVendor]'))
ALTER TABLE [dbo].[ItemUploadHeaderVendor] NOCHECK CONSTRAINT [FK_ItemUploadHeaderVendor_Vendor]


GO
/******************************************************************************
		Altering Table #153 --- Apr 24 2018 12:10:45.9767697PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__LastVendo__Vendo__7CA47C3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor] DROP CONSTRAINT [FK__LastVendo__Vendo__7CA47C3F]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__LastVendo__Vendo__7CA47C3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor]  WITH NOCHECK ADD  CONSTRAINT [FK__LastVendo__Vendo__7CA47C3F] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__LastVendo__Vendo__7CA47C3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor] NOCHECK CONSTRAINT [FK__LastVendo__Vendo__7CA47C3F]


GO
/******************************************************************************
		Altering Table #152 --- Apr 24 2018 12:10:46.0783257PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept] DROP CONSTRAINT [FK_Sales_SumBySubDept_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumBySubDept_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept] NOCHECK CONSTRAINT [FK_Sales_SumBySubDept_Date]


GO
/******************************************************************************
		Altering Table #151 --- Apr 24 2018 12:10:46.1886702PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] DROP CONSTRAINT [FK_Sales_SumByItem_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] NOCHECK CONSTRAINT [FK_Sales_SumByItem_Date]


GO
/******************************************************************************
		Altering Table #150 --- Apr 24 2018 12:10:46.2951087PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] DROP CONSTRAINT [FK_Sales_SumByItem_Date_Wkly]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Date_Wkly] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] NOCHECK CONSTRAINT [FK_Sales_SumByItem_Date_Wkly]


GO
/******************************************************************************
		Altering Table #149 --- Apr 24 2018 12:10:46.3761582PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TlogReprocessRequest_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[TlogReprocessRequest]'))
ALTER TABLE [dbo].[TlogReprocessRequest] DROP CONSTRAINT [FK_TlogReprocessRequest_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TlogReprocessRequest_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[TlogReprocessRequest]'))
ALTER TABLE [dbo].[TlogReprocessRequest]  WITH CHECK ADD  CONSTRAINT [FK_TlogReprocessRequest_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_TlogReprocessRequest_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[TlogReprocessRequest]'))
ALTER TABLE [dbo].[TlogReprocessRequest] NOCHECK CONSTRAINT [FK_TlogReprocessRequest_Date]


GO
/******************************************************************************
		Altering Table #148 --- Apr 24 2018 12:10:46.4679492PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payment_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payment_SumByRegister]'))
ALTER TABLE [dbo].[Payment_SumByRegister] DROP CONSTRAINT [FK_Payment_SumByRegister_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payment_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payment_SumByRegister]'))
ALTER TABLE [dbo].[Payment_SumByRegister]  WITH CHECK ADD  CONSTRAINT [FK_Payment_SumByRegister_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Payment_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payment_SumByRegister]'))
ALTER TABLE [dbo].[Payment_SumByRegister] NOCHECK CONSTRAINT [FK_Payment_SumByRegister_Date]


GO
/******************************************************************************
		Altering Table #147 --- Apr 24 2018 12:10:48.5293407PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByRegister]'))
ALTER TABLE [dbo].[Buggy_SumByRegister] DROP CONSTRAINT [FK_Buggy_SumByRegister_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByRegister]'))
ALTER TABLE [dbo].[Buggy_SumByRegister]  WITH CHECK ADD  CONSTRAINT [FK_Buggy_SumByRegister_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByRegister]'))
ALTER TABLE [dbo].[Buggy_SumByRegister] NOCHECK CONSTRAINT [FK_Buggy_SumByRegister_Date]


GO
/******************************************************************************
		Altering Table #146 --- Apr 24 2018 12:10:48.9511887PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByCashier_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByCashier]'))
ALTER TABLE [dbo].[Buggy_SumByCashier] DROP CONSTRAINT [FK_Buggy_SumByCashier_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByCashier_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByCashier]'))
ALTER TABLE [dbo].[Buggy_SumByCashier]  WITH CHECK ADD  CONSTRAINT [FK_Buggy_SumByCashier_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByCashier_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByCashier]'))
ALTER TABLE [dbo].[Buggy_SumByCashier] NOCHECK CONSTRAINT [FK_Buggy_SumByCashier_Date]


GO
/******************************************************************************
		Altering Table #145 --- Apr 24 2018 12:10:49.4413917PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderItem_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_OrderItem_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderItem_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH NOCHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_OrderItem_ID] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderItem_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] NOCHECK CONSTRAINT [FK_SuspendedAvgCost_OrderItem_ID]


GO
/******************************************************************************
		Altering Table #144 --- Apr 24 2018 12:10:49.5273237PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4010Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4010Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4010Detail] DROP CONSTRAINT [FK_OrderItemCOOL4010Detail_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4010Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4010Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4010Detail]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItemCOOL4010Detail_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4010Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4010Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4010Detail] NOCHECK CONSTRAINT [FK_OrderItemCOOL4010Detail_OrderItem]


GO
/******************************************************************************
		Altering Table #143 --- Apr 24 2018 12:10:49.6152087PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4020Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4020Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4020Detail] DROP CONSTRAINT [FK_OrderItemCOOL4020Detail_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4020Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4020Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4020Detail]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItemCOOL4020Detail_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4020Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4020Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4020Detail] NOCHECK CONSTRAINT [FK_OrderItemCOOL4020Detail_OrderItem]


GO
/******************************************************************************
		Altering Table #142 --- Apr 24 2018 12:10:49.7050467PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory] DROP CONSTRAINT [FK_ItemHistory_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemHistory_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory] NOCHECK CONSTRAINT [FK_ItemHistory_OrderItem]


GO
/******************************************************************************
		Altering Table #141 --- Apr 24 2018 12:10:49.8339447PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue] DROP CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue] NOCHECK CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem]


GO
/******************************************************************************
		Altering Table #140 --- Apr 24 2018 12:10:49.9726077PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] NOCHECK CONSTRAINT [FK_OrderItemQueue_Item]


GO
/******************************************************************************
		Altering Table #139 --- Apr 24 2018 12:10:50.7850557PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_ItemLocaleChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[ItemLocaleChangeQueue]'))
ALTER TABLE [mammoth].[ItemLocaleChangeQueue] DROP CONSTRAINT [FK_ItemLocaleChangeQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_ItemLocaleChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[ItemLocaleChangeQueue]'))
ALTER TABLE [mammoth].[ItemLocaleChangeQueue]  WITH CHECK ADD  CONSTRAINT [FK_ItemLocaleChangeQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[mammoth].[FK_ItemLocaleChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[ItemLocaleChangeQueue]'))
ALTER TABLE [mammoth].[ItemLocaleChangeQueue] NOCHECK CONSTRAINT [FK_ItemLocaleChangeQueue_Item]


GO
/******************************************************************************
		Altering Table #138 --- Apr 24 2018 12:10:50.9315307PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] NOCHECK CONSTRAINT [FK_OrderItemQueueBak_Item]


GO
/******************************************************************************
		Altering Table #137 --- Apr 24 2018 12:10:51.0526167PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemSignAttribute_Item_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemSignAttribute]'))
ALTER TABLE [dbo].[ItemSignAttribute] DROP CONSTRAINT [FK_ItemSignAttribute_Item_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemSignAttribute_Item_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemSignAttribute]'))
ALTER TABLE [dbo].[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_Item_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemSignAttribute_Item_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemSignAttribute]'))
ALTER TABLE [dbo].[ItemSignAttribute] NOCHECK CONSTRAINT [FK_ItemSignAttribute_Item_Item_Key]


GO
/******************************************************************************
		Altering Table #136 --- Apr 24 2018 12:10:51.6951537PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IRISKeyToIRMAKey_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[IRISKeyToIRMAKey]'))
ALTER TABLE [dbo].[IRISKeyToIRMAKey] DROP CONSTRAINT [FK_IRISKeyToIRMAKey_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IRISKeyToIRMAKey_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[IRISKeyToIRMAKey]'))
ALTER TABLE [dbo].[IRISKeyToIRMAKey]  WITH CHECK ADD  CONSTRAINT [FK_IRISKeyToIRMAKey_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_IRISKeyToIRMAKey_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[IRISKeyToIRMAKey]'))
ALTER TABLE [dbo].[IRISKeyToIRMAKey] NOCHECK CONSTRAINT [FK_IRISKeyToIRMAKey_Item_Key]


GO
/******************************************************************************
		Altering Table #135 --- Apr 24 2018 12:10:51.7830387PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH CHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] NOCHECK CONSTRAINT [FK_SuspendedAvgCost_Item_Key]


GO
/******************************************************************************
		Altering Table #134 --- Apr 24 2018 12:10:51.8787357PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_PriceChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[PriceChangeQueue]'))
ALTER TABLE [mammoth].[PriceChangeQueue] DROP CONSTRAINT [FK_PriceChangeQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_PriceChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[PriceChangeQueue]'))
ALTER TABLE [mammoth].[PriceChangeQueue]  WITH CHECK ADD  CONSTRAINT [FK_PriceChangeQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[mammoth].[FK_PriceChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[PriceChangeQueue]'))
ALTER TABLE [mammoth].[PriceChangeQueue] NOCHECK CONSTRAINT [FK_PriceChangeQueue_Item]


GO
/******************************************************************************
		Altering Table #133 --- Apr 24 2018 12:10:52.0125162PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail] DROP CONSTRAINT [FK_ItemUploadDetail_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail]  WITH CHECK ADD  CONSTRAINT [FK_ItemUploadDetail_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail] NOCHECK CONSTRAINT [FK_ItemUploadDetail_Item]


GO
/******************************************************************************
		Altering Table #132 --- Apr 24 2018 12:10:52.1326257PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand] DROP CONSTRAINT [FK_OnHand_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand]  WITH CHECK ADD  CONSTRAINT [FK_OnHand_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OnHand_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand] NOCHECK CONSTRAINT [FK_OnHand_Item]


GO
/******************************************************************************
		Altering Table #131 --- Apr 24 2018 12:10:54.5016147PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseItemChange_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseItemChange]'))
ALTER TABLE [dbo].[WarehouseItemChange] DROP CONSTRAINT [FK_WarehouseItemChange_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseItemChange_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseItemChange]'))
ALTER TABLE [dbo].[WarehouseItemChange]  WITH NOCHECK ADD  CONSTRAINT [FK_WarehouseItemChange_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_WarehouseItemChange_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseItemChange]'))
ALTER TABLE [dbo].[WarehouseItemChange] NOCHECK CONSTRAINT [FK_WarehouseItemChange_Item]


GO
/******************************************************************************
		Altering Table #130 --- Apr 24 2018 12:10:55.0543137PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor] DROP CONSTRAINT [FK_ItemVendor_Item1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemVendor_Item1] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor] NOCHECK CONSTRAINT [FK_ItemVendor_Item1]


GO
/******************************************************************************
		Altering Table #129 --- Apr 24 2018 12:10:55.1041152PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconItemChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconItemChangeQueue]'))
ALTER TABLE [dbo].[IconItemChangeQueue] DROP CONSTRAINT [FK_IconItemChangeQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconItemChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconItemChangeQueue]'))
ALTER TABLE [dbo].[IconItemChangeQueue]  WITH CHECK ADD  CONSTRAINT [FK_IconItemChangeQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_IconItemChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconItemChangeQueue]'))
ALTER TABLE [dbo].[IconItemChangeQueue] NOCHECK CONSTRAINT [FK_IconItemChangeQueue_Item]


GO
/******************************************************************************
		Altering Table #128 --- Apr 24 2018 12:10:55.1666112PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMExcludedItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMExcludedItem]'))
ALTER TABLE [dbo].[PMExcludedItem] DROP CONSTRAINT [FK_PMExcludedItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMExcludedItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMExcludedItem]'))
ALTER TABLE [dbo].[PMExcludedItem]  WITH NOCHECK ADD  CONSTRAINT [FK_PMExcludedItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PMExcludedItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMExcludedItem]'))
ALTER TABLE [dbo].[PMExcludedItem] NOCHECK CONSTRAINT [FK_PMExcludedItem_Item]


GO
/******************************************************************************
		Altering Table #127 --- Apr 24 2018 12:10:55.2095772PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Planogram_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Planogram]'))
ALTER TABLE [dbo].[Planogram] DROP CONSTRAINT [FK_Planogram_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Planogram_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Planogram]'))
ALTER TABLE [dbo].[Planogram]  WITH CHECK ADD  CONSTRAINT [FK_Planogram_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Planogram_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Planogram]'))
ALTER TABLE [dbo].[Planogram] NOCHECK CONSTRAINT [FK_Planogram_Item]


GO
/******************************************************************************
		Altering Table #126 --- Apr 24 2018 12:10:55.2662142PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOnOrder_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOnOrder]'))
ALTER TABLE [dbo].[ItemOnOrder] DROP CONSTRAINT [FK_ItemOnOrder_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOnOrder_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOnOrder]'))
ALTER TABLE [dbo].[ItemOnOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemOnOrder_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOnOrder_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOnOrder]'))
ALTER TABLE [dbo].[ItemOnOrder] NOCHECK CONSTRAINT [FK_ItemOnOrder_Item]


GO
/******************************************************************************
		Altering Table #125 --- Apr 24 2018 12:10:55.3208982PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper] DROP CONSTRAINT [FK_Shipper_Item2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper]  WITH NOCHECK ADD  CONSTRAINT [FK_Shipper_Item2] FOREIGN KEY([Shipper_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper] NOCHECK CONSTRAINT [FK_Shipper_Item2]


GO
/******************************************************************************
		Altering Table #124 --- Apr 24 2018 12:10:55.3814412PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper] DROP CONSTRAINT [FK_Shipper_Item3]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper]  WITH NOCHECK ADD  CONSTRAINT [FK_Shipper_Item3] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper] NOCHECK CONSTRAINT [FK_Shipper_Item3]


GO
/******************************************************************************
		Altering Table #123 --- Apr 24 2018 12:10:55.5152217PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_Item]


GO
/******************************************************************************
		Altering Table #122 --- Apr 24 2018 12:10:55.6050597PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LastVendor_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor] DROP CONSTRAINT [FK_LastVendor_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LastVendor_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_LastVendor_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_LastVendor_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor] NOCHECK CONSTRAINT [FK_LastVendor_Item]


GO
/******************************************************************************
		Altering Table #121 --- Apr 24 2018 12:10:55.7183337PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_Item__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_Item__OLD__] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK_OrderItem_Item__OLD__]


GO
/******************************************************************************
		Altering Table #120 --- Apr 24 2018 12:10:55.8355137PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] DROP CONSTRAINT [FK_Sales_SumByItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] NOCHECK CONSTRAINT [FK_Sales_SumByItem_Item]


GO
/******************************************************************************
		Altering Table #119 --- Apr 24 2018 12:10:55.9351167PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorStoreItemIdentifier_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorStoreItemIdentifier]'))
ALTER TABLE [dbo].[CompetitorStoreItemIdentifier] DROP CONSTRAINT [FK_CompetitorStoreItemIdentifier_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorStoreItemIdentifier_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorStoreItemIdentifier]'))
ALTER TABLE [dbo].[CompetitorStoreItemIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorStoreItemIdentifier_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_CompetitorStoreItemIdentifier_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorStoreItemIdentifier]'))
ALTER TABLE [dbo].[CompetitorStoreItemIdentifier] NOCHECK CONSTRAINT [FK_CompetitorStoreItemIdentifier_Item]


GO
/******************************************************************************
		Altering Table #118 --- Apr 24 2018 12:10:56.0503437PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] DROP CONSTRAINT [FK_Sales_SumByItem_Item_Wkly]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Item_Wkly] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] NOCHECK CONSTRAINT [FK_Sales_SumByItem_Item_Wkly]


GO
/******************************************************************************
		Altering Table #117 --- Apr 24 2018 12:10:56.1489702PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] DROP CONSTRAINT [FK_ItemUomOverride_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemUomOverride_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] NOCHECK CONSTRAINT [FK_ItemUomOverride_Item]


GO
/******************************************************************************
		Altering Table #116 --- Apr 24 2018 12:10:56.2368552PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice] DROP CONSTRAINT [FK_CompetitorPrice_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorPrice_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice] NOCHECK CONSTRAINT [FK_CompetitorPrice_Item]


GO
/******************************************************************************
		Altering Table #115 --- Apr 24 2018 12:10:56.5464057PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] NOCHECK CONSTRAINT [FK_ItemOverride_Item_Key]


GO
/******************************************************************************
		Altering Table #114 --- Apr 24 2018 12:10:56.6587032PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_Item]


GO
/******************************************************************************
		Altering Table #113 --- Apr 24 2018 12:11:27.9096327PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemNutrition_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemNutrition]'))
ALTER TABLE [dbo].[ItemNutrition] DROP CONSTRAINT [FK_ItemNutrition_ItemKey]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemNutrition_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemNutrition]'))
ALTER TABLE [dbo].[ItemNutrition]  WITH CHECK ADD  CONSTRAINT [FK_ItemNutrition_ItemKey] FOREIGN KEY([ItemKey])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemNutrition_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemNutrition]'))
ALTER TABLE [dbo].[ItemNutrition] NOCHECK CONSTRAINT [FK_ItemNutrition_ItemKey]


GO
/******************************************************************************
		Altering Table #112 --- Apr 24 2018 12:11:28.1703582PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreItemExtended_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItemExtended]'))
ALTER TABLE [dbo].[StoreItemExtended] DROP CONSTRAINT [FK_StoreItemExtended_ItemKey]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreItemExtended_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItemExtended]'))
ALTER TABLE [dbo].[StoreItemExtended]  WITH CHECK ADD  CONSTRAINT [FK_StoreItemExtended_ItemKey] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_StoreItemExtended_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItemExtended]'))
ALTER TABLE [dbo].[StoreItemExtended] NOCHECK CONSTRAINT [FK_StoreItemExtended_ItemKey]


GO
/******************************************************************************
		Altering Table #111 --- Apr 24 2018 12:11:28.5131097PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo] DROP CONSTRAINT [FK_CompetitorImportInfo_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorImportInfo_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo] NOCHECK CONSTRAINT [FK_CompetitorImportInfo_Item]


GO
/******************************************************************************
		Altering Table #110 --- Apr 24 2018 12:11:28.8910152PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride] DROP CONSTRAINT [FK_ItemScaleOverride_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemScaleOverride_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride] NOCHECK CONSTRAINT [FK_ItemScaleOverride_Item_Key]


GO
/******************************************************************************
		Altering Table #109 --- Apr 24 2018 12:11:28.9974537PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChainItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChainItem]'))
ALTER TABLE [dbo].[ItemChainItem] DROP CONSTRAINT [FK_ItemChainItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChainItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChainItem]'))
ALTER TABLE [dbo].[ItemChainItem]  WITH CHECK ADD  CONSTRAINT [FK_ItemChainItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemChainItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChainItem]'))
ALTER TABLE [dbo].[ItemChainItem] NOCHECK CONSTRAINT [FK_ItemChainItem_Item]


GO
/******************************************************************************
		Altering Table #108 --- Apr 24 2018 12:11:29.1214692PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale] DROP CONSTRAINT [FK_ItemScale_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale]  WITH CHECK ADD  CONSTRAINT [FK_ItemScale_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale] NOCHECK CONSTRAINT [FK_ItemScale_Item]


GO
/******************************************************************************
		Altering Table #107 --- Apr 24 2018 12:11:29.4193017PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK__Price__Item_Key__4E6F0AFB]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK__Price__Item_Key__4E6F0AFB] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] NOCHECK CONSTRAINT [FK__Price__Item_Key__4E6F0AFB]


GO
/******************************************************************************
		Altering Table #106 --- Apr 24 2018 12:11:30.9914667PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_Price_LinkCode]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_LinkCode] FOREIGN KEY([LinkedItem])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] NOCHECK CONSTRAINT [FK_Price_LinkCode]


GO
/******************************************************************************
		Altering Table #105 --- Apr 24 2018 12:11:43.4105937PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemAttribute_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemAttribute]'))
ALTER TABLE [dbo].[ItemAttribute] DROP CONSTRAINT [FK_ItemAttribute_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemAttribute_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemAttribute]'))
ALTER TABLE [dbo].[ItemAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemAttribute_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemAttribute_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemAttribute]'))
ALTER TABLE [dbo].[ItemAttribute] NOCHECK CONSTRAINT [FK_ItemAttribute_Item]


GO
/******************************************************************************
		Altering Table #104 --- Apr 24 2018 12:11:44.0912142PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RIFIrmaRla_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PIFI_ItemRla]'))
ALTER TABLE [dbo].[PIFI_ItemRla] DROP CONSTRAINT [FK_RIFIrmaRla_Item1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RIFIrmaRla_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PIFI_ItemRla]'))
ALTER TABLE [dbo].[PIFI_ItemRla]  WITH CHECK ADD  CONSTRAINT [FK_RIFIrmaRla_Item1] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_RIFIrmaRla_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PIFI_ItemRla]'))
ALTER TABLE [dbo].[PIFI_ItemRla] NOCHECK CONSTRAINT [FK_RIFIrmaRla_Item1]


GO
/******************************************************************************
		Altering Table #103 --- Apr 24 2018 12:11:44.3880702PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_AllergenRla_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item_AllergenRla]'))
ALTER TABLE [dbo].[Item_AllergenRla] DROP CONSTRAINT [FK_Item_AllergenRla_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_AllergenRla_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item_AllergenRla]'))
ALTER TABLE [dbo].[Item_AllergenRla]  WITH CHECK ADD  CONSTRAINT [FK_Item_AllergenRla_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Item_AllergenRla_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item_AllergenRla]'))
ALTER TABLE [dbo].[Item_AllergenRla] NOCHECK CONSTRAINT [FK_Item_AllergenRla_Item]


GO
/******************************************************************************
		Altering Table #102 --- Apr 24 2018 12:11:44.5472397PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreItem_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItem]'))
ALTER TABLE [dbo].[StoreItem] DROP CONSTRAINT [FK__StoreItem_ItemKey]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreItem_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItem]'))
ALTER TABLE [dbo].[StoreItem]  WITH CHECK ADD  CONSTRAINT [FK__StoreItem_ItemKey] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__StoreItem_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItem]'))
ALTER TABLE [dbo].[StoreItem] NOCHECK CONSTRAINT [FK__StoreItem_ItemKey]


GO
/******************************************************************************
		Altering Table #101 --- Apr 24 2018 12:11:54.2946627PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocationItems_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocationItems]'))
ALTER TABLE [dbo].[InventoryLocationItems] DROP CONSTRAINT [FK_InventoryLocationItems_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocationItems_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocationItems]'))
ALTER TABLE [dbo].[InventoryLocationItems]  WITH CHECK ADD  CONSTRAINT [FK_InventoryLocationItems_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocationItems_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocationItems]'))
ALTER TABLE [dbo].[InventoryLocationItems] NOCHECK CONSTRAINT [FK_InventoryLocationItems_Item]


GO
/******************************************************************************
		Altering Table #100 --- Apr 24 2018 12:11:55.3522122PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaxOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaxOverride]'))
ALTER TABLE [dbo].[TaxOverride] DROP CONSTRAINT [FK_TaxOverride_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaxOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaxOverride]'))
ALTER TABLE [dbo].[TaxOverride]  WITH CHECK ADD  CONSTRAINT [FK_TaxOverride_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_TaxOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaxOverride]'))
ALTER TABLE [dbo].[TaxOverride] NOCHECK CONSTRAINT [FK_TaxOverride_Item]


GO
/******************************************************************************
		Altering Table #99 --- Apr 24 2018 12:11:55.5123582PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue] DROP CONSTRAINT [FK_VendorItemDiscrepancyQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue]  WITH CHECK ADD  CONSTRAINT [FK_VendorItemDiscrepancyQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue] NOCHECK CONSTRAINT [FK_VendorItemDiscrepancyQueue_Item]


GO
/******************************************************************************
		Altering Table #98 --- Apr 24 2018 12:11:55.6666452PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key_365]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride365]'))
ALTER TABLE [dbo].[ItemOverride365] DROP CONSTRAINT [FK_ItemOverride_Item_Key_365]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key_365]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride365]'))
ALTER TABLE [dbo].[ItemOverride365]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Item_Key_365] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key_365]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride365]'))
ALTER TABLE [dbo].[ItemOverride365] NOCHECK CONSTRAINT [FK_ItemOverride_Item_Key_365]


GO
/******************************************************************************
		Altering Table #97 --- Apr 24 2018 12:11:55.7555067PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] NOCHECK CONSTRAINT [FK_PriceBatchDetail_Item]


GO
/******************************************************************************
		Altering Table #96 --- Apr 24 2018 12:12:37.1473887PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_LinkCode]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_LinkCode] FOREIGN KEY([LinkedItem])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] NOCHECK CONSTRAINT [FK_PriceBatchDetail_LinkCode]


GO
/******************************************************************************
		Altering Table #95 --- Apr 24 2018 12:13:17.5686297PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EInvoicing_Item_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[EInvoicing_Item]'))
ALTER TABLE [dbo].[EInvoicing_Item] DROP CONSTRAINT [FK_EInvoicing_Item_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EInvoicing_Item_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[EInvoicing_Item]'))
ALTER TABLE [dbo].[EInvoicing_Item]  WITH CHECK ADD  CONSTRAINT [FK_EInvoicing_Item_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_EInvoicing_Item_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[EInvoicing_Item]'))
ALTER TABLE [dbo].[EInvoicing_Item] NOCHECK CONSTRAINT [FK_EInvoicing_Item_Item]


GO
/******************************************************************************
		Altering Table #94 --- Apr 24 2018 12:14:25.8650397PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserRoleSubTeamRla_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleSubTeamRla]'))
ALTER TABLE [dbo].[UserRoleSubTeamRla] DROP CONSTRAINT [FK_UserRoleSubTeamRla_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserRoleSubTeamRla_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleSubTeamRla]'))
ALTER TABLE [dbo].[UserRoleSubTeamRla]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleSubTeamRla_SubTeam] FOREIGN KEY([SubTeamID])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_UserRoleSubTeamRla_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleSubTeamRla]'))
ALTER TABLE [dbo].[UserRoleSubTeamRla] NOCHECK CONSTRAINT [FK_UserRoleSubTeamRla_SubTeam]


GO
/******************************************************************************
		Altering Table #93 --- Apr 24 2018 12:14:26.0173737PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory] DROP CONSTRAINT [FK_ItemHistory_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory]  WITH CHECK ADD  CONSTRAINT [FK_ItemHistory_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory] NOCHECK CONSTRAINT [FK_ItemHistory_SubTeam]


GO
/******************************************************************************
		Altering Table #92 --- Apr 24 2018 12:16:34.7552277PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSubTe__SubTe__0ACBABC0]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSubTeam]'))
ALTER TABLE [dbo].[ZoneSubTeam] DROP CONSTRAINT [FK__ZoneSubTe__SubTe__0ACBABC0]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSubTe__SubTe__0ACBABC0]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSubTeam]'))
ALTER TABLE [dbo].[ZoneSubTeam]  WITH CHECK ADD  CONSTRAINT [FK__ZoneSubTe__SubTe__0ACBABC0] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__ZoneSubTe__SubTe__0ACBABC0]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSubTeam]'))
ALTER TABLE [dbo].[ZoneSubTeam] NOCHECK CONSTRAINT [FK__ZoneSubTe__SubTe__0ACBABC0]


GO
/******************************************************************************
		Altering Table #91 --- Apr 24 2018 12:16:34.9095147PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PromotionalOffer_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PromotionalOffer]'))
ALTER TABLE [dbo].[PromotionalOffer] DROP CONSTRAINT [FK_PromotionalOffer_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PromotionalOffer_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PromotionalOffer]'))
ALTER TABLE [dbo].[PromotionalOffer]  WITH CHECK ADD  CONSTRAINT [FK_PromotionalOffer_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PromotionalOffer_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PromotionalOffer]'))
ALTER TABLE [dbo].[PromotionalOffer] NOCHECK CONSTRAINT [FK_PromotionalOffer_SubTeam]


GO
/******************************************************************************
		Altering Table #90 --- Apr 24 2018 12:16:35.0452482PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_SubTeam_No]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_SubTeam_No]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_SubTeam_No]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH CHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_SubTeam_No] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_SubTeam_No]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] NOCHECK CONSTRAINT [FK_SuspendedAvgCost_SubTeam_No]


GO
/******************************************************************************
		Altering Table #89 --- Apr 24 2018 12:16:35.1809817PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__UsersSubT__SubTe__0F353234]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersSubTeam]'))
ALTER TABLE [dbo].[UsersSubTeam] DROP CONSTRAINT [FK__UsersSubT__SubTe__0F353234]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__UsersSubT__SubTe__0F353234]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersSubTeam]'))
ALTER TABLE [dbo].[UsersSubTeam]  WITH CHECK ADD  CONSTRAINT [FK__UsersSubT__SubTe__0F353234] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__UsersSubT__SubTe__0F353234]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersSubTeam]'))
ALTER TABLE [dbo].[UsersSubTeam] NOCHECK CONSTRAINT [FK__UsersSubT__SubTe__0F353234]


GO
/******************************************************************************
		Altering Table #88 --- Apr 24 2018 12:16:35.3879997PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam_Hist]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceHistory]'))
ALTER TABLE [dbo].[PriceHistory] DROP CONSTRAINT [FK_ExceptionSubTeam_SubTeam_Hist]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam_Hist]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceHistory]'))
ALTER TABLE [dbo].[PriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_ExceptionSubTeam_SubTeam_Hist] FOREIGN KEY([ExceptionSubteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam_Hist]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceHistory]'))
ALTER TABLE [dbo].[PriceHistory] NOCHECK CONSTRAINT [FK_ExceptionSubTeam_SubTeam_Hist]


GO
/******************************************************************************
		Altering Table #87 --- Apr 24 2018 12:19:08.0461977PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue] DROP CONSTRAINT [FK_ItemHistoryOther_Hist_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue]  WITH CHECK ADD  CONSTRAINT [FK_ItemHistoryOther_Hist_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue] NOCHECK CONSTRAINT [FK_ItemHistoryOther_Hist_SubTeam]


GO
/******************************************************************************
		Altering Table #86 --- Apr 24 2018 12:19:08.2668867PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail] DROP CONSTRAINT [FK_ItemUploadDetail_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail]  WITH CHECK ADD  CONSTRAINT [FK_ItemUploadDetail_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail] NOCHECK CONSTRAINT [FK_ItemUploadDetail_SubTeam]


GO
/******************************************************************************
		Altering Table #85 --- Apr 24 2018 12:19:08.4182442PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_DistSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam] DROP CONSTRAINT [FK_DistSubTeam_DistSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_DistSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_DistSubTeam_DistSubTeam] FOREIGN KEY([DistSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_DistSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam] NOCHECK CONSTRAINT [FK_DistSubTeam_DistSubTeam]


GO
/******************************************************************************
		Altering Table #84 --- Apr 24 2018 12:19:08.5940142PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_RetailSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam] DROP CONSTRAINT [FK_DistSubTeam_RetailSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_RetailSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_DistSubTeam_RetailSubTeam] FOREIGN KEY([RetailSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_RetailSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam] NOCHECK CONSTRAINT [FK_DistSubTeam_RetailSubTeam]


GO
/******************************************************************************
		Altering Table #83 --- Apr 24 2018 12:19:08.7424422PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand] DROP CONSTRAINT [FK_OnHand_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand]  WITH CHECK ADD  CONSTRAINT [FK_OnHand_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OnHand_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand] NOCHECK CONSTRAINT [FK_OnHand_SubTeam]


GO
/******************************************************************************
		Altering Table #82 --- Apr 24 2018 12:19:10.1603202PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__SubTe__248EAB1F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice] DROP CONSTRAINT [FK__OrderInvo__SubTe__248EAB1F]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__SubTe__248EAB1F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice]  WITH CHECK ADD  CONSTRAINT [FK__OrderInvo__SubTe__248EAB1F] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__SubTe__248EAB1F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice] NOCHECK CONSTRAINT [FK__OrderInvo__SubTe__248EAB1F]


GO
/******************************************************************************
		Altering Table #81 --- Apr 24 2018 12:19:13.4423367PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSuppl__SubTe__2827047D]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSupply]'))
ALTER TABLE [dbo].[ZoneSupply] DROP CONSTRAINT [FK__ZoneSuppl__SubTe__2827047D]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSuppl__SubTe__2827047D]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSupply]'))
ALTER TABLE [dbo].[ZoneSupply]  WITH CHECK ADD  CONSTRAINT [FK__ZoneSuppl__SubTe__2827047D] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__ZoneSuppl__SubTe__2827047D]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSupply]'))
ALTER TABLE [dbo].[ZoneSupply] NOCHECK CONSTRAINT [FK__ZoneSuppl__SubTe__2827047D]


GO
/******************************************************************************
		Altering Table #80 --- Apr 24 2018 12:19:13.6220127PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK_DeletedOrder_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrder_SubTeam] FOREIGN KEY([Transfer_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_SubTeam]


GO
/******************************************************************************
		Altering Table #79 --- Apr 24 2018 12:19:13.7596992PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK_DeletedOrder_SubTeam1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrder_SubTeam1] FOREIGN KEY([Transfer_To_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_SubTeam1]


GO
/******************************************************************************
		Altering Table #78 --- Apr 24 2018 12:19:13.9022682PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK_DeletedOrder_SupplyTransferToSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH CHECK ADD  CONSTRAINT [FK_DeletedOrder_SupplyTransferToSubTeam] FOREIGN KEY([SupplyTransferToSubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] NOCHECK CONSTRAINT [FK_DeletedOrder_SupplyTransferToSubTeam]


GO
/******************************************************************************
		Altering Table #77 --- Apr 24 2018 12:19:15.5789187PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NoTagThresholdSubteamOverride_SubteamNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[NoTagThresholdSubteamOverride]'))
ALTER TABLE [dbo].[NoTagThresholdSubteamOverride] DROP CONSTRAINT [FK_NoTagThresholdSubteamOverride_SubteamNumber]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NoTagThresholdSubteamOverride_SubteamNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[NoTagThresholdSubteamOverride]'))
ALTER TABLE [dbo].[NoTagThresholdSubteamOverride]  WITH CHECK ADD  CONSTRAINT [FK_NoTagThresholdSubteamOverride_SubteamNumber] FOREIGN KEY([SubteamNumber])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_NoTagThresholdSubteamOverride_SubteamNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[NoTagThresholdSubteamOverride]'))
ALTER TABLE [dbo].[NoTagThresholdSubteamOverride] NOCHECK CONSTRAINT [FK_NoTagThresholdSubteamOverride_SubteamNumber]


GO
/******************************************************************************
		Altering Table #76 --- Apr 24 2018 12:19:15.7361352PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMSubTeamInclude_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSubTeamInclude]'))
ALTER TABLE [dbo].[PMSubTeamInclude] DROP CONSTRAINT [FK_PMSubTeamInclude_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMSubTeamInclude_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSubTeamInclude]'))
ALTER TABLE [dbo].[PMSubTeamInclude]  WITH CHECK ADD  CONSTRAINT [FK_PMSubTeamInclude_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PMSubTeamInclude_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSubTeamInclude]'))
ALTER TABLE [dbo].[PMSubTeamInclude] NOCHECK CONSTRAINT [FK_PMSubTeamInclude_SubTeam]


GO
/******************************************************************************
		Altering Table #75 --- Apr 24 2018 12:19:15.8777277PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam] FOREIGN KEY([Transfer_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_SubTeam]


GO
/******************************************************************************
		Altering Table #74 --- Apr 24 2018 12:19:32.7662952PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SubTeam1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam1] FOREIGN KEY([Transfer_To_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_SubTeam1]


GO
/******************************************************************************
		Altering Table #73 --- Apr 24 2018 12:19:33.7476777PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam] FOREIGN KEY([SupplyTransferToSubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] NOCHECK CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam]


GO
/******************************************************************************
		Altering Table #72 --- Apr 24 2018 12:19:34.1646432PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept] DROP CONSTRAINT [FK_Sales_SumBySubDept_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumBySubDept_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept] NOCHECK CONSTRAINT [FK_Sales_SumBySubDept_SubTeam]


GO
/******************************************************************************
		Altering Table #71 --- Apr 24 2018 12:19:34.5571962PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] DROP CONSTRAINT [FK_Sales_SumByItem_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumByItem_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] NOCHECK CONSTRAINT [FK_Sales_SumByItem_SubTeam]


GO
/******************************************************************************
		Altering Table #70 --- Apr 24 2018 12:19:34.6499637PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_Fact_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_Fact]'))
ALTER TABLE [dbo].[Sales_Fact] DROP CONSTRAINT [FK_Sales_Fact_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_Fact_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_Fact]'))
ALTER TABLE [dbo].[Sales_Fact]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Fact_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_Fact_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_Fact]'))
ALTER TABLE [dbo].[Sales_Fact] NOCHECK CONSTRAINT [FK_Sales_Fact_SubTeam]


GO
/******************************************************************************
		Altering Table #69 --- Apr 24 2018 12:22:01.0361022PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] DROP CONSTRAINT [FK_Sales_SumByItem_SubTeam_Wkly]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumByItem_SubTeam_Wkly] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] NOCHECK CONSTRAINT [FK_Sales_SumByItem_SubTeam_Wkly]


GO
/******************************************************************************
		Altering Table #68 --- Apr 24 2018 12:22:01.1718357PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCategory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCategory]'))
ALTER TABLE [dbo].[ItemCategory] DROP CONSTRAINT [FK_ItemCategory_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCategory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCategory]'))
ALTER TABLE [dbo].[ItemCategory]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemCategory_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemCategory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCategory]'))
ALTER TABLE [dbo].[ItemCategory] NOCHECK CONSTRAINT [FK_ItemCategory_SubTeam]


GO
/******************************************************************************
		Altering Table #67 --- Apr 24 2018 12:22:01.3192872PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] DROP CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH CHECK ADD  CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] NOCHECK CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]


GO
/******************************************************************************
		Altering Table #66 --- Apr 24 2018 12:22:02.9227002PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] DROP CONSTRAINT [FK_StoreSubTeam_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
ON DELETE CASCADE
ON UPDATE CASCADE
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] NOCHECK CONSTRAINT [FK_StoreSubTeam_SubTeam]


GO
/******************************************************************************
		Altering Table #65 --- Apr 24 2018 12:22:03.6365217PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ShelfTagRule_Subteam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ShelfTagRule]'))
ALTER TABLE [dbo].[ShelfTagRule] DROP CONSTRAINT [FK_ShelfTagRule_Subteam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ShelfTagRule_Subteam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ShelfTagRule]'))
ALTER TABLE [dbo].[ShelfTagRule]  WITH CHECK ADD  CONSTRAINT [FK_ShelfTagRule_Subteam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ShelfTagRule_Subteam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ShelfTagRule]'))
ALTER TABLE [dbo].[ShelfTagRule] NOCHECK CONSTRAINT [FK_ShelfTagRule_Subteam]


GO
/******************************************************************************
		Altering Table #64 --- Apr 24 2018 12:22:04.0114977PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__SubTeam_No__0BAD2365]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__SubTeam_No__0BAD2365] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__SubTeam_No__0BAD2365]


GO
/******************************************************************************
		Altering Table #63 --- Apr 24 2018 12:22:04.6930947PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_ExceptionSubTeam_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_ExceptionSubTeam_SubTeam] FOREIGN KEY([ExceptionSubteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] NOCHECK CONSTRAINT [FK_ExceptionSubTeam_SubTeam]


GO
/******************************************************************************
		Altering Table #62 --- Apr 24 2018 12:22:05.5914747PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_SubTeam] FOREIGN KEY([DistSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK_Item_SubTeam]


GO
/******************************************************************************
		Altering Table #61 --- Apr 24 2018 12:22:06.7271442PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocation_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocation]'))
ALTER TABLE [dbo].[InventoryLocation] DROP CONSTRAINT [FK_InventoryLocation_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocation_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocation]'))
ALTER TABLE [dbo].[InventoryLocation]  WITH CHECK ADD  CONSTRAINT [FK_InventoryLocation_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocation_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocation]'))
ALTER TABLE [dbo].[InventoryLocation] NOCHECK CONSTRAINT [FK_InventoryLocation_SubTeam]


GO
/******************************************************************************
		Altering Table #60 --- Apr 24 2018 12:22:06.8287002PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Recipe_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Recipe]'))
ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_Recipe_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Recipe_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Recipe]'))
ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_SubTeam] FOREIGN KEY([SubTeamID])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_Recipe_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Recipe]'))
ALTER TABLE [dbo].[Recipe] NOCHECK CONSTRAINT [FK_Recipe_SubTeam]


GO
/******************************************************************************
		Altering Table #59 --- Apr 24 2018 12:22:06.9468567PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MasterCycleCount_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[CycleCountMaster]'))
ALTER TABLE [dbo].[CycleCountMaster] DROP CONSTRAINT [FK_MasterCycleCount_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MasterCycleCount_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[CycleCountMaster]'))
ALTER TABLE [dbo].[CycleCountMaster]  WITH CHECK ADD  CONSTRAINT [FK_MasterCycleCount_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_MasterCycleCount_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[CycleCountMaster]'))
ALTER TABLE [dbo].[CycleCountMaster] NOCHECK CONSTRAINT [FK_MasterCycleCount_SubTeam]


GO
/******************************************************************************
		Altering Table #58 --- Apr 24 2018 12:22:07.1538747PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubTeam_CatalogSchedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[CatalogSchedule]'))
ALTER TABLE [dbo].[CatalogSchedule] DROP CONSTRAINT [FK_SubTeam_CatalogSchedule]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubTeam_CatalogSchedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[CatalogSchedule]'))
ALTER TABLE [dbo].[CatalogSchedule]  WITH CHECK ADD  CONSTRAINT [FK_SubTeam_CatalogSchedule] FOREIGN KEY([SubTeamNo])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_SubTeam_CatalogSchedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[CatalogSchedule]'))
ALTER TABLE [dbo].[CatalogSchedule] NOCHECK CONSTRAINT [FK_SubTeam_CatalogSchedule]


GO
/******************************************************************************
		Altering Table #57 --- Apr 24 2018 12:22:07.2398067PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreSubteamDiscountException_SubteamNo]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubteamDiscountException]'))
ALTER TABLE [dbo].[StoreSubteamDiscountException] DROP CONSTRAINT [FK__StoreSubteamDiscountException_SubteamNo]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreSubteamDiscountException_SubteamNo]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubteamDiscountException]'))
ALTER TABLE [dbo].[StoreSubteamDiscountException]  WITH CHECK ADD  CONSTRAINT [FK__StoreSubteamDiscountException_SubteamNo] FOREIGN KEY([Subteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__StoreSubteamDiscountException_SubteamNo]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubteamDiscountException]'))
ALTER TABLE [dbo].[StoreSubteamDiscountException] NOCHECK CONSTRAINT [FK__StoreSubteamDiscountException_SubteamNo]


GO
/******************************************************************************
		Altering Table #56 --- Apr 24 2018 12:22:07.3276917PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH CHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] NOCHECK CONSTRAINT [FK_SuspendedAvgCost_OrderHeader_ID]


GO
/******************************************************************************
		Altering Table #55 --- Apr 24 2018 12:22:07.4175297PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID]


GO
/******************************************************************************
		Altering Table #54 --- Apr 24 2018 12:22:07.5102972PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party] DROP CONSTRAINT [FK_OrderInvoice_Freight3Party_OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_Freight3Party_OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party] NOCHECK CONSTRAINT [FK_OrderInvoice_Freight3Party_OrderHeader_ID]


GO
/******************************************************************************
		Altering Table #53 --- Apr 24 2018 12:22:07.6059942PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Order__1FEDB87C]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Order__1FEDB87C] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Order__1FEDB87C]


GO
/******************************************************************************
		Altering Table #52 --- Apr 24 2018 12:22:07.6948557PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice] DROP CONSTRAINT [FK__OrderInvo__OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderInvo__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice] NOCHECK CONSTRAINT [FK__OrderInvo__OrderHeader_ID]


GO
/******************************************************************************
		Altering Table #51 --- Apr 24 2018 12:22:07.7837172PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__OrderHeader_ID__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__OrderHeader_ID__OLD__] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK__OrderItem__OrderHeader_ID__OLD__]


GO
/******************************************************************************
		Altering Table #50 --- Apr 24 2018 12:22:07.8745317PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList] DROP CONSTRAINT [FK__ReturnOrd__OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList]  WITH NOCHECK ADD  CONSTRAINT [FK__ReturnOrd__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList] NOCHECK CONSTRAINT [FK__ReturnOrd__OrderHeader_ID]


GO
/******************************************************************************
		Altering Table #49 --- Apr 24 2018 12:22:07.9633932PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__Retur__0731BBB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList] DROP CONSTRAINT [FK__ReturnOrd__Retur__0731BBB4]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__Retur__0731BBB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList]  WITH NOCHECK ADD  CONSTRAINT [FK__ReturnOrd__Retur__0731BBB4] FOREIGN KEY([ReturnOrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__Retur__0731BBB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList] NOCHECK CONSTRAINT [FK__ReturnOrd__Retur__0731BBB4]


GO
/******************************************************************************
		Altering Table #48 --- Apr 24 2018 12:22:08.0493252PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderTransmissionOverride_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderTransmissionOverride]'))
ALTER TABLE [dbo].[OrderTransmissionOverride] DROP CONSTRAINT [FK_OrderTransmissionOverride_OrderHeader]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderTransmissionOverride_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderTransmissionOverride]'))
ALTER TABLE [dbo].[OrderTransmissionOverride]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransmissionOverride_OrderHeader] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderTransmissionOverride_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderTransmissionOverride]'))
ALTER TABLE [dbo].[OrderTransmissionOverride] NOCHECK CONSTRAINT [FK_OrderTransmissionOverride_OrderHeader]


GO
/******************************************************************************
		Altering Table #47 --- Apr 24 2018 12:22:10.1820012PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] NOCHECK CONSTRAINT [FK_OrderItemQueue_ItemUnit]


GO
/******************************************************************************
		Altering Table #46 --- Apr 24 2018 12:22:10.6097082PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] NOCHECK CONSTRAINT [FK_OrderItemQueueBak_ItemUnit]


GO
/******************************************************************************
		Altering Table #45 --- Apr 24 2018 12:22:10.7210292PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__CostU__1A34DF26__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__CostU__1A34DF26__OLD__] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK__OrderItem__CostU__1A34DF26__OLD__]


GO
/******************************************************************************
		Altering Table #44 --- Apr 24 2018 12:22:10.8196557PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__Freig__1B29035F__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Freig__1B29035F__OLD__] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK__OrderItem__Freig__1B29035F__OLD__]


GO
/******************************************************************************
		Altering Table #43 --- Apr 24 2018 12:22:10.9212117PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__Handl__1C1D2798__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Handl__1C1D2798__OLD__] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK__OrderItem__Handl__1C1D2798__OLD__]


GO
/******************************************************************************
		Altering Table #42 --- Apr 24 2018 12:22:11.0217912PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__Quant__21D600EE__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Quant__21D600EE__OLD__] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK__OrderItem__Quant__21D600EE__OLD__]


GO
/******************************************************************************
		Altering Table #41 --- Apr 24 2018 12:22:11.1184647PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_ItemUnit__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit__OLD__] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit__OLD__]


GO
/******************************************************************************
		Altering Table #40 --- Apr 24 2018 12:22:11.2190442PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RipeItemUnitExt_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[RipeItemUnitExt]'))
ALTER TABLE [dbo].[RipeItemUnitExt] DROP CONSTRAINT [FK_RipeItemUnitExt_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RipeItemUnitExt_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[RipeItemUnitExt]'))
ALTER TABLE [dbo].[RipeItemUnitExt]  WITH CHECK ADD  CONSTRAINT [FK_RipeItemUnitExt_ItemUnit] FOREIGN KEY([ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_RipeItemUnitExt_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[RipeItemUnitExt]'))
ALTER TABLE [dbo].[RipeItemUnitExt] NOCHECK CONSTRAINT [FK_RipeItemUnitExt_ItemUnit]


GO
/******************************************************************************
		Altering Table #39 --- Apr 24 2018 12:22:11.3245062PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_ItemUnit1__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit1__OLD__] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit1__OLD__]


GO
/******************************************************************************
		Altering Table #38 --- Apr 24 2018 12:22:11.4270387PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__CostU]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__CostU]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__CostU]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__CostU] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__CostU]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__CostU]


GO
/******************************************************************************
		Altering Table #37 --- Apr 24 2018 12:22:11.5256652PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Freig]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__Freig]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Freig]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__Freig] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Freig]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Freig]


GO
/******************************************************************************
		Altering Table #36 --- Apr 24 2018 12:22:11.6233152PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Handl]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__Handl]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Handl]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__Handl] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Handl]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Handl]


GO
/******************************************************************************
		Altering Table #35 --- Apr 24 2018 12:22:11.7199887PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Quant]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__Quant]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Quant]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__Quant] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Quant]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK__DeletedOrderItem__Quant]


GO
/******************************************************************************
		Altering Table #34 --- Apr 24 2018 12:22:11.8166622PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_ItemUnit]


GO
/******************************************************************************
		Altering Table #33 --- Apr 24 2018 12:22:11.9162652PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_ItemUnit1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_ItemUnit1]


GO
/******************************************************************************
		Altering Table #32 --- Apr 24 2018 12:22:12.0129387PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_ItemUnit2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_DeletedOrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] NOCHECK CONSTRAINT [FK_DeletedOrderItem_ItemUnit2]


GO
/******************************************************************************
		Altering Table #31 --- Apr 24 2018 12:22:15.5820462PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_CostUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory] DROP CONSTRAINT [FK_VendorCostHistory_ItemUnit_CostUnitID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_CostUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory]  WITH CHECK ADD  CONSTRAINT [FK_VendorCostHistory_ItemUnit_CostUnitID] FOREIGN KEY([CostUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_CostUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory] NOCHECK CONSTRAINT [FK_VendorCostHistory_ItemUnit_CostUnitID]


GO
/******************************************************************************
		Altering Table #30 --- Apr 24 2018 12:25:56.0034852PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_FreightUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory] DROP CONSTRAINT [FK_VendorCostHistory_ItemUnit_FreightUnitID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_FreightUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory]  WITH CHECK ADD  CONSTRAINT [FK_VendorCostHistory_ItemUnit_FreightUnitID] FOREIGN KEY([FreightUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_FreightUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory] NOCHECK CONSTRAINT [FK_VendorCostHistory_ItemUnit_FreightUnitID]


GO
/******************************************************************************
		Altering Table #29 --- Apr 24 2018 12:30:42.1179852PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_RetailItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] DROP CONSTRAINT [FK_ItemUomOverride_RetailItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_RetailItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemUomOverride_RetailItemUnit] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_RetailItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] NOCHECK CONSTRAINT [FK_ItemUomOverride_RetailItemUnit]


GO
/******************************************************************************
		Altering Table #28 --- Apr 24 2018 12:30:42.8493837PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__CostU__1A34DF26]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__CostU__1A34DF26] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__CostU__1A34DF26]


GO
/******************************************************************************
		Altering Table #27 --- Apr 24 2018 12:30:43.4470017PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_ScaleItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] DROP CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_ScaleItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit] FOREIGN KEY([Scale_ScaleUomUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_ScaleItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] NOCHECK CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit]


GO
/******************************************************************************
		Altering Table #26 --- Apr 24 2018 12:30:44.2272252PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Freig__1B29035F]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Freig__1B29035F] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Freig__1B29035F]


GO
/******************************************************************************
		Altering Table #25 --- Apr 24 2018 12:39:33.4052697PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Handl__1C1D2798]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Handl__1C1D2798] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Handl__1C1D2798]


GO
/******************************************************************************
		Altering Table #24 --- Apr 24 2018 12:45:06.9981762PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Quant__21D600EE]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Quant__21D600EE] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK__OrderItem__Quant__21D600EE]


GO
/******************************************************************************
		Altering Table #23 --- Apr 24 2018 12:45:07.8848382PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Distribution_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Distribution_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Distribution_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Distribution_Unit_ID] FOREIGN KEY([Distribution_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Distribution_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] NOCHECK CONSTRAINT [FK_ItemOverride_Distribution_Unit_ID]


GO
/******************************************************************************
		Altering Table #22 --- Apr 24 2018 12:45:08.8740327PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit]


GO
/******************************************************************************
		Altering Table #21 --- Apr 24 2018 12:45:09.9813837PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Package_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Package_Unit_ID] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] NOCHECK CONSTRAINT [FK_ItemOverride_Package_Unit_ID]


GO
/******************************************************************************
		Altering Table #20 --- Apr 24 2018 12:45:10.7332887PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit1]


GO
/******************************************************************************
		Altering Table #19 --- Apr 24 2018 12:45:11.3445777PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Retail_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Retail_Unit_ID] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] NOCHECK CONSTRAINT [FK_ItemOverride_Retail_Unit_ID]


GO
/******************************************************************************
		Altering Table #18 --- Apr 24 2018 12:45:12.0574227PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit2]


GO
/******************************************************************************
		Altering Table #17 --- Apr 24 2018 12:53:18.3231747PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Vendor_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Vendor_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Vendor_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Vendor_Unit_ID] FOREIGN KEY([Vendor_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Vendor_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] NOCHECK CONSTRAINT [FK_ItemOverride_Vendor_Unit_ID]


GO
/******************************************************************************
		Altering Table #16 --- Apr 24 2018 12:53:19.1688237PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Manufacturing_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Manufacturing_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Manufacturing_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Manufacturing_Unit_ID] FOREIGN KEY([Manufacturing_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Manufacturing_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] NOCHECK CONSTRAINT [FK_ItemOverride_Manufacturing_Unit_ID]


GO
/******************************************************************************
		Altering Table #15 --- Apr 24 2018 12:53:20.0955222PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Cost_Unit___1FB41C12]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Cost_Unit___1FB41C12] FOREIGN KEY([Cost_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__Cost_Unit___1FB41C12]


GO
/******************************************************************************
		Altering Table #14 --- Apr 24 2018 12:53:21.1208472PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Distributi__1EBFF7D9]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Distributi__1EBFF7D9] FOREIGN KEY([Distribution_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__Distributi__1EBFF7D9]


GO
/******************************************************************************
		Altering Table #13 --- Apr 24 2018 12:53:22.2682347PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Freight_Un__20A8404B]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Freight_Un__20A8404B] FOREIGN KEY([Freight_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__Freight_Un__20A8404B]


GO
/******************************************************************************
		Altering Table #12 --- Apr 24 2018 12:53:23.4429642PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_ScaleUOMUnit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride] DROP CONSTRAINT [FK_ItemScaleOverride_ScaleUOMUnit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_ScaleUOMUnit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemScaleOverride_ScaleUOMUnit_ID] FOREIGN KEY([Scale_ScaleUOMUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_ScaleUOMUnit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride] NOCHECK CONSTRAINT [FK_ItemScaleOverride_ScaleUOMUnit_ID]


GO
/******************************************************************************
		Altering Table #11 --- Apr 24 2018 12:53:24.1636212PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Package_Un__0E899010]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Package_Un__0E899010] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__Package_Un__0E899010]


GO
/******************************************************************************
		Altering Table #10 --- Apr 24 2018 12:53:25.1752752PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E]


GO
/******************************************************************************
		Altering Table #9 --- Apr 24 2018 12:53:26.2650492PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale] DROP CONSTRAINT [FK_ItemScale_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale]  WITH CHECK ADD  CONSTRAINT [FK_ItemScale_ItemUnit] FOREIGN KEY([Scale_ScaleUOMUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale] NOCHECK CONSTRAINT [FK_ItemScale_ItemUnit]


GO
/******************************************************************************
		Altering Table #8 --- Apr 24 2018 12:53:27.1165572PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_1__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion] DROP CONSTRAINT [FK_ItemConversion_1__33]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_1__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemConversion_1__33] FOREIGN KEY([FromUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_1__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion] NOCHECK CONSTRAINT [FK_ItemConversion_1__33]


GO
/******************************************************************************
		Altering Table #7 --- Apr 24 2018 12:53:27.8079192PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_2__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion] DROP CONSTRAINT [FK_ItemConversion_2__33]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_2__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemConversion_2__33] FOREIGN KEY([ToUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_2__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion] NOCHECK CONSTRAINT [FK_ItemConversion_2__33]


GO
/******************************************************************************
		Altering Table #6 --- Apr 24 2018 12:53:28.5871662PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0] FOREIGN KEY([Vendor_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] NOCHECK CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0]


GO
/******************************************************************************
		Altering Table #5 --- Apr 24 2018 12:53:29.4396507PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo] DROP CONSTRAINT [FK_CompetitorImportInfo_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorImportInfo_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo] NOCHECK CONSTRAINT [FK_CompetitorImportInfo_ItemUnit]


GO
/******************************************************************************
		Altering Table #4 --- Apr 24 2018 12:53:30.2374512PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice] DROP CONSTRAINT [FK_CompetitorPrice_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorPrice_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice] NOCHECK CONSTRAINT [FK_CompetitorPrice_ItemUnit]


GO
/******************************************************************************
		Altering Table #3 --- Apr 24 2018 12:53:31.2725412PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_Package_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_Package_Unit_ID] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] NOCHECK CONSTRAINT [FK_PriceBatchDetail_Package_Unit_ID]


GO
/******************************************************************************
		Altering Table #2 --- Apr 24 2018 12:54:40.8774612PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_Retail_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_Retail_Unit_ID] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] NOCHECK CONSTRAINT [FK_PriceBatchDetail_Retail_Unit_ID]


GO
/******************************************************************************
		Altering Table #1 --- Apr 24 2018 12:55:10.6528992PM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_ItemUnit2__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit2__OLD__] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])
IF EXISTS(SELECT 1 FROM sys.foreign_keys Where is_disabled = 1 AND object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] NOCHECK CONSTRAINT [FK_OrderItem_ItemUnit2__OLD__]


GO
