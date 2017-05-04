CREATE PROCEDURE [dbo].[InvoiceDiscrepanciesReport]
    @Vendor_ID			int,
    @Store_No			int,
    @StartDate			datetime,
    @EndDate			datetime,
    @INVDiscrepancySent smallint, -- -1 = All; 1 = Sent; 0 = Not Sent
    @AutomatedReport	bit,
    @PaymentDiscrep		bit = 1,
    @PackDiscrep		bit = 0,
    @QtyDiscrep			bit = 0,
    @CostDiscrep		bit = 0,
    @NoIdDiscrep		bit = 0
AS
   -- **************************************************************************
   -- Procedure: InvoiceDiscrepanciesReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date			Init		TFS		Comment
   -- 04/21/2010	BBB			12498	Updated SP to be more readable; removed security
   --									grants; moved conditions into modules;
   -- 04/22/2010	BBB			12493	Added condition for Automated Report which is called
   --									from Subscription on InvoiceDiscrepancies Report;
   -- 05/05/2010	BBB			12654	Corrected issue with AutomatedReport not returning
   --									all problem types in it's output
   -- 08/16/2011	Denis Ng	1644	Create Payment Confirmation Report
   -- 2011/12/29	KM			3744	Coding standards; usage review;
   -- 03/07/2012	TD			5169	Changed filter in output 
   --									to orderitem.ResolutionCodeID 
   --									from orderitem.ReasoncodedetailID (cost adjustment reason code)
   --									and where the oh.uploaddate isn't null
   -- 03/22/2012	TD			5463	added orderitem.resolutioncodeid, applied coding standards to sql output
   -- 04/10/2012	TD			5783	excluded orderitem.paymenttypeid of 0 from report
   -- 06/04/2012	TD			6520,6558	undid where clause change for tfs 6471 by looking for 
   --										'oi.ResolutionCodeID IS NOT NULL' AND 'oh.UploadedDate IS NOT NULL'
   -- 06/13/2012	TD			6643	Changed filtering from only items whose orderitem.ReceivedItemCost < orderitem.InvoiceExtendedCost
   --									to orderitem.ReceivedItemCost <> orderitem.InvoiceExtendedCost
   --									and removed reference to paymenttypeid, so that all discrepancies can be shown.			
   -- **************************************************************************
BEGIN
    SET NOCOUNT ON
    
	--**************************************************************************
	-- Declare internal variables
	--************************************************************************** 
    DECLARE @Orders TABLE 
    (
		OrderHeader_ID	int, 
		OrderItem_ID	int, 
		InvoiceTotal	money, 
		PaymentTotal	money
	)

	--**************************************************************************
	-- Populate internal variables
	--**************************************************************************     
	-- Condition #1
	IF @PaymentDiscrep = 1 AND @AutomatedReport = 0
		BEGIN
			INSERT INTO @Orders (OrderHeader_ID, OrderItem_ID) 
				SELECT 
					oh.OrderHeader_ID, 
					oi.OrderItem_ID
				FROM
					dbo.OrderHeader				(nolock) oh
					INNER JOIN dbo.Vendor		(nolock) v	ON 	oh.ReceiveLocation_ID	= v.Vendor_ID
					INNER JOIN dbo.OrderItem	(nolock) oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID	
				WHERE 
					oh.InvoiceDiscrepancy		= 1 
					AND (
						(@INVDiscrepancySent	= -1) 
						OR 
						(oh.InvoiceDiscrepancySentDate IS NOT NULL AND @INVDiscrepancySent = 1) 
						OR 
						(oh.InvoiceDiscrepancySentDate IS NULL AND @INVDiscrepancySent = 0)
						)
					AND oh.Vendor_ID			=	ISNULL(@Vendor_ID, oh.Vendor_ID)
					AND oh.CloseDate			>=	ISNULL(@StartDate, oh.CloseDate) 
					AND oh.CloseDate			<=	ISNULL(DATEADD(day, 1, @EndDate), oh.CloseDate)
					AND v.Store_No				=	ISNULL(@Store_No, v.Store_No)
					AND CASE 
							WHEN ISNULL(@AutomatedReport, 0) = 0 THEN 
								0 
							ELSE 
								oh.UploadedDate 
						END						IS NOT NULL
					AND (ROUND(oi.ReceivedItemCost, 2)	<>	ROUND(oi.InvoiceExtendedCost, 2))
					AND oh.PayByAgreedCost				=	1
				GROUP BY 
					oh.OrderHeader_ID, 
					oi.OrderItem_ID
		END
		
	-- Condition #2
	IF (@PackDiscrep = 1 OR @QtyDiscrep = 1 OR @CostDiscrep = 1) AND @AutomatedReport = 0
		BEGIN
			INSERT INTO @Orders (OrderHeader_ID, OrderItem_ID) 
				SELECT 
					oh.OrderHeader_ID, 
					oi.OrderItem_ID
				FROM 
					dbo.OrderHeader					(nolock) oh
					INNER JOIN dbo.Vendor			(nolock) v	ON	oh.ReceiveLocation_ID	= v.Vendor_ID
					INNER JOIN dbo.OrderItem		(nolock) oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID	
																AND oi.InvoiceCost			IS NOT NULL			-- There is eInvoice info loaded
					INNER JOIN dbo.eInvoicing_Item  (nolock) ei ON	oh.eInvoice_ID			= ei.eInvoice_ID 
																AND oi.Item_Key				= ei.item_key			
				WHERE 
					oh.Vendor_ID	=	ISNULL(@Vendor_ID, oh.Vendor_ID)
					AND CloseDate	>=	ISNULL(@StartDate, oh.CloseDate) 
					AND CloseDate	<=	ISNULL(DATEADD(day, 1, @EndDate), oh.CloseDate)
					AND v.Store_No	=	ISNULL(@Store_No, v.Store_No)
					AND (
						((@PackDiscrep = 1) AND (oi.Package_Desc1 <> ei.case_pack))
						OR
						((@QtyDiscrep = 1) AND (ISNULL(oi.eInvoiceQuantity,0) <> ISNULL(oi.QuantityReceived, 0)))
						OR 
						((@CostDiscrep = 1) AND (ROUND(oi.Cost, 2) <> ROUND(oi.InvoiceCost, 2)))
						)
					AND oh.PayByAgreedCost				=	1
				GROUP BY 
					oh.OrderHeader_ID,
					oi.OrderItem_ID
		END
    
	-- Condition #3
	IF (@NoIdDiscrep = 1 OR @PaymentDiscrep = 1) AND @AutomatedReport = 0
		BEGIN
			INSERT INTO @Orders (OrderHeader_ID, OrderItem_ID) 
				SELECT
					oh.OrderHeader_ID, 
					NULL
				FROM 
					dbo.eInvoicing_Item			(nolock) ei
					INNER JOIN dbo.OrderHeader	(nolock) oh ON	ei.eInvoice_ID			= oh.eInvoice_ID	
					INNER JOIN dbo.Vendor		(nolock) v	ON	oh.ReceiveLocation_ID	= v.Vendor_ID		
				WHERE 
					(ei.IsNotIdentifiable	= 1
					OR 
					ei.IsNotOrdered			= 1
					)
					AND (
						@NoIdDiscrep = 1
						OR (
							@PaymentDiscrep = 1 
							AND oh.InvoiceDiscrepancy = 1 
							AND (
								(@INVDiscrepancySent = -1) 
								OR 
								(oh.InvoiceDiscrepancySentDate IS NOT NULL AND @INVDiscrepancySent = 1) 
								OR 
								(oh.InvoiceDiscrepancySentDate IS NULL AND @INVDiscrepancySent = 0)
								)
							)
						)
					AND oh.Vendor_ID		=	ISNULL(@Vendor_ID, oh.Vendor_ID)
					AND oh.CloseDate		>=	ISNULL(@StartDate, oh.CloseDate)
					AND oh.CloseDate		<=	ISNULL(DATEADD(day, 1, @EndDate), oh.CloseDate)
					AND v.Store_No			=	ISNULL(@Store_No, v.Store_No) 
					AND oh.PayByAgreedCost	=	1
				GROUP BY 
					oh.OrderHeader_ID
		END
		
	-- Condition #4
	IF @AutomatedReport = 1
		BEGIN					
			-- Set Discrep types
			SET @PaymentDiscrep	= 1
			SET @PackDiscrep	= 1
			SET @QtyDiscrep		= 1
			SET @CostDiscrep	= 1
			SET @NoIdDiscrep	= 1
			
			-- PaymentDiscrep
			BEGIN
				INSERT INTO @Orders (OrderHeader_ID, OrderItem_ID) 
					SELECT 
						oh.OrderHeader_ID, 
						oi.OrderItem_ID
					FROM
						dbo.OrderHeader				(nolock) oh
						INNER JOIN dbo.Vendor		(nolock) v	ON	oh.ReceiveLocation_ID	= v.Vendor_ID			
						INNER JOIN dbo.OrderItem	(nolock) oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID	
					WHERE 
						oh.InvoiceDiscrepancy				= 1 
						AND oh.InvoiceProcessingDiscrepancy = 1
						AND (
							(@INVDiscrepancySent			= -1) 
							OR 
							(oh.InvoiceDiscrepancySentDate IS NOT NULL AND @INVDiscrepancySent = 1) 
							OR 
							(oh.InvoiceDiscrepancySentDate IS NULL AND @INVDiscrepancySent = 0)
							)
						AND oh.Vendor_ID			=	ISNULL(@Vendor_ID, oh.Vendor_ID)
						AND oh.CloseDate			>=	ISNULL(@StartDate, oh.CloseDate) 
						AND oh.CloseDate			<=	ISNULL(DATEADD(day, 1, @EndDate), oh.CloseDate)
						AND v.Store_No				=	ISNULL(@Store_No, v.Store_No)
						AND CASE 
								WHEN ISNULL(@AutomatedReport, 0) = 0 THEN 
									0 
								ELSE 
									oh.UploadedDate 
							END						IS NOT NULL
						AND (ROUND(oi.ReceivedItemCost, 2)	<>	ROUND(oi.InvoiceExtendedCost, 2))
						AND oh.PayByAgreedCost				=	1
					GROUP BY 
						oh.OrderHeader_ID, 
						oi.OrderItem_ID
			END
				
			-- PackDiscrep OR QtyDiscrep OR CostDiscrep
			BEGIN
				INSERT INTO @Orders (OrderHeader_ID, OrderItem_ID) 
					SELECT 
						oh.OrderHeader_ID, 
						oi.OrderItem_ID
					FROM 
						dbo.OrderHeader				(nolock) oh
					INNER JOIN dbo.Vendor			(nolock) v	ON	oh.ReceiveLocation_ID	= v.Vendor_ID
					INNER JOIN dbo.OrderItem		(nolock) oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID	
																AND oi.InvoiceCost			IS NOT NULL			-- There is eInvoice info loaded
					INNER JOIN dbo.eInvoicing_Item  (nolock) ei ON	oh.eInvoice_ID			= ei.eInvoice_ID 
																AND oi.Item_Key				= ei.item_key			
					WHERE 
						oh.InvoiceDiscrepancy				= 1 
						AND oh.InvoiceProcessingDiscrepancy = 1
						AND (
							(@INVDiscrepancySent			= -1) 
							OR 
							(oh.InvoiceDiscrepancySentDate IS NOT NULL AND @INVDiscrepancySent = 1) 
							OR 
							(oh.InvoiceDiscrepancySentDate IS NULL AND @INVDiscrepancySent = 0)
							)
						AND oh.Vendor_ID	=	ISNULL(@Vendor_ID, oh.Vendor_ID)
						AND CloseDate		>=	ISNULL(@StartDate, oh.CloseDate) 
						AND CloseDate		<=	ISNULL(DATEADD(day, 1, @EndDate), oh.CloseDate)
						AND v.Store_No		=	ISNULL(@Store_No, v.Store_No)
						AND (
							((@PackDiscrep = 1) AND (oi.Package_Desc1 <> ei.case_pack))
							OR
							((@QtyDiscrep = 1) AND (ISNULL(oi.eInvoiceQuantity,0) <> ISNULL(oi.QuantityReceived, 0)))
							OR 
							((@CostDiscrep = 1) AND (ROUND(oi.Cost, 2) <> ROUND(oi.InvoiceCost, 2)))
							)
						AND oh.PayByAgreedCost			=	1
					GROUP BY 
						oh.OrderHeader_ID,
						oi.OrderItem_ID
			END
		    
			-- NoIdDiscrep OR PaymentDiscrep
			BEGIN
				INSERT INTO @Orders (OrderHeader_ID, OrderItem_ID) 
					SELECT
						oh.OrderHeader_ID, 
						NULL
					FROM 
						dbo.eInvoicing_Item			(nolock) ei
						INNER JOIN dbo.OrderHeader	(nolock) oh ON ei.eInvoice_ID			= oh.eInvoice_ID	
						INNER JOIN dbo.Vendor		(nolock) v	ON oh.ReceiveLocation_ID	= v.Vendor_ID		
					WHERE 
						oh.InvoiceDiscrepancy				= 1 
						AND oh.InvoiceProcessingDiscrepancy = 1
						AND (
							(@INVDiscrepancySent			= -1) 
							OR 
							(oh.InvoiceDiscrepancySentDate IS NOT NULL AND @INVDiscrepancySent = 1) 
							OR 
							(oh.InvoiceDiscrepancySentDate IS NULL AND @INVDiscrepancySent = 0)
							)
						AND
						(ei.IsNotIdentifiable	= 1
						OR 
						ei.IsNotOrdered			= 1
						)
						AND (
							@NoIdDiscrep = 1
							OR (
								@PaymentDiscrep = 1 
								AND oh.InvoiceDiscrepancy = 1 
								AND (
									(@INVDiscrepancySent = -1) 
									OR 
									(oh.InvoiceDiscrepancySentDate IS NOT NULL AND @INVDiscrepancySent = 1) 
									OR 
									(oh.InvoiceDiscrepancySentDate IS NULL AND @INVDiscrepancySent = 0)
									)
								)
							)
						AND oh.Vendor_ID		=	ISNULL(@Vendor_ID, oh.Vendor_ID)
						AND oh.CloseDate		>=	ISNULL(@StartDate, oh.CloseDate)
						AND oh.CloseDate		<=	ISNULL(DATEADD(day, 1, @EndDate), oh.CloseDate)
						AND v.Store_No			=	ISNULL(@Store_No, v.Store_No) 
						AND oh.PayByAgreedCost	=	1
					GROUP BY 
						oh.OrderHeader_ID
			END				
		END

	-- Set InvoiceTotal for Orders
    UPDATE 
		@Orders
    SET 
		InvoiceTotal = ISNULL(SumInvoiceExtendedCost, 0), 
		PaymentTotal = SumReceivedItemCost
    FROM 
		@Orders O
		INNER JOIN	(
						SELECT 
							[OrderHeader_ID]			= oi.OrderHeader_ID, 
							[SumInvoiceExtendedCost]	= SUM(oi.InvoiceExtendedCost), 
							[SumReceivedItemCost]		= SUM(oi.ReceivedItemCost)
						FROM 
							dbo.OrderItem (nolock) oi
						WHERE 
							oi.OrderHeader_ID IN (SELECT OrderHeader_ID FROM @Orders)
					
					GROUP BY 
						oi.OrderHeader_ID) soi ON soi.OrderHeader_ID = O.OrderHeader_ID

	-- Set InvoiceTotal for eInvoice Orders	        
	UPDATE 
		@Orders
	SET 
		InvoiceTotal = ISNULL(InvoiceTotal, 0) + ISNULL(SumINVExtendedCost, 0)
	FROM 
		@Orders O
	INNER JOIN	(
					SELECT 
						[OrderHeader_ID]		= OrderHeader_ID,
						[SumINVExtendedCost]	= SUM(ext_cost)
					FROM 
						dbo.eInvoicing_Item			(nolock) ei
						INNER JOIN dbo.OrderHeader	(nolock) oh	ON	ei.eInvoice_ID = oh.eInvoice_ID 
					WHERE 
						(ei.IsNotIdentifiable = 1 OR ei.IsNotOrdered = 1
				)
				
				GROUP BY 
					oh.OrderHeader_ID) eInvItm ON eInvItm.OrderHeader_ID = O.OrderHeader_ID
	 
    -- Freight calculation
    UPDATE 
		@Orders
    SET 
		InvoiceTotal = ISNULL(o.InvoiceTotal, 0) + oiv.InvoiceFreight,
        PaymentTotal = ISNULL(o.PaymentTotal, 0) + oiv.InvoiceFreight  
    FROM 
		@Orders O
		INNER JOIN dbo.OrderInvoice (nolock) oiv ON	O.OrderHeader_ID = oiv.OrderHeader_ID 
    WHERE 
		ISNULL(oiv.InvoiceFreight, 0) > 0
 
 	--**************************************************************************
	-- Output SQL
	--**************************************************************************     
	SELECT
		[OrderHeader_ID]		= oh.OrderHeader_ID,
		[Vendor_ID]				= v.Vendor_ID,
		[CompanyName]			= v.CompanyName, 
		[InvoiceNumber]			= oh.InvoiceNumber, 
		[InvoiceDate]			= oh.InvoiceDate, 
		[InvoiceTotal]			= o.InvoiceTotal,
		[Store_Name]			= vr.CompanyName,
		[POType]				= oes.Description,
		[PONumber]				= oh.OrderHeader_ID,
		[PODate]				= oh.CloseDate,
		[PaymentTotal]			= o.PaymentTotal,
		[Identifier]			= ii.Identifier,
		[VIN]					= iv.Item_ID,
		[Item_Description]		= iv.VendorItemDescription,
		[Package_Desc1]			= oi.Package_Desc1,
		[OrderUOM]				= qu.EDISysCode, 
		[OrderUOMCost]			= dbo.fn_CostConversion(oi.Cost, oi.CostUnit, oi.QuantityUnit, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID),
		[InvoiceUOMCost]		= dbo.fn_CostConversion(oi.InvoiceCost, oi.CostUnit, oi.QuantityUnit, oi.Package_Desc1, oi.Package_Desc2, oi.Package_Unit_ID),
		[ReceivedQTY]			= oi.QuantityReceived,
		[ShippedQTY]			= oi.eInvoiceQuantity,
		[ReceivedItemCost]		= oi.ReceivedItemCost,
		[InvoiceExtendedCost]	= oi.InvoiceExtendedCost,
		[QuantityOrdered]		= oi.QuantityOrdered,
		[Unit_Name]				= iu.Unit_Name,
		[Expected_Date]			= oh.Expected_Date,
		[ReasonCode]			= rcd.ReasonCode,  -- cost adjustment reason orderitem level
		[CostAdjustCode]		= car.ReasonCode,  -- cost adjustment reason orderheader level
		[ReceivingReasonCode]	= rrc.ReasonCode,  -- receiving exception code orderitem level
		[ResolutionReasonCode]	= prc.ReasonCode,  -- PO Admin resolution code orderitem level
		[DiscountType]			= CASE 
									WHEN oi.DiscountType > 0 THEN 
										CASE oi.DiscountType 
											WHEN 1 THEN 
												'Cash Discount' 
											WHEN 2 THEN 
												'Percent Discount' 
											WHEN 3 THEN 
												'Free Goods' 
										END 
									ELSE 
										CASE 
											WHEN oh.DiscountType = 2 THEN 
												'Percent Discount' 
										END
								  END,
		[QuantityDiscount]		= CASE 
									WHEN oi.DiscountType > 0 THEN oi.QuantityDiscount
									ELSE	CASE 
												WHEN oh.DiscountType > 0 THEN oh.QuantityDiscount 
											END 
								  END,
		[StatusCode]			= '' 
	FROM 
		@Orders o
		INNER JOIN	dbo.OrderHeader			(nolock) oh		ON	o.OrderHeader_ID					= oh.OrderHeader_ID		
		INNER JOIN	dbo.Vendor				(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID				
		INNER JOIN	dbo.Vendor				(nolock) vr		ON	oh.ReceiveLocation_ID				= vr.Vendor_ID			
		INNER JOIN	dbo.OrderInvoice		(nolock) oiv	ON	oh.OrderHeader_ID					= oiv.OrderHeader_ID		
		INNER JOIN	dbo.OrderItem			(nolock) oi		ON	o.OrderHeader_ID					= oi.OrderHeader_ID		
														AND o.OrderItem_ID							= oi.OrderItem_ID			
		INNER JOIN	dbo.ItemIdentifier		(nolock) ii		ON	oi.Item_Key							= ii.Item_Key				
														AND ii.Default_Identifier					= 1
		INNER JOIN	dbo.ItemVendor			(nolock) iv		ON	oi.Item_Key							= iv.Item_Key				
														AND oh.Vendor_ID							= iv.Vendor_ID			
		INNER JOIN	dbo.Item				(nolock) i		ON	oi.Item_Key							= i.Item_Key				
		INNER JOIN	dbo.ItemUnit			(nolock) qu		ON	oi.QuantityUnit						= qu.Unit_ID				
		INNER JOIN	dbo.ItemUnit			(nolock) iu		ON	oi.InvoiceQuantityUnit				= iu.Unit_ID				
		LEFT JOIN	dbo.OrderExternalSource (nolock) oes	ON	oh.OrderExternalSourceID			= oes.ID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) car	ON	oh.ReasonCodeDetailID				= car.ReasonCodeDetailID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rcd	ON	oi.ReasonCodeDetailID				= rcd.ReasonCodeDetailID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rrc	ON	oi.ReceivingDiscrepancyReasonCodeID = rrc.ReasonCodeDetailID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) prc	ON	oi.ResolutionCodeID					= prc.ReasonCodeDetailID 
	WHERE 
		oi.ResolutionCodeID IS NOT NULL
		AND oh.UploadedDate IS NOT NULL
	
	UNION
	
	-- Output #2
	SELECT
		[OrderHeader_ID]		= oh.OrderHeader_ID,
		[Vendor_ID]				= v.Vendor_ID,
		[CompanyName]			= v.CompanyName, 
		[InvoiceNumber]			= oh.InvoiceNumber, 
		[InvoiceDate]			= oh.InvoiceDate, 
		[InvoiceTotal]			= o.InvoiceTotal,
		[Store_Name]			= vr.CompanyName,
		[POType]				= oes.Description,
		[PONumber]				= oh.OrderHeader_ID,
		[PODate]				= oh.CloseDate,
		[PaymentTotal]			= o.PaymentTotal,
		[upc]					= ei.upc,
		[VIN]					= ei.vendor_Item_num,
		[Descrip]				= ei.Descrip,
		[case_pack]				= ei.case_pack,
		[OrderUOM]				= '0',
		[OrderUOMCost]			= 0,
		[InvoiceUOMCost]		= ei.unit_cost,
		[ReceivedQTY]			= 0,
		[ShippedQTY]			= ei.qty_shipped,
		[ReceivedItemCost]		= 0,
		[ext_cost]				= ei.ext_cost,
		[QuantityOrdered]		= oi.QuantityOrdered,
		[Unit_Name]				= iu.Unit_Name,
		[Expected_Date]			= oh.Expected_Date,
		[ReasonCode]			= rcd.ReasonCode,  -- cost adjustment reason orderitem level
		[CostAdjustCode]		= car.ReasonCode,  -- cost adjustment reason orderheader level
		[ReceivingReasonCode]	= rrc.ReasonCode,  -- receiving exception code orderitem level
		[ResolutionReasonCode]	= prc.ReasonCode,  -- PO Admin resolution code orderitem level
		[DiscountType]			= '',
		[QuantityDiscount]		= NULL,
		[StatusCode]			= CASE 
									WHEN ei.Item_Key IS NULL THEN 
										'NOID' 
									ELSE 
										'NORD' 
								END
	FROM 
		@Orders O
		INNER JOIN	dbo.OrderHeader			(nolock) oh		ON	o.OrderHeader_ID					= oh.OrderHeader_ID	
														AND o.OrderItem_ID							IS NULL
		INNER JOIN	dbo.OrderItem			(nolock) oi		ON	o.OrderHeader_ID					= oi.OrderHeader_ID	
														AND o.OrderItem_ID							= oi.OrderItem_ID		
		INNER JOIN	dbo.eInvoicing_Item		(nolock) ei		ON	oh.EInvoice_ID						= ei.EInvoice_ID		
		INNER JOIN	dbo.Vendor				(nolock) v		ON	oh.Vendor_ID						= v.Vendor_ID			
		INNER JOIN	dbo.Vendor				(nolock) vr		ON	oh.ReceiveLocation_ID				= vr.Vendor_ID		
		INNER JOIN	dbo.OrderInvoice		(nolock) oiv	ON	oiv.OrderHeader_ID					= oh.OrderHeader_ID
		INNER JOIN	dbo.ItemUnit			(nolock) iu		ON	oi.InvoiceQuantityUnit				= iu.Unit_ID			
		LEFT JOIN	dbo.OrderExternalSource (nolock) oes	ON	oh.OrderExternalSourceID			= oes.ID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) car	ON	oh.ReasonCodeDetailID				= car.ReasonCodeDetailID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rcd	ON	oi.ReasonCodeDetailID				= rcd.ReasonCodeDetailID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) rrc	ON	oi.ReceivingDiscrepancyReasonCodeID	= rrc.ReasonCodeDetailID 
		LEFT JOIN	dbo.ReasonCodeDetail	(nolock) prc	ON	oi.ResolutionCodeID					= prc.ReasonCodeDetailID 
	WHERE 
		(
			ei.IsNotIdentifiable	= 1 
			OR 
			ei.IsNotOrdered			= 1
		)
		AND oi.ResolutionCodeID IS NOT NULL
		AND oh.UploadedDate IS NOT NULL


 	--**************************************************************************
	-- Used in the Procurement to Payment (P2P) portion of IRMA.  Called
	-- from the ItemCatalogLib.Order.UpdateP2PInvoiceDiscrepancySentDate procedure.
	--**************************************************************************  
	IF @Vendor_ID IS NOT NULL AND @AutomatedReport = 1
		BEGIN
			UPDATE 
				OrderHeader
			SET
				InvoiceDiscrepancySentDate		= GETDATE(),
				InvoiceProcessingDiscrepancy	= 0
			WHERE  
				Vendor_ID							= @Vendor_ID
				AND InvoiceDiscrepancy				= 1
				AND InvoiceDiscrepancySentDate		IS NULL
				AND UploadedDate					IS NOT NULL
				AND	InvoiceProcessingDiscrepancy	= 1
		END
	
	SET NOCOUNT OFF
END
SET QUOTED_IDENTIFIER ON
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[InvoiceDiscrepanciesReport] TO [IRMAReportsRole]
    AS [dbo];

