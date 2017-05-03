CREATE PROCEDURE [dbo].[GetKitchenRoutes]
AS 
BEGIN
	SELECT KitchenRoute_ID, Value
	FROM KitchenRoute (nolock)
	ORDER BY Value
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetKitchenRoutes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetKitchenRoutes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetKitchenRoutes] TO [IRMAReportsRole]
    AS [dbo];

