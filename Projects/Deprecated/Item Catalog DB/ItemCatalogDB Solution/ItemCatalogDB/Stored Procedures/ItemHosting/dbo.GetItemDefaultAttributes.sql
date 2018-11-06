SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetItemDefaultAttributes')
	BEGIN
		DROP  Procedure  dbo.GetItemDefaultAttributes
	END

GO

CREATE PROCEDURE dbo.GetItemDefaultAttributes
	(
	@ProdHierarchyLevel4_ID As Int
	,@Category_ID As Int
	)
AS

	SELECT ida.ItemDefaultAttribute_ID
		,ida.AttributeName
		,ida.AttributeField
		,ida.Type
		,ida.Active
		,ida.ControlOrder
		,ida.ControlType
		,ida.PopulateProcedure
		,ida.IndexField
		,ida.DescriptionField
		,idv.ItemDefaultValue_ID
		,idv.Value
	FROM ItemDefaultAttribute (NOLOCK) ida LEFT OUTER JOIN ItemDefaultValue (NOLOCK) idv
		ON ida.ItemDefaultAttribute_ID = idv.ItemDefaultAttribute_ID
		AND (@ProdHierarchyLevel4_ID is null OR idv.ProdHierarchyLevel4_ID = @ProdHierarchyLevel4_ID)
		AND (@Category_ID is null OR idv.Category_ID = @Category_ID)
	WHERE ida.Active = 1
	ORDER BY ida.ControlOrder

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

