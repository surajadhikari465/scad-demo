IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetAllSuspendedOrderSearch]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetAllSuspendedOrderSearch]
GO

CREATE PROCEDURE [dbo].[GetAllSuspendedOrderSearch]
	@OrderHeader_ID					int,
	@OrderInvoice_ControlGroup_ID	int,
	@Vendor_ID						int,
	@Vendor_Key						varchar(10),
	@InvoiceNumber					varchar(16),
	@InvoiceDateStart				smalldatetime,	
	@InvoiceDateEnd					smalldatetime,	
	@OrderDateStart					smalldatetime,
	@OrderDateEnd					smalldatetime,	
	@StoreList						varchar(MAX),
	@EInvoiceOnly					bit,
	@Identifier 					varchar(15),
	@VIN    					    varchar(15)
AS 
-- **************************************************************************************************************************
-- Procedure: GetAllSuspendedOrderSearch()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from InvoiceMatching to return multiple result sets
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/17/2011	BBB   	1600	Updated for code standards and readability
-- 03/18/2011	BBB   	1600	Parameter sniffing adjustment;
-- 04/07/2011	DBS   	1799	Adjusted cost sum so the invoice cost doesn't get multiplied by the number of invoice items;
-- 07/24/2011   FA      2408    Fixed the calculation of doctype 'Other/None' flag
-- 2011/12/14	KM		3744	Replaced SUM(oi.ReceivedItemCost) with call to oh.AdjustedReceivedCost
-- 2011/12/16	KM		3744	Added oh.AdjustedReceivedCost to GROUP BY clause
-- 2012/10/10	HK		7419	Added select oh.DSDorder
-- 10/24/2012   FA      7552    Modifed date range filters for optimization 
-- 10/29/2012	HK		8327	Changed end date to from 12/31/2999 to 12/31/2078. 2999 throw exceptions
-- 11/05/2012	HK		8329	Add partial shippment in select column
-- 11/28/2012	HK		8329	Add oh.DSDOrder=1, so it will show in spot tool
-- 12/13/2012   FA      9462    Modified the logic for DocTypeOther for DSD orders
-- 1/3/2013     HK      9695    Add PO with invoiceDate is null will be pulled out 
-- 1/29/2013    FA      9716    Added eInvoiceRequired column
-- 2/21/2013    FA      9402    Added SourcePONumber column
-- 03/11/2013	FA		8325	Added Total Refused column
-- **************************************************************************************************************************
BEGIN
	SET NOCOUNT ON;

	-- **************************************************************************
	-- Added in local variables to eliminate parameter sniffing causing issues on some DB servers
	-- **************************************************************************
	declare @LocalOrderHeader_ID int
	set @LocalOrderHeader_ID = @OrderHeader_ID
	
	declare @LocalOrderInvoice_ControlGroup_ID int
	Set @LocalOrderInvoice_ControlGroup_ID = @OrderInvoice_ControlGroup_ID
	
	declare @LocalVendor_ID int
	set @LocalVendor_ID = @Vendor_ID
		
	declare @LocalVendor_Key varchar(10)
	Set @LocalVendor_Key = @Vendor_Key
		
	declare @LocalInvoiceNumber varchar(16)
	set @LocalInvoiceNumber = @InvoiceNumber

	declare @LocalInvoiceDateStart smalldatetime	
	set @LocalInvoiceDateStart = ISNULL(@InvoiceDateStart, '1/1/1900')
	
	declare @LocalInvoiceDateEnd smalldatetime	
	set @LocalInvoiceDateEnd = ISNULL(@InvoiceDateEnd, '12/31/2078')

	declare @LocalOrderDateStart smalldatetime
	set @LocalOrderDateStart = ISNULL(@OrderDateStart, '1/1/1900')

	declare @LocalOrderDateEnd smalldatetime
	set @LocalOrderDateEnd = ISNULL(@OrderDateEnd, '12/31/2078')	

	declare @LocalStoreList varchar(MAX)
	set @LocalStoreList = @StoreList
		
	declare @LocalEInvoiceOnly bit	
	set @LocalEInvoiceOnly = @EInvoiceOnly
	
	declare @LocalIdentifier varchar(15)
	set @LocalIdentifier = @Identifier
	
	declare @LocalVIN varchar(15)
	set @LocalVIN = @VIN

	--**************************************************************************
	--Set variables
	--**************************************************************************
	IF @OrderDateStart is NULL and @OrderDateEnd is null
		BEGIN
			set @LocalOrderDateEnd = GETDATE() 
			set @LocalOrderDateStart = DateADD(day, -60, GETDATE())
		END 
	

	--**************************************************************************
	--Result Set #1 Orders with Invoice Data
	--**************************************************************************
	SELECT
		[OrderHeader_ID]			=	oh.OrderHeader_ID,
		[SourcePONumber]            =	CONVERT(varchar(15), ISNULL(oh.OrderExternalSourceOrderID, oh.OrderHeader_ID)) + ' - (' + ISNULL(os.Description, 'IRMA') + ')',
		[CloseDate]					=	oh.CloseDate,
		[VStoreName]				=	vs.CompanyName,
		[SubTeamName]				=	st.SubTeam_Name,
		[GLAccount]					=	CASE oh.ProductType_ID
											WHEN 1 THEN 
												ISNULL(CONVERT(varchar(6), st.GLPurchaseAcct), '500000')
											WHEN 2 THEN 
												ISNULL(CONVERT(varchar(6), st.GLPackagingAcct), '510000')
											WHEN 3 THEN 
												ISNULL(CONVERT(varchar(6), st.GLSuppliesAcct), '800000')
											ELSE 
												'0' 
										END,
		[Vendor_ID]					=	oh.Vendor_ID,
		[VendorName]				=	V.CompanyName,					    	
		[VendorPSExportID]			=	V.PS_Export_Vendor_ID,
		[Vendor_Key]				=	V.Vendor_Key,
		[InvoiceNumber]				=	oh.InvoiceNumber,
		[MatchingValidationCode]	=	oh.MatchingValidationCode,
		[MatchingDate]				=	oh.MatchingDate,
		[MatchingUser_ID]			=	oh.MatchingUser_ID,
		[MatchingValidationDesc]	=	VC.Description,
		[InvoiceSubteam_No]			=	ov.SubTeam_No,
		[vStoreID]					=	vs.Store_No,                            
		[PayByAgreedCost]			=	oh.PayByAgreedCost,
		[PaymentType]				=   case when oh.PayByAgreedCost = 1 then 'Pay Agreed Cost' else 'Pay By Invoice' end,										
		[VState]					=	vs.State,									
		[VZoneID]					=	S.Zone_ID,									
		[DocumentDataExists]		=	CASE ISNULL(oh.VendorDoc_ID, '')		
											WHEN '' THEN 
												0
											ELSE 
												1
										END,
		[TotalInvoiceCost]			=	ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0),
		[TotalInvoiceCostNoCharges] =	ISNULL(ROUND(ov.InvoiceTotalCost, 2, 1), 0),
		[TotalInvoiceFreight]		=	ISNULL(ov.InvoiceFreight, 0),
		[TotalOrderCost]			=	ROUND(CASE ISNULL(oh.DiscountType, -1)
											WHEN 1 THEN 
												ISNULL(oh.AdjustedReceivedCost, 0) - oh.QuantityDiscount
											WHEN 2 THEN 
												ISNULL(oh.AdjustedReceivedCost, 0) - (ISNULL(oh.AdjustedReceivedCost, 0) * (oh.QuantityDiscount / 100))
											WHEN 4 THEN 
												ISNULL(oh.AdjustedReceivedCost, 0) - (ISNULL(oh.AdjustedReceivedCost, 0) * (oh.QuantityDiscount / 100))
											ELSE 
												ISNULL(oh.AdjustedReceivedCost, 0)
										END, 2, 1),
		[TotalCostDiff]				=	CASE ISNULL(oh.DiscountType, -1)
											WHEN 1 THEN 
												ABS(ISNULL(oh.AdjustedReceivedCost, 0) - oh.QuantityDiscount) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
											WHEN 2 THEN 
												ABS((ISNULL(oh.AdjustedReceivedCost, 0)) - ((ISNULL(oh.AdjustedReceivedCost, 0)) * (oh.QuantityDiscount / 100))) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
											WHEN 4 THEN 
												ABS((ISNULL(oh.AdjustedReceivedCost, 0)) - ((ISNULL(oh.AdjustedReceivedCost, 0)) * (oh.QuantityDiscount / 100))) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
											ELSE 
												ABS((ISNULL(oh.AdjustedReceivedCost, 0))) - (ABS(ISNULL(ov.InvoiceCost, 0) + ISNULL(ov.InvoiceFreight, 0)))
										END,
		[OrderDate]					=	oh.OrderDate,								
		[InvoiceDate]				=	oh.InvoiceDate,
		[SentDate]					=	oh.SentDate,
		[POCostEffectiveDate]		=	oh.POCostDate,
		[POCreator]					=	uo.FullName, 
		[POCloser]					=	uc.FullName, 
		[MatchedeEInvoiceExists]	=	CASE ISNULL(oh.eInvoice_Id, '')
											WHEN '' THEN 
												0
											ELSE
												1
										END,
		[POTransmissionType]		=	CASE 
											WHEN oh.Fax_Order = 1 THEN 
												'Fax'
											WHEN oh.Email_Order = 1 THEN 
												'Email'
											WHEN oh.Electronic_Order = 1 THEN 
												'Electronic'
											ELSE 
												'Manual'
										END, 
		[Notes]						=	oh.OrderHeaderDesc,
		[PaymentTerms]				=	PT.Description,
		[NoOfDaysSuspended]			=	DATEDIFF(DAY, oh.CloseDate, GETDATE()),	
		[Beverage]					=	ST.Beverage,
		[CreditPO]					=	oh.Return_Order,
		[AdjustedCost]				=	CASE 
											WHEN oh.DiscountType > 0 THEN 
												1
											WHEN 0 < SUM(oi.DiscountType) Then 
												1 
											WHEN 0 < SUM(oi.AdjustedCost) Then 
												1 
											ELSE 
												0
										END,
		[DocTypeOther]				=	CASE ISNULL(oh.VendorDoc_ID, '')		
											WHEN '' THEN 
												CASE WHEN oh.InvoiceNumber is null or oh.InvoiceNumber = '' THEN
													1
												ELSE
													CASE WHEN oh.DSDOrder = 1 and oh.eInvoice_Id is null THEN
														1
													ELSE 
														0
													END
												END		
											ELSE 
												1										
										END,
		[CbW]						=	CASE 
											WHEN 0 < ANY (Select SUM(cast(I.CostedByWeight as Int) + cast(I.CatchweightRequired as Int))
													From OrderItem OI 
														INNER JOIN Item I ON OI.Item_Key = I.Item_Key 
													Where OI.OrderHeader_ID = oh.OrderHeader_ID 
													Group by OI.DiscountType) Then 1 
											ELSE 0
										END,
		[QtyMismatch]				=	dbo.fn_IsQuantityMismatch(oh.OrderHeader_ID),
		[CostNotByCase]				=	Case
											When (	Select COUNT(*) from OrderItem OI
													INNER JOIN ItemUnit IU on IU.Unit_ID = OI.CostUnit
													Where IU.Unit_Name <> 'CASE' AND OI.Package_Desc1 > 1 
														AND OI.OrderHeader_ID = oh.OrderHeader_ID 
												) > 0 Then 1
											Else 0
										End,
		[OrderUnitMismatch]			=	Case 
											When (	Select count(*) 
													From OrderItem OI 
														INNER JOIN Item I on I.Item_Key = OI.Item_Key 
													Where OI.QuantityUnit <> I.Vendor_Unit_ID 
														AND OI.OrderHeader_ID = oh.OrderHeader_ID
												) > 0 Then 1
											Else 0
										End,
		[Charges]					=	ROUND(ISNULL((SELECT SUM(Value) FROM OrderInvoiceCharges WHERE OrderHeader_Id = oh.OrderHeader_ID AND IsAllowance = 0), 0), 2, 1),
		[POAdminNotes]				=	oh.AdminNotes,
		[ResolutionCode]			=	rc.ReasonCodeDesc,
		[ResolutionCodeId]			=	oh.ResolutionCodeID,
		[OrderType_ID]				=	oh.OrderType_ID,
		[eInvoice_Id]				=	oh.eInvoice_Id,
		[CreatedBy]					=	oh.CreatedBy,
		[ClosedBy]					=	oh.ClosedBy,
		[Status]					=	case when oh.MatchingValidationCode > 0 and oh.InReview = 0 and oh.ApprovedDate IS NULL then 'Open'
										when oh.MatchingValidationCode > 0 and oh.InReview =1 and oh.ApprovedDate IS NULL then 'In Review'
										when oh.MatchingValidationCode > 0 and oh.InReview =0 and oh.ResolutionCodeID IS NOT NULL then 'Resolved'
										end,
		[InReviewUsername]			=	uir.username,
		[InReviewFullname]			=	uir.fullname,
		[ReceivingDocument]			=	oh.DSDOrder,
		[PartialShipment]			=	oh.PartialShipment,
		[EinvoiceRequired]          =   v.EinvoiceRequired,
		[TotalRefused]				=	oh.TotalRefused
	FROM 
		dbo.OrderHeader										(nolock) oh
		INNER JOIN	dbo.OrderItem							(nolock) oi		ON	oh.OrderHeader_Id					= oi.OrderHeader_Id
		LEFT JOIN	dbo.OrderInvoice						(nolock) ov		ON	oh.OrderHeader_Id					= ov.OrderHeader_Id
		INNER JOIN	dbo.Vendor								(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID
		INNER JOIN	dbo.Subteam								(nolock) st		ON	oh.Transfer_to_Subteam				= st.Subteam_No
		INNER JOIN	dbo.Vendor								(nolock) vs		ON	oh.PurchaseLocation_ID				= vs.Vendor_ID
		INNER JOIN	dbo.Store								(nolock) s		ON	vs.Store_No							= s.Store_No		
		INNER JOIN  fn_Parse_List(@LocalStoreList, '|')				 sl		ON	sl.Key_Value						= vs.Store_No 
		INNER JOIN	dbo.ItemIdentifier						(nolock) ii		ON	ii.Item_Key							= oi.Item_Key
																			    AND ii.Default_Identifier = 1
		INNER JOIN	dbo.ItemVendor							(nolock) iv		ON	iv.Item_Key							= oi.Item_Key
																			    AND iv.Vendor_ID					= v.Vendor_ID
		LEFT JOIN	dbo.ReasonCodeDetail					(nolock) rc		ON	oh.ResolutionCodeID					= rc.ReasonCodeDetailID
		LEFT JOIN	dbo.Users								(nolock) uo		ON	oh.createdby						= uo.User_ID
		LEFT JOIN	dbo.Users								(nolock) uc		ON	oh.ClosedBy							= uc.User_ID
		LEFT JOIN	dbo.Users								(nolock) uir	ON	oh.InReviewUser						= uir.User_ID
		LEFT JOIN	dbo.VendorPaymentTerms					(nolock) pt		ON	v.PaymentTermID						= pt.PaymentTermID
		LEFT JOIN	dbo.OrderInvoice_ControlGroupInvoice	(nolock) cgi	ON	oh.OrderHeader_ID					= cgi.OrderHeader_ID
		LEFT JOIN	dbo.OrderInvoice_ControlGroup			(nolock) cg		ON	cgi.OrderInvoice_ControlGroup_ID	= cg.OrderInvoice_ControlGroup_ID 
		LEFT JOIN	dbo.ValidationCode						(nolock) vc		ON	oh.MatchingValidationCode			= vc.ValidationCode
		LEFT JOIN	dbo.Store								(nolock) vendst ON	vendst.Store_No						= v.Store_No	
		LEFT JOIN	dbo.OrderExternalSource					(nolock) os     ON	oh.OrderExternalSourceID			= os.ID			
	WHERE
		-- Purchase Order Number search criteria 
		(@LocalOrderHeader_ID IS NULL
			OR (@LocalOrderHeader_ID IS NOT NULL 
				AND (oh.OrderHeader_ID = @LocalOrderHeader_ID or oh.OrderExternalSourceOrderID = @LocalOrderHeader_ID)))
		-- Vendor Invoice Number search criteria
		AND (@LocalInvoiceNumber IS NULL
			OR (@LocalInvoiceNumber IS NOT NULL 
				AND oh.InvoiceNumber = @LocalInvoiceNumber))
		-- Vendor ID search criteria
		AND (@LocalVendor_ID IS NULL
			OR (@LocalVendor_ID IS NOT NULL 
				AND V.Vendor_ID = @LocalVendor_ID))
		-- Identifier search criteria
		AND (@LocalIdentifier IS NULL
			OR (@LocalIdentifier IS NOT NULL 
				AND ii.Identifier = @LocalIdentifier))
		-- VIN search criteria
		AND (@LocalVIN IS NULL
			OR (@LocalVIN IS NOT NULL 
				AND iV.Item_ID = @LocalVIN ))
		-- Vendor Key search criteria
		AND (@LocalVendor_Key IS NULL
			OR (@LocalVendor_Key IS NOT NULL
				AND V.Vendor_Key = @LocalVendor_Key))
		AND ((@LocalEInvoiceOnly = 0)
			OR (@LocalEInvoiceOnly = 1 AND oh.eInvoice_Id is not null))

		-- Invoice Date search criteria
		AND ((oh.InvoiceDate BETWEEN @LocalInvoiceDateStart AND @LocalInvoiceDateEnd)
		OR oh.invoiceDate is null)
		
		-- Order Date search criteria
		AND oh.OrderDate BETWEEN @LocalOrderDateStart AND @LocalOrderDateEnd
		
		-- Control Group search criteria 
		AND (@LocalOrderInvoice_ControlGroup_ID IS NULL
			OR (@LocalOrderInvoice_ControlGroup_ID IS NOT NULL
				AND CG.OrderInvoice_ControlGroup_ID = @LocalOrderInvoice_ControlGroup_ID
				AND CG.OrderInvoice_ControlGroupStatus_ID = 2
				AND (CGI.ValidationCode = 0 OR dbo.fn_IsWarningValidationCode(CGI.ValidationCode) = 1)))
		-- All orders returned must be in the SUSPENDED state AND have invoice data
		AND oh.CloseDate IS NOT NULL
		AND oh.ApprovedDate IS NULL
		AND ISNULL(oh.UploadedDate, oh.AccountingUploadDate) IS NULL   
		AND ISNULL(vendst.Manufacturer, 0) = 0	
	GROUP BY 
		oh.OrderHeader_ID, 
		oh.CloseDate,
		oh.ProductType_ID, 
		st.SubTeam_Name,
		st.GLPurchaseAcct, 
		st.GLSuppliesAcct, 
		st.GLPackagingAcct,
		v.PS_Export_Vendor_ID,
		oh.OrderDate,
		oh.SentDate,
		oh.CreatedBy, 
		oh.ClosedBy,
		oh.eInvoice_Id,
		oh.Fax_Order, 
		oh.Email_Order, 
		oh.Electronic_Order, 
		oh.OrderHeaderDesc, 
		oh.Return_Order,
		oh.DiscountType, 
		oh.QuantityDiscount, 
		oh.InvoiceDate, 
		oh.InvoiceNumber, 
		vs.CompanyName,
		oh.Vendor_ID, 
		v.CompanyName, 
		oh.VendorDoc_ID, 
		v.Vendor_Key, 
		oh.MatchingValidationCode, 
		oh.MatchingDate, 
		oh.MatchingUser_ID,
		vc.Description, 
		ov.SubTeam_No, 
		vs.Store_no, 
		oh.PayByAgreedCost, 
		vs.State, 
		s.Zone_ID,
		oh.POCostDate, 
		st.Beverage, 
		oh.ResolutionCodeID, 
		oh.AdminNotes, 
		oh.OrderType_ID, 
		oh.eInvoice_Id, 
		oh.CreatedBy, 
		oh.ClosedBy, 
		v.PaymentTermID, 
		uo.FullName, 
		uc.FullName, 
		pt.Description, 
		rc.ReasonCodeDesc,
		ov.InvoiceCost,
		ov.InvoiceTotalCost,
		ov.InvoiceFreight,
		oh.ApprovedDate,
		oh.inreview,
		uir.username,
		uir.fullname,
		oh.AdjustedReceivedCost,
		oh.DSDOrder,
		oh.PartialShipment,
		v.EinvoiceRequired,
		oh.OrderExternalSourceOrderID,
        oh.TotalRefused,
		os.Description
	ORDER BY 
		oh.OrderHeader_ID

END
GO
