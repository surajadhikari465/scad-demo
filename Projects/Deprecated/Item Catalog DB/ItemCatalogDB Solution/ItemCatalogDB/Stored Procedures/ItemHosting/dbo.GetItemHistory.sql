SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetItemHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    drop procedure [dbo].[GetItemHistory]
GO

CREATE PROCEDURE [dbo].[GetItemHistory]
    @ItemIdentifier	varchar(13),
    @Store_No		int,
    @Vendor_ID		int,
    @TopN			int,
    @StartDate		varchar(10),
    @EndDate		varchar(10)
AS 
   -- **************************************************************************
   -- Procedure: GetItemHistory()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   -- This procedure is called from ItemHistory.vb
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 06/09/2009	BBB				Added ItemIdentifier and Item_Description to output
   -- 06/23/2009	BSR				Added Qty Ordered to the output
   -- 06/24/2009	BSR				Added Store_No to the output
   -- 08/11/2009    CV				Bug 10749 #2 fix. Used DATEADD(dd, 0, DATEDIFF(dd, 0, oh.CloseDate))
   --								to strip out time part where compared to @BeginDateDt and @EndDateDt.   
   -- 12/14/2011	BBB		3744	coding standards;
   -- 09/12/2013    MZ      13667   Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
   -- **************************************************************************
BEGIN
    SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	SET NOCOUNT ON

    SET ROWCOUNT @TopN
    
	--**************************************************************************
	--Create variables
	--**************************************************************************  
	DECLARE @BeginDateDt    datetime, 
			@EndDateDt      datetime

	--**************************************************************************
	--Populate variables
	--**************************************************************************  
	SELECT @BeginDateDt = CONVERT(datetime, @StartDate), 
		   @EndDateDt = CONVERT(datetime, @EndDate)
		   
	--**************************************************************************
	--SQL
	--**************************************************************************  
    SELECT
		oh.OrderHeader_ID, 
		i.Item_Description,
		ii.Identifier,
		oh.OrderDate, 
		v.CompanyName, 
		oi.Cost, 
		oi.QuantityDiscount, 
        oi.DiscountType, 
		oi.QuantityOrdered,
        oi.QuantityReceived,
        oi.Handling, 
        oi.Freight, 
        oi.AdjustedCost, 
        oi.LandedCost, 
        [StoreNo]			=	vr.Store_No,
        [ReceiveLocation]	=	vr.CompanyName, 
        iuq.Unit_Name, 
        oi.ReceivedItemCost, 
        oi.ReceivedItemFreight, 
        oi.LineItemFreight, 
        oi.LineItemCost, 
        oi.DateReceived
    FROM
		OrderHeader					(nolock) oh
		INNER JOIN OrderItem		(nolock) oi		ON	oh.OrderHeader_ID		= oi.OrderHeader_ID 
        INNER JOIN ItemIdentifier	(nolock) ii		ON	ii.Item_Key				= oi.Item_Key
													AND ii.Default_Identifier	= 1
        INNER JOIN Vendor			(nolock) vr		ON	oh.ReceiveLocation_ID	= vr.Vendor_ID  
        INNER JOIN Vendor			(nolock) v		ON	v.Vendor_ID				= oh.Vendor_ID
        INNER JOIN ItemUnit			(nolock) iuq	ON	iuq.Unit_ID				= oi.QuantityUnit
		INNER JOIN Item				(nolock) i		ON	oi.Item_Key				= i.Item_Key
    WHERE 
		ii.Identifier				= @ItemIdentifier 
        AND ii.Deleted_Identifier	= 0
        AND vr.Store_No				= ISNULL(@Store_No,vr.Store_No)
        AND v.Vendor_ID				= ISNULL(@Vendor_ID, v.Vendor_ID)   
		AND DATEADD(dd, 0, DATEDIFF(dd, 0, oh.CloseDate))
			BETWEEN ISNULL(@BeginDateDt, DATEADD(dd, 0, DATEDIFF(dd, 0, oh.CloseDate))) 
			AND ISNULL(@EndDateDt, DATEADD(dd, 0, DATEDIFF(dd, 0, oh.CloseDate)))        
    ORDER BY 
		oh.OrderHeader_ID DESC, 
		oi.OrderItem_ID
    

    SET ROWCOUNT 0
    
    SET NOCOUNT OFF
END
