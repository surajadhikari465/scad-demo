CREATE PROCEDURE dbo.GetProposedNetCostValues
	@Item_Key int,
	@Vendor_ID int,
	@StoreList varchar(8000),
	@StoreListSeparator char(1),
	@CaseAmt decimal(10,4),
	@CaseAmtType char,	
	@StartDate smalldatetime,
	@EndDate smalldatetime,
	@VendorDealHistory_ID int = -1,
	@NotStackable bit = 0
AS
BEGIN
    SET NOCOUNT ON
    
    -- Used for validating whether a new or update to an
    -- existing deal would result in a zero or negative net cost.
    
	SELECT S.Store_Name, VCS.Store_No, UnitCost, NetCost,
			CASE WHEN @CaseAmtType = '%' THEN (@CaseAmt / 100) * UnitCost 
				ELSE @CaseAmt
				END AS ProposedDiscountAmt,
			NetCost - (CASE
			           WHEN
			               -- if there are any nonstackable deals and the deal
			               -- being validated is stackable, then we ignore it.
					       @NotStackable = 0 AND
						       dbo.fn_NonStackableDealCount(VCS.Store_No, @Item_Key, @Vendor_ID, @StartDate, @EndDate, @VendorDealHistory_ID) > 0
					   THEN 0
					   ELSE
							CASE WHEN @CaseAmtType = '%' THEN (@CaseAmt / 100) * UnitCost 
							ELSE @CaseAmt
							END
						END)AS ProposedNetCost
	FROM fn_VendorCostStoresValidation (@Item_Key, @Vendor_ID, @StartDate, @EndDate, @VendorDealHistory_ID, @NotStackable) VCS
	INNER JOIN
		Store S
		ON S.Store_No = VCS.Store_No
	INNER JOIN
		fn_Parse_List(@StoreList, @StoreListSeparator) SL
		ON SL.Key_Value = S.Store_No
	WHERE (NetCost - (CASE
			           WHEN
			               -- if there are any nonstackable deals and the deal
			               -- being validated is stackable, then we ignore it.
					       @NotStackable = 0 AND
						       dbo.fn_NonStackableDealCount(VCS.Store_No, @Item_Key, @Vendor_ID, @StartDate, @EndDate, @VendorDealHistory_ID) > 0
					   THEN 0
					   ELSE
							CASE WHEN @CaseAmtType = '%' THEN (@CaseAmt / 100) * UnitCost 
							ELSE @CaseAmt
							END
						END)) <= 0 -- LOOKS FOR ITEMS WHERE ProposedNetCost IS <= 0
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProposedNetCostValues] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetProposedNetCostValues] TO [IRMAClientRole]
    AS [dbo];

