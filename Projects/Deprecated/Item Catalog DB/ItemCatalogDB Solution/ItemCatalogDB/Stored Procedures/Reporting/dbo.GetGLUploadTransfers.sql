SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].GetGLUploadTransfers') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].GetGLUploadTransfers
GO

CREATE PROCEDURE dbo.GetGLUploadTransfers
    @StartDate	datetime,
    @EndDate	datetime,
    @Store_No	int			= NULL,
    @IsUploaded bit			= 0,
    @CurrDate	datetime	= NULL
AS
-- **************************************************************************
-- Procedure: GetGLUploadTransfers

-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 12/14/2009	BR		11422	Added functionality to output the TransferUnit to enable store to store transfers
-- 12/08/2010	DBS		13766	Shaved off a day
-- 2011/12/21	KM		3744	extension change; coding standards
-- 2012/09/24   FA      7548    Added functionality for transfer on packaging and supplies  
-- 2012/12/18   FA      9540    Fixed the to-subteam for supplies transfers       
-- **************************************************************************
BEGIN
      SET NOCOUNT ON
      IF @StartDate	IS NULL
      OR @EndDate	IS NULL
         BEGIN
			
			-- previous Monday through Sunday  
			SELECT
               @StartDate = CONVERT ( datetime,CONVERT ( varchar(255),DATEADD ( "day" , 1 - DATEPART ( dw , ISNULL ( @CurrDate , GETDATE ( ) ) ) - 6 , ISNULL ( @CurrDate , GETDATE ( ) ) ),101 ) ) ,
               @EndDate = CONVERT ( datetime,CONVERT ( varchar(255),DATEADD ( "day" , 2 - DATEPART ( dw , ISNULL ( @CurrDate , GETDATE ( ) ) ) , ISNULL ( @CurrDate , GETDATE ( ) ) ),101 ) )
         END
      ELSE
         BEGIN
           SELECT
               @StartDate = CONVERT ( datetime,CONVERT ( varchar(255),@StartDate,101 ) ) ,
               @EndDate = CONVERT ( datetime,CONVERT ( varchar(255),@EndDate + 1,101 ) )
         END
      
      DECLARE @Orders TABLE ( OrderHeader_ID int PRIMARY KEY )
      IF @IsUploaded = 0
         BEGIN
			INSERT  INTO	@Orders 
			SELECT
				oh.OrderHeader_ID
			FROM
				OrderHeader				(nolock) oh
                INNER JOIN OrderItem	(nolock) oi	ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
                INNER JOIN Vendor		(nolock) v	ON	oh.Vendor_ID			= v.Vendor_ID
                INNER JOIN Store		(nolock) sv	ON	v.Store_No				= sv.Store_No
                INNER JOIN Vendor		(nolock) rv	ON	oh.ReceiveLocation_ID	= rv.Vendor_ID
                INNER JOIN Store		(nolock) sr	ON	rv.Store_No				= sr.Store_No
			WHERE
				-- OrderType_ID = 3: Transfer Order Type 
				-- fn_GetCustomerType() = 3: Customer and Vendor are regional
				
                oh.OrderType_ID = 3
				AND oh.AccountingUploadDate IS NULL
				AND dbo.fn_GetCustomerType (sr.Store_No, sr.Internal, sr.BusinessUnit_ID)	= 3
				AND dbo.fn_VendorType (v.PS_Vendor_ID, v.WFM, v.Store_No, sv.Internal)		= 3
				AND DATEDIFF(DAY, @StartDate, DateReceived)	>=	0
				AND DATEDIFF(DAY, DateReceived, @EndDate)	>	0
				AND v.Store_No = ISNULL ( @Store_No , v.Store_No )
				AND CloseDate IS NOT NULL
			GROUP BY
				oh.OrderHeader_ID
         END
      ELSE
		BEGIN
			INSERT  INTO	@Orders
			SELECT
				oh.OrderHeader_ID
			FROM
				OrderHeader				(nolock)	oh
				INNER JOIN OrderItem	(nolock)	oi	ON	oi.OrderHeader_ID		= oh.OrderHeader_ID
				INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
				INNER JOIN Store		(nolock)	sv	ON	v.Store_No				= sv.Store_No
				INNER JOIN Vendor		(nolock)	rv	ON	oh.ReceiveLocation_ID	= rv.Vendor_ID
				INNER JOIN Store		(nolock)	sr	ON	rv.Store_No				= sr.Store_No
			WHERE
				-- OrderType_ID = 3: Transfer Order Type 
				-- fn_GetCustomerType() = 3: Customer and Vendor are regional
				
				oh.OrderType_ID = 3 
				AND oh.AccountingUploadDate IS NOT NULL 
				AND dbo.fn_GetCustomerType(sr.Store_No, sr.Internal, sr.BusinessUnit_ID)	= 3
				AND dbo.fn_VendorType(v.PS_Vendor_ID, v.WFM, v.Store_No, sv.Internal)		= 3
				AND DATEDIFF(DAY , @StartDate, DateReceived)	>=	0
				AND DATEDIFF(DAY , DateReceived, @EndDate)		>	0
				AND v.Store_No = ISNULL(@Store_No, v.Store_No)
				AND CloseDate IS NOT NULL
			GROUP BY
				oh.OrderHeader_ID
         END
      
		SELECT
			SubTeam				= Transfer_SubTeam,
			Account             = CASE oh.ProductType_ID
				                     WHEN 1 THEN ISNULL(CONVERT(varchar(7), st.GLPurchaseAcct), '500000')
				                     WHEN 2 THEN ISNULL(CONVERT(varchar(7), dbo.fn_GetGLPackagingAccountNumber(oh.OrderHeader_ID)), '510000')
				                     WHEN 3 THEN ISNULL(CONVERT(varchar(7),  dbo.fn_GetGLSuppliesAccountNumber(oh.OrderHeader_ID)), '800000')
			                       ELSE '0' END,
			Unit				= sr.BusinessUnit_ID,
			DeptID				= ISNULL(sst.PS_Team_No, ''),
			[Product]			= ISNULL(sst.PS_SubTeam_No, ''),
			Amount				= ROUND(SUM((ReceivedItemCost + ReceivedItemFreight) *	(CASE 
																							WHEN Return_Order = 1 THEN -1
																							ELSE 1
																						END) * -1), 2),
			[Description]		= oh.OrderHeader_ID,
			TransferUnit		= sv.BusinessUnit_ID
		FROM
			OrderHeader				(nolock)	oh
			INNER JOIN @Orders					O	ON	oh.OrderHeader_ID		= O.OrderHeader_ID 
			INNER JOIN OrderItem	(nolock)	oi	ON	oh.OrderHeader_ID		= oi.OrderHeader_ID 
			INNER JOIN Vendor		(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
			INNER JOIN Store		(nolock)	sv	ON	v.Store_No				= sv.Store_No 
			INNER JOIN Vendor		(nolock)	rv	ON	oh.ReceiveLocation_ID	= rv.Vendor_ID
			INNER JOIN Store		(nolock)	sr	ON	rv.Store_No				= sr.Store_No
			INNER JOIN SubTeam		(nolock)	st	ON	oh.Transfer_SubTeam		= st.SubTeam_No 
			LEFT  JOIN StoreSubTeam (nolock)	sst	ON	sv.Store_No				= sst.Store_No 
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
			sv.BusinessUnit_ID
			
		UNION ALL
      
		SELECT
			SubTeam			= ISNULL(oh.SupplyTransferToSubTeam, oh.Transfer_To_SubTeam),
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
			TransferUnit	= sr.BusinessUnit_ID
		FROM
			OrderHeader					(nolock)	oh
			INNER JOIN @Orders						O	ON	oh.OrderHeader_ID											= O.OrderHeader_ID 
			INNER JOIN OrderItem		(nolock)	oi	ON	oh.OrderHeader_ID											= oi.OrderHeader_ID 
			INNER JOIN Vendor			(nolock)	v	ON	oh.Vendor_ID												= v.Vendor_ID
			INNER JOIN Store			(nolock)	sv	ON	v.Store_No													= sv.Store_No 
			INNER JOIN Vendor			(nolock)	rv	ON	oh.ReceiveLocation_ID										= rv.Vendor_ID
			INNER JOIN Store			(nolock)	sr	ON	rv.Store_No													= sr.Store_No
			INNER JOIN SubTeam			(nolock)	st	ON	ISNULL(oh.SupplyTransferToSubTeam, oh.Transfer_To_SubTeam)	= st.SubTeam_No 
			LEFT  JOIN StoreSubTeam		(nolock)	sst	ON	sr.Store_No													= sst.Store_No
														AND ISNULL(oh.SupplyTransferToSubTeam, oh.Transfer_To_SubTeam)		= sst.SubTeam_No
		GROUP BY
			GLTransferAcct,
			GLPackagingAcct,
			GLSuppliesAcct,
			sr.BusinessUnit_ID,
			sst.PS_Team_No,
			sst.PS_SubTeam_No,
			Transfer_To_SubTeam,
			SupplyTransferToSubTeam,
			oh.OrderHeader_ID,
			oh.ProductType_ID
		ORDER BY
			Unit,
			Account,
			DeptID,
			[Product],
			[Description]
	
	SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



