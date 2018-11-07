CREATE  FUNCTION [dbo].[fn_IsCustomerFacingScaleItem]
	(@Identifier varchar(13)
)
RETURNS bit
AS

BEGIN  
	DECLARE @return BIT;

	IF EXISTS (
				SELECT * 
				FROM dbo.ItemCustomerFacingScale icfs 
				JOIN ItemIdentifier ii ON icfs.Item_Key = ii.Item_Key
				WHERE ii.Identifier = @Identifier
					AND ii.Deleted_Identifier = 0
			  )
		SET @return = 1;
	ELSE
		SET @return = 0;

	RETURN @return;
END