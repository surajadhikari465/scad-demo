CREATE Procedure [dbo].[CountUnrankedSustainabilityRequiredOrderItems]
    @OrderHeader_ID int
AS
SELECT COUNT(*) AS OrderItemCount 
FROM OrderItem OI (nolock) 
INNER JOIN Item (nolock) ON Item.Item_Key = OI.Item_Key
WHERE OrderHeader_ID = @OrderHeader_ID
    AND Item.SustainabilityRankingRequired = 1 
    AND OI.SustainabilityRankingID IS NULL
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountUnrankedSustainabilityRequiredOrderItems] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountUnrankedSustainabilityRequiredOrderItems] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountUnrankedSustainabilityRequiredOrderItems] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CountUnrankedSustainabilityRequiredOrderItems] TO [IRMAReportsRole]
    AS [dbo];

