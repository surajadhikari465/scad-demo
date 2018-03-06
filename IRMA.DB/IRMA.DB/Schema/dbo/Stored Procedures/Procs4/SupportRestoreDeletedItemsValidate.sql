CREATE PROCEDURE dbo.SupportRestoreDeletedItemsValidate
	@Identifier NVARCHAR(13)
AS
BEGIN
PRINT '[' + CONVERT(NVARCHAR, GETDATE(), 121) + '] ' + 'Begin: [SupportRestoreDeletedItemsValidate.sql]'

	IF 0 = (SELECT COUNT(*) FROM ItemIdentifier ii WHERE ii.Identifier = @Identifier)
	BEGIN
		SELECT @Identifier + ' does not exist.' AS ValidationError
	END
	ELSE IF 0 = (SELECT COUNT(*) 
				 FROM ItemIdentifier ii 
				 JOIN Item i ON ii.Item_Key = i.Item_Key
				 WHERE ii.Identifier = @Identifier
					AND i.Remove_Item = 1) 
	BEGIN
		SELECT @Identifier + '''s Remove_Item is not equal to 1.' AS ValidationError
	END

PRINT '[' + CONVERT(NVARCHAR, GETDATE(), 121) + '] ' + 'Finish: [SupportRestoreDeletedItemsValidate.sql]'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsValidate] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SupportRestoreDeletedItemsValidate] TO [IRSUser]
    AS [dbo];