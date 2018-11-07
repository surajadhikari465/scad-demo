CREATE PROCEDURE dbo.GetCostPromoCodeTypes
AS
BEGIN
    SET NOCOUNT ON

	SELECT CostPromoCodeTypeID, 
		CostPromoCode, 
		(CONVERT(varchar, CostPromoCode) + ' - ' + CostPromoDesc) AS CostPromoDesc
    FROM CostPromoCodeType
	ORDER BY CostPromoCode
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCostPromoCodeTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCostPromoCodeTypes] TO [IRMAClientRole]
    AS [dbo];

