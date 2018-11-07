CREATE PROCEDURE dbo.GetAPUpAccruals
@Store_No	int
AS

-- **************************************************************************
-- Procedure: GetAPUpAccruals
--    Author: n/a
--      Date: n/a
--
-- Description: n/a
--
-- Modification History:
-- Date       	Init  	TFS   	Comment
-- 2011/12/21	KM		3744	Code formatting; extension change; ReceivedItemCost calculation review
-- 2012/01/05	KM		3744	Replace SUM(oi.ReceivedItemCost) with call to oh.AdjustedReceivedCost
-- 09/18/2013   MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
-- **************************************************************************

BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
    SET NOCOUNT ON

	DECLARE
		@Error_No				int, 
		@CurrDate				datetime, 
		@ThisPeriodBeginDate	datetime
	        
	SELECT @Error_No = 0
	
	-- Get this period begin date
    SELECT @CurrDate = GETDATE()
    EXEC GetBeginPeriodDate @CurrDate, @BP_Date = @ThisPeriodBeginDate OUTPUT
    
    SELECT @Error_No = @@ERROR

    IF @Error_No <> 0
    BEGIN
        SET NOCOUNT OFF
        RAISERROR ('GetAPUpAccruals, GetBeginPeriodDate (This Period), failed with Error = %d', 16, 1, @Error_No)
        RETURN
    END

    -- Get list of Orders to be uploaded
    DECLARE @UploadOrders TABLE (OrderHeader_ID int PRIMARY KEY)

    INSERT INTO
		@UploadOrders
    SELECT DISTINCT
		oh.OrderHeader_ID
    FROM
		OrderHeader				(nolock)	oh
		INNER JOIN Vendor		(nolock)	v	ON oh.Vendor_ID				= v.Vendor_ID
		INNER JOIN Vendor		(nolock)	sv	ON oh.ReceiveLocation_ID	= sv.Vendor_ID
		INNER JOIN Store		(nolock)	s	ON sv.Store_No				= s.Store_No
    WHERE
		oh.Transfer_SubTeam IS NULL
		AND v.WFM = 0
		AND ISNULL(oh.UploadedDate, @ThisPeriodBeginDate) >= @ThisPeriodBeginDate
		AND s.Store_No = ISNULL(@Store_No, s.Store_No)
		AND ((ISNULL(oh.CloseDate, (	SELECT TOP 1
											oi.DateReceived 
                                        FROM
											OrderItem (nolock)	oi
                                        WHERE
											 oi.OrderHeader_ID	= oh.OrderHeader_ID 
											 AND oi.DateReceived IS NOT NULL)) >= '2003-12-22')
		AND (ISNULL(oh.CloseDate, (		SELECT TOP 1 
											oi.DateReceived 
                                        FROM
											OrderItem (nolock)	oi
                                        WHERE 
											oi.OrderHeader_ID	= oh.OrderHeader_ID 
											AND oi.DateReceived IS NOT NULL)) < @ThisPeriodBeginDate))

    -- Add up received cost for items with Sales Accounts and treat as the invoice cost - must be separated in the upload
    DECLARE
		@SalesAccountCost TABLE 
		(
			OrderHeader_ID	int, 
			Sales_Account	varchar(6),
			SubTeam_No		int, 
			InvoiceCost		smallmoney, 
			InvoiceFreight	smallmoney, 
			PRIMARY KEY NONCLUSTERED (OrderHeader_ID, SubTeam_No)
		)


    INSERT INTO
		@SalesAccountCost
    SELECT
		oh.OrderHeader_ID, 
		i.Sales_Account, 
		i.SubTeam_No, 
		oh.AdjustedReceivedCost, 
		SUM(oi.ReceivedItemFreight)
    FROM
		OrderHeader				(nolock)	oh
		INNER JOIN @UploadOrders			UO	ON oh.OrderHeader_ID	= UO.OrderHeader_ID
		INNER JOIN OrderItem	(nolock)	oi	ON oh.OrderHeader_ID	= oi.OrderHeader_ID
		INNER JOIN Item			(nolock)	i	ON oi.Item_Key			= i.Item_Key
    WHERE
		i.Sales_Account IS NOT NULL
        AND oh.Transfer_To_SubTeam IS NULL
	GROUP BY
		oh.OrderHeader_ID, 
		i.Sales_Account, 
		i.SubTeam_No,
		oh.AdjustedReceivedCost
	
	UNION
	
    SELECT 
		oh.OrderHeader_ID, 
		i.Sales_Account, 
		oh.Transfer_To_SubTeam, 
		oh.AdjustedReceivedCost, 
		SUM(oi.ReceivedItemFreight)
    FROM
		OrderHeader				(nolock)	oh
		INNER JOIN @UploadOrders			UO	ON oh.OrderHeader_ID	= UO.OrderHeader_ID
		INNER JOIN OrderItem	(nolock)	oi	ON oh.OrderHeader_ID	= oi.OrderHeader_ID
		INNER JOIN Item			(nolock)	i	ON oi.Item_Key			= i.Item_Key
    WHERE 
		i.Sales_Account IS NOT NULL
        AND oh.Transfer_To_SubTeam IS NOT NULL
    GROUP BY 
		oh.OrderHeader_ID, 
		i.Sales_Account, 
		oh.Transfer_To_SubTeam,
		oh.AdjustedReceivedCost

    -- Final OrderInvoice data using Sales Account stuff from above and the actual OrderInvoice table
    DECLARE @OrderInvoice TABLE
    (
		OrderHeader_ID		int, 
		Sales_Account		varchar(6), 
		SubTeam_No			int, 
		InvoiceCost			smallmoney, 
		InvoiceFreight		smallmoney
	)
    
    INSERT INTO
		@OrderInvoice
    
    -- OrderInvoice without OrderItems that have Sales Accounts
    SELECT
		oi.OrderHeader_ID,
		'500000', 
		oi.SubTeam_No, 
        InvoiceCost		= (ISNULL(oi.InvoiceCost, 0) - ISNULL(SA.SA_InvoiceCost, 0)), 
        InvoiceFreight	= (ISNULL(oi.InvoiceFreight, 0) - ISNULL(SA.SA_InvoiceFreight, 0))
    FROM 
		@UploadOrders							UO
		LEFT JOIN OrderInvoice		(nolock)	oi	ON	UO.OrderHeader_ID = oi.OrderHeader_ID
		
		-- OrderItems with Sales Account so amount can be subtracted
		LEFT JOIN (	SELECT
						SAC.OrderHeader_ID, 
						SAC.SubTeam_No, 
						SA_InvoiceCost		= SUM(SAC.InvoiceCost), 
						SA_InvoiceFreight	= SUM(SAC.InvoiceFreight)
					FROM 
						@SalesAccountCost SAC
					GROUP BY 
						OrderHeader_ID, 
						SubTeam_No)				SA	ON  oi.OrderHeader_ID	= SA.OrderHeader_ID 
													AND oi.SubTeam_No		= SA.SubTeam_No
	WHERE
		(ISNULL(oi.InvoiceCost, 0) - ISNULL(SA.SA_InvoiceCost, 0)) <> 0
		OR (ISNULL(oi.InvoiceFreight, 0) - ISNULL(SA.SA_InvoiceFreight, 0)) <> 0
		
    UNION
    
    -- OrderItem totals for OrderItems with Sales Accounts
    SELECT
		SAC.OrderHeader_ID, 
		SAC.Sales_Account, 
		SAC.SubTeam_No, 
		SAC.InvoiceCost, 
		SAC.InvoiceFreight
    FROM
		@SalesAccountCost SAC

--**************************************************************************
-- Main SQL
--**************************************************************************
    SELECT 
		Account					=	ISNULL(OIV.Sales_Account, 0),
		Unit					=	s.BusinessUnit_ID,
        Team					=	CASE
										WHEN ISNULL(OIV.Sales_Account, 0) = '500000' THEN ISNULL(sst.Team_No, 0) 
										ELSE 0 
									END,
		Dept					=	CASE
										WHEN ISNULL(OIV.Sales_Account, 0) = '500000' THEN ISNULL(OIV.SubTeam_No, 0) 
										ELSE 0 
									END,
		Amount					=	(ISNULL(OIV.InvoiceCost,0) + ISNULL(OIV.InvoiceFreight,0)) *	(CASE
																										WHEN oh.Return_Order = 1 THEN -1 
																										ELSE 1 
																									END),
		Description				=	ISNULL(oh.InvoiceNumber, '') + ' ' + v.CompanyName,
		PONumber				=	oh.OrderHeader_ID,
		RecvLogNo				=	ISNULL(RecvLog_No, 0)
    
    FROM
		OrderHeader					(nolock)	oh
		INNER JOIN @UploadOrders				UO	ON  oh.OrderHeader_ID		= UO.OrderHeader_ID
		INNER JOIN Vendor			(nolock)	v	ON	oh.Vendor_ID			= v.Vendor_ID
		INNER JOIN Vendor			(nolock)	sv	ON	oh.ReceiveLocation_ID	= sv.Vendor_ID
		INNER JOIN Store			(nolock)	s	ON	sv.Store_No				= s.Store_No
		LEFT  JOIN @OrderInvoice				OIV	ON	oh.OrderHeader_ID		= OIV.OrderHeader_ID 
		LEFT  JOIN StoreSubTeam		(nolock)	sst	ON	s.Store_No				= sst.Store_No
													AND OIV.SubTeam_No			= sst.SubTeam_No
		LEFT  JOIN SubTeam			(nolock)	st	ON	OIV.SubTeam_No			= st.SubTeam_No
    
    ORDER BY
		s.BusinessUnit_ID, 
		sst.Team_No, 
		OIV.SubTeam_No, 
		v.CompanyName + ' ' + InvoiceNumber, 
		oh.OrderHeader_ID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAccruals] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAccruals] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAccruals] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAPUpAccruals] TO [IRMAReportsRole]
    AS [dbo];

