CREATE PROCEDURE [dbo].[GetVendorItemStatuses]
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT StatusID, StatusName
    FROM VendorItemStatuses
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetVendorItemStatuses] TO [IRMAClientRole]
    AS [dbo];

