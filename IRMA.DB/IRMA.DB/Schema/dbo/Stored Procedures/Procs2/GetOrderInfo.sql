CREATE PROCEDURE [dbo].[GetOrderInfo]
	@OrderHeader_ID		int,
	@CreatedBy			int = 0
AS
   -- **************************************************************************
   -- Procedure: GetOrderInfo()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from Orders.vb
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 12/13/2010	BBB		13334	removed deprecated and unused code
   -- 1/18/2011		TTL		759		Added OH.POCostDate to result set.
   -- 7/20/2011		MD		2095	Added OH.ReasonCodeDetailID to result set.
   -- 08/26/2011    FA      2891    Added code to look for PO from external source
   -- 12/05/2011	BBB		3744	extension; coding standards; new columns; 
   --								new boolean logic for deprecated Last and User SPs;
   -- 2011/12/30	KM		3744	Replace [InvoiceAmount] aggregation with call to
   --								OrderInvoice.InvoiceTotalCost; remove deprecated
   --								internal variable @SACTotal;
   -- 2012/02/03    FA      4652    Made '@CreatedBy' parameter optional
   -- 2012/02/06    MZ      4633    Add a call to UpdateOrderHeaderCosts stored procedure to populate OrderedCost on the PO.
   -- 2012/05/02    MZ      5713    Add [IsSOGOrder] flag to be returned to identify a SOG order
   -- 2012/10/02	DF		8265	Add DSDOrder flag to be returned to identify a DSD order
   -- 2012/10/11	KM		8122	Select Vendor.AllowReceiveAll
   -- 2012/10/30	DF		8339	Added ReceivedDate to return the earliest date an OrderItem was received for this order.
   -- 2012/11/07    MZ      8331    Added oh.RefuseReceivingReasonID to the returned result
   -- 2012/11/07	BS		2752	Added oh.eInvoice_id, oh.VendorDoc_ID, oh.VendorDocDate, 
   --								oh.CurrencyID, oh.RefuseReceivingReasonID, v.EinvoiceRequired,
   --								ov.InvoiceCost, ov.InvoiceFreight, & OrderFreight.
   --								These were all added for Service Library purposes so that GetOrderStatus
   --								and GetOrderInvoice did not need to be added to Service Library separately   
   -- 11/07/2012	AB      8312	Added search for deleted orders
   -- 02/01/2013	BBB		9715	Added additional (nolock) hints to select statements to alleviate persistent blocking
   -- 03/12/2013	FA		8325	Added TotalRefused column in the SELECT
   -- 06/25/2013	td		12063	added v.AllowBarcodePOReport flag  
   -- **************************************************************************
BEGIN
	SET NOCOUNT ON
	-- Create  table variable
	DEClARE @Order TABLE
	(
		OrderHeader_ID int,
		Temperature tinyint,
		QuantityDiscount decimal(9, 2),
		Accounting_In_DateStamp smalldatetime,
		OrderDate smalldatetime,
		CloseDate datetime,
		SentDate smalldatetime,
		Expected_Date smalldatetime,
		InvoiceDate smalldatetime,
		UploadedDate smalldatetime, 
		ApprovedDate smalldatetime,
		OrderType_ID int, 
		ProductType_ID int, 
		CreatedBy int, 
		Transfer_SubTeam int, 
		Transfer_To_SubTeam int, 
		Vendor_ID int, 
		ReceiveLocation_ID int, 
		DiscountType int, 
		PurchaseLocation_ID int, 
		Fax_Order bit, 
		Email_Order bit, 
		Electronic_Order bit, 
		Sent bit, 
		Return_Order bit, 
		OriginalCloseDate datetime, 
		User_ID int, 
		InvoiceNumber varchar(20), 
		OverrideTransmissionMethod bit, 
		IsDropShipment bit, 
		Freight3Party_OrderCost smallmoney,
		AdjustedReceivedCost money, 
		OriginalReceivedCost money, 
		APUploadedCost money, 
		POCostDate datetime, 
		ReasonCodeDetailID int,
		IsDeleted bit,
		DSDOrder bit,
		ClosedBy int,
		SupplyTransferToSubTeam int,
		ApprovedBy int,
		RecvLog_No int,
		WarehouseSent bit,
		WarehouseSentDate smalldatetime,
		OrderHeaderDesc varchar(4000),
		AccountingUploadDate datetime,
		OrderedCost money,
		WarehouseCancelled datetime,
		PayByAgreedCost bit,
		OrderExternalSourceID int,
		ReturnOrderHeader_ID int,
		InvoiceTotalCost money,
		PartialShipment bit,
		DeletedDate smalldatetime,
		UserName varchar(25),
		ReasonCodeDesc varchar(50),
		RefuseReceivingReasonID int,
		eInvoice_Id int,
		VendorDoc_ID varchar(16),
		VendorDocDate smalldatetime,
		CurrencyID int,
		InvoiceCost money,
		InvoiceFreight money,
		TotalRefused money
	 )
 

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
	DECLARE @SOG_OrderExternalSourceID int
	
    SELECT @SOG_OrderExternalSourceID = ID FROM OrderExternalSource WHERE Description = 'SOG'

	IF @CreatedBy <> 0
		SET @OrderHeader_ID = ISNULL((SELECT MAX(OrderHeader_ID) FROM OrderHeader (nolock) WHERE CreatedBy = @CreatedBy), 0)

	IF @OrderHeader_ID = 0
		SET @OrderHeader_ID = (SELECT MAX(OrderHeader_ID) FROM OrderHeader (nolock))

	IF (SELECT OrderedCost FROM OrderHeader WHERE OrderHeader_ID = @OrderHeader_ID)	IS NULL
		EXEC UpdateOrderHeaderCosts @OrderHeader_ID
--populate 	table variable	
		INSERT INTO @Order
SELECT OrderHeader_ID, Temperature, QuantityDiscount, Accounting_In_DateStamp, OrderDate, CloseDate, SentDate, Expected_Date, InvoiceDate, UploadedDate, 
               ApprovedDate, OrderType_ID, ProductType_ID, CreatedBy, Transfer_SubTeam, Transfer_To_SubTeam, Vendor_ID, ReceiveLocation_ID, DiscountType, 
               PurchaseLocation_ID, Fax_Order, Email_Order, Electronic_Order, Sent, Return_Order, OriginalCloseDate, do.User_ID, InvoiceNumber, OverrideTransmissionMethod, 
               IsDropShipment, Freight3Party_OrderCost, AdjustedReceivedCost, OriginalReceivedCost, APUploadedCost, POCostDate, do.ReasonCodeDetailID,1 IsDeleted,DSDOrder,
               ClosedBy,SupplyTransferToSubTeam,ApprovedBy,RecvLog_No,WarehouseSent,WarehouseSentDate,OrderHeaderDesc,AccountingUploadDate,OrderedCost,WarehouseCancelled,
               PayByAgreedCost,OrderExternalSourceID,ReturnOrderHeader_ID,InvoiceTotalCost,PartialShipment,DeleteDate,UserName,ReasonCodeDesc,RefuseReceivingReasonID,
               eInvoice_Id, VendorDoc_ID, VendorDocDate, CurrencyID, InvoiceCost ,InvoiceFreight, TotalRefused 
FROM  dbo.DeletedOrder (nolock) do
LEFT OUTER JOIN	Users			(nolock) 		ON	do.User_ID = Users.User_ID
LEFT OUTER JOIN	ReasonCodeDetail	(nolock)  rcd		ON	do.DeletedReason = rcd.ReasonCodeDetailID
 where OrderHeader_ID = @OrderHeader_ID
UNION ALL
SELECT OrderHeader_ID, Temperature, QuantityDiscount, Accounting_In_DateStamp, OrderDate, CloseDate, SentDate, Expected_Date, InvoiceDate, UploadedDate, 
               ApprovedDate, OrderType_ID, ProductType_ID, CreatedBy, Transfer_SubTeam, Transfer_To_SubTeam, Vendor_ID, ReceiveLocation_ID, DiscountType, 
               PurchaseLocation_ID, Fax_Order, Email_Order, Electronic_Order, Sent, Return_Order, OriginalCloseDate, User_ID, InvoiceNumber, OverrideTransmissionMethod, 
               IsDropShipment, Freight3Party_OrderCost, AdjustedReceivedCost, OriginalReceivedCost, APUploadedCost, POCostDate, ReasonCodeDetailID,0 IsDeleted,DSDOrder,
               ClosedBy,SupplyTransferToSubTeam,ApprovedBy,RecvLog_No,WarehouseSent,WarehouseSentDate,OrderHeaderDesc,AccountingUploadDate,OrderedCost,WarehouseCancelled,
               PayByAgreedCost,OrderExternalSourceID,null,null,PartialShipment,null,null,null,RefuseReceivingReasonID,
               eInvoice_Id, VendorDoc_ID, VendorDocDate, CurrencyID,null, null, TotalRefused
FROM  dbo.OrderHeader (nolock) where OrderHeader_ID = @OrderHeader_ID
	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT
		oh.OrderHeader_ID, 
		oh.Temperature, 	
		oh.Accounting_In_DateStamp,  
		v.CompanyName, 
		[CreatedByName]				=	u.UserName, 
		st.SubTeam_Name, 
		[Transfer_To_SubTeamName]	=	stt.SubTeam_Name,  
		oh.OrderDate,
		oh.CloseDate, 
		oh.SentDate, 
		oh.Expected_Date, 
		oh.InvoiceDate,   
		oh.UploadedDate, 
		oh.ApprovedDate, 
		oh.OrderType_ID, 
		[ProductType_ID]				= ISNULL(oh.ProductType_ID, 1),   
		oh.CreatedBy, 
		oh.Transfer_SubTeam, 
		oh.Transfer_To_SubTeam,   
		oh.Vendor_ID, 
		[Vendor_Store_No]				= ISNULL(v.Store_No, 0), 		
		[PurchaseLocation_ID]		    = ISNULL(oh.PurchaseLocation_ID,0),   
		oh.ReceiveLocation_ID, 
		oh.Fax_Order, 
		oh.Email_Order, 
		oh.Electronic_Order,
		oh.Sent, 
		oh.Return_Order,   
		oh.OriginalCloseDate,
		oh.User_ID, 
		oh.InvoiceNumber, 
		[ReturnOrder_ID]	=				CASE oh.IsDeleted
											WHEN 0 THEN rf.ReturnOrderHeader_ID 
											ELSE oh.ReturnOrderHeader_ID
											END,  
		oh.OverrideTransmissionMethod,  
		oh.isDropShipment, 
		[OriginalOrder_ID]			=	rt.OrderHeader_ID, 
		vr.Store_No,
		v.POTransmissionTypeID, 
		[WFM_Store]					=	ISNULL(sv.WFM_Store, 0),  
		[HFM_Store]					=	ISNULL(sv.MEGA_Store, 0),  
		[Store_Vend]				=	ISNULL(CASE WHEN sv.WFM_Store = 1 OR sv.Mega_Store = 1 THEN 1 ELSE 0 END, 0),
		RecvLog_No, 
		WarehouseSent, 
		WarehouseSentDate,   
		[EXEWarehouse]				=	ISNULL(sv.EXEWarehouse, sr.EXEWarehouse),  
		[From_SubTeam_Unrestricted]	=	CASE   
											WHEN ((st.SubTeamType_ID = 2)   -- Manufacturing  
											  OR (st.SubTeamType_ID = 3) -- RetailManufacturing  
											  OR (st.SubTeamType_ID = 4) -- Expense  
											  ) THEN 1 -- Unrestricted  
											ELSE 0 -- Restricted to retail subteam  
										END,  
		[To_SubTeam_Unrestricted]	=	CASE   
											WHEN ((stt.SubTeamType_ID = 2)   -- Manufacturing  
											  OR (stt.SubTeamType_ID = 3) -- RetailManufacturing  
											  OR (stt.SubTeamType_ID = 4) -- Expense  
											  ) THEN 1 -- Unrestricted  
											ELSE 0 -- Restricted to retail subteam  
										END,  
		[ItemsReceived]				=	CASE oh.IsDeleted
												    WHEN 0 THEN(SELECT COUNT(*) FROM OrderItem (nolock) WHERE OrderHeader_ID = oh.OrderHeader_ID AND DateReceived IS NOT NULL)  
													ELSE (SELECT COUNT(*) FROM DeletedOrderItem (nolock) WHERE OrderHeader_ID = oh.OrderHeader_ID AND DateReceived IS NOT NULL)
										END,  
		[OrderEnd]					=	CASE 
											WHEN oh.OrderType_ID = 2 -- Distribution  
											THEN CASE WHEN oh.Transfer_SubTeam = oh.Transfer_To_SubTeam   
													THEN ISNULL(CONVERT(varchar(255), zst.OrderEnd, 108), '0') -- not tranfer order  
													ELSE ISNULL(CONVERT(varchar(255), zst.OrderEndTransfers, 108), '0') -- is transfer order  
												 END  				 
											ELSE '0' 
										END,   
		[CurrSysTime]				=	CONVERT(varchar(255), GETDATE(), 108),  
		[Distribution_Center]		=	ISNULL(sv.Distribution_Center, 0),   
		[ReceivingStore_Distribution_Center]	=	ISNULL(sr.Distribution_Center, 0),   
		[Manufacturer]				=	ISNULL(sv.Manufacturer,0),   
		[WFM]						=	ISNULL(v.WFM, 0),  
		[IsEXEDistributed]			=	CASE oh.IsDeleted
												    WHEN 0 THEN(SELECT COUNT(*) FROM OrderItem (nolock) INNER JOIN Item (nolock) ON Item.Item_Key = OrderItem.Item_Key WHERE OrderHeader_ID = oh.OrderHeader_ID AND EXEDistributed = 1) 
													ELSE (SELECT COUNT(*) FROM DeletedOrderItem (nolock) INNER JOIN Item (nolock) ON Item.Item_Key = DeletedOrderItem.Item_Key WHERE OrderHeader_ID = oh.OrderHeader_ID AND EXEDistributed = 1)
										END,
		--,   
		[InvoiceAmount]				=	CASE oh.IsDeleted
											WHEN 0 THEN ISNULL(ov.InvoiceTotalCost, 0)
											ELSE ISNULL(oh.InvoiceTotalCost, 0)
										END, 
		
		[ClosedByUserName]			=	ISNULL(uc.UserName, ''), 
		[ApprovedByUserName]		=	ISNULL(ua.UserName, ''),
		[Freight3Party_OrderCost]	=	ISNULL(oh.Freight3Party_OrderCost, 0), 
		[PSVendorID]				=	v.PS_Vendor_ID,
		[ShipToStoreNo]				=	vp.Store_No,
		[BuyerName]					=	u.FullName,
		[BuyerEmail]				=	ISNULL(u.EMail, ''),
		[Notes]						=	ISNULL(oh.OrderHeaderDesc, ''),
		[DiscountAmount]			=	ISNULL(oh.QuantityDiscount,0),
		[AllowanceDiscountAmount]	=	ISNULL(oh.QuantityDiscount,0),
		[ISAReceiverQualifier]		=	NULL,
		[ISAReceiverID]				=	NULL,
		[Store_Phone]				=	ISNULL(vp.Phone, ''),
		[DUNSPlusFour]				=	NULL,
		[CostEffectiveDate]			=	NULL,
		[IsDropShipment]			=	NULL,
		[QuantityDiscount]		    =	ISNULL(oh.QuantityDiscount,0),
		[DiscountType]				=	ISNULL(oh.DiscountType,0),
		[StoreCompanyName]			=	vr.CompanyName,
		[ShipToStoreCompanyName]	=	vp.CompanyName,
		[IsVendorExternal]			=	CASE WHEN v.Customer = 0 AND v.InternalCustomer = 0 THEN 1 ELSE 0 END,
		[SupplyType_SubTeamName]	=	sts.SubTeam_Name,
		oh.AccountingUploadDate,  
		[LineItemFreight]			=	ISNULL(SUM(oi.LineItemFreight), 0),
		oh.OrderedCost,
		[AdjustedReceivedCost]		=	ISNULL(oh.AdjustedReceivedCost, 0),
		[OriginalReceivedCost]		=	ISNULL(oh.OriginalReceivedCost, 0),
		[UploadedCost]				=	ISNULL(oh.APUploadedCost, 0),
		WarehouseCancelled, 
		PayByAgreedCost,
		TotalHandlingCharge			=	ISNULL(SUM(oi.HandlingCharge * oi.QuantityOrdered), 0),
		POCostDate					=	oh.POCostDate,
		ReasonCodeDetailID			=	oh.ReasonCodeDetailID,
        [IsSOGOrder]                = 	CASE WHEN ISNULL(oh.OrderExternalSourceID,0) = @SOG_OrderExternalSourceID THEN 1 ELSE 0 END,
        oh.DSDOrder,
		v.AllowReceiveAll,
		[ReceivedDate]				=	CASE oh.IsDeleted
												    WHEN 0 THEN MIN(oi.DateReceived)
													ELSE MIN(doi.DateReceived)
										END,
		IsDeleted,
		DeletedDate ,
		oh.UserName ,
		ReasonCodeDesc ,
		oh.eInvoice_Id,
		oh.VendorDoc_ID,
		oh.VendorDocDate,
		oh.CurrencyID,
		oh.RefuseReceivingReasonID,
		v.EinvoiceRequired,
		oh.PartialShipment,
		InvoiceCost					=	CASE oh.IsDeleted
											WHEN 0 THEN ov.InvoiceCost
											ELSE oh.InvoiceCost
										END,
		InvoiceFreight				=	CASE oh.IsDeleted
											WHEN 0 THEN ov.InvoiceFreight
											ELSE oh.InvoiceFreight
										END,
		[OrderFreight]				=	CASE oh.IsDeleted
											WHEN 0 THEN SUM(oi.ReceivedItemFreight)
											ELSE SUM(doi.ReceivedItemFreight)
										END,
		oh.TotalRefused,
		v.AllowBarcodePOReport
		
	FROM 
		(select top 1 * from @Order)		oh
		LEFT JOIN	Vendor			(nolock) v		ON	oh.Vendor_ID				= v.Vendor_ID
		LEFT JOIN	Users			(nolock) u		ON	oh.CreatedBy				= u.User_ID
		LEFT JOIN	Vendor			(nolock) vr		ON	oh.ReceiveLocation_ID		= vr.Vendor_ID
		LEFT JOIN	Vendor			(nolock) vp		ON	oh.PurchaseLocation_ID		= vp.Vendor_ID
		LEFT JOIN	OrderItem		(nolock) oi		ON	oh.OrderHeader_ID			= oi.OrderHeader_ID
		LEFT JOIN	DeletedOrderItem(nolock) doi	ON	oh.OrderHeader_ID			= doi.OrderHeader_ID
		LEFT JOIN	ReturnOrderList (nolock) rf		ON	oh.OrderHeader_ID			= rf.OrderHeader_ID
		LEFT JOIN	ReturnOrderList (nolock) rt		ON	oh.OrderHeader_ID			= rt.ReturnOrderHeader_ID
		LEFT JOIN	SubTeam			(nolock) st		ON	oh.Transfer_SubTeam			= st.SubTeam_No
		LEFT JOIN	SubTeam			(nolock) stt	ON	oh.Transfer_To_SubTeam		= stt.SubTeam_No
		LEFT JOIN	SubTeam			(nolock) sts	ON	oh.SupplyTransferToSubTeam	= sts.SubTeam_No
		LEFT JOIN	Store			(nolock) sv		ON	v.Store_No					= sv.Store_No
		LEFT JOIN	Store			(nolock) sr		ON	vr.Store_No					= sr.Store_No
		LEFT JOIN	ZoneSubTeam		(nolock) zst	ON	sr.Zone_ID					= zst.Zone_ID 
													AND sv.Store_No					= zst.Supplier_Store_No
													AND oh.Transfer_SubTeam			= zst.SubTeam_No
		LEFT JOIN	OrderInvoice	(nolock) ov		ON	oh.OrderHeader_ID			= ov.OrderHeader_ID  
		LEFT JOIN	Users			(nolock) uc		ON	oh.ClosedBy					= uc.User_ID
		LEFT JOIN	Users			(nolock) ua		ON	oh.ApprovedBy				= ua.User_ID
	WHERE 
		oh.OrderHeader_ID		=	@OrderHeader_ID
	GROUP BY
		oh.OrderHeader_ID, 
		oh.Temperature, 
		oh.QuantityDiscount, 
		oh.Accounting_In_DateStamp,  
		v.CompanyName, 
		u.UserName, 
		st.SubTeam_Name, 
		stt.SubTeam_Name,  
		oh.OrderDate,
		oh.CloseDate, 
		oh.SentDate, 
		oh.Expected_Date, 
		oh.InvoiceDate,   
		oh.UploadedDate, 
		oh.ApprovedDate, 
		oh.OrderType_ID, 
		oh.ProductType_ID,   
		oh.CreatedBy, 
		oh.Transfer_SubTeam, 
		oh.Transfer_To_SubTeam,   
		oh.Vendor_ID, 
		v.Store_No, 
		oh.DiscountType, 
		oh.PurchaseLocation_ID,   
		oh.ReceiveLocation_ID, 
		oh.Fax_Order, 
		oh.Email_Order, 
		oh.Electronic_Order,
		oh.Sent, 
		oh.Return_Order,   
		oh.OriginalCloseDate,
		oh.User_ID, 
		oh.InvoiceNumber, 
		rf.ReturnOrderHeader_ID,  
		oh.OverrideTransmissionMethod,  
		oh.isDropShipment, 
		rt.OrderHeader_ID, 
		vr.Store_No,
		v.POTransmissionTypeID, 
		sv.WFM_Store,  
		sv.MEGA_Store,  
		RecvLog_No, 
		WarehouseSent, 
		WarehouseSentDate,   
		sv.EXEWarehouse, 
		sr.EXEWarehouse,  
		st.SubTeamType_ID,  
		stt.SubTeamType_ID,  
		oh.OrderType_ID,
		zst.OrderEnd,
		zst.OrderEndTransfers,   
		sv.Distribution_Center,   
		sr.Distribution_Center,   
		sv.Manufacturer,   
		v.WFM,  
		uc.UserName, 
		ua.UserName,
		oh.Freight3Party_OrderCost, 
		v.PS_Vendor_ID,
		vp.Store_No,
		u.FullName,
		u.EMail,
		oh.OrderHeaderDesc,
		oh.QuantityDiscount,
		vp.Phone,
		vr.CompanyName,
		vp.CompanyName,
		v.Customer,
		v.InternalCustomer,
		sts.SubTeam_Name,
		oh.AccountingUploadDate,  
		oh.OrderedCost,
		oh.AdjustedReceivedCost,
		oh.OriginalReceivedCost,
		oh.APUploadedCost,
		WarehouseCancelled, 
		PayByAgreedCost,
		oh.POCostDate,
		oh.ReasonCodeDetailID,
		ov.InvoiceTotalCost,
		oh.OrderExternalSourceID,
		oh.DSDOrder,
		v.AllowReceiveAll,
		oh.IsDeleted,
		oh.ReturnOrderHeader_ID,
		oh.InvoiceTotalCost,
		DeletedDate ,
		oh.UserName ,
		ReasonCodeDesc ,
		oh.RefuseReceivingReasonID,
        eInvoice_Id,
        VendorDoc_ID,
        VendorDocDate,
        oh.CurrencyID,
        v.EinvoiceRequired,
		ov.InvoiceCost,
		ov.InvoiceFreight,
		oh.InvoiceCost,
		oh.InvoiceFreight,
		oh.PartialShipment,
		oh.TotalRefused,
		v.AllowBarcodePOReport
	SET NOCOUNT OFF
END 
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfo] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfo] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfo] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetOrderInfo] TO [IRMAReportsRole]
    AS [dbo];

