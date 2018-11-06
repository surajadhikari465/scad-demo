IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StoreOpsOrdersExport]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[StoreOpsOrdersExport]
GO

CREATE PROCEDURE [dbo].[StoreOpsOrdersExport]
AS
-- **************************************************************************
-- Procedure: StoreOpsOrdersExport()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure queues up orders to be picked up by StoreOps
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 07.23.2009	DS		xxxxx	Added Currency from OrderHeader to feed.
-- 07.27.2009	MU		xxxxx	use OrderHeader.PurchaseAccountsTotal for Invoice Cost when the order has been uploaded
-- 01.12.2011   DS		xxxxx	Delete script had been added in the configuration for items w/out businessunitid.  
--								I added a dummy businessunitid to deleted queue items so they would move over to store ops as they used to.
-- 08.17.2011	DN		xxxxx	Use uploaded cost value instead of the invoice cost to send to StoreOps.
-- 11.11.2011	MD		3475	fixed the issue introduced by TFS 1638. We need to send the PurchaseAccountsTotal to StoreOps after APUpload
--								as that number does not incldue the SAC Charges.
-- 12.27.2011	BBB		3744	calculation validation;
-- 11.07.2012   MZ		8331	Send refused POs to StoreOps as deleted POs
-- 04.11.2013   MZ      11957   Added ISNULL to set invoice cost to 0 for Free Fill POs that don't have invoice records.
-- 2013-05-27	KM		12423	Undo MZ 11957 because it's really a problem with the IRMA Service Library, not this procedure.
-- **************************************************************************

BEGIN
	SELECT
		OrderExportQueueID,
		OrderHeader.OrderHeader_ID, 
		[PS_Vendor_ID]				= LTRIM(RTRIM(V.PS_Vendor_ID)),
		[VendorBusinessUnit_ID]		= VBU.BusinessUnit_ID, 
		Transfer_PS_SubTeam_No,
		SBU.BusinessUnit_ID, 
		StoreSubTeam.PS_SubTeam_No, 
		SUM(CASE WHEN ISNULL(Sales_Account, '') <> '891000'
						  THEN CASE WHEN CloseDate IS NULL 
										  THEN 
											 CASE WHEN OrderHeader.OrderType_ID = 2 
												THEN
													ISNULL(LineItemCost,0) + ISNULL(LineItemFreight,0)
												ELSE
													ISNULL(LineItemCost,0) + ISNULL(LineItemFreight,0) + ISNULL(HandlingCharge, 0) * ISNULL(QuantityOrdered,0)
												END									
										  ELSE 
											 CASE WHEN OrderHeader.OrderType_ID = 2 
												THEN
													ISNULL(ReceivedItemCost,0) + ISNULL(ReceivedItemFreight,0) 
												ELSE
													ISNULL(ReceivedItemCost,0) + ISNULL(ReceivedItemFreight,0) + ISNULL(HandlingCharge, 0) * ISNULL(QuantityReceived,0)									
												END	
										  END
						   ELSE 0 END)  * CASE WHEN IsDestination = 0
														 THEN CASE WHEN OrderHeader.Return_Order = 1 THEN 1 ELSE -1 END
														 ELSE CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END
														 END  AS PurchaseAmt, 
		   MAX(OrderHeader.SentDate) SentDate, 
		   MAX(OrderHeader.Expected_Date) Expected_Date, 
		   MAX(OrderHeader.CloseDate) CloseDate, 
		   MAX(OrderItem.DateReceived) ReceivedDate,
		   MAX(OrderHeader.InvoiceDate) InvoiceDate, 
		   MAX(OrderHeader.InvoiceNumber) VendorOrder_ID, 
		   CASE WHEN OrderHeader.UploadedDate is not null 
		   THEN 
				CASE WHEN 0 < SUM(CASE WHEN ISNULL(Sales_Account, '') = '891000' THEN 1 ELSE 0 END) 
						THEN 
							0
						ELSE 
							OrderHeader.PurchaseAccountsTotal 
				END
		   ELSE
				((SELECT SUM(InvoiceCost + InvoiceFreight) FROM OrderInvoice (nolock) WHERE OrderInvoice.OrderHeader_ID = OrderHeader.OrderHeader_ID)
				- SUM(CASE WHEN ISNULL(Sales_Account, '') = '891000' 
						   THEN CASE WHEN CloseDate IS NULL 
								THEN 
									CASE WHEN OrderHeader.OrderType_ID = 2 
										THEN
											ISNULL(LineItemCost,0) + ISNULL(LineItemFreight,0)
										ELSE
											ISNULL(LineItemCost,0) + ISNULL(LineItemFreight,0) + ISNULL(HandlingCharge, 0) * ISNULL(QuantityOrdered,0)
										END									
								ELSE 
									CASE WHEN OrderHeader.OrderType_ID = 2 
										THEN
											ISNULL(ReceivedItemCost,0) + ISNULL(ReceivedItemFreight,0) 
										ELSE
											ISNULL(ReceivedItemCost,0) + ISNULL(ReceivedItemFreight,0) + ISNULL(HandlingCharge, 0) * ISNULL(QuantityReceived,0)									
										END	
								END
						   ELSE 0 END)) 
				* CASE WHEN IsDestination = 0
					THEN CASE WHEN OrderHeader.Return_Order = 1 THEN 1 ELSE -1 END
					ELSE CASE WHEN OrderHeader.Return_Order = 1 THEN -1 ELSE 1 END
				END 
		   END AS InvoiceCost, 
		   MAX(REPLACE(REPLACE(orderheader.OrderHeaderDesc,char(13),char(3)), '|', '')) as OrderHeaderDesc, 
		   CASE WHEN OrderHeader.Return_Order = 0 THEN 0 ELSE 1 END As Return_Order, 
		   CASE WHEN OrderHeader.ProductType_ID IN (2,3) 
				THEN 1 
				ELSE 0 
				END AS Package,
		   OrderHeader.OrderType_ID,
		   EO.QueueInsertedDate as UpdateDate,
		   OrderHeader.DVOOrderID,
		   0 AS IsDeleted		,
		   C.CurrencyCode 		
	FROM
		(-- External vendor orders going to retail stores
		SELECT OrderExportQueueID, QueueInsertedDate, NULL Transfer_PS_SubTeam_No, OH.OrderHeader_ID, OH.Vendor_ID, OH.ReceiveLocation_ID, OH.Transfer_To_SubTeam SubTeam_No, 1 IsDestination
		FROM 
			tmpOrderExport EO
		INNER JOIN
			dbo.OrderHeader OH (nolock)
			ON OH.OrderHeader_ID = EO.OrderHeader_ID
		INNER JOIN
			dbo.Vendor RcvV (nolock)
			ON RcvV.Vendor_ID = OH.ReceiveLocation_ID
		INNER JOIN
			Store RcvS (nolock)
			on RcvS.Store_no = RcvV.Store_no
		WHERE Transfer_SubTeam IS NULL
			AND (RcvS.WFM_Store = 1 OR RcvS.Mega_Store = 1 OR RcvS.Distribution_Center = 1 OR RcvS.Manufacturer = 1)
		UNION
		-- Internal orders for the vendor where the vendor is a retail store
		SELECT OrderExportQueueID, QueueInsertedDate, StoreSubTeam.PS_SubTeam_No Transfer_PS_SubTeam_No, OH.OrderHeader_ID, V.Vendor_ID, OH.Vendor_ID ReceiveLocation_ID, OH.Transfer_SubTeam SubTeam_No, 0 IsDestination
		FROM 
			tmpOrderExport EO
		INNER JOIN
			dbo.OrderHeader OH (nolock)
			ON OH.OrderHeader_ID = EO.OrderHeader_ID
		INNER JOIN
			dbo.Vendor V (nolock)
			ON OH.PurchaseLocation_ID = V.Vendor_ID
		INNER JOIN 
			dbo.store VendS (nolock) --"vendor" store
			ON VendS.Store_no = V.Store_no   
		INNER JOIN 
			StoreSubTeam (nolock)
			ON StoreSubTeam.Store_No = VendS.Store_No and OH.Transfer_To_Subteam = StoreSubteam.Subteam_No          
		WHERE Transfer_SubTeam IS NOT NULL
			AND (VendS.WFM_Store = 1 OR VendS.Mega_Store = 1 OR VendS.Distribution_Center = 1 OR VendS.Manufacturer = 1)
		UNION
		-- Internal orders for the receiving retail stores
		SELECT OrderExportQueueID, QueueInsertedDate, StoreSubteam.PS_Subteam_No Transfer_PS_SubTeam_No, OH.OrderHeader_ID, OH.Vendor_ID, OH.ReceiveLocation_ID, OH.Transfer_To_SubTeam SubTeam_No, 1 IsDestination
		FROM 
			tmpOrderExport EO
		INNER JOIN
			dbo.OrderHeader OH (nolock)
			ON OH.OrderHeader_ID = EO.OrderHeader_ID
		INNER JOIN
			dbo.Vendor RcvV (nolock)
			ON RcvV.Vendor_ID = OH.Vendor_ID
		INNER JOIN
			dbo.Store RcvS (nolock)--"receiving" store
			ON RcvS.Store_no = RcvV.Store_no 
		INNER JOIN 
			dbo.StoreSubTeam (nolock)
			ON StoreSubTeam.Store_No = RcvV.Store_No and OH.Transfer_Subteam = StoreSubteam.Subteam_No                          
		WHERE Transfer_SubTeam IS NOT NULL
			AND (RcvS.WFM_Store = 1 or RcvS.Mega_Store = 1 OR RcvS.Distribution_Center = 1 OR RcvS.Manufacturer = 1))
		EO
	INNER JOIN
		OrderHeader (nolock) 
		ON EO.OrderHeader_ID = OrderHeader.OrderHeader_ID AND OrderHeader.RefuseReceivingReasonID is NULL
	INNER JOIN
		dbo.OrderItem (NOLOCK)
		ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
	INNER JOIN
		dbo.Item (nolock)
		ON OrderItem.Item_Key = Item.Item_Key
	INNER JOIN
		dbo.Vendor SV (nolock)
		on SV.Vendor_ID = EO.ReceiveLocation_ID
	INNER JOIN
		dbo.Store SBU (nolock)
		on SBU.Store_no = SV.Store_no
	INNER JOIN
		dbo.Vendor V (nolock)
		ON V.Vendor_ID = EO.Vendor_ID
	INNER JOIN
		dbo.SubTeam (nolock)
		ON SubTeam.SubTeam_No = EO.SubTeam_No
	INNER JOIN 
		dbo.StoreSubTeam (nolock)
		ON StoreSubTeam.SubTeam_No = SubTeam.SubTeam_No AND SBU.Store_no = StoreSubTeam.Store_No
	LEFT JOIN
		dbo.Store VBU (nolock)
		ON V.Store_No = VBU.Store_No
	LEFT JOIN dbo.Currency C on C.CurrencyID = OrderHeader.CurrencyID

	GROUP BY OrderExportQueueID, OrderHeader.OrderHeader_ID, V.PS_Vendor_ID, 
			 Transfer_PS_SubTeam_No, VBU.BusinessUnit_ID, StoreSubTeam.PS_SubTeam_No, SBU.BusinessUnit_id, 
			 EO.SubTeam_No, OrderHeader.Return_Order, OrderHeader.ProductType_ID, IsDestination,
			 OrderHeader.OrderType_ID, EO.QueueInsertedDate, OrderHeader.DVOOrderID, C.CurrencyCode,
			 OrderHeader.UploadedDate, OrderHeader.PurchaseAccountsTotal 
	
	UNION

	SELECT 
		null as OrderExportQueueID,
		DQ.OrderHeader_ID, 
		null as PS_Vendor_ID,
		null as VendorBusinessUnit_ID, 
		null as Transfer_PS_SubTeam_No,
		9999 as BusinessUnit_ID, 
		null as PS_SubTeam_No, 
		null as PurchaseAmt, 
		null as SentDate, 
		null as Expected_Date, 
		null as CloseDate, 
		null as ReceivedDate,
		null as InvoiceDate, 
		null as VendorOrder_ID, 
		null as InvoiceCost, 
		null as OrderHeaderDesc, 
		null as Return_Order, 
		null as Package,
		null as OrderType_ID,
		null as UpdateDate,
		null as DVOOrderID,
		1 AS IsDeleted,
		null AS CurrencyCode		
	FROM
		OrderExportDeletedQueue DQ
	WHERE DQ.DeliveredToStoreOpsDate is null

	UNION

	SELECT 
		null as OrderExportQueueID,
		EO2.OrderHeader_ID, 
		null as PS_Vendor_ID,
		null as VendorBusinessUnit_ID, 
		null as Transfer_PS_SubTeam_No,
		9999 as BusinessUnit_ID, 
		null as PS_SubTeam_No, 
		null as PurchaseAmt, 
		null as SentDate, 
		null as Expected_Date, 
		null as CloseDate, 
		null as ReceivedDate,
		null as InvoiceDate, 
		null as VendorOrder_ID, 
		null as InvoiceCost, 
		null as OrderHeaderDesc, 
		null as Return_Order, 
		null as Package,
		null as OrderType_ID,
		null as UpdateDate,
		null as DVOOrderID,
		1 AS IsDeleted,
		null AS CurrencyCode		
	FROM
		tmpOrderExport EO2
	INNER JOIN
		OrderHeader (nolock) 
		ON EO2.OrderHeader_ID = OrderHeader.OrderHeader_ID AND OrderHeader.RefuseReceivingReasonID is NOT NULL
	WHERE EO2.DeliveredToStoreOpsDate is null
	ORDER BY OrderExportQueueID

END
GO