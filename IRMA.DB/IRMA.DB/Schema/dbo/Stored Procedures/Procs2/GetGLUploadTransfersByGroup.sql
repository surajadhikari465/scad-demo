
CREATE PROCEDURE [dbo].[GetGLUploadTransfersByGroup]
    @CurrDate	datetime	= NULL,
	@Region_Code varchar(3)
AS
-- **************************************************************************
-- Procedure: GetGLUploadTransfersByGroup

-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 03/21/2013   FA      11384   Initial version     
-- 03/21/2013	FA		11384	Added logic for same store transfer
-- 03/22/2013	FA		11384	Added logic to return the data for prior week
-- 03/25/2013	FA		11384	Fixed the grouping logic of GL lines
-- 03/28/2013   FA      11384   Added Currency column in the SELECT
-- 04/05/2013	FA		11838	Modified the SQL to do grouping only for in-store transfers
-- 03/07/2016   FA      14545   Added region parameter to filter transfers by region as RM database now hosts both RM and TS regions
-- **************************************************************************
BEGIN
	SET NOCOUNT ON
    
    DECLARE		@TransferBookedDate DateTime
	DECLARE		@StartDate			DateTime
	DECLARE		@EndDate			DateTime

	IF @CurrDate is null
		SELECT	@CurrDate = GETDATE()

	SELECT	 @TransferBookedDate = @CurrDate
 
 	-- previous monday through Sunday  
	SELECT
		@StartDate = CONVERT ( datetime,CONVERT ( varchar(255),DATEADD ( day , 1 - DATEPART ( dw , ISNULL ( @CurrDate , GETDATE ( ) ) ) - 6 , ISNULL ( @CurrDate , GETDATE ( ) ) ),101 ) ) ,
		@EndDate = CONVERT ( datetime,CONVERT ( varchar(255),DATEADD ( day , 2 - DATEPART ( dw , ISNULL ( @CurrDate , GETDATE ( ) ) ) , ISNULL ( @CurrDate , GETDATE ( ) ) ),101 ) )
		
    --select @startDate, @EndDate

    --SELECT @StartDate = '01/01/2011'
    --SELECT @EndDate = '03/28/2013'
     
	DECLARE @Orders TABLE ( OrderHeader_ID int PRIMARY KEY )

	INSERT  INTO
		@Orders 
		SELECT
			oh.OrderHeader_ID
		FROM
			OrderHeader				        (nolock) oh
            INNER JOIN OrderItem	        (nolock) oi	 ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
            INNER JOIN Vendor		        (nolock) v	 ON	oh.Vendor_ID			= v.Vendor_ID
            INNER JOIN Store		        (nolock) sv	 ON	v.Store_No				= sv.Store_No
  	        INNER JOIN StoreRegionMapping   (nolock) srm ON sv.Store_No             = srm.Store_no AND @Region_Code = srm.Region_Code	
            INNER JOIN Vendor		(nolock) rv	ON	oh.ReceiveLocation_ID	= rv.Vendor_ID
            INNER JOIN Store		(nolock) sr	ON	rv.Store_No				= sr.Store_No
		WHERE				
			oh.OrderType_ID = 3
			AND oh.AccountingUploadDate IS NULL
			AND dbo.fn_GetCustomerType (sr.Store_No, sr.Internal, sr.BusinessUnit_ID)	= 3
			AND dbo.fn_VendorType (v.PS_Vendor_ID, v.WFM, v.Store_No, sv.Internal)		= 3
			AND DATEDIFF(DAY, @StartDate, DateReceived)	>=	0
			AND DATEDIFF(DAY, DateReceived, @EndDate)	>	0
			AND CloseDate IS NOT NULL
		GROUP BY
			oh.OrderHeader_ID 
 

    CREATE TABLE #tmpGLTrans(
		Subteam integer,
		Account integer,
		Unit integer,
		DeptID integer,
		Product integer,
		Amount money,
		[Description] integer,
		TransferUnit integer,
		Currency varchar(3)
    )
	
	INSERT INTO #tmpGLTrans
		SELECT
			SubTeam				= Transfer_SubTeam,
			Account             = CASE oh.ProductType_ID
				                     WHEN 1 THEN ISNULL(CONVERT(varchar(7), st.GLPurchaseAcct), '500000')
				                     WHEN 2 THEN ISNULL(CONVERT(varchar(7), dbo.fn_GetGLPackagingAccountNumber(oh.OrderHeader_ID)), '510000')
				                     WHEN 3 THEN ISNULL(CONVERT(varchar(7),  dbo.fn_GetGLSuppliesAccountNumber(oh.OrderHeader_ID)), '800000')
			                       ELSE '0' END,
			Unit				= sv.BusinessUnit_ID,
			DeptID				= ISNULL(sst.PS_Team_No, ''),
			[Product]			= ISNULL(sst.PS_SubTeam_No, ''),
			Amount				= ROUND(SUM((ReceivedItemCost + ReceivedItemFreight) *	(CASE 
																							WHEN Return_Order = 1 THEN -1
																							ELSE 1
																						END) * -1), 2),
			[Description]		= oh.OrderHeader_ID,
			TransferUnit		= sr.BusinessUnit_ID,
			Currency			= c.CurrencyCode
		FROM
			OrderHeader						(nolock)	oh
			INNER JOIN @Orders							O	ON	oh.OrderHeader_ID		= O.OrderHeader_ID 
			INNER JOIN OrderItem			(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID 
			INNER JOIN Vendor				(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN Store				(nolock)	sv	ON	v.Store_No				= sv.Store_No 
			INNER JOIN Vendor				(nolock)	rv	ON	oh.ReceiveLocation_ID	= rv.Vendor_ID
			INNER JOIN Store				(nolock)	sr	ON	rv.Store_No				= sr.Store_No
			INNER JOIN SubTeam				(nolock)	st	ON	oh.Transfer_SubTeam		= st.SubTeam_No 
	        INNER JOIN StoreJurisdiction	(nolock)	sj	ON	sj.StoreJurisdictionID	= sr.StoreJurisdictionID
            INNER JOIN Currency				(nolock)	c	ON	c.CurrencyID			= sj.CurrencyID
			LEFT  JOIN StoreSubTeam			(nolock)	sst	ON	sv.Store_No				= sst.Store_No 
															AND oh.Transfer_SubTeam		= sst.SubTeam_No 
		GROUP BY
			GLPurchaseAcct,
			GLTransferAcct,
			GLPackagingAcct,
			GLSuppliesAcct,
			sr.BusinessUnit_ID,
			sst.PS_Team_No,
			sst.PS_SubTeam_No,
			Transfer_SubTeam,
			oh.OrderHeader_ID,
			oh.ProductType_ID,
			sv.BusinessUnit_ID,
			c.CurrencyCode
			
		UNION ALL
      
		SELECT
			SubTeam			= oh.Transfer_To_SubTeam, -- ISNULL(oh.SupplyTransferToSubTeam, oh.Transfer_To_SubTeam),
			Account         = CASE oh.ProductType_ID
				                     WHEN 1 THEN ISNULL(CONVERT(varchar(7), st.GLTransferAcct), '500000')
				                     WHEN 2 THEN ISNULL(CONVERT(varchar(7), dbo.fn_GetGLPackagingAccountNumber(oh.OrderHeader_ID)), '510000')
				                     WHEN 3 THEN ISNULL(CONVERT(varchar(7),  dbo.fn_GetGLSuppliesAccountNumber(oh.OrderHeader_ID)), '800000')
			                  ELSE '0' END,
			Unit			= sr.BusinessUnit_ID, 
			DeptID			= ISNULL(sst.PS_Team_No, ''),
			[Product]		= ISNULL(sst.PS_SubTeam_No, ''),
			Amount			= ROUND(SUM((ReceivedItemCost + ReceivedItemFreight) *	(CASE 
																						WHEN Return_Order = 1 THEN -1
																						ELSE 1
																					END)), 2),
			[Description]	= oh.OrderHeader_ID,
			TransferUnit	= sv.BusinessUnit_ID,
			Currency		= c.CurrencyCode
		FROM
			OrderHeader						(nolock)	oh
			INNER JOIN @Orders						O	ON	oh.OrderHeader_ID												= O.OrderHeader_ID 
			INNER JOIN OrderItem			(nolock)	oi	ON	oh.OrderHeader_ID											= oi.OrderHeader_ID 
			INNER JOIN Vendor				(nolock)	v	ON	oh.Vendor_ID												= v.Vendor_ID
			INNER JOIN Store				(nolock)	sv	ON	v.Store_No													= sv.Store_No 
			INNER JOIN Vendor				(nolock)	rv	ON	oh.ReceiveLocation_ID										= rv.Vendor_ID
			INNER JOIN Store				(nolock)	sr	ON	rv.Store_No													= sr.Store_No
			INNER JOIN SubTeam				(nolock)	st	ON	ISNULL(oh.SupplyTransferToSubTeam, oh.Transfer_To_SubTeam)	= st.SubTeam_No 
            INNER JOIN StoreJurisdiction	(nolock)	sj	ON	sj.StoreJurisdictionID										= sr.StoreJurisdictionID
            INNER JOIN Currency				(nolock)	c	ON	c.CurrencyID												= sj.CurrencyID
			LEFT  JOIN StoreSubTeam			(nolock)	sst	ON	sr.Store_No													= sst.Store_No
														        AND ISNULL(oh.SupplyTransferToSubTeam, oh.Transfer_To_SubTeam)		= sst.SubTeam_No
		GROUP BY
			GLTransferAcct,
			GLPackagingAcct,
			GLSuppliesAcct,
			sv.BusinessUnit_ID,
			sst.PS_Team_No,
			sst.PS_SubTeam_No,
			Transfer_To_SubTeam,
			SupplyTransferToSubTeam,
			oh.OrderHeader_ID,
			oh.ProductType_ID,
			sr.BusinessUnit_ID,
			c.CurrencyCode
			
		ORDER BY
			Unit,
			TransferUnit,
			Account,
			DeptID,
			[Product],
			[Description]

	--SELECT * FROM #tmpGLTrans

	CREATE TABLE #tmpStoreGLLines(
		Unit integer,
		Account integer,
		DeptID integer,
		Product integer,
		Amount money,
		SameStore integer,
		Currency varchar(3)
    )
	
	INSERT INTO #tmpStoreGLLines
		SELECT 
			Unit,
			Account,
			DeptID,
			Product,
			Amount = Sum(Amount),
			1,
			Currency
		FROM
			#tmpGLTrans
		WHERE 
			Unit = TransferUnit
		GROUP BY 
			Account, 
			Unit,
			DeptID, 
			Product,
			Currency
		ORDER BY
			Unit,
			Account
		
	INSERT INTO #tmpStoreGLLines
		SELECT 
			Unit,
			Account,
			DeptID,
			Product,
			Amount, 
			0,
			Currency
		FROM
			#tmpGLTrans
		WHERE 
			Unit <> TransferUnit
		ORDER BY	
			Unit,
			Account

	SELECT * FROM #tmpStoreGLLines ORDER BY SameStore Desc
	
	DROP TABLE #tmpGLTrans
	DROP TABLE #tmpStoreGLLines
	SET NOCOUNT OFF
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadTransfersByGroup] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadTransfersByGroup] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadTransfersByGroup] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetGLUploadTransfersByGroup] TO [IRMAReportsRole]
    AS [dbo];

