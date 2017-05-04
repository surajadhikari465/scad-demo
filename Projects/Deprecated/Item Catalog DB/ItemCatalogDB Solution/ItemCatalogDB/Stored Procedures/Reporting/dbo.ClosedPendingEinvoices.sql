
/****** Object:  StoredProcedure [dbo].[ClosedPendingEinvoices]    Script Date: 05/02/2012 18:08:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ClosedPendingEinvoices]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ClosedPendingEinvoices]
GO

/****** Object:  StoredProcedure [dbo].[ClosedPendingEinvoices]    Script Date: 05/02/2012 18:08:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ClosedPendingEinvoices] 
AS 
-- **************************************************************************
-- Procedure: ClosedPendingEinvoices
--    Author: Mugdha Deshpande
--      Date: 2011/08/08
--
-- Description:
-- This stored procedure lists all Closed POs for 
-- EInvoice Required Vendors that have items received and are 
-- closed with a Document type of None.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/21	KM		3744	coding standards;
-- 2011/12/23	BAS		3744	removed aggregation of Received Item Cost and change it 
--								to consume new column OrderHeader.AdjustedReceivedCost
-- 2012/05/02	TD		6115	add ReturnOrder column to report. 
-- **************************************************************************

BEGIN
    
	SET NOCOUNT ON
	
	SELECT
		VendorID				=	v.Vendor_ID,
		VendorName				=	v.CompanyName,
		PONumber				=	oh.OrderHeader_ID,
		SumReceivedItemCost		=	oh.AdjustedReceivedCost,
		ClosedBy				=	u.UserName,
		CloseDate				=	oh.CloseDate,
		ReturnOrder				=	oh.Return_Order 	
	FROM
		OrderHeader				(nolock) oh
		INNER JOIN Vendor		(nolock) v	ON oh.Vendor_ID		= v.Vendor_ID
		INNER JOIN Users		(nolock) u	ON oh.ClosedBy		= u.User_ID
	WHERE 
		oh.CloseDate				IS NOT NULL 
		AND oh.ApprovedDate			IS NULL
		AND oh.InvoiceNumber		IS NULL
		AND oh.Einvoice_ID			IS NULL
		AND v.EinvoiceRequired		= 1	
		AND oh.AdjustedReceivedCost > 0
	ORDER BY
		oh.CloseDate DESC
	
	SET NOCOUNT OFF
END
GO


