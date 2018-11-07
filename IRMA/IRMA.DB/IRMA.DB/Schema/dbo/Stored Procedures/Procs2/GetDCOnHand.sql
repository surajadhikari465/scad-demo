CREATE PROCEDURE [dbo].[GetDCOnHand] 
    @Item_Key int

AS 

BEGIN

SELECT 
Quantity, Weight
FROM   OnHand OH (nolock)
INNER JOIN Store S (nolock) on OH.Store_No = S.Store_No
WHERE  OH.Item_Key = @Item_Key 
AND (S.Distribution_Center = 1 OR S.Manufacturer = 1) AND S.Regional = 0

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDCOnHand] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDCOnHand] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDCOnHand] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDCOnHand] TO [IRMAReportsRole]
    AS [dbo];

