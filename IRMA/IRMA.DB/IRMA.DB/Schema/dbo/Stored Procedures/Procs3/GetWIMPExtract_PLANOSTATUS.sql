CREATE PROCEDURE dbo.GetWIMPExtract_PLANOSTATUS
AS
BEGIN
    SET NOCOUNT ON
    
		SELECT	ItemIdentifier.Identifier, ItemAttribute.Text_3
		FROM    ItemIdentifier
		INNER JOIN ItemAttribute ON ItemIdentifier.Item_Key = ItemAttribute.Item_Key
		WHERE ItemAttribute.Text_3 IS NOT NULL

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_PLANOSTATUS] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_PLANOSTATUS] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_PLANOSTATUS] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetWIMPExtract_PLANOSTATUS] TO [IRMAReportsRole]
    AS [dbo];

