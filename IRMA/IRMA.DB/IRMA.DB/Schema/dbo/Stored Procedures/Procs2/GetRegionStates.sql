CREATE PROCEDURE dbo.GetRegionStates

AS

BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT State 
    FROM Vendor
    INNER JOIN Store ON Store.Store_No = Vendor.Store_No
    INNER JOIN Zone ON Zone.Zone_ID = Store.Zone_ID
    WHERE Region_ID IS NOT NULL
    AND LEN(RTRIM(ISNULL(State, ''))) > 0
    ORDER BY State

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionStates] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionStates] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionStates] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionStates] TO [IRMAReportsRole]
    AS [dbo];

