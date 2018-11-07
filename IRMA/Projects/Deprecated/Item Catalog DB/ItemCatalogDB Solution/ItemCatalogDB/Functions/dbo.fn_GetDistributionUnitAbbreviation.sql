if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetDistributionUnitAbbreviation]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetDistributionUnitAbbreviation]
GO

CREATE  FUNCTION [dbo].[fn_GetDistributionUnitAbbreviation]
	(@Item_Key int)
RETURNS varchar(5)
AS

BEGIN 
-- returns empty string if unit id not found
	DECLARE @return varchar(5)    
    
    SELECT @return = Unit_Abbreviation FROM ItemUnit WHERE Unit_ID = (SELECT Distribution_Unit_ID FROM Item WHERE Item_Key = @Item_Key)
			        
	RETURN ISNULL(@return,'')
END 

GO
 