CREATE PROCEDURE dbo.GetCostAdjustmentReasons
AS
BEGIN
    
    SELECT
		CostAdjustmentReason_ID,
		Description,
		IsDefault
    FROM
		CostAdjustmentReason
    
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetCostAdjustmentReasons] TO [IRMAClientRole]
    AS [dbo];

