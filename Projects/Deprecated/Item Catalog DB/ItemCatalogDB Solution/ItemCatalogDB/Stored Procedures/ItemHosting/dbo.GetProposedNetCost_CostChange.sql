/****** Object:  StoredProcedure [dbo].[GetProposedNetCost_CostChange]    Script Date: 2/12/2007 11:06:56 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetProposedNetCost_CostChange]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetProposedNetCost_CostChange]
GO

/****** Object:  StoredProcedure [dbo].[GetProposedNetCost_CostChange]    Script Date: 2/12/2007 11:06:56 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.GetProposedNetCost_CostChange
	@Item_Key int,
	@Vendor_ID int,
	@StoreList varchar(8000),
	@StoreListSeparator char(1),
	@NewCost decimal(10,4),
	@StartDate smalldatetime
AS
BEGIN
    SET NOCOUNT ON
    
    --TAKES IN THE PROPOSED NEW COST AMT THE USER HAS NOT YET SAVED AND SUBTRACTS ANY DISCOUNTS/ALLOWANCES
    --FOR THE GIVEN ITEM/VENDOR/STORE FOR THE PROPOSED START DATE THE USER HAS ALSO ENTERED.  CALCULATIONS 
    --ARE PERFORMED TO RETURN ANY RECORDS WHERE THE NET COST WILL RESULT IN A NEGATIVE (OR ZERO) DOLLAR AMT

	SELECT S.Store_Name, VCS.Store_No, UnitCost, NetCost,
		@NewCost AS ProposedCost,
		@NewCost - ISNULL(dbo.fn_ItemNetDiscount(S.Store_No, @Item_Key, @Vendor_ID, ISNULL(@NewCost, 0), @StartDate),0) AS ProposedNetCost
	FROM fn_VendorCostStores (@Item_Key, @Vendor_ID, @StartDate) VCS
	INNER JOIN
		Store S
		ON S.Store_No = VCS.Store_No
	INNER JOIN
		fn_Parse_List(@StoreList, @StoreListSeparator) SL
		ON SL.Key_Value = S.Store_No
	WHERE (@NewCost - ISNULL(dbo.fn_ItemNetDiscount(S.Store_No, @Item_Key, @Vendor_ID, ISNULL(@NewCost, 0), @StartDate),0)) <= 0 -- LOOKS FOR ITEMS WHERE ProposedNetCost IS <= 0
    
    SET NOCOUNT OFF
END
GO
    