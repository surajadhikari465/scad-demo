CREATE FUNCTION [dbo].[fn_IsSoldAsEachCostedByWeightItem]
(
    @Identifier  varchar(13)
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
		@IsWeightUnit		= iu.Weight_Unit,
		@IsPackageUnit		= iu.IsPackageUnit,
		@IsCostedByWeight	= i.CostedByWeight	 
    FROM 
		Item 						(nolock) i
		INNER JOIN ItemIdentifier	(nolock) ii ON i.Item_Key = ii.Item_Key
		INNER JOIN ItemUnit			(nolock) iu ON i.Retail_Unit_ID = iu.Unit_ID  
    WHERE ii.Identifier = @Identifier And ii.Default_Identifier = 1
    
    If ((@IsWeightUnit = 0 AND @IsPackageUnit = 0) AND @IsCostedByWeight = 1) RETURN 1
    Return 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsSoldAsEachCostedByWeightItem] TO [IRMAClientRole]
    AS [dbo];

