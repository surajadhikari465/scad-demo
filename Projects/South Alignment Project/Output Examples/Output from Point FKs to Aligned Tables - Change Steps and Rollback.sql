/******************************************************************************
Status: SO - Point FKs to Aligned Tables (takes about 46 minutes in TEST): --- Mar 15 2018 10:30:44.3438349AM
Status: Approximate Target End Time for Script to Complete: --- Mar 15 2018 11:16:44.3496945AM
Status: 1. Manually Handle FKs with Mulitple Columns --- Mar 15 2018 10:30:44.3555541AM
******************************************************************************/
/******************************************************************************
		Altering Table #176 --- Mar 15 2018 10:30:48.4904785AM
******************************************************************************/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_StoreSubTeam] FOREIGN KEY([Store_No], [TransferToSubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])

GO
/******************************************************************************
		Altering Table #175 --- Mar 15 2018 10:30:49.1194089AM
******************************************************************************/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_StoreSubTeam] FOREIGN KEY([Store_No], [TransferToSubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])

GO
/******************************************************************************
		Altering Table #174 --- Mar 15 2018 10:30:49.6487261AM
******************************************************************************/

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ScanGunStoreSubTeam_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ScanGunStoreSubTeam]'))
ALTER TABLE [dbo].[ScanGunStoreSubTeam] DROP CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ScanGunStoreSubTeam_StoreSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ScanGunStoreSubTeam]'))
ALTER TABLE [dbo].[ScanGunStoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_ScanGunStoreSubTeam_StoreSubTeam] FOREIGN KEY([Store_No], [SubTeam_No])
REFERENCES [dbo].[StoreSubTeam] ([Store_No], [SubTeam_No])

GO
/******************************************************************************
Status: 2. Dynamically Generate SQL to Point to Aligned Tables --- Mar 15 2018 10:30:49.9622147AM
Status: Approximate Target End Time for this Step to Complete: --- [dbo].[OrderItem] --- Mar 15 2018 11:16:50.1585113AM
******************************************************************************/
/******************************************************************************
		Altering Table #173 --- Mar 15 2018 10:30:51.0589365AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Purch]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK__DeletedOrder__Purch]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Purch]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrder__Purch] FOREIGN KEY([PurchaseLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #172 --- Mar 15 2018 10:30:51.2933205AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Recei]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK__DeletedOrder__Recei]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Recei]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrder__Recei] FOREIGN KEY([ReceiveLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #171 --- Mar 15 2018 10:30:51.4827809AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__LastVendo__Vendo__7CA47C3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor] DROP CONSTRAINT [FK__LastVendo__Vendo__7CA47C3F]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__LastVendo__Vendo__7CA47C3F]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor]  WITH NOCHECK ADD  CONSTRAINT [FK__LastVendo__Vendo__7CA47C3F] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #170 --- Mar 15 2018 10:30:51.6175517AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Vendo]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK__DeletedOrder__Vendo]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrder__Vendo]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrder__Vendo] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #169 --- Mar 15 2018 10:30:51.6507561AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_3__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor] DROP CONSTRAINT [FK_ItemVendor_3__16]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_3__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemVendor_3__16] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #168 --- Mar 15 2018 10:30:51.7308373AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorDocTransfer_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorDocTransfer]'))
ALTER TABLE [dbo].[VendorDocTransfer] DROP CONSTRAINT [FK_VendorDocTransfer_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorDocTransfer_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorDocTransfer]'))
ALTER TABLE [dbo].[VendorDocTransfer]  WITH CHECK ADD  CONSTRAINT [FK_VendorDocTransfer_Vendor] FOREIGN KEY([VendorID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #167 --- Mar 15 2018 10:30:52.8646699AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreVendor]'))
ALTER TABLE [dbo].[StoreVendor] DROP CONSTRAINT [FK_StoreVendor_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreVendor]'))
ALTER TABLE [dbo].[StoreVendor]  WITH CHECK ADD  CONSTRAINT [FK_StoreVendor_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #166 --- Mar 15 2018 10:30:53.0951475AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party] DROP CONSTRAINT [FK_OrderInvoice_Freight3Party_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_Freight3Party_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #165 --- Mar 15 2018 10:30:53.3236719AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PayOrderedCost_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayOrderedCost]'))
ALTER TABLE [dbo].[PayOrderedCost] DROP CONSTRAINT [FK_PayOrderedCost_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PayOrderedCost_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[PayOrderedCost]'))
ALTER TABLE [dbo].[PayOrderedCost]  WITH CHECK ADD  CONSTRAINT [FK_PayOrderedCost_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #164 --- Mar 15 2018 10:30:53.6420435AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupInvoice]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupInvoice] DROP CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupInvoice_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupInvoice]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupInvoice]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_ControlGroupInvoice_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #163 --- Mar 15 2018 10:30:53.8832637AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Purch__025D5595]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Purch__025D5595]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Purch__025D5595] FOREIGN KEY([PurchaseLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #162 --- Mar 15 2018 10:30:55.8784575AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DSDVendorStore_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[DSDVendorStore]'))
ALTER TABLE [dbo].[DSDVendorStore] DROP CONSTRAINT [FK_DSDVendorStore_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DSDVendorStore_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[DSDVendorStore]'))
ALTER TABLE [dbo].[DSDVendorStore]  WITH CHECK ADD  CONSTRAINT [FK_DSDVendorStore_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #161 --- Mar 15 2018 10:30:56.0522923AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupLog_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupLog]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupLog] DROP CONSTRAINT [FK_OrderInvoice_ControlGroupLog_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_ControlGroupLog_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_ControlGroupLog]'))
ALTER TABLE [dbo].[OrderInvoice_ControlGroupLog]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_ControlGroupLog_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #160 --- Mar 15 2018 10:30:56.4136343AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Recei__04459E07]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Recei__04459E07]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK__OrderHead__Recei__04459E07] FOREIGN KEY([ReceiveLocation_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #159 --- Mar 15 2018 10:30:58.0865501AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK__OrderHead__Vendo__07220AB2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderHead__Vendo__07220AB2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderHead__Vendo__07220AB2] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #158 --- Mar 15 2018 10:30:58.2926127AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceMatchingTolerance_VendorOverride]'))
ALTER TABLE [dbo].[InvoiceMatchingTolerance_VendorOverride] DROP CONSTRAINT [FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[InvoiceMatchingTolerance_VendorOverride]'))
ALTER TABLE [dbo].[InvoiceMatchingTolerance_VendorOverride]  WITH CHECK ADD  CONSTRAINT [FK_InvoiceMatchingTolerance_VendorOverride_Vendor_ID] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #157 --- Mar 15 2018 10:30:58.6900889AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemManger_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemManager]'))
ALTER TABLE [dbo].[ItemManager] DROP CONSTRAINT [FK_ItemManger_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemManger_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemManager]'))
ALTER TABLE [dbo].[ItemManager]  WITH CHECK ADD  CONSTRAINT [FK_ItemManger_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #156 --- Mar 15 2018 10:30:59.0885417AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue] DROP CONSTRAINT [FK_VendorItemDiscrepancyQueue_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue]  WITH CHECK ADD  CONSTRAINT [FK_VendorItemDiscrepancyQueue_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #155 --- Mar 15 2018 10:30:59.4899243AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseVendorChange_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseVendorChange]'))
ALTER TABLE [dbo].[WarehouseVendorChange] DROP CONSTRAINT [FK_WarehouseVendorChange_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseVendorChange_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseVendorChange]'))
ALTER TABLE [dbo].[WarehouseVendorChange]  WITH CHECK ADD  CONSTRAINT [FK_WarehouseVendorChange_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #154 --- Mar 15 2018 10:30:59.5348479AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_1__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contact]'))
ALTER TABLE [dbo].[Contact] DROP CONSTRAINT [FK_Contact_1__16]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Contact_1__16]') AND parent_object_id = OBJECT_ID(N'[dbo].[Contact]'))
ALTER TABLE [dbo].[Contact]  WITH CHECK ADD  CONSTRAINT [FK_Contact_1__16] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #153 --- Mar 15 2018 10:30:59.9293943AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadHeaderVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadHeaderVendor]'))
ALTER TABLE [dbo].[ItemUploadHeaderVendor] DROP CONSTRAINT [FK_ItemUploadHeaderVendor_Vendor]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadHeaderVendor_Vendor]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadHeaderVendor]'))
ALTER TABLE [dbo].[ItemUploadHeaderVendor]  WITH CHECK ADD  CONSTRAINT [FK_ItemUploadHeaderVendor_Vendor] FOREIGN KEY([Vendor_ID])
REFERENCES [dbo].[Vendor] ([Vendor_ID])


GO
/******************************************************************************
		Altering Table #152 --- Mar 15 2018 10:31:00.1461995AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByCashier_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByCashier]'))
ALTER TABLE [dbo].[Buggy_SumByCashier] DROP CONSTRAINT [FK_Buggy_SumByCashier_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByCashier_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByCashier]'))
ALTER TABLE [dbo].[Buggy_SumByCashier]  WITH CHECK ADD  CONSTRAINT [FK_Buggy_SumByCashier_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #151 --- Mar 15 2018 10:31:01.0466247AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept] DROP CONSTRAINT [FK_Sales_SumBySubDept_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumBySubDept_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #150 --- Mar 15 2018 10:31:01.0749461AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] DROP CONSTRAINT [FK_Sales_SumByItem_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #149 --- Mar 15 2018 10:31:01.1159633AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] DROP CONSTRAINT [FK_Sales_SumByItem_Date_Wkly]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Date_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Date_Wkly] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #148 --- Mar 15 2018 10:31:01.1638167AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TlogReprocessRequest_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[TlogReprocessRequest]'))
ALTER TABLE [dbo].[TlogReprocessRequest] DROP CONSTRAINT [FK_TlogReprocessRequest_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TlogReprocessRequest_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[TlogReprocessRequest]'))
ALTER TABLE [dbo].[TlogReprocessRequest]  WITH CHECK ADD  CONSTRAINT [FK_TlogReprocessRequest_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #147 --- Mar 15 2018 10:31:01.2126467AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payment_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payment_SumByRegister]'))
ALTER TABLE [dbo].[Payment_SumByRegister] DROP CONSTRAINT [FK_Payment_SumByRegister_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Payment_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Payment_SumByRegister]'))
ALTER TABLE [dbo].[Payment_SumByRegister]  WITH CHECK ADD  CONSTRAINT [FK_Payment_SumByRegister_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #146 --- Mar 15 2018 10:31:03.4832417AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByRegister]'))
ALTER TABLE [dbo].[Buggy_SumByRegister] DROP CONSTRAINT [FK_Buggy_SumByRegister_Date]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Buggy_SumByRegister_Date]') AND parent_object_id = OBJECT_ID(N'[dbo].[Buggy_SumByRegister]'))
ALTER TABLE [dbo].[Buggy_SumByRegister]  WITH CHECK ADD  CONSTRAINT [FK_Buggy_SumByRegister_Date] FOREIGN KEY([Date_Key])
REFERENCES [dbo].[Date] ([Date_Key])


GO
/******************************************************************************
		Altering Table #145 --- Mar 15 2018 10:31:03.7762217AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderItem_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_OrderItem_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderItem_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH NOCHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_OrderItem_ID] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])


GO
/******************************************************************************
		Altering Table #144 --- Mar 15 2018 10:31:03.8738817AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4010Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4010Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4010Detail] DROP CONSTRAINT [FK_OrderItemCOOL4010Detail_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4010Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4010Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4010Detail]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItemCOOL4010Detail_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])


GO
/******************************************************************************
		Altering Table #143 --- Mar 15 2018 10:31:03.9783779AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4020Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4020Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4020Detail] DROP CONSTRAINT [FK_OrderItemCOOL4020Detail_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemCOOL4020Detail_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemCOOL4020Detail]'))
ALTER TABLE [dbo].[OrderItemCOOL4020Detail]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItemCOOL4020Detail_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])


GO
/******************************************************************************
		Altering Table #142 --- Mar 15 2018 10:31:04.0838507AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory] DROP CONSTRAINT [FK_ItemHistory_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemHistory_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])


GO
/******************************************************************************
		Altering Table #141 --- Mar 15 2018 10:31:04.1785809AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue] DROP CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_OrderItem]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemHistoryOther_Hist_OrderItem] FOREIGN KEY([OrderItem_ID])
REFERENCES [dbo].[OrderItem] ([OrderItem_ID])


GO
/******************************************************************************
		Altering Table #140 --- Mar 15 2018 10:31:04.4315203AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EInvoicing_Item_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[EInvoicing_Item]'))
ALTER TABLE [dbo].[EInvoicing_Item] DROP CONSTRAINT [FK_EInvoicing_Item_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EInvoicing_Item_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[EInvoicing_Item]'))
ALTER TABLE [dbo].[EInvoicing_Item]  WITH CHECK ADD  CONSTRAINT [FK_EInvoicing_Item_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #139 --- Mar 15 2018 10:31:44.8979179AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseItemChange_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseItemChange]'))
ALTER TABLE [dbo].[WarehouseItemChange] DROP CONSTRAINT [FK_WarehouseItemChange_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WarehouseItemChange_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[WarehouseItemChange]'))
ALTER TABLE [dbo].[WarehouseItemChange]  WITH NOCHECK ADD  CONSTRAINT [FK_WarehouseItemChange_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #138 --- Mar 15 2018 10:31:45.1948043AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor] DROP CONSTRAINT [FK_ItemVendor_Item1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemVendor_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemVendor]'))
ALTER TABLE [dbo].[ItemVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemVendor_Item1] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #137 --- Mar 15 2018 10:31:45.5043865AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconItemChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconItemChangeQueue]'))
ALTER TABLE [dbo].[IconItemChangeQueue] DROP CONSTRAINT [FK_IconItemChangeQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IconItemChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[IconItemChangeQueue]'))
ALTER TABLE [dbo].[IconItemChangeQueue]  WITH CHECK ADD  CONSTRAINT [FK_IconItemChangeQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #136 --- Mar 15 2018 10:31:45.8032261AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMExcludedItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMExcludedItem]'))
ALTER TABLE [dbo].[PMExcludedItem] DROP CONSTRAINT [FK_PMExcludedItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMExcludedItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMExcludedItem]'))
ALTER TABLE [dbo].[PMExcludedItem]  WITH NOCHECK ADD  CONSTRAINT [FK_PMExcludedItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #135 --- Mar 15 2018 10:31:46.1596851AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Planogram_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Planogram]'))
ALTER TABLE [dbo].[Planogram] DROP CONSTRAINT [FK_Planogram_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Planogram_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Planogram]'))
ALTER TABLE [dbo].[Planogram]  WITH CHECK ADD  CONSTRAINT [FK_Planogram_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #134 --- Mar 15 2018 10:31:46.4028585AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOnOrder_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOnOrder]'))
ALTER TABLE [dbo].[ItemOnOrder] DROP CONSTRAINT [FK_ItemOnOrder_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOnOrder_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOnOrder]'))
ALTER TABLE [dbo].[ItemOnOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemOnOrder_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #133 --- Mar 15 2018 10:31:46.6792363AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper] DROP CONSTRAINT [FK_Shipper_Item2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item2]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper]  WITH NOCHECK ADD  CONSTRAINT [FK_Shipper_Item2] FOREIGN KEY([Shipper_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #132 --- Mar 15 2018 10:31:47.0610869AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper] DROP CONSTRAINT [FK_Shipper_Item3]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Shipper_Item3]') AND parent_object_id = OBJECT_ID(N'[dbo].[Shipper]'))
ALTER TABLE [dbo].[Shipper]  WITH NOCHECK ADD  CONSTRAINT [FK_Shipper_Item3] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #131 --- Mar 15 2018 10:31:47.3706691AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #130 --- Mar 15 2018 10:31:47.6392341AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LastVendor_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor] DROP CONSTRAINT [FK_LastVendor_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_LastVendor_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[LastVendor]'))
ALTER TABLE [dbo].[LastVendor]  WITH NOCHECK ADD  CONSTRAINT [FK_LastVendor_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #129 --- Mar 15 2018 10:31:47.8306477AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #128 --- Mar 15 2018 10:31:54.9510383AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_Item__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_Item__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_Item__OLD__] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #127 --- Mar 15 2018 10:31:55.1434285AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] DROP CONSTRAINT [FK_Sales_SumByItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #126 --- Mar 15 2018 10:31:55.3885551AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorStoreItemIdentifier_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorStoreItemIdentifier]'))
ALTER TABLE [dbo].[CompetitorStoreItemIdentifier] DROP CONSTRAINT [FK_CompetitorStoreItemIdentifier_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorStoreItemIdentifier_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorStoreItemIdentifier]'))
ALTER TABLE [dbo].[CompetitorStoreItemIdentifier]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorStoreItemIdentifier_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #125 --- Mar 15 2018 10:31:55.6219625AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK__Price__Item_Key__4E6F0AFB]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Price__Item_Key__4E6F0AFB]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK__Price__Item_Key__4E6F0AFB] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #124 --- Mar 15 2018 10:31:56.7362631AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] DROP CONSTRAINT [FK_Sales_SumByItem_Item_Wkly]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_Item_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumByItem_Item_Wkly] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #123 --- Mar 15 2018 10:31:56.9442789AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] DROP CONSTRAINT [FK_ItemUomOverride_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemUomOverride_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #122 --- Mar 15 2018 10:31:57.1454585AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice] DROP CONSTRAINT [FK_CompetitorPrice_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorPrice_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #121 --- Mar 15 2018 10:31:57.4853153AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_Price_LinkCode]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Price_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_Price_LinkCode] FOREIGN KEY([LinkedItem])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #120 --- Mar 15 2018 10:31:59.1426055AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #119 --- Mar 15 2018 10:31:59.3125339AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemNutrition_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemNutrition]'))
ALTER TABLE [dbo].[ItemNutrition] DROP CONSTRAINT [FK_ItemNutrition_ItemKey]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemNutrition_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemNutrition]'))
ALTER TABLE [dbo].[ItemNutrition]  WITH CHECK ADD  CONSTRAINT [FK_ItemNutrition_ItemKey] FOREIGN KEY([ItemKey])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #118 --- Mar 15 2018 10:31:59.6084437AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreItemExtended_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItemExtended]'))
ALTER TABLE [dbo].[StoreItemExtended] DROP CONSTRAINT [FK_StoreItemExtended_ItemKey]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreItemExtended_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItemExtended]'))
ALTER TABLE [dbo].[StoreItemExtended]  WITH CHECK ADD  CONSTRAINT [FK_StoreItemExtended_ItemKey] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #117 --- Mar 15 2018 10:32:00.1768249AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo] DROP CONSTRAINT [FK_CompetitorImportInfo_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorImportInfo_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #116 --- Mar 15 2018 10:32:00.5635585AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride] DROP CONSTRAINT [FK_ItemScaleOverride_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemScaleOverride_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #115 --- Mar 15 2018 10:32:00.8780237AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChainItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChainItem]'))
ALTER TABLE [dbo].[ItemChainItem] DROP CONSTRAINT [FK_ItemChainItem_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemChainItem_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemChainItem]'))
ALTER TABLE [dbo].[ItemChainItem]  WITH CHECK ADD  CONSTRAINT [FK_ItemChainItem_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #114 --- Mar 15 2018 10:32:01.2159273AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale] DROP CONSTRAINT [FK_ItemScale_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale]  WITH CHECK ADD  CONSTRAINT [FK_ItemScale_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #113 --- Mar 15 2018 10:32:01.6749293AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #112 --- Mar 15 2018 10:32:04.3996433AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #111 --- Mar 15 2018 10:32:05.0344333AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_ItemLocaleChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[ItemLocaleChangeQueue]'))
ALTER TABLE [mammoth].[ItemLocaleChangeQueue] DROP CONSTRAINT [FK_ItemLocaleChangeQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_ItemLocaleChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[ItemLocaleChangeQueue]'))
ALTER TABLE [mammoth].[ItemLocaleChangeQueue]  WITH CHECK ADD  CONSTRAINT [FK_ItemLocaleChangeQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #110 --- Mar 15 2018 10:32:05.3147175AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_LinkCode]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_LinkCode]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_LinkCode] FOREIGN KEY([LinkedItem])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #109 --- Mar 15 2018 10:32:08.3206923AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #108 --- Mar 15 2018 10:32:08.6625023AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemAttribute_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemAttribute]'))
ALTER TABLE [dbo].[ItemAttribute] DROP CONSTRAINT [FK_ItemAttribute_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemAttribute_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemAttribute]'))
ALTER TABLE [dbo].[ItemAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemAttribute_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #107 --- Mar 15 2018 10:32:09.2269771AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemSignAttribute_Item_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemSignAttribute]'))
ALTER TABLE [dbo].[ItemSignAttribute] DROP CONSTRAINT [FK_ItemSignAttribute_Item_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemSignAttribute_Item_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemSignAttribute]'))
ALTER TABLE [dbo].[ItemSignAttribute]  WITH CHECK ADD  CONSTRAINT [FK_ItemSignAttribute_Item_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #106 --- Mar 15 2018 10:32:09.8969247AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IRISKeyToIRMAKey_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[IRISKeyToIRMAKey]'))
ALTER TABLE [dbo].[IRISKeyToIRMAKey] DROP CONSTRAINT [FK_IRISKeyToIRMAKey_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_IRISKeyToIRMAKey_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[IRISKeyToIRMAKey]'))
ALTER TABLE [dbo].[IRISKeyToIRMAKey]  WITH CHECK ADD  CONSTRAINT [FK_IRISKeyToIRMAKey_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #105 --- Mar 15 2018 10:32:10.1840451AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RIFIrmaRla_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PIFI_ItemRla]'))
ALTER TABLE [dbo].[PIFI_ItemRla] DROP CONSTRAINT [FK_RIFIrmaRla_Item1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RIFIrmaRla_Item1]') AND parent_object_id = OBJECT_ID(N'[dbo].[PIFI_ItemRla]'))
ALTER TABLE [dbo].[PIFI_ItemRla]  WITH CHECK ADD  CONSTRAINT [FK_RIFIrmaRla_Item1] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #104 --- Mar 15 2018 10:32:10.6459769AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_AllergenRla_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item_AllergenRla]'))
ALTER TABLE [dbo].[Item_AllergenRla] DROP CONSTRAINT [FK_Item_AllergenRla_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_AllergenRla_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item_AllergenRla]'))
ALTER TABLE [dbo].[Item_AllergenRla]  WITH CHECK ADD  CONSTRAINT [FK_Item_AllergenRla_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #103 --- Mar 15 2018 10:32:10.9125887AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreItem_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItem]'))
ALTER TABLE [dbo].[StoreItem] DROP CONSTRAINT [FK__StoreItem_ItemKey]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreItem_ItemKey]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreItem]'))
ALTER TABLE [dbo].[StoreItem]  WITH CHECK ADD  CONSTRAINT [FK__StoreItem_ItemKey] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #102 --- Mar 15 2018 10:32:12.9478231AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_Item_Key]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_Item_Key]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH CHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_Item_Key] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #101 --- Mar 15 2018 10:32:13.1314239AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocationItems_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocationItems]'))
ALTER TABLE [dbo].[InventoryLocationItems] DROP CONSTRAINT [FK_InventoryLocationItems_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocationItems_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocationItems]'))
ALTER TABLE [dbo].[InventoryLocationItems]  WITH CHECK ADD  CONSTRAINT [FK_InventoryLocationItems_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #100 --- Mar 15 2018 10:32:14.3170163AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_PriceChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[PriceChangeQueue]'))
ALTER TABLE [mammoth].[PriceChangeQueue] DROP CONSTRAINT [FK_PriceChangeQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[mammoth].[FK_PriceChangeQueue_Item]') AND parent_object_id = OBJECT_ID(N'[mammoth].[PriceChangeQueue]'))
ALTER TABLE [mammoth].[PriceChangeQueue]  WITH CHECK ADD  CONSTRAINT [FK_PriceChangeQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #99 --- Mar 15 2018 10:32:14.6324581AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaxOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaxOverride]'))
ALTER TABLE [dbo].[TaxOverride] DROP CONSTRAINT [FK_TaxOverride_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TaxOverride_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[TaxOverride]'))
ALTER TABLE [dbo].[TaxOverride]  WITH CHECK ADD  CONSTRAINT [FK_TaxOverride_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #98 --- Mar 15 2018 10:32:15.0689983AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue] DROP CONSTRAINT [FK_VendorItemDiscrepancyQueue_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorItemDiscrepancyQueue_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorItemDiscrepancyQueue]'))
ALTER TABLE [dbo].[VendorItemDiscrepancyQueue]  WITH CHECK ADD  CONSTRAINT [FK_VendorItemDiscrepancyQueue_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #97 --- Mar 15 2018 10:32:15.5104215AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key_365]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride365]'))
ALTER TABLE [dbo].[ItemOverride365] DROP CONSTRAINT [FK_ItemOverride_Item_Key_365]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Item_Key_365]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride365]'))
ALTER TABLE [dbo].[ItemOverride365]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Item_Key_365] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #96 --- Mar 15 2018 10:32:15.7955887AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail] DROP CONSTRAINT [FK_ItemUploadDetail_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail]  WITH CHECK ADD  CONSTRAINT [FK_ItemUploadDetail_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #95 --- Mar 15 2018 10:32:16.0094641AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand] DROP CONSTRAINT [FK_OnHand_Item]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_Item]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand]  WITH CHECK ADD  CONSTRAINT [FK_OnHand_Item] FOREIGN KEY([Item_Key])
REFERENCES [dbo].[Item] ([Item_Key])


GO
/******************************************************************************
		Altering Table #94 --- Mar 15 2018 10:32:18.6355415AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_RetailSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam] DROP CONSTRAINT [FK_DistSubTeam_RetailSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_RetailSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_DistSubTeam_RetailSubTeam] FOREIGN KEY([RetailSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #93 --- Mar 15 2018 10:32:18.7556633AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreSubteamDiscountException_SubteamNo]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubteamDiscountException]'))
ALTER TABLE [dbo].[StoreSubteamDiscountException] DROP CONSTRAINT [FK__StoreSubteamDiscountException_SubteamNo]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__StoreSubteamDiscountException_SubteamNo]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubteamDiscountException]'))
ALTER TABLE [dbo].[StoreSubteamDiscountException]  WITH CHECK ADD  CONSTRAINT [FK__StoreSubteamDiscountException_SubteamNo] FOREIGN KEY([Subteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #92 --- Mar 15 2018 10:32:18.9138725AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand] DROP CONSTRAINT [FK_OnHand_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OnHand_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OnHand]'))
ALTER TABLE [dbo].[OnHand]  WITH CHECK ADD  CONSTRAINT [FK_OnHand_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #91 --- Mar 15 2018 10:32:21.6805803AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue] DROP CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__SignQueue__SubTe__7D29F9E4]') AND parent_object_id = OBJECT_ID(N'[dbo].[SignQueue]'))
ALTER TABLE [dbo].[SignQueue]  WITH CHECK ADD  CONSTRAINT [FK__SignQueue__SubTe__7D29F9E4] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #90 --- Mar 15 2018 10:32:27.3663455AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__SubTe__248EAB1F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice] DROP CONSTRAINT [FK__OrderInvo__SubTe__248EAB1F]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__SubTe__248EAB1F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice]  WITH CHECK ADD  CONSTRAINT [FK__OrderInvo__SubTe__248EAB1F] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #89 --- Mar 15 2018 10:32:28.5490081AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSuppl__SubTe__2827047D]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSupply]'))
ALTER TABLE [dbo].[ZoneSupply] DROP CONSTRAINT [FK__ZoneSuppl__SubTe__2827047D]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSuppl__SubTe__2827047D]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSupply]'))
ALTER TABLE [dbo].[ZoneSupply]  WITH CHECK ADD  CONSTRAINT [FK__ZoneSuppl__SubTe__2827047D] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #88 --- Mar 15 2018 10:32:28.6359255AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam] DROP CONSTRAINT [FK_StoreSubTeam_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_StoreSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[StoreSubTeam]'))
ALTER TABLE [dbo].[StoreSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_StoreSubTeam_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #87 --- Mar 15 2018 10:32:28.7345621AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK_DeletedOrder_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrder_SubTeam] FOREIGN KEY([Transfer_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #86 --- Mar 15 2018 10:32:28.8263625AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK_DeletedOrder_SubTeam1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrder_SubTeam1] FOREIGN KEY([Transfer_To_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #85 --- Mar 15 2018 10:32:28.9142565AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder] DROP CONSTRAINT [FK_DeletedOrder_SupplyTransferToSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrder_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrder]'))
ALTER TABLE [dbo].[DeletedOrder]  WITH CHECK ADD  CONSTRAINT [FK_DeletedOrder_SupplyTransferToSubTeam] FOREIGN KEY([SupplyTransferToSubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #84 --- Mar 15 2018 10:32:29.9142949AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NoTagThresholdSubteamOverride_SubteamNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[NoTagThresholdSubteamOverride]'))
ALTER TABLE [dbo].[NoTagThresholdSubteamOverride] DROP CONSTRAINT [FK_NoTagThresholdSubteamOverride_SubteamNumber]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_NoTagThresholdSubteamOverride_SubteamNumber]') AND parent_object_id = OBJECT_ID(N'[dbo].[NoTagThresholdSubteamOverride]'))
ALTER TABLE [dbo].[NoTagThresholdSubteamOverride]  WITH CHECK ADD  CONSTRAINT [FK_NoTagThresholdSubteamOverride_SubteamNumber] FOREIGN KEY([SubteamNumber])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #83 --- Mar 15 2018 10:32:30.0031655AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMSubTeamInclude_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSubTeamInclude]'))
ALTER TABLE [dbo].[PMSubTeamInclude] DROP CONSTRAINT [FK_PMSubTeamInclude_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PMSubTeamInclude_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PMSubTeamInclude]'))
ALTER TABLE [dbo].[PMSubTeamInclude]  WITH CHECK ADD  CONSTRAINT [FK_PMSubTeamInclude_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #82 --- Mar 15 2018 10:32:30.0939893AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept] DROP CONSTRAINT [FK_Sales_SumBySubDept_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumBySubDept_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumBySubDept]'))
ALTER TABLE [dbo].[Sales_SumBySubDept]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumBySubDept_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #81 --- Mar 15 2018 10:32:30.3117711AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem] DROP CONSTRAINT [FK_Sales_SumByItem_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItem]'))
ALTER TABLE [dbo].[Sales_SumByItem]  WITH NOCHECK ADD  CONSTRAINT [FK_Sales_SumByItem_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #80 --- Mar 15 2018 10:32:30.3918523AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_Fact_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_Fact]'))
ALTER TABLE [dbo].[Sales_Fact] DROP CONSTRAINT [FK_Sales_Fact_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_Fact_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_Fact]'))
ALTER TABLE [dbo].[Sales_Fact]  WITH CHECK ADD  CONSTRAINT [FK_Sales_Fact_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #79 --- Mar 15 2018 10:33:57.5485193AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly] DROP CONSTRAINT [FK_Sales_SumByItem_SubTeam_Wkly]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Sales_SumByItem_SubTeam_Wkly]') AND parent_object_id = OBJECT_ID(N'[dbo].[Sales_SumByItemWkly]'))
ALTER TABLE [dbo].[Sales_SumByItemWkly]  WITH CHECK ADD  CONSTRAINT [FK_Sales_SumByItem_SubTeam_Wkly] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #78 --- Mar 15 2018 10:33:57.6715709AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__SubTeam_No__0BAD2365]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__SubTeam_No__0BAD2365]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__SubTeam_No__0BAD2365] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #77 --- Mar 15 2018 10:34:24.4421301AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price] DROP CONSTRAINT [FK_ExceptionSubTeam_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Price]'))
ALTER TABLE [dbo].[Price]  WITH CHECK ADD  CONSTRAINT [FK_ExceptionSubTeam_SubTeam] FOREIGN KEY([ExceptionSubteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #76 --- Mar 15 2018 10:34:25.0554349AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCategory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCategory]'))
ALTER TABLE [dbo].[ItemCategory] DROP CONSTRAINT [FK_ItemCategory_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemCategory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemCategory]'))
ALTER TABLE [dbo].[ItemCategory]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemCategory_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #75 --- Mar 15 2018 10:34:25.1130543AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK_Item_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Item_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK_Item_SubTeam] FOREIGN KEY([DistSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #74 --- Mar 15 2018 10:34:33.9337055AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserRoleSubTeamRla_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleSubTeamRla]'))
ALTER TABLE [dbo].[UserRoleSubTeamRla] DROP CONSTRAINT [FK_UserRoleSubTeamRla_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_UserRoleSubTeamRla_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[UserRoleSubTeamRla]'))
ALTER TABLE [dbo].[UserRoleSubTeamRla]  WITH CHECK ADD  CONSTRAINT [FK_UserRoleSubTeamRla_SubTeam] FOREIGN KEY([SubTeamID])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #73 --- Mar 15 2018 10:34:34.0323421AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ShelfTagRule_Subteam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ShelfTagRule]'))
ALTER TABLE [dbo].[ShelfTagRule] DROP CONSTRAINT [FK_ShelfTagRule_Subteam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ShelfTagRule_Subteam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ShelfTagRule]'))
ALTER TABLE [dbo].[ShelfTagRule]  WITH CHECK ADD  CONSTRAINT [FK_ShelfTagRule_Subteam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #72 --- Mar 15 2018 10:34:34.1173063AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocation_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocation]'))
ALTER TABLE [dbo].[InventoryLocation] DROP CONSTRAINT [FK_InventoryLocation_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_InventoryLocation_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[InventoryLocation]'))
ALTER TABLE [dbo].[InventoryLocation]  WITH CHECK ADD  CONSTRAINT [FK_InventoryLocation_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #71 --- Mar 15 2018 10:34:34.2178961AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSubTe__SubTe__0ACBABC0]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSubTeam]'))
ALTER TABLE [dbo].[ZoneSubTeam] DROP CONSTRAINT [FK__ZoneSubTe__SubTe__0ACBABC0]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ZoneSubTe__SubTe__0ACBABC0]') AND parent_object_id = OBJECT_ID(N'[dbo].[ZoneSubTeam]'))
ALTER TABLE [dbo].[ZoneSubTeam]  WITH CHECK ADD  CONSTRAINT [FK__ZoneSubTe__SubTe__0ACBABC0] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #70 --- Mar 15 2018 10:34:34.5089229AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam] FOREIGN KEY([Transfer_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #69 --- Mar 15 2018 10:34:44.3315657AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PromotionalOffer_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PromotionalOffer]'))
ALTER TABLE [dbo].[PromotionalOffer] DROP CONSTRAINT [FK_PromotionalOffer_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PromotionalOffer_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[PromotionalOffer]'))
ALTER TABLE [dbo].[PromotionalOffer]  WITH CHECK ADD  CONSTRAINT [FK_PromotionalOffer_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #68 --- Mar 15 2018 10:34:44.4204363AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SubTeam1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SubTeam1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SubTeam1] FOREIGN KEY([Transfer_To_SubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #67 --- Mar 15 2018 10:34:46.2349591AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory] DROP CONSTRAINT [FK_ItemHistory_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistory_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistory]'))
ALTER TABLE [dbo].[ItemHistory]  WITH CHECK ADD  CONSTRAINT [FK_ItemHistory_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #66 --- Mar 15 2018 10:36:20.9348845AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader] DROP CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderHeader_SupplyTransferToSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderHeader]'))
ALTER TABLE [dbo].[OrderHeader]  WITH CHECK ADD  CONSTRAINT [FK_OrderHeader_SupplyTransferToSubTeam] FOREIGN KEY([SupplyTransferToSubTeam])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #65 --- Mar 15 2018 10:36:30.3395425AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_SubTeam_No]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_SubTeam_No]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_SubTeam_No]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH CHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_SubTeam_No] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #64 --- Mar 15 2018 10:36:30.6862355AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__UsersSubT__SubTe__0F353234]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersSubTeam]'))
ALTER TABLE [dbo].[UsersSubTeam] DROP CONSTRAINT [FK__UsersSubT__SubTe__0F353234]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__UsersSubT__SubTe__0F353234]') AND parent_object_id = OBJECT_ID(N'[dbo].[UsersSubTeam]'))
ALTER TABLE [dbo].[UsersSubTeam]  WITH CHECK ADD  CONSTRAINT [FK__UsersSubT__SubTe__0F353234] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #63 --- Mar 15 2018 10:36:30.9733559AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Recipe_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Recipe]'))
ALTER TABLE [dbo].[Recipe] DROP CONSTRAINT [FK_Recipe_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Recipe_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[Recipe]'))
ALTER TABLE [dbo].[Recipe]  WITH CHECK ADD  CONSTRAINT [FK_Recipe_SubTeam] FOREIGN KEY([SubTeamID])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #62 --- Mar 15 2018 10:36:31.2331315AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam_Hist]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceHistory]'))
ALTER TABLE [dbo].[PriceHistory] DROP CONSTRAINT [FK_ExceptionSubTeam_SubTeam_Hist]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ExceptionSubTeam_SubTeam_Hist]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceHistory]'))
ALTER TABLE [dbo].[PriceHistory]  WITH CHECK ADD  CONSTRAINT [FK_ExceptionSubTeam_SubTeam_Hist] FOREIGN KEY([ExceptionSubteam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #61 --- Mar 15 2018 10:38:05.6586323AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue] DROP CONSTRAINT [FK_ItemHistoryOther_Hist_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemHistoryOther_Hist_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemHistoryQueue]'))
ALTER TABLE [dbo].[ItemHistoryQueue]  WITH CHECK ADD  CONSTRAINT [FK_ItemHistoryOther_Hist_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #60 --- Mar 15 2018 10:38:05.9447761AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MasterCycleCount_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[CycleCountMaster]'))
ALTER TABLE [dbo].[CycleCountMaster] DROP CONSTRAINT [FK_MasterCycleCount_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_MasterCycleCount_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[CycleCountMaster]'))
ALTER TABLE [dbo].[CycleCountMaster]  WITH CHECK ADD  CONSTRAINT [FK_MasterCycleCount_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #59 --- Mar 15 2018 10:38:06.7162901AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail] DROP CONSTRAINT [FK_ItemUploadDetail_SubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUploadDetail_SubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUploadDetail]'))
ALTER TABLE [dbo].[ItemUploadDetail]  WITH CHECK ADD  CONSTRAINT [FK_ItemUploadDetail_SubTeam] FOREIGN KEY([SubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #58 --- Mar 15 2018 10:38:06.8852419AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubTeam_CatalogSchedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[CatalogSchedule]'))
ALTER TABLE [dbo].[CatalogSchedule] DROP CONSTRAINT [FK_SubTeam_CatalogSchedule]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SubTeam_CatalogSchedule]') AND parent_object_id = OBJECT_ID(N'[dbo].[CatalogSchedule]'))
ALTER TABLE [dbo].[CatalogSchedule]  WITH CHECK ADD  CONSTRAINT [FK_SubTeam_CatalogSchedule] FOREIGN KEY([SubTeamNo])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #57 --- Mar 15 2018 10:38:07.1372047AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_DistSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam] DROP CONSTRAINT [FK_DistSubTeam_DistSubTeam]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DistSubTeam_DistSubTeam]') AND parent_object_id = OBJECT_ID(N'[dbo].[DistSubTeam]'))
ALTER TABLE [dbo].[DistSubTeam]  WITH CHECK ADD  CONSTRAINT [FK_DistSubTeam_DistSubTeam] FOREIGN KEY([DistSubTeam_No])
REFERENCES [dbo].[SubTeam] ([SubTeam_No])


GO
/******************************************************************************
		Altering Table #56 --- Mar 15 2018 10:38:07.3227587AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderTransmissionOverride_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderTransmissionOverride]'))
ALTER TABLE [dbo].[OrderTransmissionOverride] DROP CONSTRAINT [FK_OrderTransmissionOverride_OrderHeader]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderTransmissionOverride_OrderHeader]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderTransmissionOverride]'))
ALTER TABLE [dbo].[OrderTransmissionOverride]  WITH CHECK ADD  CONSTRAINT [FK_OrderTransmissionOverride_OrderHeader] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #55 --- Mar 15 2018 10:38:10.6773797AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #54 --- Mar 15 2018 10:38:11.0602069AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Order__1FEDB87C]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Order__1FEDB87C]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Order__1FEDB87C] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #53 --- Mar 15 2018 10:38:11.2633397AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party] DROP CONSTRAINT [FK_OrderInvoice_Freight3Party_OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderInvoice_Freight3Party_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice_Freight3Party]'))
ALTER TABLE [dbo].[OrderInvoice_Freight3Party]  WITH CHECK ADD  CONSTRAINT [FK_OrderInvoice_Freight3Party_OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #52 --- Mar 15 2018 10:38:11.4508469AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice] DROP CONSTRAINT [FK__OrderInvo__OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderInvo__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderInvoice]'))
ALTER TABLE [dbo].[OrderInvoice]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderInvo__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #51 --- Mar 15 2018 10:38:11.6149157AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__OrderHeader_ID__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__OrderHeader_ID__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__OrderHeader_ID__OLD__] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #50 --- Mar 15 2018 10:38:11.7994931AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList] DROP CONSTRAINT [FK__ReturnOrd__OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList]  WITH NOCHECK ADD  CONSTRAINT [FK__ReturnOrd__OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #49 --- Mar 15 2018 10:38:11.9811407AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__Retur__0731BBB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList] DROP CONSTRAINT [FK__ReturnOrd__Retur__0731BBB4]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__ReturnOrd__Retur__0731BBB4]') AND parent_object_id = OBJECT_ID(N'[dbo].[ReturnOrderList]'))
ALTER TABLE [dbo].[ReturnOrderList]  WITH NOCHECK ADD  CONSTRAINT [FK__ReturnOrd__Retur__0731BBB4] FOREIGN KEY([ReturnOrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #48 --- Mar 15 2018 10:38:12.2936527AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost] DROP CONSTRAINT [FK_SuspendedAvgCost_OrderHeader_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_SuspendedAvgCost_OrderHeader_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[SuspendedAvgCost]'))
ALTER TABLE [dbo].[SuspendedAvgCost]  WITH CHECK ADD  CONSTRAINT [FK_SuspendedAvgCost_OrderHeader_ID] FOREIGN KEY([OrderHeader_ID])
REFERENCES [dbo].[OrderHeader] ([OrderHeader_ID])


GO
/******************************************************************************
		Altering Table #47 --- Mar 15 2018 10:38:12.3463891AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_ItemUnit__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit__OLD__] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #46 --- Mar 15 2018 10:38:12.7712101AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_ItemUnit2__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit2__OLD__] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #45 --- Mar 15 2018 10:39:25.7212769AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RipeItemUnitExt_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[RipeItemUnitExt]'))
ALTER TABLE [dbo].[RipeItemUnitExt] DROP CONSTRAINT [FK_RipeItemUnitExt_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RipeItemUnitExt_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[RipeItemUnitExt]'))
ALTER TABLE [dbo].[RipeItemUnitExt]  WITH CHECK ADD  CONSTRAINT [FK_RipeItemUnitExt_ItemUnit] FOREIGN KEY([ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #44 --- Mar 15 2018 10:39:26.1304723AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK_OrderItem_ItemUnit1__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit1__OLD__] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #43 --- Mar 15 2018 10:39:26.3082135AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__CostU]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__CostU]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__CostU]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__CostU] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #42 --- Mar 15 2018 10:39:26.5777551AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Freig]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__Freig]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Freig]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__Freig] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #41 --- Mar 15 2018 10:39:26.7828411AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Handl]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__Handl]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Handl]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__Handl] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #40 --- Mar 15 2018 10:39:27.0787509AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__CostU__1A34DF26]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__CostU__1A34DF26] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #39 --- Mar 15 2018 10:39:27.2906731AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Freig__1B29035F]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Freig__1B29035F] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #38 --- Mar 15 2018 10:41:39.3816827AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Quant]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK__DeletedOrderItem__Quant]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__DeletedOrderItem__Quant]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__DeletedOrderItem__Quant] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #37 --- Mar 15 2018 10:41:39.6316923AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Handl__1C1D2798]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK__OrderItem__Handl__1C1D2798] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #36 --- Mar 15 2018 10:43:45.1941309AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK__OrderItem__Quant__21D600EE]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Quant__21D600EE] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #35 --- Mar 15 2018 10:43:45.2908143AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #34 --- Mar 15 2018 10:43:45.3992169AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_ItemUnit1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_DeletedOrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #33 --- Mar 15 2018 10:43:45.5076195AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem] DROP CONSTRAINT [FK_DeletedOrderItem_ItemUnit2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DeletedOrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[DeletedOrderItem]'))
ALTER TABLE [dbo].[DeletedOrderItem]  WITH CHECK ADD  CONSTRAINT [FK_DeletedOrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #32 --- Mar 15 2018 10:43:47.1502607AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #31 --- Mar 15 2018 10:43:47.2645229AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit1]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit1]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH NOCHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit1] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #30 --- Mar 15 2018 10:43:47.3836681AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Cost_Unit___1FB41C12]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Cost_Unit___1FB41C12]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Cost_Unit___1FB41C12] FOREIGN KEY([Cost_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #29 --- Mar 15 2018 10:43:47.6991099AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem] DROP CONSTRAINT [FK_OrderItem_ItemUnit2]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItem_ItemUnit2]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem]'))
ALTER TABLE [dbo].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ItemUnit2] FOREIGN KEY([InvoiceQuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #28 --- Mar 15 2018 10:46:09.4066997AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Distributi__1EBFF7D9]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Distributi__1EBFF7D9]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Distributi__1EBFF7D9] FOREIGN KEY([Distribution_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #27 --- Mar 15 2018 10:46:09.7084691AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Freight_Un__20A8404B]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Freight_Un__20A8404B]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Freight_Un__20A8404B] FOREIGN KEY([Freight_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #26 --- Mar 15 2018 10:46:10.0346535AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_CostUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory] DROP CONSTRAINT [FK_VendorCostHistory_ItemUnit_CostUnitID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_CostUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory]  WITH CHECK ADD  CONSTRAINT [FK_VendorCostHistory_ItemUnit_CostUnitID] FOREIGN KEY([CostUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #25 --- Mar 15 2018 10:47:49.4984337AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_FreightUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory] DROP CONSTRAINT [FK_VendorCostHistory_ItemUnit_FreightUnitID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_VendorCostHistory_ItemUnit_FreightUnitID]') AND parent_object_id = OBJECT_ID(N'[dbo].[VendorCostHistory]'))
ALTER TABLE [dbo].[VendorCostHistory]  WITH CHECK ADD  CONSTRAINT [FK_VendorCostHistory_ItemUnit_FreightUnitID] FOREIGN KEY([FreightUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #24 --- Mar 15 2018 10:49:16.7088137AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Package_Un__0E899010]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Package_Un__0E899010]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Package_Un__0E899010] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #23 --- Mar 15 2018 10:49:17.0213257AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Retail_Uni__1BE38B2E]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Retail_Uni__1BE38B2E] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #22 --- Mar 15 2018 10:49:17.3201653AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_RetailItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] DROP CONSTRAINT [FK_ItemUomOverride_RetailItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_RetailItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemUomOverride_RetailItemUnit] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #21 --- Mar 15 2018 10:49:17.4246615AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_ScaleItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride] DROP CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemUomOverride_ScaleItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemUomOverride]'))
ALTER TABLE [dbo].[ItemUomOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemUomOverride_ScaleItemUnit] FOREIGN KEY([Scale_ScaleUomUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #20 --- Mar 15 2018 10:49:17.4803277AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item] DROP CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__Item__Vendor_Uni__1DCBD3A0]') AND parent_object_id = OBJECT_ID(N'[dbo].[Item]'))
ALTER TABLE [dbo].[Item]  WITH CHECK ADD  CONSTRAINT [FK__Item__Vendor_Uni__1DCBD3A0] FOREIGN KEY([Vendor_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #19 --- Mar 15 2018 10:49:17.7283841AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Distribution_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Distribution_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Distribution_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Distribution_Unit_ID] FOREIGN KEY([Distribution_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #18 --- Mar 15 2018 10:49:17.8182313AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Package_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Package_Unit_ID] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #17 --- Mar 15 2018 10:49:17.8992891AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Retail_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Retail_Unit_ID] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #16 --- Mar 15 2018 10:49:17.9862065AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Vendor_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Vendor_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Vendor_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Vendor_Unit_ID] FOREIGN KEY([Vendor_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #15 --- Mar 15 2018 10:49:18.0760537AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Manufacturing_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride] DROP CONSTRAINT [FK_ItemOverride_Manufacturing_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemOverride_Manufacturing_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemOverride]'))
ALTER TABLE [dbo].[ItemOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemOverride_Manufacturing_Unit_ID] FOREIGN KEY([Manufacturing_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #14 --- Mar 15 2018 10:49:18.1668775AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_ScaleUOMUnit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride] DROP CONSTRAINT [FK_ItemScaleOverride_ScaleUOMUnit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScaleOverride_ScaleUOMUnit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScaleOverride]'))
ALTER TABLE [dbo].[ItemScaleOverride]  WITH CHECK ADD  CONSTRAINT [FK_ItemScaleOverride_ScaleUOMUnit_ID] FOREIGN KEY([Scale_ScaleUOMUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #13 --- Mar 15 2018 10:49:18.2567247AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale] DROP CONSTRAINT [FK_ItemScale_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemScale_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemScale]'))
ALTER TABLE [dbo].[ItemScale]  WITH CHECK ADD  CONSTRAINT [FK_ItemScale_ItemUnit] FOREIGN KEY([Scale_ScaleUOMUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #12 --- Mar 15 2018 10:49:18.3905189AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_1__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion] DROP CONSTRAINT [FK_ItemConversion_1__33]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_1__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemConversion_1__33] FOREIGN KEY([FromUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #11 --- Mar 15 2018 10:49:18.4872023AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue] DROP CONSTRAINT [FK_OrderItemQueue_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueue_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueue]'))
ALTER TABLE [dbo].[OrderItemQueue]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueue_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #10 --- Mar 15 2018 10:49:18.6805691AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_2__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion] DROP CONSTRAINT [FK_ItemConversion_2__33]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_ItemConversion_2__33]') AND parent_object_id = OBJECT_ID(N'[dbo].[ItemConversion]'))
ALTER TABLE [dbo].[ItemConversion]  WITH NOCHECK ADD  CONSTRAINT [FK_ItemConversion_2__33] FOREIGN KEY([ToUnit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #9 --- Mar 15 2018 10:49:18.7723695AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_Package_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Package_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_Package_Unit_ID] FOREIGN KEY([Package_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #8 --- Mar 15 2018 10:49:21.5156389AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak] DROP CONSTRAINT [FK_OrderItemQueueBak_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_OrderItemQueueBak_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItemQueueBak]'))
ALTER TABLE [dbo].[OrderItemQueueBak]  WITH CHECK ADD  CONSTRAINT [FK_OrderItemQueueBak_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #7 --- Mar 15 2018 10:49:21.6201351AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail] DROP CONSTRAINT [FK_PriceBatchDetail_Retail_Unit_ID]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_PriceBatchDetail_Retail_Unit_ID]') AND parent_object_id = OBJECT_ID(N'[dbo].[PriceBatchDetail]'))
ALTER TABLE [dbo].[PriceBatchDetail]  WITH CHECK ADD  CONSTRAINT [FK_PriceBatchDetail_Retail_Unit_ID] FOREIGN KEY([Retail_Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #6 --- Mar 15 2018 10:49:24.5040349AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__CostU__1A34DF26__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__CostU__1A34DF26__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__CostU__1A34DF26__OLD__] FOREIGN KEY([CostUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #5 --- Mar 15 2018 10:49:24.5948587AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__Freig__1B29035F__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Freig__1B29035F__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Freig__1B29035F__OLD__] FOREIGN KEY([FreightUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #4 --- Mar 15 2018 10:49:24.6651739AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__Handl__1C1D2798__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Handl__1C1D2798__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Handl__1C1D2798__OLD__] FOREIGN KEY([HandlingUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #3 --- Mar 15 2018 10:49:24.7149805AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo] DROP CONSTRAINT [FK_CompetitorImportInfo_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorImportInfo_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorImportInfo]'))
ALTER TABLE [dbo].[CompetitorImportInfo]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorImportInfo_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #2 --- Mar 15 2018 10:49:24.7794361AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__] DROP CONSTRAINT [FK__OrderItem__Quant__21D600EE__OLD__]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK__OrderItem__Quant__21D600EE__OLD__]') AND parent_object_id = OBJECT_ID(N'[dbo].[OrderItem__OLD__]'))
ALTER TABLE [dbo].[OrderItem__OLD__]  WITH NOCHECK ADD  CONSTRAINT [FK__OrderItem__Quant__21D600EE__OLD__] FOREIGN KEY([QuantityUnit])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
/******************************************************************************
		Altering Table #1 --- Mar 15 2018 10:49:25.0821821AM
******************************************************************************/
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice] DROP CONSTRAINT [FK_CompetitorPrice_ItemUnit]
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_CompetitorPrice_ItemUnit]') AND parent_object_id = OBJECT_ID(N'[dbo].[CompetitorPrice]'))
ALTER TABLE [dbo].[CompetitorPrice]  WITH CHECK ADD  CONSTRAINT [FK_CompetitorPrice_ItemUnit] FOREIGN KEY([Unit_ID])
REFERENCES [dbo].[ItemUnit] ([Unit_ID])


GO
