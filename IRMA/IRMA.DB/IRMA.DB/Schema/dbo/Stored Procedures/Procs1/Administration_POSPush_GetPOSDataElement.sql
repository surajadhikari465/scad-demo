CREATE PROCEDURE dbo.Administration_POSPush_GetPOSDataElement 
	@POSDataTypeKey int,
	@BooleanOnly bit = 0	-- flag to only return elements which evaluate to True/False
AS
-- Reads all of the POSDataElement values for the POSDataType.

BEGIN
SELECT POSDataTypeKey, DataElement, Description, IsBoolean   
FROM 
POSDataElement 
WHERE POSDataTypeKey=@POSDataTypeKey AND 
	  ISNULL(IsBoolean, 0) >= ISNULL(@BooleanOnly, 0)
ORDER BY DataElement 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSDataElement] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSDataElement] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSDataElement] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSDataElement] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Administration_POSPush_GetPOSDataElement] TO [IRMAReportsRole]
    AS [dbo];

