SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InvoiceManifestReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InvoiceManifestReport]
GO

CREATE PROCEDURE dbo.InvoiceManifestReport 
	@FromVendor			int,
	@ToVendor			int,
	@StartDate			DateTime,
	@EndDate			DateTime,
	@Distribution_Only	bit,
	@SubTeam_No			int
AS 
-- **************************************************************************
-- Procedure: InvoiceManifestReport
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2009/07/14	DS		10358	Added SubTeam_No variable
-- 2011/12/21	KM		3744	Added update history template; change file extension; coding standards;
-- 2012/01/05	KM		3744	Replaced aggregation SUM(oi.ReceivedItemCost) with new column oh.AdjustedReceivedCost;
-- 09/18/2013   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- **************************************************************************
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT @SubTeam_No = ISNULL(@Subteam_No, 0)

	SELECT
		oh.OrderHeader_ID, 
		oh.OrderDate, 
		oh.CloseDate, 
		oh.DiscountType, 
		oh.QuantityDiscount,
		VendorName			= v.CompanyName,
		ReceiverName		= rv.CompanyName,
		st.SubTeam_No, 
		st.SubTeam_Name,
		OrderCost			= CASE WHEN Return_Order = 0 THEN 1 ELSE -1 END * oh.AdjustedReceivedCost, 
		OrderFreight		= SUM(CASE WHEN Return_Order = 0 THEN 1 ELSE -1 END * oi.ReceivedItemFreight),
		OrderTotal			= (oh.AdjustedReceivedCost + SUM(oi.ReceivedItemFreight)) * CASE WHEN Return_Order = 0 THEN 1 ELSE -1 END 
	FROM
		OrderItem				(nolock) oi
		INNER JOIN OrderHeader	(nolock) oh		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
		INNER JOIN Item			(nolock) i		ON	oi.Item_Key				= i.Item_Key 
		INNER JOIN SubTeam		(nolock) st		ON	(CASE
														WHEN Transfer_SubTeam IS NULL THEN i.SubTeam_No 
														ELSE ISNULL(Transfer_To_SubTeam, i.SubTeam_No)
													END)					= st.SubTeam_No 
		INNER JOIN Vendor		(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID 
		INNER JOIN Vendor		(nolock) rv		ON	oh.ReceiveLocation_ID	= rv.Vendor_ID 
	WHERE 
		CloseDate					>=	@StartDate
		AND CloseDate				<	@EndDate 
		AND (Transfer_SubTeam		IS NOT NULL OR @Distribution_Only = 0)
		AND oh.Vendor_ID			=	ISNULL(@FromVendor, oh.Vendor_ID) 
		AND oh.ReceiveLocation_ID	=	ISNULL(@ToVendor, oh.ReceiveLocation_ID) 
		AND @Subteam_No				=	(CASE
											WHEN (@Subteam_No = 0 OR @Subteam_No = NULL) THEN @Subteam_No 
											ELSE ISNULL(Transfer_To_SubTeam, i.SubTeam_No) 
										END)
	GROUP BY 
		oh.OrderHeader_ID, 
		oh.OrderDate, 
		oh.CloseDate, 
		oh.DiscountType, 
        oh.QuantityDiscount, 
        v.CompanyName, 
        rv.CompanyName, 
        st.SubTeam_No, 
        st.SubTeam_Name,
        oh.Return_Order,
        oh.AdjustedReceivedCost
	ORDER BY
		rv.CompanyName,
		v.CompanyName,
		st.SubTeam_Name,
		oh.OrderHeader_ID
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO