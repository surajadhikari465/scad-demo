IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetCurrentSumDiscounts]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
	DROP FUNCTION [dbo].[fn_GetCurrentSumDiscounts]
GO

CREATE FUNCTION [dbo].[fn_GetCurrentSumDiscounts]
(
	@Item_Key int,
    @Store_No int
)
	RETURNS decimal(9,2)
AS
-- **************************************************************************
-- Procedure: fn_GetCurrentSumDiscounts()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This function is called from multiple stored procedures to return the sum
-- of all discounts for an store-item-vendor
--
-- Modification History:
-- Date		   Init TFS		Comment
-- 07/06/2010  BBB  xxxxx	Applied coding standards and modification history
-- 07/08/2010  BBB	xxxxx	Removed InsertDate conversion
-- **************************************************************************
BEGIN
	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
	DECLARE @CurrentSumDiscounts	decimal(9,2)
	DECLARE @CurrDate				datetime
	DECLARE @StoreItemVendorID		int

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
        
    SELECT @StoreItemVendorID = dbo.fn_GetStoreItemVendorID(@Item_Key, @Store_No)

	--**************************************************************************
	--Main SQL
	--**************************************************************************
	SELECT TOP 1 
		@CurrentSumDiscounts = SUM(CaseAmt)
	FROM 
		VendorDealHistory			(nolock) vdh
		INNER JOIN	StoreItemVendor (nolock) siv	ON siv.StoreItemVendorID = vdh.StoreItemVendorID
	WHERE 
		vdh.StoreItemVendorId		=	@StoreItemVendorID
		AND @CurrDate				BETWEEN vdh.StartDate AND vdh.EndDate
		AND vdh.VendordealTypeID	=	2
	GROUP BY
		InsertDate
	ORDER BY
		InsertDate DESC
		
	--**************************************************************************
	--Return
	--**************************************************************************
	RETURN ISNULL(@CurrentSumDiscounts, 0)
	
END

GO