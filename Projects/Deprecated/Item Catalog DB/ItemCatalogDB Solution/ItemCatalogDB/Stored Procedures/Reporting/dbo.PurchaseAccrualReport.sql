SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PurchaseAccrualReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PurchaseAccrualReport]
GO

CREATE PROCEDURE [dbo].[PurchaseAccrualReport]
    @Store_No	int,
    @Subteam_No int,
    @AsOfDate	datetime

AS

-- **************************************************************************
-- Procedure: PurchaseAccrualReport
--    Author: Sekhara Kothuri
--      Date: 2007/10/01
--
-- Description:
-- Returns the output of records related to Purchase Accrual Report.
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/21	KM		3744	Coding standards;
-- 2011/12/22	BAS		3744	changed aggregate of OrderItem.ReceivedItemCost
--								to consume the new column OrderHeader.AdjustedReceivedCost
-- 2013/09/13   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- ************************************************************************** 

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON
    
	--Finding Purchase Location Id(Vendor_ID) for the store.
	DECLARE @PLoc_ID int

	SELECT
		@PLoc_ID=Vendor_ID 
	FROM 
		Vendor 
	WHERE 
		Store_no=@Store_No 

	SELECT 
		'Store'				= @Store_No,
		'BusinessUnitId'	= S.BusinessUnit_Id,
		'AsOfDate'			= @AsOfDate,
		'Invoice Num'		= OH.InvoiceNumber,
		'PO'				= OH.OrderHeader_Id,
		'Sub Team'			= OH.Transfer_To_SubTeam,
		'TeamNo'			= ST.Team_No,
		'Rcv Log Date'		= OH.RecvLogDate,
		'Close Date'		= OH.CloseDate,
		'Last Rcv Date'		= MAX(OI.DateReceived),
		'Invoice Num'		= OH.InvoiceNumber,
		'Vendor'			= OH.Vendor_ID,
		'Receiver'			= OH.RecvLog_No,
		'Vendor Name'		= V.CompanyName,
		OH.Ordertype_ID,
		'Rcvd Cost'			= oh.AdjustedReceivedCost
		
	FROM 
		OrderHeader				(nolock) oh 
		INNER JOIN OrderItem	(nolock) oi		ON	OH.OrderHeader_ID		= OI.OrderHeader_ID
		INNER JOIN Vendor		(nolock) v		ON	OH.Vendor_ID			= V.Vendor_Id 
		INNER JOIN SubTeam		(nolock) st		ON	OH.Transfer_To_SubTeam	= ST.SubTeam_No 
		INNER JOIN Store		(nolock) s		ON	@Store_No				= S.Store_No 
		
	WHERE
		(OI.QuantityReceived > 0  
		AND (OH.CloseDate <= @AsOfDate OR OH.CloseDate IS NULL) 
		AND OH.purchaseLocation_id = @PLoc_ID
		AND OH.Transfer_To_SubTeam = ISNULL(@Subteam_No, OH.Transfer_To_SubTeam)
		AND ((OH.Invoicenumber IS NULL OR OH.Invoicedate IS NULL) OR (OH.CloseDate IS NOT NULL AND OH.ApprovedDate IS NULL))
		AND OH.ordertype_id = 1)
		
	GROUP BY 
		OH.purchaseLocation_id,
		OH.InvoiceNumber,
		OH.OrderHeader_Id,
		OH.Transfer_To_SubTeam,
		OH.RecvLogDate,
		OH.CloseDate,
		OH.Vendor_ID,
		OH.RecvLog_No,
		V.CompanyName,
		OH.Ordertype_ID,
		S.BusinessUnit_ID,
		ST.Team_No,
		oh.AdjustedReceivedCost

	SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO