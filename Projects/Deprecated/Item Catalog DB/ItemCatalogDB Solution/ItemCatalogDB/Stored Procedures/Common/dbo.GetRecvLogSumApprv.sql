/****** Object:  StoredProcedure [dbo].[GetRecvLogSumApprv]    Script Date: 02/07/2012 18:06:28 ******/
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetRecvLogSumApprv]') AND type in (N'P', N'PC'))
begin
	EXEC ('CREATE PROCEDURE [dbo].[GetRecvLogSumApprv] AS SELECT 1')
end
GO

/****** Object:  StoredProcedure [dbo].[GetRecvLogSumApprv]    Script Date: 02/07/2012 18:06:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetRecvLogSumApprv] 
	@Store_No	int,
	@Begin		varchar(255),
	@End		varchar(255),
	@SubTeam_No	int

AS 

-- **************************************************************************
-- Procedure: GetRecvLogSumApprv()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single RDL file and used in summation on 
-- that report.
--
-- Modification History:
-- Date			Init	TFS		Comment
-- 07/14/2009	BBB				Updated SP to be more readable; Added join to Currency 
--								table to return CurrencyCode to report; added CurrencyCode
--								group by
-- 09/21/2009	RDE		11010	Added PS_Team_NO and PS_SubTeam_NO per TFS 11010
-- 09/17/2009	BSR				Added case statements to negate invoice amounts on credits
-- 10/26/2009	MU		11302	incorporated charges and allowances per TFS 11302
-- 02/01/2010	MU		11881	updated to exclude allocated charges per tfs 11881
-- 11/15/2010	MU		13614	updated to include 3rd Party Freight invoices per tfs 13614
-- 12.15.2011	BBB		3374	coding standards;
-- 02/07/2012	td		4781	changed where clause in 'a' to filter on vendor (v) address 
--								instead of store (vr) address  
-- 2013-04-01	KM		11769	Get CurrencyCode from StoreJurisdiction instead of OrderHeader.  This allows the report to work for GBP.
-- 12/09/2013   FA		2101	Added Sales_Acccount to the primary key in the temp table

-- **************************************************************************

BEGIN  
	SET NOCOUNT ON;

	--**************************************************************************   
	--Create and populate SP variables   
	--************************************************************************** 
	DECLARE  @Begin_Date DATETIME, 
			 @End_Date   DATETIME 

	SELECT @Begin_Date	= Convert(DATETIME,@Begin), 
		   @End_Date	= Convert(DATETIME,@End) 

	DECLARE  @UploadOrders  TABLE( 
								  Orderheader_id INT    PRIMARY KEY 
								  ) 

	--**************************************************************************   
	-- Get a list of affected orders for the next step   
	--************************************************************************** 
	INSERT INTO @UploadOrders 
		SELECT DISTINCT 
			oh.Orderheader_id 
		FROM   
			Orderheader					(nolock) oh 
			INNER JOIN	Vendor			(nolock) vr		ON oh.Receivelocation_id	= vr.Vendor_id 
			INNER JOIN	Store			(nolock) s		ON vr.Store_no				= s.Store_no 
			LEFT JOIN	Orderinvoice	(nolock) ov		ON oh.Orderheader_id		= ov.Orderheader_id 
		WHERE  
			(s.Store_no = @Store_No) 
			AND (oh.Recvlogdate >= @Begin_Date 
			AND oh.Recvlogdate	< Dateadd(D,1,@End_Date)) 
			AND Isnull(ov.Subteam_no,Isnull(@SubTeam_No,0)) = Isnull(@SubTeam_No,Isnull(ov.Subteam_no,Isnull(@SubTeam_No,0))) 

	--**************************************************************************   
	-- Add up received cost for items with Sales Accounts and treat as the invoice cost - must be separated in AP   
	--************************************************************************** 
	DECLARE  @SalesAccountCost  TABLE( 
									  Orderheader_id INT, 
									  Sales_account  VARCHAR(6), 
									  Subteam_no     INT, 
									  Invoicecost    SMALLMONEY, 
									  Invoicefreight SMALLMONEY , 
										 PRIMARY KEY NONCLUSTERED ( OrderHeader_ID,SubTeam_No, Sales_Account )) 

	INSERT INTO @SalesAccountCost 
		SELECT 
			oh.Orderheader_id, 
			i.Sales_account, 
			i.Subteam_no, 
			SUM(oi.Receiveditemcost), 
			SUM(oi.Receiveditemfreight) 
		FROM
			Orderheader					(nolock) oh
			INNER JOIN Orderitem		(nolock) oi		ON oh.Orderheader_id	= oi.Orderheader_id
			INNER JOIN Item				(nolock) i		ON oi.Item_key			= i.Item_key 
			INNER JOIN @UploadOrders			 uo		ON oh.Orderheader_id	= uo.Orderheader_id 
		WHERE
			i.Sales_account				IS NOT NULL 
			AND oh.Transfer_to_subteam	IS NULL 
		GROUP BY
			oh.Orderheader_id, 
			i.Sales_account, 
			i.Subteam_no 

		UNION 

		SELECT
			oh.Orderheader_id, 
			i.Sales_account, 
			oh.Transfer_to_subteam, 
			SUM(oi.ReceivedItemCost), 
			SUM(oi.ReceivedItemFreight) 
		FROM
			Orderheader					(nolock) oh
			INNER JOIN Orderitem		(nolock) oi		ON oh.Orderheader_id	= oi.Orderheader_id
			INNER JOIN Item				(nolock) i		ON oi.Item_key			= i.Item_key
			INNER JOIN @UploadOrders			 uo		ON oh.Orderheader_id	= uo.Orderheader_id 
		WHERE
			i.Sales_account				IS NOT NULL 
			AND oh.Transfer_to_subteam	IS NOT NULL 
		GROUP BY 
			oh.Orderheader_id, 
			i.Sales_account, 
			oh.Transfer_to_subteam 

	--**************************************************************************  
	-- Invoice Charges and Allocations
	--**************************************************************************
	DECLARE @InvoiceCharges TABLE	(
									OrderHeader_ID	int, 
									Sales_Account	varchar(6), 
									SubTeam_No		int, 
									Value smallmoney
									) 

	DECLARE @AllocatedChargeTypeID int
	
	SELECT @AllocatedChargeTypeID = SACType_Id from EInvoicing_SACTypes where SACType = 'Allocated'
	
	INSERT INTO @InvoiceCharges  
		SELECT   
			oh.OrderHeader_ID,   
			st.GLPurchaseAcct AS Sales_Account,   
			st.SubTeam_No,   
			oic.Value  
		FROM   
			OrderHeader						(nolock) oh
			INNER JOIN	OrderInvoiceCharges (nolock) oic	ON	oh.OrderHeader_ID	=	oic.OrderHeader_ID 
															AND oic.SACType_ID		!=	@AllocatedChargeTypeID
			INNER JOIN	@UploadOrders				 uo		ON	oh.OrderHeader_ID	=	uo.OrderHeader_ID 
			LEFT JOIN	SubTeam				(nolock) st		ON	oic.SubTeam_No		=	st.SubTeam_No
  
	--**************************************************************************   
	-- Report Output   
	--**************************************************************************
	SELECT   
		[Sales_Account],
		[Team_No],
		[SubTeam_No],
		[PS_Team_No],
		[PS_Subteam_No],
		[No_PO]			=	SUM([No_PO]),
		[Total]			=	SUM([Total]),
		[CurrencyCode]
	FROM
		  (
			SELECT 
				[Sales_Account]	= iov.Sales_account, 
				[Team_No]		= sst.Team_no, 
				[SubTeam_No]	= iov.Subteam_no,
				[PS_Team_No]	= sst.PS_Team_No,  
				[PS_Subteam_No]	= sst.PS_SubTeam_No,
				[No_PO]			= COUNT(*), 
				[Total]			=	SUM((iov.InvoiceCost * (CASE 
																WHEN oh.Return_Order = 1 THEN 
																	-1 
																ELSE 
																	1 
																END))
										+ (iov.InvoiceFreight * (CASE 
																WHEN oh.Return_Order = 1 THEN 
																	-1 
																ELSE 
																	1 
																END))),
				[CurrencyCode] = C.Currencycode 
			FROM
				OrderHeader						(nolock) oh
				INNER JOIN Vendor				(nolock) v	ON oh.Vendor_id				= v.Vendor_id 
				INNER JOIN Vendor				(nolock) vr ON oh.Receivelocation_id	= vr.Vendor_id 
				INNER JOIN Store				(nolock) s	ON vr.Store_no				= s.Store_no 
				INNER JOIN StoreJurisdiction	(nolock) sj	ON s.StoreJurisdictionID	= sj.StoreJurisdictionID
				INNER JOIN @UploadOrders				 Uo ON oh.Orderheader_id		= Uo.Orderheader_id
				LEFT JOIN	
							-- OrderInvoice without OrderItems that have Sales Accounts   
							(SELECT
								ov.Orderheader_id, 
								[Sales_Account]		=	CASE oh.ProductType_ID
															WHEN 1 THEN ISNULL(CONVERT(varchar(6), st.GLPurchaseAcct), '500000')
															WHEN 2 THEN ISNULL(CONVERT(varchar(6), st.GLPackagingAcct), '510000')
															WHEN 3 THEN ISNULL(CONVERT(varchar(6), st.GLSuppliesAcct), '800000')
														END,  
									ov.Subteam_no, 
									[Invoicecost]		=	(ov.Invoicecost - Isnull(Sa.Sa_invoicecost,0)), 
									[Invoicefreight]	=	(ov.Invoicefreight - Isnull(Sa.Sa_invoicefreight,0))
							FROM
								Orderinvoice				(nolock) ov
								INNER JOIN OrderHeader		(nolock) oh	ON	ov.OrderHeader_ID	= oh.OrderHeader_ID
								INNER JOIN SubTeam			(nolock) st ON  ov.SubTeam_No		= st.SubTeam_No
								INNER JOIN @UploadOrders			 uo ON	ov.Orderheader_id	= uo.Orderheader_id 
								LEFT JOIN	
											-- OrderItems with Sales Account so amount can be subtracted   
											(SELECT
												Sac.Orderheader_id, 
												Sac.Subteam_no, 
												Sum(Sac.Invoicecost)    AS Sa_invoicecost, 
												Sum(Sac.Invoicefreight) AS Sa_invoicefreight 
											FROM     
												@SalesAccountCost Sac 
											GROUP BY
												Sac.Orderheader_id, 
												Sac.Subteam_no
											) Sa 
																		ON	ov.Orderheader_id	= Sa.Orderheader_id
																		AND ov.Subteam_no		= Sa.Subteam_no
							WHERE
								((ov.Invoicecost - Isnull(Sa.Sa_invoicecost,0)) <> 0) 
								OR 
								((ov.Invoicefreight - Isnull(Sa.Sa_invoicefreight,0)) <> 0) 

							UNION 

							-- OrderItem totals for OrderItems with Sales Accounts   						
							SELECT
								Sac.Orderheader_id, 
								Sac.Sales_account, 
								Sac.Subteam_no, 
								Sac.Invoicecost, 
								Sac.Invoicefreight 
							FROM 
								@SalesAccountCost Sac
							
							UNION ALL
							
							-- Invoice Charges and Allocations
							SELECT
								ic.OrderHeader_ID,
								ic.Sales_Account,
								ic.Subteam_No,
								ic.Value,
								0
							FROM
								@InvoiceCharges ic
							 
							) AS						 iov	ON	oh.Orderheader_id	= iov.Orderheader_id 
				LEFT JOIN Storesubteam			(nolock) sst	ON	s.Store_no			= sst.Store_no 
																AND iov.Subteam_no		= sst.Subteam_no 
				LEFT JOIN Currency				(nolock) c		ON	sj.Currencyid		= c.Currencyid 
			WHERE
				((oh.Uploadeddate IS NOT NULL) 
					OR ((oh.Approveddate IS NOT NULL) 
						AND (v.Ps_vendor_id IS NOT NULL 
							AND v.Ps_location_code IS NOT NULL 
							AND v.Ps_address_sequence IS NOT NULL) 
						AND (Isnull(Invoicecost,0) <> 0 
							OR Isnull(Invoicefreight,0) <> 0) 
						AND (sst.Team_no IS NOT NULL))) 
			GROUP BY 
				Sales_account, 
				CurrencyCode, 
				sst.Team_no, 
				iov.Subteam_no,
				sst.PS_Subteam_no,
				sst.PS_Team_no 
					 
			UNION ALL
			--**************************************************************************  
			-- 3rd Party Freight invoices
			--**************************************************************************
			SELECT 
				[Sales_Account]	=	CASE oh.ProductType_ID
										WHEN 1 THEN ISNULL(CONVERT(varchar(6), ST.GLPurchaseAcct), '500000')
										WHEN 2 THEN ISNULL(CONVERT(varchar(6), ST.GLPackagingAcct), '510000')
										WHEN 3 THEN ISNULL(CONVERT(varchar(6), ST.GLSuppliesAcct), '800000') 
									END,
				[Team_No]		=	sst.PS_Team_No,
				[SubTeam_No]	=	oh.Transfer_To_SubTeam,
				[PS_Team_No]	=	sst.PS_Team_No, 
				[PS_SubTeam_No] =	sst.PS_SubTeam_No, 	   
				[No_PO]			=	COUNT(*),
				[Total]			=	SUM(ISNULL(oif.InvoiceCost,0)),
				[CurrencyCode]	=	FC.CurrencyCode
			FROM
				OrderInvoice_Freight3Party	(nolock) oif
				INNER JOIN @UploadOrders			 uo		ON	oif.OrderHeader_ID		= uo.OrderHeader_ID
				INNER JOIN dbo.OrderHeader	(nolock) oh		ON	oif.OrderHeader_ID		= oh.OrderHeader_ID
				INNER JOIN dbo.SubTeam		(nolock) st		ON  oh.Transfer_To_SubTeam	= st.SubTeam_No
				INNER JOIN Vendor			(nolock) vf		ON	oif.Vendor_ID			= vf.Vendor_ID
				INNER JOIN Vendor			(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID
				INNER JOIN Vendor			(nolock) vr		ON  oh.ReceiveLocation_ID	= vr.Vendor_ID
				INNER JOIN Store			(nolock) s		ON  s.Store_No				= vr.Store_no
				INNER JOIN StoreJurisdiction(nolock) sj		ON  s.StoreJurisdictionID	= sj.StoreJurisdictionID
				INNER JOIN StoreSubTeam		(nolock) sst	ON	s.Store_No				= sst.Store_No 
															AND oh.Transfer_To_SubTeam = sst.SubTeam_No   
				LEFT JOIN Currency			(nolock) FC		ON	sj.CurrencyID			= FC.CurrencyID   
			WHERE
				oh.UploadedDate IS NOT NULL OR
					(oh.ApprovedDate IS NOT NULL 
					AND (  
						vf.PS_Vendor_ID IS NOT NULL AND
						vf.PS_Location_Code IS NOT NULL AND
						vf.PS_Address_Sequence IS NOT NULL )  
					AND ( 
						v.PS_Vendor_ID IS NOT NULL AND
						v.PS_Location_Code IS NOT NULL AND
						v.PS_Address_Sequence IS NOT NULL )  
					AND ( ISNULL(oif.InvoiceCost, 0) != 0 ) 
					
					AND sst.Team_No IS NOT NULL)
			GROUP BY 
				oh.ProductType_ID, 
				ST.GLPurchaseAcct, 
				ST.GLPackagingAcct, 
				ST.GLSuppliesAcct,
				CurrencyCode,
				sst.Team_No,
				oh.Transfer_To_SubTeam,
				sst.PS_SubTeam_No,
				sst.PS_Team_No
		) a
	GROUP BY 
		[Sales_Account],
		[Team_No],
		[SubTeam_No],
		[PS_Team_No],
		[PS_Subteam_No],
		[CurrencyCode]
	ORDER BY 
		CurrencyCode DESC

	SET NOCOUNT OFF;	  
END
GO