SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetItemDefaultValues')
	BEGIN
		DROP  Procedure  dbo.GetItemDefaultValues
	END

GO

CREATE PROCEDURE dbo.[GetItemDefaultValues]
	(
	@ProdHierarchyLevel4_ID As Int
	,@Category_ID As Int
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
	WHERE 	((ProdHierarchyLevel4_ID is null AND @ProdHierarchyLevel4_ID is null)  OR ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID)
		AND ((Category_ID is null AND @Category_ID is null) OR Category_ID = @Category_ID)
		
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

