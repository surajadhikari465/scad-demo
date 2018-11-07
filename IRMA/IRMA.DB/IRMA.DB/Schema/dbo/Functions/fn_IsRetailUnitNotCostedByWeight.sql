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
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsRetailUnitNotCostedByWeight] TO [IRMAClientRole]
    AS [dbo];

