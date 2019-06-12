CREATE PROCEDURE [dbo].[PlumCorpChgQueueTmpDeletedItemsValidate]	
@ScanCode NVARCHAR(13)
AS

BEGIN
	SELECT Case WHEN EXISTS
	(SELECT 1 FROM PLUMCorpChgQueueTmp pct
	JOIN ItemIdentifier ii WITH (NOLOCK) ON ii.Item_Key = pct.Item_Key
	WHERE ii.Identifier = @ScanCode) 
	THEN 1 ELSE 0 END IsExist;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PlumCorpChgQueueTmpDeletedItemsValidate] TO [IRMAClientRole]
    AS [dbo];
