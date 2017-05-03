CREATE PROCEDURE dbo.GetItemIdentifersForItem
    @Item_Key int
AS

BEGIN
    SET NOCOUNT ON
 
    SELECT II.Identifier
    FROM ItemIdentifier II (nolock)
    WHERE II.Item_Key = @Item_Key

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetItemIdentifersForItem] TO [IRMAClientRole]
    AS [dbo];

