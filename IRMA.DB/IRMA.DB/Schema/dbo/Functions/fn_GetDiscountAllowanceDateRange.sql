﻿CREATE FUNCTION [dbo].[fn_GetDiscountAllowanceDateRange]
(
	@Item_Key int,
    @Store_No int,
    @DiscORAllow varchar
)
	RETURNS varchar(30)
AS
-- **************************************************************************
-- Procedure: fn_GetDiscountAllowanceDateRange()
--    Author: n/a
--      Date: n/a
--
-- Description:
-- This function is called from multiple stored procedures to return the sum
-- of all allowancees/discounts for an store-item-vendor based upon date range
--
-- Modification History:
-- Date		   Init TFS		Comment
-- 07/06/2010  BBB	xxxxx	Applied coding standards and modification history
-- 07/08/2010  BBB	xxxxx	Removed InsertDate conversion
-- **************************************************************************
BEGIN
	--**************************************************************************
	--Declare internal variables
	--**************************************************************************
    DECLARE @CurrDate			datetime
    DECLARE @StartDate			datetime
    DECLARE @EndDate			datetime
    DECLARE @StoreItemVendorID	int
    DECLARE @Result				varchar(30)

	--**************************************************************************
	--Populate internal variables
	--**************************************************************************
    SELECT @CurrDate = CONVERT(datetime, CONVERT(varchar(255), GETDATE(), 101))
        
    SELECT @StoreItemVendorID = dbo.fn_GetStoreItemVendorID(@Item_Key, @Store_No)

	--**************************************************************************
	--Main SQL
	--**************************************************************************
	IF @DiscORAllow = 'A'
		BEGIN
			--SELECTS THE MAX StartDate
			SELECT TOP 1
				@StartDate = MAX(StartDate)
			FROM 
				VendorDealHistory			(nolock) vdh
				INNER JOIN	StoreItemVendor (nolock) siv	ON siv.StoreItemVendorID = vdh.StoreItemVendorID
			WHERE 
				vdh.StoreItemVendorId						= @StoreItemVendorID
				AND @CurrDate								BETWEEN vdh.StartDate AND vdh.EndDate
				AND vdh.VendordealTypeID					= 1
			GROUP BY
				InsertDate
			ORDER BY
				InsertDate DESC	
	                
			--SELECTS THE MIN EndDate
			SELECT TOP 1
				@EndDate = MIN(EndDate)
			FROM 
				VendorDealHistory			(nolock) vdh
				INNER JOIN	StoreItemVendor (nolock) siv	ON siv.StoreItemVendorID = vdh.StoreItemVendorID
			WHERE 
				vdh.StoreItemVendorId						= @StoreItemVendorID
				AND @CurrDate								BETWEEN vdh.StartDate AND vdh.EndDate
				AND vdh.VendordealTypeID					= 1
			GROUP BY
				InsertDate
			ORDER BY
				InsertDate DESC
		END
	ELSE
		BEGIN
			--SELECTS THE MAX StartDate
			SELECT TOP 1
				@StartDate = MAX(StartDate)
			FROM 
				VendorDealHistory			(nolock) vdh
				INNER JOIN	StoreItemVendor (nolock) siv	ON siv.StoreItemVendorID = vdh.StoreItemVendorID
			WHERE 
				vdh.StoreItemVendorId						= @StoreItemVendorID
				AND @CurrDate								BETWEEN vdh.StartDate AND vdh.EndDate
				AND vdh.VendordealTypeID					= 2
			GROUP BY
				InsertDate
			ORDER BY
				InsertDate DESC	
	                
			--SELECTS THE MIN EndDate
			SELECT TOP 1
				@EndDate = MIN(EndDate)
			FROM 
				VendorDealHistory			(nolock) vdh
				INNER JOIN	StoreItemVendor (nolock) siv	ON siv.StoreItemVendorID = vdh.StoreItemVendorID
			WHERE 
				vdh.StoreItemVendorId						= @StoreItemVendorID
				AND @CurrDate								BETWEEN vdh.StartDate AND vdh.EndDate
				AND vdh.VendordealTypeID					= 2
			GROUP BY
				InsertDate
			ORDER BY
				InsertDate DESC
		END		

	--**************************************************************************
	--Return
	--**************************************************************************
    SELECT @Result = CONVERT(varchar(255), @StartDate, 101) + ' - ' + CONVERT(varchar(255), @EndDate, 101)
        
    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetDiscountAllowanceDateRange] TO [IRMAClientRole]
    AS [dbo];

