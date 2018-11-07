CREATE Procedure dbo.SLIM_GetItemRejectInfo
	@RequestIdList varchar(MAX)
AS
	SELECT ii.Identifier, i.Item_Description
	FROM SLIM_InStoreSpecials iss
	JOIN Item i ON i.Item_Key = iss.Item_Key
	JOIN ItemIdentifier ii ON ii.Item_Key = iss.Item_Key
	WHERE ii.Default_Identifier = 1 AND
		  iss.RequestId IN (SELECT Key_Value FROM dbo.fn_Parse_List(@RequestIdList,'|'))
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SLIM_GetItemRejectInfo] TO [IRMASLIMRole]
    AS [dbo];

