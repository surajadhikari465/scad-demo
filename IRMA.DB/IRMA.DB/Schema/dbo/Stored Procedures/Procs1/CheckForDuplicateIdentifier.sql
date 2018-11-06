CREATE PROCEDURE dbo.CheckForDuplicateIdentifier 
    @Identifier varchar(13) 
AS 
BEGIN
    SET NOCOUNT ON

    SELECT COUNT(*) AS IdentifierCount
    FROM ItemIdentifier
    WHERE Identifier = @Identifier AND Deleted_Identifier = 0

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateIdentifier] TO [IRMAExcelRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckForDuplicateIdentifier] TO [IRMASLIMRole]
    AS [dbo];

