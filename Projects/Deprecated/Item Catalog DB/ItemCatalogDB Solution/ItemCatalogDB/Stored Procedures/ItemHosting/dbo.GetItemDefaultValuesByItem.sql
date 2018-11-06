SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetItemDefaultValuesByItem')
	BEGIN
		DROP  Procedure  dbo.GetItemDefaultValuesByItem
	END

GO

CREATE PROCEDURE dbo.GetItemDefaultValuesByItem
	(
	@Item_Key As Int
	)
	
AS

	SELECT idv.ItemDefaultValue_ID
		,idv.ItemDefaultAttribute_ID
		,idv.ProdHierarchyLevel4_ID
		,idv.Category_ID
		,idv.Value
		,ida.AttributeField
		,ida.Type
	FROM ItemDefaultValue (NOLOCK) idv
		JOIN ItemDefaultAttribute (NOLOCK) ida ON ida.ItemDefaultAttribute_ID = idv.ItemDefaultAttribute_ID
		JOIN Item (NOLOCK) itm ON itm.Item_Key = @Item_Key
			AND ((itm.ProdHierarchyLevel4_ID is null AND idv.ProdHierarchyLevel4_ID is null)  OR itm.ProdHierarchyLevel4_ID = idv.ProdHierarchyLevel4_ID)
			AND ((itm.Category_ID is null AND idv.Category_ID is null) OR itm.Category_ID = idv.Category_ID)			
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


