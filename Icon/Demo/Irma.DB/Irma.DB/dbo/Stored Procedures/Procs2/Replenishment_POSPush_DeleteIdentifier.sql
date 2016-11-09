CREATE PROCEDURE dbo.Replenishment_POSPush_DeleteIdentifier
@Identifier_ID int
AS 

BEGIN
DELETE 
FROM ItemIdentifier
WHERE Identifier_ID = @Identifier_ID 
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_DeleteIdentifier] TO [IRMAReportsRole]
    AS [dbo];

