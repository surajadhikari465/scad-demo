CREATE PROCEDURE dbo.GetPOSSystemTypes 
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT POSSystemId, POSSystemType
    FROM POSSystemTypes (NOLOCK)     
    ORDER BY POSSystemType
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOSSystemTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPOSSystemTypes] TO [IRMAClientRole]
    AS [dbo];

