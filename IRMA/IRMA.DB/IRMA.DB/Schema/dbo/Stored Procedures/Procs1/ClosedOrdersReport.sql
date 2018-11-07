CREATE PROCEDURE [dbo].[ClosedOrdersReport]
@SubTeam_No int,
@StartDate datetime,
@EndDate datetime,
@InternalCustomer int,
@Vendor_Id int

AS

-- **************************************************************************
	-- Procedure: ClosedOrdersReport()
	--    Author: n/a
	--      Date: n/a
	--
	-- Modification History:
	-- Date			Init	Comment
	-- 11/2/2007	HH		Changed @StartDate and @EndDate from varchar datatype to smalldatetime
	-- 12/14/2011	BS		Coding Standards.  Verified Line Item Received Cost calculation.
-- **************************************************************************

BEGIN
    SET NOCOUNT ON
    
    SELECT 
		[VendorName]			=	v.CompanyName,
		[PurchaserName]			=	vp.CompanyName,
		oh.OrderHeader_ID,
		oh.OrderDate,
		oh.CloseDate,
		ii.Identifier, 
		i.Item_Description,
		iog.Origin_Name, 
		oi.Package_Desc1, 
		oi.Package_Desc2, 
		[Package_Unit]			=	iup.Unit_Name,
		[QuantityOrdered]		=	CONVERT(INT,
												CASE 
													WHEN Return_Order = 0 THEN 
														oi.QuantityOrdered
													ELSE 
														-oi.QuantityOrdered
												END),
		[QuantityReceived]		=	CONVERT(INT,
												CASE 
													WHEN Return_Order = 0 THEN 
														oi.QuantityReceived
													ELSE
														-oi.QuantityReceived
												END),
		[Total_Weight]			=	CASE 
										WHEN Return_Order = 0 THEN
											oi.Total_Weight
										ELSE 
											-oi.Total_Weight
									END,
		[QuantityUnit]			=	iuq.Unit_Name,
		[CaseCost]				=	(oi.UnitCost * oi.Package_Desc1),
		[FreightCost]			=	((oi.UnitExtCost - oi.UnitCost) * oi.Package_Desc1),
		[ReceivedItemCost]		=	CASE 
										WHEN Return_Order = 0 THEN
											ReceivedItemCost
										ELSE
											-ReceivedItemCost
									END, 
		[ReceivedItemFreight]	=	CASE
										WHEN Return_Order = 0 THEN
											ReceivedItemFreight
										ELSE
											-ReceivedItemFreight
									END,
		[ReceivedItemExtCost]	=	CASE 
										WHEN Return_Order = 0 THEN
											(ReceivedItemCost + ReceivedItemFreight)
										ELSE
											-(ReceivedItemCost + ReceivedItemFreight)
									END, 
		oh.InvoiceNumber,
		[InvoiceCost]			=	CASE 
										WHEN Return_Order = 0 THEN
											(isnull(oi.InvoiceCost, 0) + isnull(oiv.InvoiceFreight,0))
										ELSE
											-(isnull(oi.InvoiceCost, 0) + isnull(oiv.InvoiceFreight,0))
									END,
		[CloseByFullName]		=	ISNULL(FullName, '[NA]')
	
	
    FROM 
		OrderItem						(nolock) oi
        INNER JOIN OrderHeader			(nolock) oh		ON	oi.OrderHeader_ID = oh.OrderHeader_ID
        INNER JOIN Item					(nolock) i		ON	oi.Item_Key = i.Item_Key
        INNER JOIN ItemIdentifier		(nolock) ii		ON	i.Item_Key = ii.Item_Key
														AND ii.Default_Identifier = 1
        INNER JOIN Vendor				(nolock) v		ON	oh.Vendor_ID = v.Vendor_ID
        INNER JOIN Vendor				(nolock) vp		ON	oh.PurchaseLocation_ID = vp.Vendor_ID
        INNER JOIN SubTeam				(nolock) st		ON	(st.SubTeam_No =(
																			CASE 
																				WHEN Transfer_SubTeam IS NOT NULL THEN
																					i.SubTeam_No 
																				ELSE
																					ISNULL(Transfer_To_SubTeam, i.SubTeam_No)
																			END
																			))
        LEFT JOIN ItemOrigin			(nolock) iog	ON i.Origin_ID = iog.Origin_ID 
        LEFT JOIN ItemUnit				(nolock) iuq	ON oi.QuantityUnit = iuq.Unit_ID 
        LEFT JOIN ItemUnit				(nolock) iup	ON oi.Package_Unit_ID = iup.Unit_ID
        LEFT JOIN OrderInvoice			(nolock) oiv	ON oh.OrderHeader_ID = oiv.OrderHeader_ID
        LEFT JOIN Users					(nolock) u		ON oh.ClosedBy = u.User_ID
		  
    WHERE 
		st.SubTeam_No					= ISNULL(@SubTeam_No, st.SubTeam_No)
		AND vp.Vendor_Id				= ISNULL(@Vendor_Id, vp.Vendor_Id)
		AND closedate					>= ISNULL(@StartDate, oh.closedate)
		AND closedate					<= ISNULL(@EndDate, oh.closedate)
		AND v.InternalCustomer			<= @InternalCustomer
		      
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClosedOrdersReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClosedOrdersReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClosedOrdersReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ClosedOrdersReport] TO [IRMAReportsRole]
    AS [dbo];

