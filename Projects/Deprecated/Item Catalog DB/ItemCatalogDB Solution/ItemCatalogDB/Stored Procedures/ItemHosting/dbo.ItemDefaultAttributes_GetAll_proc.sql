/****** Object:  StoredProcedure [dbo].[ItemDefaultAttributes_GetAll]    Script Date: 07/25/2012 14:57:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ItemDefaultAttributes_GetAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[ItemDefaultAttributes_GetAll]
GO

/****** Object:  StoredProcedure [dbo].[ItemDefaultAttributes_GetAll]    Script Date: 07/25/2012 14:57:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[ItemDefaultAttributes_GetAll]
	--(
	--@ProdHierarchyLevel4_ID As Int
	--,@Category_ID As Int
	--)
AS

	SELECT ida.ItemDefaultAttribute_ID,
		ida.AttributeName,
		ida.AttributeField,
		ida.Active,
		ida.ControlOrder,
		CASE ida.ControlType
			WHEN 1 THEN 'Text Field'
			WHEN 2 THEN 'Dropdown List'
			WHEN 3 THEN 'Checkbox'
		END as 'ControlTypeName',
		ida.ControlType
	FROM ItemDefaultAttribute (NOLOCK) ida 
       ORDER BY ida.ControlOrder

GO


