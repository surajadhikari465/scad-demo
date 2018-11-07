CREATE PROCEDURE dbo.GetItemInfoByIdentifier
    @Identifier varchar(20)
AS 

BEGIN
    SET NOCOUNT ON
    
    SELECT Item.*
    FROM Item (NOLOCK)
    INNER JOIN
        ItemIdentifier (NOLOCK)
        ON ItemIdentifier.Item_Key = Item.Item_Key 
    WHERE ItemIdentifier.Identifier = @Identifier
     AND NOT Deleted_Identifier = 1
    
    SET NOCOUNT OFF

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfoByIdentifier] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfoByIdentifier] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfoByIdentifier] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfoByIdentifier] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemInfoByIdentifier] TO [IRMAExcelRole]
    AS [dbo];

