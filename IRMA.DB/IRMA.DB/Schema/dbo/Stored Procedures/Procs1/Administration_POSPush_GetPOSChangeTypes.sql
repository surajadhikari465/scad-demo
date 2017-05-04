CREATE PROCEDURE dbo.Administration_POSPush_GetPOSChangeTypes
 AS 
-- Queries the POSChangeType  table to retrieve all of the available entries.
BEGIN

SELECT POSChangeTypeKey,POSDataTypeKey, ChangeTypeDesc  
FROM POSChangeType 
ORDER BY ChangeTypeDesc
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSChangeTypes] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSChangeTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSChangeTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSChangeTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSChangeTypes] TO [IRMAReportsRole]
    AS [dbo];

