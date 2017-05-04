if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[EIM_GetChainIdsFromNameList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure [dbo].[EIM_GetChainIdsFromNameList]
GO

-- Add chain names to a comma delimited list of chain ids.
CREATE PROCEDURE dbo.EIM_GetChainIdsFromNameList
    @ItemChainNameList varchar(8000)
    ,
    @ItemChainIdList varchar(8000) OUTPUT
AS
BEGIN
    SET NOCOUNT ON
    
    SET @ItemChainIdList = NULL

	SELECT @ItemChainIdList =
			COALESCE(@ItemChainIdList + ', ', '') + IsNull(CAST(ItemChain.ItemChainID AS varchar(200)), '-1')
		FROM dbo.fn_ParseStringList(@ItemChainNameList, ',') ChainNameTable
			LEFT JOIN ItemChain  (nolock)
				ON LOWER(RTRIM(LTRIM(ItemChain.ItemChainDesc))) = LOWER(RTRIM(LTRIM(ChainNameTable.Key_Value)))
		ORDER BY ItemChainDesc
		
    SET NOCOUNT OFF
END
GO   