CREATE PROCEDURE [dbo].[PlumCorpChgQueueTmpDeletedItems]
	@ScanCode NVARCHAR(MAX)
AS
BEGIN
	DELETE pcc
	FROM PLUMCorpChgQueueTmp pcc
	JOIN ItemIdentifier ii ON ii.Item_Key = pcc.Item_Key
	JOIN (SELECT DISTINCT Key_Value AS ScanCode FROM fn_ParseStringList(@ScanCode, '|')) temp ON temp.ScanCode = ii.identifier
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PlumCorpChgQueueTmpDeletedItems] TO [IRMAClientRole]
    AS [dbo];
