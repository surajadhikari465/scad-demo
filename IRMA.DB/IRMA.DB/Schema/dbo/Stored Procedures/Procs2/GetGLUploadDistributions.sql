CREATE PROCEDURE [dbo].[GetGLUploadDistributions]
    @Store_No		int,
    @CurrDate		datetime	= null,
	@StartDate		datetime	= null,
	@EndDate		datetime	= null,
	@IsUploaded		bit			= 0,
	@AutoGLUpload	bit			= 0
AS
   
   -- **************************************************************************
   -- Procedure: GetGLUploadDistributions()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from the Replenishment module GLDAO.vb
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 2011/12/21	KM		3744	extension change
   -- 12/08/2010	DBS				Shaved off a day
   -- 09/06/2010	BSR				Moved the TransferUnit to the last column of output
   -- 08/02/2010	BBB				Added TransferUnit as alias output of Unit for consumption
   -- 04/06/2010	BBB				Removed VendorSubTeam from query
   -- 03/22/2010	BBB				Moved union of credit/debit queries to temp tables 
   --								so as to eliminate any PO that was not in the other
   -- 03/09/2010	BBB				Updated final output to ignore any rows with Account=0
   -- 03/03/2010	RDS				Modify this proc to take date parameters when provided;
   --								Ensure proper PS Dept and Product values are included
   -- 12/15/2009	BBB				updated to include IsNull option to retrieve value from 
   --								VendorSubTeam if it exists
   -- 11/16/2009	BBB				Updated for readability and coding standard
   -- **************************************************************************

BEGIN
    SET NOCOUNT ON
	--**************************************************************************
	-- Set internal variables
	--**************************************************************************
	IF (@StartDate IS NULL OR @EndDate IS NULL) AND @AutoGLUpload = 0
		BEGIN
			-- previous Monday thru Sunday
			SELECT @StartDate	= CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 1 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())) - 6, ISNULL(@CurrDate, GETDATE())), 101)),
				   @EndDate		= CONVERT(datetime, CONVERT(varchar(255), DATEADD("day", 2 - DATEPART(dw, ISNULL(@CurrDate, GETDATE())), ISNULL(@CurrDate, GETDATE())), 101))
		END
	ELSE IF @AutoGLUpload = 1
		BEGIN
			SELECT @StartDate	= CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101)),
				   @EndDate		= CONVERT(datetime, CONVERT(varchar(255), GETDATE()+1, 101))
		END

    DECLARE @Orders TABLE (OrderHeader_ID int PRIMARY KEY)
    
    DECLARE @Debits TABLE 
					(
					SubTeam		int,
					Account		int,
					Unit		int,
					DeptID		int,
					Product		int,
					Amount		decimal(9,2),
					Description	varchar(18)
					)

    DECLARE @Credits TABLE 
					(
					SubTeam		int,
					Account		int,
					Unit		int,
					DeptID		int,
					Product		int,
					Amount		decimal(9,2),
					Description	varchar(18)
					)

	--**************************************************************************
	-- Load temp table
	-- With Distribution Order Types that have not yet been uploaded
	-- Where Customer is not external and Vendor is Regional
	--**************************************************************************
	IF @IsUploaded = 0
		BEGIN
			INSERT INTO @Orders
				SELECT
					oh.OrderHeader_ID
				FROM 
					OrderHeader				(nolock) oh
					INNER JOIN	OrderItem	(nolock) oi		ON oi.OrderHeader_ID		= oh.OrderHeader_ID
					INNER JOIN	Vendor		(nolock) v		ON oh.Vendor_ID				= v.Vendor_ID
					INNER JOIN	Store		(nolock) sv		ON sv.Store_No				= v.Store_No
					INNER JOIN	Vendor		(nolock) vr		ON oh.ReceiveLocation_ID	= vr.Vendor_ID
					INNER JOIN	Store		(nolock) sr		ON vr.Store_No				= sr.Store_No 
				WHERE 
					oh.OrderType_ID																=	2
					AND oh.AccountingUploadDate													IS	NULL
					AND dbo.fn_GetCustomerType(sr.Store_No, sr.Internal, sr.BusinessUnit_ID)	<>	1  
					AND dbo.fn_VendorType(v.PS_Vendor_ID, v.WFM, v.Store_No, sv.Internal)		=	3
					AND v.Store_No																=	ISNULL(@Store_No, v.Store_No)
					AND oh.CloseDate															IS	NOT NULL
				GROUP BY 
					oh.OrderHeader_ID
				HAVING 
					MIN(oi.DateReceived)		>= @StartDate 
					AND MIN(oi.DateReceived)	< @EndDate		
		END

	--**************************************************************************
	-- Load temp table
	-- With Distribution Order Types that have already been uploaded
	-- Where Customer is not external and Vendor is Regional
	--**************************************************************************
	ELSE
		BEGIN
			INSERT INTO @Orders
				SELECT 
					oh.OrderHeader_ID
				FROM 
					OrderHeader				(nolock) oh
					INNER JOIN	OrderItem	(nolock) oi		ON oi.OrderHeader_ID		= oh.OrderHeader_ID
					INNER JOIN	Vendor		(nolock) v		ON oh.Vendor_ID				= v.Vendor_ID
					INNER JOIN	Store		(nolock) sv		ON sv.Store_No				= v.Store_No
					INNER JOIN	Vendor		(nolock) vr		ON oh.ReceiveLocation_ID	= vr.Vendor_ID
					INNER JOIN	Store		(nolock) sr		ON vr.Store_No				= sr.Store_No 
				WHERE 
					oh.OrderType_ID																=	2
					AND oh.AccountingUploadDate													IS	NOT NULL
					AND dbo.fn_GetCustomerType(sr.Store_No, sr.Internal, sr.BusinessUnit_ID)	<>	1  
					AND dbo.fn_VendorType(v.PS_Vendor_ID, v.WFM, v.Store_No, sv.Internal)	=	3
					AND v.Store_No																=	ISNULL(@Store_No, v.Store_No)
					AND oh.CloseDate															IS	NOT NULL
				GROUP BY 
					oh.OrderHeader_ID
				HAVING 
					MIN(oi.DateReceived)		>= @StartDate 
					AND MIN(oi.DateReceived)	< @EndDate			
		END

	--**************************************************************************
	-- Load temp debits table
	--**************************************************************************
	INSERT INTO @Debits
			SELECT
			[SubTeam]		=	st.SubTeam_No,
			[Account]		=	CASE 
									WHEN sr.Regional = 1 AND st.SubTeam_No = 9600 THEN 
										ISNULL(CONVERT(varchar(255), z.GLMarketingExpenseAcct), 0) 
									ELSE 
										ISNULL(st.GLDistributionAcct, 0)
								END,
			[Unit]			=	sr.BusinessUnit_ID,
			[DeptID]		=	ISNULL(sst.PS_Team_No, ''),
			[Product]		=	ISNULL(sst.PS_SubTeam_No, ''),
			[Amount]		=	ROUND(SUM((oi.ReceivedItemCost + oi.ReceivedItemFreight) * (CASE WHEN oh.Return_Order = 1 THEN -1 ELSE 1 END)),2),
			[Description]	=	CONVERT(varchar(18), oh.OrderHeader_ID)
		FROM 
			OrderHeader					(nolock) oh
			INNER JOIN	@Orders					 o		ON	o.OrderHeader_ID		= oh.OrderHeader_ID
			INNER JOIN	OrderItem		(nolock) oi		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
			INNER JOIN	Vendor			(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN	Store			(nolock) sv		ON	sv.Store_No				= v.Store_No
			INNER JOIN	Vendor			(nolock) vr		ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
			LEFT JOIN	Store			(nolock) sr		ON	vr.Store_No				= sr.Store_No
			INNER JOIN	SubTeam			(nolock) st		ON	oh.Transfer_To_SubTeam	= st.SubTeam_No
			LEFT JOIN	StoreSubTeam	(nolock) sst	ON	sr.Store_No				= sst.Store_No 
														AND	st.SubTeam_No			= sst.SubTeam_No
			LEFT JOIN	Zone			(nolock) z		ON	sv.Zone_ID				= z.Zone_ID	
		GROUP BY 
			st.GLDistributionAcct, 
			sr.BusinessUnit_ID, 
			sr.Regional, 
			sst.PS_Team_No, 
			sst.PS_SubTeam_No,
			st.SubTeam_No, 
			v.CompanyName, 
			z.GLMarketingExpenseAcct, 
			oh.InvoiceNumber, 
			oh.OrderHeader_ID
			
	--**************************************************************************
	-- Load temp credits table
	--**************************************************************************
	INSERT INTO @Credits
			SELECT
			[SubTeam]		=	oh.Transfer_SubTeam,
			[Account]		=	MAX(CASE 
										WHEN (sv.Mega_Store = 1 OR sv.WFM_Store = 1) AND (sr.Mega_Store = 1 OR sr.WFM_Store = 1 OR sr.Regional = 1) THEN 
											ISNULL(st.GLDistributionAcct, 0) 
										ELSE 
											ISNULL(st.GLSalesAcct, 0) 
									END), 
			[Unit]			=	sv.BusinessUnit_ID,
			[DeptID]		=	ISNULL(sst.PS_Team_No, ''),
			[Product]		=	ISNULL(sst.PS_SubTeam_No, ''),
			[Amount]		=	ROUND(SUM((oi.ReceivedItemCost + oi.ReceivedItemFreight) * (CASE WHEN oh.Return_Order = 1 THEN -1 ELSE 1 END) * -1),2),
			[Description]	=	CONVERT(varchar(18), oh.OrderHeader_ID)
		FROM
			OrderHeader					(nolock) oh
			INNER JOIN	@Orders					 o		ON	o.OrderHeader_ID		= oh.OrderHeader_ID
			INNER JOIN	OrderItem		(nolock) oi		ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
			INNER JOIN	Vendor			(nolock) v		ON	oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN	Store			(nolock) sv		ON	sv.Store_No				= v.Store_No
			INNER JOIN	Vendor			(nolock) vr		ON	oh.ReceiveLocation_ID	= vr.Vendor_ID
			LEFT JOIN	Store			(nolock) sr		ON	vr.Store_No				= sr.Store_No
			INNER JOIN	SubTeam			(nolock) st		ON	oh.Transfer_SubTeam		= st.SubTeam_No
			LEFT JOIN	StoreSubTeam	(nolock) sst	ON	sv.Store_No				= sst.Store_No 
														AND oh.Transfer_SubTeam		= sst.SubTeam_No
		GROUP BY 
			sv.BusinessUnit_ID, 
			sst.PS_Team_No, 
			sst.PS_SubTeam_No,
			oh.Transfer_SubTeam, 
			oh.OrderHeader_ID,
			st.SubTeam_No

	--**************************************************************************
	-- Load primary data based upon data within temp table
	--**************************************************************************
	SELECT
		[SubTeam],
		[Account],
		[Unit],
		[DeptID],
		[Product],
		[Amount],
		[Description],
		[TransferUnit] = [Unit]
	FROM
		(
		
		(SELECT * FROM @Debits WHERE Description IN (SELECT Description FROM @Credits WHERE Account <> 0))
	    
		UNION ALL

		(SELECT * FROM @Credits WHERE Description IN (SELECT Description FROM @Debits WHERE Account <> 0))

		) inner_result
	WHERE
		Account <> 0
	ORDER BY 
		[Unit], 
		[Account], 
		[DeptID], 
		[Product]

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadDistributions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadDistributions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadDistributions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadDistributions] TO [IRMAReportsRole]
    AS [dbo];

