CREATE PROCEDURE dbo.UpdateItemIdentifierDefault 
@Item_Key int,
@Identifier_ID int
AS

UPDATE ItemIdentifier
SET Default_Identifier = (CASE WHEN Identifier_ID = @Identifier_ID THEN 1 ELSE 0 END)
WHERE Item_Key = @Item_Key
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemIdentifierDefault] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemIdentifierDefault] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateItemIdentifierDefault] TO [IRMAReportsRole]
    AS [dbo];

