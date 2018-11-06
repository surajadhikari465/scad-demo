if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetRetailUnitAbbreviation]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetRetailUnitAbbreviation]
GO

CREATE  FUNCTION [dbo].[fn_GetRetailUnitAbbreviation]
	(@Item_Key int)
RETURNS varchar(5)
AS

BEGIN 
-- returns empty string if unit id not found
	DECLARE @return varchar(5)    
    
    SELECT @return = Unit_Abbreviation FROM ItemUnit WHERE Unit_ID = (SELECT Retail_Unit_ID FROM Item WHERE Item_Key = @Item_Key)
			        
	RETURN ISNULL(@return,'')
END 

GO