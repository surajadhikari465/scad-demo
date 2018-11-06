
--=====================================================================
--*********      dbo.ItemAttributes_GetAttributeIdentifiersByItemKey                           
--=====================================================================

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ItemAttributes_GetAttributeIdentifiersByItemKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ItemAttributes_GetAttributeIdentifiersByItemKey]
GO

CREATE PROCEDURE dbo.ItemAttributes_GetAttributeIdentifiersByItemKey
	@Item_Key int
	
AS

	-- Created By:	James Winfield
	-- Created   :	Feb 23, 2007

	SELECT
		[AttributeIdentifier_ID],
		[Screen_Text],
		[field_type],
		[combo_box],
		[max_width],
		[default_value],
		[field_values]
	FROM AttributeIdentifier (NOLOCK) 
	WHERE @Item_Key = @Item_Key
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
