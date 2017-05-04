SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetOrderCurrency]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetOrderCurrency]
GO

CREATE PROCEDURE [dbo].[GetOrderCurrency]
    @OrderID	int,
	@RegionCode varchar(2)

AS 

   -- **************************************************************************
   -- Procedure: GetOrderCurrency()
   --    Author: Billy Blackerby
   --      Date: 09.03.09
   --
   -- Description:
   -- This procedure is called from IRMA Client code in various calls to determine
   -- retrieve currency of the store in an order.
   --
   -- Modification History:
   -- Date			Init	TFS		Comment
   -- 2013-03-15	KM		9251	Return GBP as the default currency for UK;
   -- **************************************************************************

BEGIN
    SET NOCOUNT ON    

	SELECT 
		[CurrencyCode]	=	CASE @RegionCode
								WHEN 'EU' THEN ISNULL(CurrencyCode, 'GBP')
								ELSE ISNULL(CurrencyCode, 'USD')
							END
	FROM 
		OrderHeader			(nolock) oh
		JOIN Vendor			(nolock) v	ON oh.PurchaseLocation_ID	= v.Vendor_ID
		LEFT JOIN Currency	(nolock) c	ON v.CurrencyID				= c.CurrencyID
	WHERE 
		oh.OrderHeader_ID = @OrderID

    SET NOCOUNT OFF    
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO