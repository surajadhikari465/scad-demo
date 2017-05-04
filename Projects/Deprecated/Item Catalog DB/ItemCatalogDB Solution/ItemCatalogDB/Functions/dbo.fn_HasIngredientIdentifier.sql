IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_HasIngredientIdentifier]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_HasIngredientIdentifier]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[fn_HasIngredientIdentifier]
(
	@Item_Key int
)
RETURNS bit
AS

BEGIN
	declare @retVal bit

	if exists
		 (
		   SELECT top 1 Identifier
			 FROM ItemIdentifier (nolock)
			WHERE
				item_key = @Item_key
			  AND
				Deleted_Identifier = 0
			  AND
				Remove_Identifier = 0
			  AND 
				((CONVERT(FLOAT, Identifier) >= 46000000000 And CONVERT(FLOAT, Identifier)  <= 46999999999) Or (CONVERT(FLOAT, Identifier) >= 48000000000 And CONVERT(FLOAT, Identifier) <= 48999999999))
		  )
		select @retVal = 1
	else
		select @retVal = 0
	
	return @retVal
END
go
