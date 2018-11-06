CREATE FUNCTION [dbo].[fn_ItemNetDiscountDateRange]
	(@ItemKey int, 
	 @Store_No int, 
	 @StartDate smalldatetime, 
	 @EndDate smalldatetime)

	 RETURNS SMALLMONEY
AS
BEGIN
	DECLARE @NetDiscount SMALLMONEY

	SELECT @NetDiscount =  DiscountAmt
	FROM
	(
		SELECT
			NotStackable, 
			SUM(CASE 
					WHEN VDT.CaseAmtType = '%' 
						THEN (VDH.CaseAmt / (100 * VDH.Package_Desc1)) * 
								ISNULL(dbo.fn_GetAverageCostDateRange(@ItemKey, @Store_No, @StartDate, @EndDate),0)
					ELSE VDH.CaseAmt/VDH.Package_Desc1
				END) AS DiscountAmt,
			count(*) AS cnt
		FROM 
			VendorDealHistory VDH
			INNER JOIN
				StoreItemVendor SIV
				ON SIV.StoreItemVendorID = VDH.StoreItemVendorID
			INNER JOIN
				VendorDealType VDT
				ON VDT.VendorDealTypeID = VDH.VendorDealTypeID
		WHERE 
			SIV.storeitemvendorid = dbo.fn_GetStoreItemVendorID(@ItemKey, @Store_No) and
			(@StartDate between VDH.StartDate and VDH.EndDate 
						or @EndDate between VDH.StartDate and VDH.EndDate)
		GROUP BY
			NotStackable	
		HAVING 
			(count(*) > 0 and NotStackable = 1) OR 
			 count(*) = 0
	)CostDiscount

	RETURN @NetDiscount
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ItemNetDiscountDateRange] TO [IRMAReportsRole]
    AS [dbo];

