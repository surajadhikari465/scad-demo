CREATE PROCEDURE dbo.Replenishment_POSPush_AddIdentifier
@Identifier_ID int
AS 

BEGIN
UPDATE ItemIdentifier 
SET Add_Identifier = 0
WHERE Identifier_ID = @Identifier_ID
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_AddIdentifier] TO [IRMAReportsRole]
    AS [dbo];

