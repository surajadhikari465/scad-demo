
CREATE PROCEDURE dbo.GetRecvLogSumExcp 
    @Store_No		int,
    @Begin			varchar(255),
    @End			varchar(255),
    @SubTeam_No		int

AS 

-- **************************************************************************
-- Procedure: GetRecvLogSumExcp()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single RDL file and used in summation on 
-- that report.
--
-- Modification History:
-- Date        Init		TFS		Comment
-- 07/14/2009	BBB				Updated SP to be more readable; Added join to Currency 
--								table to return CurrencyCode to report; added CurrencyCode
--								group by
-- 09/17/2009	BSR				Added case statements to negate invoice amounts on credits
-- 09/21/2009	RDE				Added PS_Team_NO and PS_SUbteam_NO as per TFS 11010
-- 10/26/2009	MU				incorporated charges and allowances per TFS 11302
-- 02/01/2010	MU				updated to exclude allocated charges per tfs 11881
-- 11/15/2010	MU				updated to include 3rd Party Freight invoices per tfs 13614
-- 12.22.2011	BBB		3744	coding standards;
-- 04/01/2013	KM		11769	Get CurrencyCode from StoreJurisdiction instead of OrderHeader.  This allows the report to work for GBP.
-- 09/12/2013   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- 12/09/2013   FA		2101	Added Sales_Acccount to the primary key in the temp table
-- **************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
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
										 PRIMARY KEY NONCLUSTERED ( OrderHeader_ID, SubTeam_No, Sales_Account )) 

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
		sum([No_PO]) as [No_PO],
		sum(isnull([Total],0)) as [Total],
		[CurrencyCode]
	FROM
		  (
		  SELECT --OrderInvoice.*
			  [Sales_Account] = ISNULL ( OrderInvoice.Sales_Account , 0 ) ,
			  [Team_No] = StoreSubTeam.Team_No,
			  [SubTeam_No] = OrderInvoice.SubTeam_No,
			  [PS_Team_No] = Storesubteam.PS_Team_No,
			  [PS_Subteam_No] = Storesubteam.PS_SubTeam_No,
			  [No_PO] = COUNT ( * ) ,
			[Total]			=	SUM((OrderInvoice.InvoiceCost * (CASE 
															WHEN OrderHeader.Return_Order = 1 THEN 
																-1 
															ELSE 
																1 
															END))
								 + (OrderInvoice.InvoiceFreight * (CASE 
															WHEN OrderHeader.Return_Order = 1 THEN 
																-1 
															ELSE 
																1 
															END))),
			  [CurrencyCode] = c.CurrencyCode
		  FROM
			OrderHeader ( NOLOCK )
			INNER JOIN @UploadOrders UO ON UO.OrderHeader_ID = OrderHeader.OrderHeader_ID
			LEFT JOIN 
			--**************************************************************************  
			-- OrderInvoice without OrderItems that have Sales Accounts  
			--**************************************************************************  
			( SELECT
			   OI.OrderHeader_ID ,
			   CASE OH.ProductType_ID
					WHEN 1 THEN ISNULL(CONVERT(varchar(6), ST.GLPurchaseAcct), '500000')
					WHEN 2 THEN ISNULL(CONVERT(varchar(6), ST.GLPackagingAcct), '510000')
					WHEN 3 THEN ISNULL(CONVERT(varchar(6), ST.GLSuppliesAcct), '800000') End
					As Sales_Account,
				OI.SubTeam_No ,
				( OI.InvoiceCost - ISNULL ( SA.SA_InvoiceCost , 0 ) ) AS InvoiceCost ,
				( OI.InvoiceFreight - ISNULL ( SA.SA_InvoiceFreight , 0 ) ) AS InvoiceFreight
			FROM
				OrderInvoice OI ( NOLOCK )
				INNER JOIN OrderHeader OH ( NOLOCK ) on OI.OrderHeader_ID = OH.OrderHeader_ID
				INNER JOIN dbo.SubTeam ST (NOLOCK) ON  ST.SubTeam_No = OI.SubTeam_No
				INNER JOIN @UploadOrders UO ON OI.OrderHeader_ID = UO.OrderHeader_ID
				LEFT JOIN 
				--**************************************************************************  
				-- OrderItems with Sales Account so amount can be subtracted  
				--**************************************************************************  
				( SELECT
					  SAC.OrderHeader_ID ,
					  SAC.SubTeam_No ,
					  SUM ( SAC.InvoiceCost ) AS SA_InvoiceCost ,
					  SUM ( SAC.InvoiceFreight ) AS SA_InvoiceFreight
				  FROM
					  @SalesAccountCost SAC
				  GROUP BY
					  OrderHeader_ID ,
					  SubTeam_No ) SA
				ON SA.OrderHeader_ID = OI.OrderHeader_ID
				   AND SA.SubTeam_No = OI.SubTeam_No
			WHERE
				  ( ( OI.InvoiceCost - ISNULL ( SA.SA_InvoiceCost , 0 ) ) <> 0 )
				  OR ( ( OI.InvoiceFreight - ISNULL ( SA.SA_InvoiceFreight , 0 ) ) <> 0 )
			UNION ALL
			--**************************************************************************  
			-- OrderItem totals for OrderItems with Sales Accounts  
			--**************************************************************************  

			SELECT
				SAC.OrderHeader_ID ,
				SAC.Sales_Account ,
				SAC.SubTeam_No ,
				SAC.InvoiceCost ,
				SAC.InvoiceFreight
			FROM
				@SalesAccountCost SAC
	            
			UNION ALL
			--**************************************************************************  
			-- Invoice Charges and Allocations
			--**************************************************************************
			SELECT
				ic.OrderHeader_ID,
				ic.Sales_Account,
				ic.Subteam_No,
				ic.Value,
				0
			FROM
				@InvoiceCharges ic        
					
			  ) AS OrderInvoice ON OrderHeader.OrderHeader_ID = OrderInvoice.OrderHeader_ID
		  INNER JOIN Vendor ( NOLOCK ) ON OrderHeader.Vendor_ID = Vendor.Vendor_ID
		  INNER JOIN Vendor AS StoreVend ( NOLOCK ) ON OrderHeader.ReceiveLocation_ID = StoreVend.Vendor_ID
		  INNER JOIN Store ( NOLOCK ) ON StoreVend.Store_No = Store.Store_No
		  INNER JOIN StoreJurisdiction	(nolock) sj	ON Store.StoreJurisdictionID = sj.StoreJurisdictionID
		  LEFT JOIN StoreSubTeam ( NOLOCK ) ON Store.Store_No = StoreSubTeam.Store_No
											AND OrderInvoice.SubTeam_No = StoreSubTeam.SubTeam_No
		  LEFT JOIN Currency ( nolock ) c  ON sj.CurrencyID = c.CurrencyID
		  WHERE
				( ( OrderHeader.UploadedDate IS NULL )
				AND ( ( OrderHeader.ApprovedDate IS NULL )
					  OR ( Vendor.PS_Vendor_ID IS NULL
						   OR Vendor.PS_Location_Code IS NULL
						   OR Vendor.PS_Address_Sequence IS NULL )
					  OR ( ISNULL ( OrderInvoice.InvoiceCost , 0 ) = 0
						   AND ISNULL ( OrderInvoice.InvoiceFreight , 0 ) = 0 )
					  OR ( StoreSubTeam.Team_No IS NULL ) ) )
		  GROUP BY
				ISNULL ( OrderInvoice.Sales_Account , 0 ) ,
				CurrencyCode ,
				StoreSubTeam.Team_No,
				OrderInvoice.SubTeam_No,
				Storesubteam.PS_SubTeam_No,
				Storesubteam.PS_Team_No
	      			    
		  UNION ALL
	--**************************************************************************  
	-- 3rd Party Freight invoices
	--**************************************************************************
		SELECT 
			[Sales_Account] = 
				CASE FOH.ProductType_ID
					WHEN 1 THEN ISNULL(CONVERT(varchar(6), ST.GLPurchaseAcct), '500000')
					WHEN 2 THEN ISNULL(CONVERT(varchar(6), ST.GLPackagingAcct), '510000')
					WHEN 3 THEN ISNULL(CONVERT(varchar(6), ST.GLSuppliesAcct), '800000') End,
			[Team_No]  = StoreSubteam.PS_Team_No,
			[SubTeam_No] = FOH.Transfer_To_SubTeam,
			[PS_Team_No] = StoreSubTeam.PS_Team_No, 
			[PS_SubTeam_No] = StoreSubTeam.PS_SubTeam_No, 	   
			[No_PO] = COUNT ( * ) ,
			[Total]	= SUM(ISNULL(F.InvoiceCost,0)),
			[CurrencyCode] = FC.CurrencyCode
		FROM OrderInvoice_Freight3Party F
			INNER JOIN @UploadOrders UO on F.OrderHeader_ID = UO.OrderHeader_ID
			INNER JOIN dbo.OrderHeader FOH ON FOH.OrderHeader_ID = F.OrderHeader_ID
			INNER JOIN dbo.SubTeam ST ON  ST.SubTeam_No = FOH.Transfer_To_SubTeam
			INNER JOIN Vendor FV on F.Vendor_ID = FV.Vendor_ID
			INNER JOIN Vendor OV on FOH.Vendor_ID = OV.Vendor_ID
			INNER JOIN Vendor ReceiveLocation(NOLOCK) ON  ReceiveLocation.Vendor_ID = FOH.ReceiveLocation_ID
			INNER JOIN Store(NOLOCK) ON  Store.Store_No = ReceiveLocation.Store_no
			INNER JOIN StoreJurisdiction	(nolock) sj	ON Store.StoreJurisdictionID = sj.StoreJurisdictionID
			INNER JOIN StoreSubTeam (NOLOCK)
				ON Store.Store_No = StoreSubTeam.Store_No 
				AND FOH.Transfer_To_SubTeam = StoreSubTeam.SubTeam_No   
			LEFT JOIN Currency  (nolock) FC ON sj.CurrencyID  = FC.CurrencyID   
		WHERE
			FOH.UploadedDate IS NULL AND
				( FOH.ApprovedDate IS NULL 
				OR (  
					FV.PS_Vendor_ID IS NULL OR
					FV.PS_Location_Code IS NULL OR
					FV.PS_Address_Sequence IS NULL )  
				OR ( 
					OV.PS_Vendor_ID IS NULL OR
					OV.PS_Location_Code IS NULL OR
					OV.PS_Address_Sequence IS NULL )  
				OR ( ISNULL(F.InvoiceCost, 0) = 0 ) 
				
				OR StoreSubTeam.Team_No IS NULL)
		GROUP BY
			FOH.ProductType_ID, ST.GLPurchaseAcct, ST.GLPackagingAcct, ST.GLSuppliesAcct,
			CurrencyCode ,
			StoreSubTeam.Team_No,
			FOH.Transfer_To_SubTeam,
			Storesubteam.PS_SubTeam_No,
			Storesubteam.PS_Team_No
	--**************************************************************************
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
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRecvLogSumExcp] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRecvLogSumExcp] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRecvLogSumExcp] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRecvLogSumExcp] TO [IRMAReportsRole]
    AS [dbo];

