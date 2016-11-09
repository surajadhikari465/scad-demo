CREATE PROCEDURE dbo.GetAPUpAppDailySum
@Begin_Date datetime,
@End_Date datetime,
@Store_No int
AS
-- **************************************************************************
-- Procedure: GetAPUpAppDailySum()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This procedure is called from a single SQL/RDL file and used to generate 
-- a specific report based upon the paramater passed to ApUpClosedReport.
--
-- Modification History:
-- Date        Init	Comment
-- 07/14/2009  BBB	Updated SP to be more readable; Added join to Currency 
--					table to return CurrencyCode to report
-- 02/01/2010  MU	updated to exclude allocated charges per tfs 11881
-- 12/23/2011  BAS	Coding standards
-- **************************************************************************
BEGIN
    SET NOCOUNT ON
   	
   	--**************************************************************************
	-- Set internal variables
	--**************************************************************************
   	DECLARE @AllocatedChargeTypeID int
		
	SELECT @AllocatedChargeTypeID = SACType_Id
	FROM EInvoicing_SACTypes 
	WHERE SACType = 'Allocated'
	

	--**************************************************************************
	-- Main SQL
	--**************************************************************************
	SELECT 
		Store_Name,
		s.BusinessUnit_ID,
		RecvLog_No,
		RecvLogDate,
		[VendorName]	= v.CompanyName, 
		InvoiceDate, 
		[Invoice_No]	= InvoiceNumber, 
		[InvoiceCost]	= (ISNULL(ov.InvoiceCost, 0)+SUM(ISNULL(oic.Value,0))) * (CASE WHEN Return_Order = 1 THEN -1 ELSE 1 END),
		[InvoiceFreight] = InvoiceFreight * (CASE WHEN Return_Order = 1 THEN -1 ELSE 1 END),
		sst.Team_No, 
		ov.SubTeam_No, 
		[PO_No]			= oh.OrderHeader_ID,
		CurrencyCode
		
	FROM
		OrderHeader						(NOLOCK) oh
		INNER JOIN	OrderItem			(NOLOCK) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
		INNER JOIN	OrderInvoice		(NOLOCK) ov		ON	oh.OrderHeader_ID		= ov.OrderHeader_ID 
		LEFT JOIN	OrderInvoiceCharges (NOLOCK) oic	ON	oh.OrderHeader_ID		= oic.OrderHeader_ID 
														AND oic.SACType_ID			!= @AllocatedChargeTypeID
		INNER JOIN	Vendor				(NOLOCK) v		ON	oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN	Vendor				(NOLOCK) vs		ON	oh.ReceiveLocation_ID	= vs.Vendor_ID
		INNER JOIN	Store				(NOLOCK) s		ON	vs.Store_No				= s.Store_No
		INNER JOIN	StoreSubTeam		(NOLOCK) sst	ON	s.Store_No				= sst.Store_No 
														AND ov.SubTeam_No			= sst.SubTeam_No
		LEFT JOIN	Currency			(nolock) c		ON	oh.CurrencyID			= c.CurrencyID
		
	WHERE 
		(Transfer_SubTeam	IS NULL)
		AND (UploadedDate	IS NULL)
		AND (v.WFM			= 0)
		AND (v.PS_Vendor_ID				IS NOT NULL 
			AND v.PS_Location_Code		IS NOT NULL 
			AND v.PS_Address_Sequence	IS NOT NULL)
		AND (s.Store_No					= @Store_No 
			OR @Store_No	IS NULL)
		AND oh.ApprovedDate	>= ISNULL(@Begin_Date, oh.ApprovedDate)
		AND oh.ApprovedDate	< DATEADD(d, 1, ISNULL(@End_Date, oh.ApprovedDate))
		
	GROUP BY 
		Store_Name,
		s.BusinessUnit_ID,
		RecvLog_No,
		RecvLogDate,
		v.CompanyName, 
		InvoiceDate,
		VendorDocDate, 
		InvoiceNumber,
		VendorDoc_ID,
		ov.InvoiceCost,
		Return_Order,
		InvoiceFreight,
		sst.Team_No, 
		ov.SubTeam_No, 
		oh.OrderHeader_ID,
		CurrencyCode
		
	ORDER BY
		CurrencyCode DESC
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAppDailySum] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAppDailySum] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAppDailySum] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAppDailySum] TO [IRMAReportsRole]
    AS [dbo];

