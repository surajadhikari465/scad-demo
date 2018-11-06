CREATE PROCEDURE dbo.SupportRestoreDeletedItems
	@Identifiers NVARCHAR(MAX)
AS
BEGIN
PRINT '[' + CONVERT(NVARCHAR, GETDATE(), 121) + '] ' + 'Begin: [SupportRestoreDeletedItems.sql]'
	CREATE TABLE #TempItemIdentifier
	(
		Identifier VARCHAR(13) PRIMARY KEY
	)

	INSERT INTO #TempItemIdentifier
	SELECT Key_Value
	FROM fn_ParseStringList(@Identifiers, '|') list

	BEGIN TRY
		INSERT INTO SupportRestoreDeletedItemsItemKeys(Item_Key)
		SELECT i.Item_Key
		FROM Item i
		JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
		JOIN #TempItemIdentifier temp ON ii.Identifier = temp.Identifier
		WHERE i.Remove_Item = 1

		UPDATE ii
		SET Remove_Identifier = 0,
			Deleted_Identifier = 0
		FROM Item i
		JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
		JOIN #TempItemIdentifier temp ON ii.Identifier = temp.Identifier
		WHERE i.Remove_Item = 1

		UPDATE i
		SET Remove_Item = 0,
			Deleted_Item = 0
		FROM Item i
		JOIN ItemIdentifier ii ON i.Item_Key = ii.Item_Key
		JOIN #TempItemIdentifier temp ON ii.Identifier = temp.Identifier
		WHERE i.Remove_Item = 1

		DELETE SupportRestoreDeletedItemsItemKeys
		WHERE Item_Key IN (SELECT Item_Key
						   FROM ItemIdentifier ii
						   JOIN #TempItemIdentifier temp ON ii.Identifier = temp.Identifier)
	END TRY
	BEGIN CATCH
		DELETE SupportRestoreDeletedItemsItemKeys
		WHERE Item_Key IN (SELECT Item_Key
						   FROM ItemIdentifier ii
						   JOIN #TempItemIdentifier temp ON ii.Identifier = temp.Identifier)
	END CATCH

PRINT '[' + CONVERT(NVARCHAR, GETDATE(), 121) + '] ' + 'Finish: [SupportRestoreDeletedItems.sql]'
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SupportRestoreDeletedItems] TO [IRMAClientRole]
    AS [dbo];

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SupportRestoreDeletedItems] TO [IRSUser]
    AS [dbo];