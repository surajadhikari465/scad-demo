CREATE PROCEDURE [dbo].[PlumCorpChgQueueTmpDeletedItemsValidate]	
@ItemKey INT
AS

BEGIN
SELECT Case WHEN Exists(SELECT 1 FROM PLUMCorpChgQueueTmp WHERE Item_Key = @ItemKey) THEN 1 ELSE 0 END IsExist;
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PlumCorpChgQueueTmpDeletedItemsValidate] TO [IRMAClientRole]
    AS [dbo];
