/****** Object:  Trigger [OrderHeaderUpdate]    Script Date: 05/07/2008 09:01:43 ******/
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[OrderHeaderUpdate]'))
DROP TRIGGER [dbo].[OrderHeaderUpdate]
GO
/****** Object:  Trigger [OrderHeaderUpdate]    Script Date: 05/07/2008 09:01:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Trigger [OrderHeaderUpdate] 
ON dbo.OrderHeader
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
					OR (ISNULL(Inserted.OrderHeaderDesc, '') <> ISNULL(Deleted.OrderHeaderDesc, ''))
					OR (Inserted.Return_Order <> Deleted.Return_Order)
					OR (ISNULL(Inserted.InvoiceNumber, '') <> ISNULL(Deleted.InvoiceNumber, ''))
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

	IF @Error_No <> 0
	BEGIN
		DECLARE @Severity smallint
		SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
		RAISERROR ('OrderHeaderUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
	END
END

GO