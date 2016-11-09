CREATE PROCEDURE [dbo].[CheckIfItemInOpenOrder] 
    @Item_Key int

AS 

BEGIN

SELECT count(OH.OrderHeader_ID) as OpenOrderCount 
FROM OrderHeader OH (nolock) 
inner join OrderItem OI (nolock) 
	on OH.OrderHeader_ID = OI.OrderHeader_ID
WHERE 
OI.Item_Key = @Item_Key
and OH.CloseDate IS NULL

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfItemInOpenOrder] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfItemInOpenOrder] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfItemInOpenOrder] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckIfItemInOpenOrder] TO [IRMAReportsRole]
    AS [dbo];

