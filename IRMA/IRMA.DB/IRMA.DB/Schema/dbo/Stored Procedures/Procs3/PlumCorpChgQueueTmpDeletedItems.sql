CREATE PROCEDURE [dbo].[PlumCorpChgQueueTmpDeletedItems]
	@ItemKey NVARCHAR(MAX)
AS
BEGIN
        DELETE [dbo].PLUMCorpChgQueueTmp		
        WHERE Item_Key IN (SELECT DISTINCT Cast(Key_Value AS INT) FROM fn_ParseStringList(@ItemKey, '|') WHERE IsNumeric(Key_Value) = 1)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PlumCorpChgQueueTmpDeletedItems] TO [IRMAClientRole]
    AS [dbo];
