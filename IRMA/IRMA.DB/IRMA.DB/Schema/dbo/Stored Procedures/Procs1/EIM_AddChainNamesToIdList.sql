-- Add chain names to a comma delimited list of chain ids.
CREATE PROCEDURE dbo.EIM_AddChainNamesToIdList
    @ItemChainIdList varchar(1000)
    ,
    @ItemChainIdAndNameList varchar(8000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    SET @ItemChainIdAndNameList = NULL

	SELECT @ItemChainIdAndNameList =
			COALESCE(@ItemChainIdAndNameList + ', ', '') + IsNull(CAST(ItemChain.ItemChainID AS varchar(200)), '-1') + ': ' + IsNull(ItemChainDesc, 'NAME_NOT_FOUND')
		FROM dbo.fn_Parse_List(@ItemChainIdList, ',') ChainIdTable
			LEFT JOIN ItemChain  (nolock)
			ON ItemChain.ItemChainId = ChainIdTable.Key_Value
		ORDER BY ItemChainDesc
		
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[EIM_AddChainNamesToIdList] TO [IRMAClientRole]
    AS [dbo];

