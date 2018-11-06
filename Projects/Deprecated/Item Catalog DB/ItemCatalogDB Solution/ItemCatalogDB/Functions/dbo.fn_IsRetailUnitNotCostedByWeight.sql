SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsRetailUnitCostedByWeight]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsRetailUnitCostedByWeight]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_IsRetailUnitNotCostedByWeight]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_IsRetailUnitNotCostedByWeight]
GO

CREATE FUNCTION [dbo].[fn_IsRetailUnitNotCostedByWeight]
(
    @Item_Key  int
)
RETURNS Bit
AS
BEGIN
    DECLARE @IsWeightUnit as Bit	
    DECLARE @IsPackageUnit as Bit
    DECLARE @IsCostedByWeight as Bit

	SET @IsWeightUnit = 0
	SET @IsPackageUnit = 0
	SET @IsCostedByWeight = 0
	
    SELECT 
		@IsWeightUnit		= ii.Weight_Unit,
		@IsPackageUnit		= ii.IsPackageUnit,
		@IsCostedByWeight	= i.CostedByWeight	 
    FROM 
		Item 					(nolock) i
		INNER JOIN ItemUnit		(nolock) ii ON i.Retail_Unit_ID = ii.Unit_ID  
    WHERE i.Item_Key = @Item_Key
    
    If ((@IsWeightUnit = 0 AND @IsPackageUnit = 0) AND @IsCostedByWeight = 1) RETURN 1
    Return 0
END
GO
