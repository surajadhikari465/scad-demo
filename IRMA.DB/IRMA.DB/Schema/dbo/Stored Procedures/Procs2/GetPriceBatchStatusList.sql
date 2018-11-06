CREATE PROCEDURE dbo.GetPriceBatchStatusList

AS

BEGIN
    SET NOCOUNT ON

    SELECT PriceBatchStatusID, PriceBatchStatusDesc
    FROM PriceBatchStatus (nolock)
    ORDER BY PriceBatchStatusID

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchStatusList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchStatusList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchStatusList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPriceBatchStatusList] TO [IRMAReportsRole]
    AS [dbo];

