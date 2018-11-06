CREATE FUNCTION [dbo].[fn_GetAverageCostDateRange]
	(@ItemKey int, 
	 @Store_No int, 
	 @StartDate smalldatetime, 
	 @EndDate smalldatetime)

	 RETURNS SMALLMONEY
AS
BEGIN
	DECLARE @AVERAGECOST SMALLMONEY
  
	SELECT @AVERAGECOST = 
	(
		SELECT
			isnull(avg(vch.unitcost/ 
					CASE 
						WHEN vch.package_desc1 is null THEN 1
						WHEN vch.package_desc1 = 0 THEN 1
						ELSE vch.package_desc1
					END),0)
		FROM 
			VendorCostHistory vch
		WHERE
			vch.StoreItemVendorId = dbo.fn_GetStoreItemVendorID(@ItemKey, @Store_No) and
		   (@StartDate between vch.StartDate and vch.EndDate 
				or @EndDate between vch.StartDate and vch.EndDate) and
			vch.VendorCostHistoryID = (SELECT 
											max(b.VendorCostHistoryID) 
									   FROM 
											VendorCostHistory b
									   WHERE 
											vch.StoreItemVendorId = b.StoreItemVendorId and
											vch.StartDate = b.StartDate and
											vch.EndDate = b.EndDate)
	)

	RETURN @AVERAGECOST
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetAverageCostDateRange] TO [IRMAReportsRole]
    AS [dbo];

