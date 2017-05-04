CREATE PROCEDURE dbo.GetInventoryCountVendors
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT  ICVID, ICVABBRV
    FROM    CycleCountVendor
    ORDER BY ICVABBRV
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryCountVendors] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryCountVendors] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryCountVendors] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryCountVendors] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetInventoryCountVendors] TO [IRMAReportsRole]
    AS [dbo];

