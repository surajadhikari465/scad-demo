SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[POExceptionReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[POExceptionReport]
GO

CREATE PROCEDURE [dbo].[POExceptionReport]
    @BusinessUnit	int,
    @VendorNo		bigint,
    @Tolerance		int,
	@MinAmount		money,
	@DateStart		datetime,
	@DateEnd		datetime,
	@PSSubTeam		int
AS 
-- **************************************************************************
   -- Procedure: POExceptionReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 01/12/2009  BBB	Pulled SQL from RDL and made corrections requested by Sinclair;
   --					modified BU source, vendor source, updated VCH sub query, ST source
   -- 01/15/2009  BBB	Added StoreAbbr to BusUnit return
   -- 01/16/2009  BBB	Resolved issue with tolerance calculation and order of operations
   -- 01/19/2009  BBB	Added in all qualifier for BusUnit, Store, SubTeam
   -- 08/26/2010  BSR	Removed Cross Join for TFS #13330
   -- 2011/12/16  KM	Added oh.OrderedCost and oh.TotalPaidCost to Group By clause to fix build error
   -- 12/22/2011  BAS	Changed aggregation of OrderItem.ReceivedItemCost
   --					to new column OrderHeader.AdjustedReceivedCost per TFS 3744
  -- **************************************************************************
BEGIN
    SET NOCOUNT ON   
	--**************************************************************************
	-- Check for 'All' qualifiers
	--**************************************************************************
	IF @BusinessUnit = 0
		SET @BusinessUnit = NULL

	IF @PSSubTeam = 0
		SET @PSSubTeam = NULL

	IF @VendorNo = 0
		SET @VendorNo = NULL
		
	--**************************************************************************
	-- Execute query
	--**************************************************************************
	SELECT
		*,
		[Tolerance]			= ((POSysAmt - InvoiceAmt) / (InvoiceAmt))
	FROM
		(
		SELECT
			[PO]			=	oh.OrderHeader_ID,
			[PODate]		=	oh.OrderDate,
			[BusUnit]		=	CONVERT(varchar(20), s.BusinessUnit_ID) + '-' + s.StoreAbbr,
			[VendorID]		=	v.PS_Export_Vendor_ID,
			[VendorName]	=	v.CompanyName,
			[SubTeam]		=	sst.PS_SubTeam_No,
			[POSysAmt]		=	ISNULL(oh.OrderedCost, 0),
			[POSentAmt]		=	ISNULL(oh.TotalPaidCost, 0),
			[PORecdAmt]		=	ISNULL(oh.AdjustedReceivedCost, 0),
			[InvoiceAmt]	=	ov.InvoiceCost,
			[InvoiceNum]	=	oh.InvoiceNumber,
			[InvoiceDate]	=	oh.InvoiceDate

		FROM
			OrderHeader				(nolock) oh
			INNER JOIN OrderItem	(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID
			INNER JOIN OrderInvoice	(nolock) ov		ON	oh.OrderHeader_ID		= ov.OrderHeader_ID
			INNER JOIN Vendor		(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN Vendor		(nolock) vs		ON	oh.PurchaseLocation_ID	= vs.Vendor_ID
			INNER JOIN Store		(nolock) s		ON	vs.Store_No				= s.Store_No
			INNER JOIN SubTeam		(nolock) st		ON	oh.Transfer_To_Subteam	= st.SubTeam_No
			INNER JOIN StoreSubTeam	(nolock) sst	ON	s.Store_No				= sst.Store_No
													AND	oh.Transfer_To_Subteam	= sst.SubTeam_No
		WHERE
			s.BusinessUnit_ID			=	ISNULL(@BusinessUnit, s.BusinessUnit_ID)
			AND v.PS_Export_Vendor_ID	=	ISNULL(@VendorNo, v.PS_Export_Vendor_ID)
			AND oh.Transfer_To_SubTeam	=	ISNULL(@PSSubTeam, oh.Transfer_To_SubTeam)
			AND ov.InvoiceCost			>=	ISNULL(@MinAmount, ov.InvoiceCost)
			AND oh.CloseDate			>=	ISNULL(@DateStart, '1/1/1900')
			AND oh.CloseDate			<=	ISNULL(@DateEnd, '1/1/2020')
		
		) inner_result
	WHERE
		(((POSysAmt - InvoiceAmt) / (InvoiceAmt)) * 100) > ISNULL(@Tolerance, 0)
		OR
		(((POSysAmt - InvoiceAmt) / (InvoiceAmt)) * 100) < ISNULL((@Tolerance * -1), 0)


	SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
