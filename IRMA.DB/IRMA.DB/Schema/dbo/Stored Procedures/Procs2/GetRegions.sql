CREATE PROCEDURE dbo.GetRegions
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Region_ID, RegionName, RegionCode
    FROM Region
    ORDER BY RegionName
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegions] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegions] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegions] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegions] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetRegions] TO [IRMAReportsRole]
    AS [dbo];

