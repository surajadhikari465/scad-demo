if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsCaseItemUnit]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsCaseItemUnit]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

Create  FUNCTION dbo.fn_IsCaseItemUnit
	(@Unit_ID int)
RETURNS bit
AS

BEGIN 
	--RETURNS 1 (TRUE) IF THE ItemUnit.Unit_ID PASSED IN IS A 'CASE' ITEM;  RETURNS 0 (FALSE) IF NOT A CASE ITEM 
	DECLARE @return bit    
    
    SELECT @return = IsPackageUnit FROM ItemUnit WHERE Unit_ID = @Unit_ID
			        
	RETURN ISNULL(@return,0)
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

 