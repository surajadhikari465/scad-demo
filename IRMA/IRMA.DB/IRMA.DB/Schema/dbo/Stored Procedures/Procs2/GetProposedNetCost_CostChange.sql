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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProposedNetCost_CostChange] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProposedNetCost_CostChange] TO [IRMAClientRole]
    AS [dbo];

