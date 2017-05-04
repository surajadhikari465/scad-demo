CREATE PROCEDURE dbo.OutOfPeriodInvoiceReport
    @Store_No	int,
    @Subteam_No int,
    @Year		smallint,
    @Period		tinyint
AS 
   -- **************************************************************************
   -- Procedure: OutOfPeriodInvoiceReport()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from a single RDL file and generates a report consumed
   -- by SSRS procedures.
   --
   -- Modification History:
   -- Date        Init	Comment
   -- 12/04/2008  BBB	Isolated issue with Date table and passed on to SW
   -- 01/08/2009  BBB	Added in call to Date table to return end of period day
   --					to ensure that the report only returns items closed 
   --					in the requested period
   -- 01/19/2009  BBB	Updated InvoiceDate adjustment to -5days
   -- 11/13/2009  BBB	update existing SP to specifically declare table source 
   --					for BusinessUnit_ID column to prevent ambiguity between
   --					Store and Vendor table
   -- 10/29/2010  MY    Added storename
   -- 12/26/2011  BAS	Renamed file exension to .sql (TFS 3744)
   -- 12/30/2011  BAS	Updated to use new column OrderInvoice.InvoiceTotalCost (TFS 3744)
   -- 09/17/2013  MZ    13667 - Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
   -- **************************************************************************
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

----------------------------------------------
-- Get Period StartDate based upon input parameters
----------------------------------------------
DECLARE @FPStart	smalldatetime
SELECT 
	@FPStart = Date_Key
FROM	
	[Date] D
WHERE
	[Year]			= @Year 
	AND Period		= @Period
    AND [Week]		= 1 
	AND Day_Of_Week = 1

----------------------------------------------
-- Get Period EndDate based upon input parameters
----------------------------------------------
DECLARE @FPEnd		smalldatetime
SELECT 
	@FPEnd = Date_Key
FROM	
	[Date] D
WHERE
	[Year]			= @Year 
	AND Period		= @Period
    AND [Week]		= 4
	AND Day_Of_Week = 7

----------------------------------------------
--As per accounting Invoice date will be 5 days less than entered by user
----------------------------------------------
SELECT	
	[Unit]			=	s.BusinessUnit_ID,
	[StoreName]     =   s.Store_Name,
	[Ledger]		=	'ACTUALS',
	[Account]		=	'500000',
	[Team]			=	Team_No,
	[Dept]			=	oi.Subteam_No,
	[Proj]			=	NULL,
	[Aff]			=	NULL,
	[Curr]			=	'USD',
	[Amount]		=
						CASE 
							WHEN Return_Order = 1 THEN 
								-oi.InvoiceTotalCost
							ELSE 
								oi.InvoiceTotalCost
						END,
	[N/R]			=	'',
	[Rate]			=	'',
	[Rate Type]		=	'',			
	[Base Amount]	=	'', 
	[Stat]			=	'',
	[Stat Amount]	=	'',	
	[Description]	=	v.CompanyName  + ' INV#' + oh.InvoiceNumber,
	[PONumber]		=	oh.OrderHeader_ID,
	[RecvLogNo]		=	RecvLog_No,
	oh.InvoiceDate,
	oh.CloseDate,
	s.Store_No
FROM 
	OrderHeader			(nolock) oh
	JOIN Vendor			(nolock) c	ON c.Vendor_ID			= oh.PurchaseLocation_ID
	JOIN OrderInvoice	(nolock) oi	ON oi.OrderHeader_ID	= oh.OrderHeader_ID
	JOIN SubTeam		(nolock) st ON st.SubTeam_No		= oi.SubTeam_No
	JOIN Store			(nolock) s	ON s.Store_No			= c.Store_No
	JOIN Vendor			(nolock) v	ON v.Vendor_ID			= oh.Vendor_ID
WHERE 
	oh.InvoiceDate							<	DATEADD(day, -5, @FPStart)
	AND	oh.CloseDate						>=	@FPStart
	AND oh.CloseDate						<=	@FPEnd
	AND ISNULL(@Store_no,	s.Store_no)		=	s.Store_No
	AND	ISNULL(@SubTeam_no, st.SubTeam_No)	=	st.SubTeam_No
ORDER BY 
	s.BusinessUnit_ID, 
	Team_No, 
	oi.Subteam_No

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfPeriodInvoiceReport] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfPeriodInvoiceReport] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfPeriodInvoiceReport] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[OutOfPeriodInvoiceReport] TO [IRMAReportsRole]
    AS [dbo];

