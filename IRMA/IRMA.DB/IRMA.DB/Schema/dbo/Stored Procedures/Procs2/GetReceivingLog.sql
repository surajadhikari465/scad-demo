
CREATE PROCEDURE [dbo].[GetReceivingLog] 
    @Store_No	int,
    @Begin		varchar(255),
    @End		varchar(255),
    @SubTeam_No int

AS 

-- **************************************************************************
-- Procedure: GetReceivingLog()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single RDL file and generates a report consumed
-- by SSRS procedures.
--
-- Modification History:
-- Date        Init	Comment
-- 06/02/2009  BBB	Updated SP to be more readable and output to include IsNull
--					for all aggregate values utilized in RDL
-- 07/14/2009  BBB	Added join to Currency table to return CurrencyCode to report
-- 09/21/2009  RDE  Added PS_Team_No and PS_SubTeam_No per TFS 11010
-- 10/26/2009  MU	incorporated charges and allowances per TFS 11302
-- 12/28/2009  RDE  changed default sorting to Vendorname, PO_No, RecvLog_No per TFS 11012
-- 02/01/2010  MU	updated to exclude allocated charges per tfs 11881
-- 11/15/2010  MU	updated to include 3rd Party Freight invoices per tfs 13614
-- 09/30/2011  TD   updated to include distinction between einvoice, manual POs, or OL_Import, 
--					receiving refusal codes, simplified exceptions to be based on the approval date
--					to match the order screen in IRMA.
-- 11/23/2011  TD   removed address information per Bug 3651
-- 12/15/2011  BAS  Coding standards/formatting.  Verified Line Item Recieved Cost per TFS 3744
-- 12/22/2011  BAS	updated aggregation of OrderItem.ReceivedItemCost and
--					changed it to OrderHeader.AdjustedReceivedCost per TFS 3744
-- 02/06/2012  TD	changed all instances of orderheader.adjustedreceiveditemcost BACK to the
--					aggregate SUM(OrderItem.ReceivedItemCost) per bug 4691.  
-- 04/01/2013  KM	Get CurrencyCode from StoreJurisdiction instead of OrderHeader.  This allows the report to work for GBP.
-- 09/12/2013  MZ   TFS#13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- 12/09/2013   FA		2101	Added Sales_Acccount to the primary key in the temp table

-- **************************************************************************

BEGIN  
 
 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
     
 --**************************************************************************  
 --Create and populate SP variables  
 --**************************************************************************  
	DECLARE @Begin_Date datetime, @End_Date datetime  
	SELECT @Begin_Date = CONVERT(datetime, @Begin), @End_Date = CONVERT(datetime, @End)  
	DECLARE @UploadOrders TABLE (OrderHeader_ID int PRIMARY KEY)  
  
 --**************************************************************************  
 -- Get a list of affected orders for the next step  
 --**************************************************************************  
 
 	INSERT INTO @UploadOrders  
		SELECT DISTINCT   
			oh.OrderHeader_ID  
		FROM   
			OrderHeader					(nolock) oh  
			LEFT JOIN	OrderInvoice	(nolock) oi	ON oh.OrderHeader_ID	 = oi.OrderHeader_ID   
			INNER JOIN	Vendor			(nolock) sv	ON oh.ReceiveLocation_ID = sv.Vendor_ID  
			INNER JOIN	Store			(nolock) s	ON sv.Store_No			 = s.Store_No   			
		WHERE   
			s.Store_No   = @Store_No
			AND	(oh.RecvLogDate >= @Begin_Date AND   
				 oh.RecvLogDate < DATEADD(d, 1, @End_Date))
			AND ISNULL(oi.SubTeam_No, ISNULL(@SubTeam_No, 0)) = ISNULL(@SubTeam_No, ISNULL(oi.SubTeam_No, ISNULL(@SubTeam_No, 0)))  
      
 --**************************************************************************  
 -- Add up received cost for items with Sales Accounts and treat as the invoice cost - must be separated in AP  
 --**************************************************************************  
	DECLARE @SalesAccountCost TABLE (OrderHeader_ID int, Sales_Account varchar(6), SubTeam_No int, InvoiceCost smallmoney, InvoiceFreight smallmoney, PRIMARY KEY NONCLUSTERED (OrderHeader_ID, SubTeam_No, Sales_Account))  
  
    INSERT INTO @SalesAccountCost  
		SELECT   
			oh.OrderHeader_ID,   
			i.Sales_Account,   
			i.SubTeam_No,   
			SUM(oi.receiveditemcost),   
			SUM(oi.ReceivedItemFreight)  
		FROM   
			OrderHeader				 (nolock) oh  
			INNER JOIN @UploadOrders		  uo ON oh.OrderHeader_ID = uo.OrderHeader_ID  
			INNER JOIN OrderItem	 (nolock) oi ON oh.OrderHeader_ID = oi.OrderHeader_ID  
			INNER JOIN Item			 (nolock) i	 ON oi.Item_Key		  = i.Item_Key  
		WHERE   
			i.Sales_Account IS NOT NULL
			AND oh.Transfer_To_SubTeam IS NULL  
		GROUP BY   
			oh.OrderHeader_ID,   
			i.Sales_Account,   
			i.SubTeam_No
  
		UNION  
  
		SELECT   
			oh.OrderHeader_ID,   
			i.Sales_Account,   
			oh.Transfer_To_SubTeam,   
			SUM(oi.receiveditemcost),
			SUM(oi.ReceivedItemFreight)  
		FROM  
			OrderHeader				 (nolock) oh  
			INNER JOIN @UploadOrders		  uo ON oh.OrderHeader_ID = uo.OrderHeader_ID  
			INNER JOIN OrderItem	 (nolock) oi ON oh.OrderHeader_ID = oi.OrderHeader_ID  
			INNER JOIN Item			 (nolock) i  ON oi.Item_Key		  = i.Item_Key  
		WHERE   
			i.Sales_Account    IS NOT NULL  
			AND oh.Transfer_To_SubTeam IS NOT NULL  
		GROUP BY   
			oh.OrderHeader_ID,   
			i.Sales_Account,   
			oh.Transfer_To_SubTeam
			
 --**************************************************************************  
 -- Invoice Charges and Allocations
 --**************************************************************************  
	DECLARE @InvoiceCharges TABLE (OrderHeader_ID int, Sales_Account varchar(6), SubTeam_No int, Value smallmoney)  
  	DECLARE @AllocatedChargeTypeID int
	
	SELECT @AllocatedChargeTypeID = SACType_Id from EInvoicing_SACTypes where SACType = 'Allocated'
	
    INSERT INTO @InvoiceCharges  
		SELECT   
			oh.OrderHeader_ID,   
			st.GLPurchaseAcct AS Sales_Account,   
			st.SubTeam_No,   
			oic.Value  
		FROM   
			OrderHeader oh
			INNER JOIN @UploadOrders		uo	ON oh.OrderHeader_ID = uo.OrderHeader_ID 
			INNER JOIN OrderInvoiceCharges	oic ON oh.OrderHeader_ID = oic.OrderHeader_ID and oic.SACType_ID != @AllocatedChargeTypeID
			LEFT JOIN SubTeam				st	ON st.SubTeam_No	 = oic.SubTeam_No
			
			  
 --**************************************************************************  
 -- Report Output  
 --**************************************************************************  
	SELECT 
		[RecvLogDate]			= oh.recvlogdate,
		[RecvLog_No]			= oh.recvlog_no,
		[VendorName]			= v.companyname,
		[Document_No]			= Isnull(oh.invoicenumber, oh.vendordoc_id),
		[DocumentDate]			= Isnull(oh.invoicedate, oh.vendordocdate),
		[InvoiceCost]			= Isnull(oi.invoicecost		* ( CASE
																	WHEN oh.return_order = 1 THEN 
																		-1
																	ELSE
																		1
																END ), 0),
		[InvoiceFreight]		= Isnull(oi.invoicefreight	* (	CASE
																	WHEN oh.return_order = 1 THEN
																		-1
																	ELSE
																		1
																END ), 0),
		[Team_No]				= sst.team_no,
		[SubTeam_No]			= oi.subteam_no,
		[PS_Team_No]			= sst.ps_team_no,
		[PS_SubTeam_No]			= sst.ps_subteam_no,
		[PO_No]					= oh.orderheader_id,
		[Exception]				= ( CASE
										WHEN oh.approveddate IS NULL THEN
											1
										ELSE
											0
									END ),
		[PS_Vendor_ID]			=	CASE
										WHEN Isnumeric (CASE
															WHEN Len (v.ps_vendor_id) > 0 THEN
																v.ps_vendor_id
															ELSE
																NULL
														END) = 1 THEN
											CONVERT (DECIMAL(18, 0), v.ps_vendor_id)
										ELSE
											NULL
									END,
		[ps_address_sequence]	= v.ps_address_sequence,
		[sales_account]			= oi.sales_account,
		[currencycode]			= c.CurrencyCode,
		[eInvoice Loaded]		= ( CASE
										WHEN ( oh.einvoice_id > 0 ) THEN
											'Yes'
										WHEN ( oes.id = 3 ) THEN
											'Yes'
										ELSE
											'No'
									END ),
		[reason code]			= Isnull(rcd.reasoncodedesc, ''),
		[OrderLink]				= ( CASE
										WHEN ( oes.id = 3 ) THEN
											'Yes'
										ELSE
											'No'
									END )
	FROM
		orderheader					(nolock)	oh
		INNER JOIN @UploadOrders				uo	ON uo.orderheader_id		= oh.orderheader_id
		INNER JOIN vendor			(nolock)	v	ON oh.vendor_id				= v.vendor_id
		INNER JOIN vendor			(nolock)	sv	ON oh.receivelocation_id	= sv.vendor_id
		INNER JOIN store			(nolock)	s	ON sv.store_no				= s.store_no
		INNER JOIN StoreJurisdiction(nolock)	sj	ON s.StoreJurisdictionID	= sj.StoreJurisdictionID
		LEFT JOIN (
					 --**************************************************************************  
					 -- OrderInvoice without OrderItems that have Sales Accounts  
					 --**************************************************************************  
					 SELECT --1,
						oi.orderheader_id,
						CASE oh.producttype_id
							WHEN 1 THEN
								Isnull(CONVERT(VARCHAR(6), st.glpurchaseacct), '500000')
							WHEN 2 THEN
								Isnull(CONVERT(VARCHAR(6), st.glpackagingacct), '510000')
							WHEN 3 THEN
								Isnull(CONVERT(VARCHAR(6), st.glsuppliesacct), '800000')
						END														AS sales_account,
						oi.subteam_no,
						( oi.invoicecost - Isnull(sa.sa_invoicecost, 0) )       AS invoicecost,
						( oi.invoicefreight - Isnull(sa.sa_invoicefreight, 0) ) AS invoicefreight
					 FROM
						orderinvoice			(nolock)	oi
						INNER JOIN orderheader				oh	ON oi.orderheader_id	= oh.orderheader_id
						INNER JOIN dbo.subteam	(nolock)	st	ON st.subteam_no		= oi.subteam_no
						INNER JOIN @UploadOrders			uo	ON oi.orderheader_id	= uo.orderheader_id
						LEFT JOIN (
								  --**************************************************************************  
								  -- OrderItems with Sales Account so amount can be subtracted  
								  --**************************************************************************  
									SELECT
										sac.orderheader_id,
										sac.subteam_no,
										SUM(sac.invoicecost)    AS sa_invoicecost,
										SUM(sac.invoicefreight) AS sa_invoicefreight
									FROM
										@SalesAccountCost sac
									GROUP BY
										sac.orderheader_id,
										sac.subteam_no) sa
																ON sa.orderheader_id	= oi.orderheader_id
																AND sa.subteam_no = oi.subteam_no
					 WHERE
						((oi.invoicecost - Isnull(sa.sa_invoicecost, 0)) <> 0 )
						OR (( oi.invoicefreight - Isnull(sa.sa_invoicefreight, 0) ) <> 0 )
					 UNION
					 --**************************************************************************  
					 -- OrderItem totals for OrderItems with Sales Accounts  
					 --**************************************************************************  
					 SELECT --2,
						sac.orderheader_id,
						sac.sales_account,
						sac.subteam_no,
						sac.invoicecost,
						sac.invoicefreight
					 FROM
						@SalesAccountCost sac
					 UNION ALL
					 --**************************************************************************  
					 -- Invoice Charges and Allocations
					 --**************************************************************************  
					 SELECT --3,
						ic.orderheader_id,
						ic.sales_account,
						ic.subteam_no,
						ic.VALUE,
						0
					 FROM
						@InvoiceCharges ic) AS oi
														ON oh.orderheader_id = oi.orderheader_id
		   LEFT JOIN storesubteam		(nolock) sst	ON	s.store_no = sst.store_no
														AND oi.subteam_no = sst.subteam_no
		   LEFT JOIN currency			(nolock) c		ON	sj.currencyid = c.currencyid
		   LEFT JOIN reasoncodedetail	(nolock) rcd	ON rcd.reasoncodedetailid = oh.refusereceivingreasonid
		   LEFT JOIN orderexternalsource(nolock) oes	ON oes.id = oh.orderexternalsourceid
	--**************************************************************************  
	-- 3rd Party Freight
	--************************************************************************** 
	UNION ALL
	SELECT [RecvLogDate]			= foh.recvlogdate,
		   [RecvLog_No]				= foh.recvlog_no,
		   [VendorName]				= fv.companyname,
		   [Document_No]			= f.invoicenumber,
		   [DocumentDate]			= f.invoicedate,
		   [InvoiceCost]			= Isnull(f.invoicecost, 0),
		   [InvoiceFreight]			= 0,
		   [Team_No]				= sst.ps_team_no,
		   [SubTeam_No]				= foh.transfer_to_subteam,
		   [PS_Team_No]				=	CASE
											WHEN dbo.Fn_glacctincludesteamsubteam(subteamtype_id, foh.producttype_id, f.orderheader_id) = 1 THEN
												sst.ps_team_no
											ELSE
												NULL
										END,
		   [PS_SubTeam_No]			=	CASE
											WHEN dbo.Fn_glacctincludesteamsubteam(subteamtype_id, foh.producttype_id, f.orderheader_id) = 1 THEN
												sst.ps_subteam_no
											ELSE
												NULL
										END,
		   [PO_No]					= foh.orderheader_id,
		   [Exception]				= ( CASE
											WHEN foh.approveddate IS NULL THEN
												1
											ELSE
												0
										END ),
		   [PS_Vendor_ID]			=	CASE
											WHEN Isnumeric (CASE
																WHEN Len (fv.ps_vendor_id) > 0 THEN
																	fv.ps_vendor_id
																ELSE
																	NULL
															END) = 1 THEN
												CONVERT (DECIMAL(18, 0), fv.ps_vendor_id)
											ELSE
												NULL
										END,
		   [ps_address_sequence]	= fv.ps_address_sequence,
		   [sales_account]			= ( CASE foh.producttype_id
											WHEN 1 THEN 
												Isnull(CONVERT(VARCHAR(6), st.glpurchaseacct), '500000')
											WHEN 2 THEN
												Isnull(CONVERT(VARCHAR(6), st.glpackagingacct), '510000')
											WHEN 3 THEN
												Isnull(CONVERT(VARCHAR(6), st.glsuppliesacct), '800000')
										END ),
		   [currencycode]			= fc.currencycode,
		   [eInvoice Loaded]		= ( CASE
											WHEN ( foh.einvoice_id > 0 ) THEN
												'Yes'
											WHEN ( foes.id = 3 ) THEN
												'Yes'
											ELSE
												'No'
										 END ),
		   [reason code]			= Isnull(frcd.reasoncodedesc, ''),
		   [OrderLink]				= ( CASE
											WHEN ( foes.id = 3 ) THEN
												'Yes'
											ELSE
												'No'
										END )
	
	FROM   orderinvoice_freight3party					f
		   INNER JOIN @UploadOrders						uo		ON f.orderheader_id			= uo.orderheader_id
		   INNER JOIN dbo.orderheader		(nolock)	foh		ON foh.orderheader_id		= f.orderheader_id
		   INNER JOIN dbo.subteam			(nolock)	st		ON st.subteam_no			= foh.transfer_to_subteam
		   INNER JOIN vendor				(nolock)	fv		ON f.vendor_id				= fv.vendor_id
		   INNER JOIN vendor				(nolock)	ov		ON foh.vendor_id			= ov.vendor_id
		   INNER JOIN vendor				(nolock)	rv		ON rv.vendor_id				= foh.receivelocation_id
		   INNER JOIN store					(nolock)	s		ON s.store_no				= rv.store_no
		   INNER JOIN StoreJurisdiction		(nolock)	sj		ON s.StoreJurisdictionID	= sj.StoreJurisdictionID
		   INNER JOIN storesubteam			(nolock)	sst		ON s.store_no				= sst.store_no
																AND foh.transfer_to_subteam = sst.subteam_no
		   LEFT JOIN currency				(nolock)	fc		ON sj.currencyid			= fc.currencyid
		   LEFT JOIN reasoncodedetail		(nolock)	frcd	ON frcd.reasoncodedetailid	= foh.refusereceivingreasonid
		   LEFT JOIN orderexternalsource	(nolock)	foes	ON foes.id					= foh.orderexternalsourceid
	
	ORDER  BY recvlog_no 
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingLog] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingLog] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingLog] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingLog] TO [IRMAReportsRole]
    AS [dbo];

