CREATE function [dbo].[fn_GetAverageUnitWeightByItemKey]
	(@Item_Key int)
returns decimal(9,4)
as
begin
	DECLARE @AverageUnitWeight decimal(9,4)
	
	SELECT 
		@AverageUnitWeight = i.Average_Unit_Weight
	FROM item					(nolock) i
	INNER JOIN ItemIdentifier	(nolock) ii ON i.Item_Key = ii.Item_Key 
											AND ii.Default_Identifier = 1
	WHERE i.Item_Key = @Item_Key 
	RETURN @AverageUnitWeight
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetAverageUnitWeightByItemKey] TO [IRMAClientRole]
    AS [dbo];

