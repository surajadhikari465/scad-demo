CREATE PROCEDURE dbo.GetRegionList
AS

BEGIN
    SET NOCOUNT ON

    SELECT DISTINCT Region_Code 
    FROM StoreRegionMapping

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionList] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegionList] TO [IRMASchedJobsRole]
    AS [dbo];

