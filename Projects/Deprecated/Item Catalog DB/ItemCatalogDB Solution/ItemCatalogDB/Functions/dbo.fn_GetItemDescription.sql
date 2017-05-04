IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_GetItemDescription') 
    DROP FUNCTION fn_GetItemDescription
GO

CREATE FUNCTION [dbo].[fn_GetItemDescription]
    (@Identifier  varchar(13))
RETURNS VARCHAR(MAX)
AS
BEGIN
	DECLARE @Description VARCHAR(60)
    DECLARE @Item_Key INT
	
	SELECT @Description = Item.Item_Description, @Item_Key = Item.Item_Key
	FROM         Item INNER JOIN
						  ItemIdentifier ON Item.Item_Key = ItemIdentifier.Item_Key
	WHERE     (ItemIdentifier.Identifier = @Identifier)

    RETURN (CAST(@Item_Key as varchar(max)) + '|' + @Description)
END


GO