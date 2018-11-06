IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_IsRetailSaleItem]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_IsRetailSaleItem]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE FUNCTION [dbo].[fn_IsRetailSaleItem]
(
	@Item_Key int
)
RETURNS bit
AS

BEGIN
	declare @retVal bit

	if @Item_Key <= 0
		select @retVal = 0
	else
	begin
		if exists
			(
			SELECT top 1 i.Item_Key
			 FROM Item i (nolock)
			WHERE
				i.item_key = @Item_key
			  AND 
				i.Retail_Sale = 1
			)
			select @retVal = 1
		else
			select @retVal = 0
	end

	return @retVal
END
go
