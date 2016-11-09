CREATE FUNCTION [dbo].[fn_IsWeightUnit]
(
    @UnitID int
)
RETURNS Bit
AS
BEGIN
    DECLARE @IsWeightUnit as Bit	
    DECLARE @IsPackageUnit as Bit

    SELECT @IsWeightUnit = Weight_Unit 
    FROM ItemUnit 
    WHERE Unit_ID = @UnitID

    SELECT @IsPackageUnit = IsPackageUnit 
    FROM ItemUnit 
    WHERE Unit_ID = @UnitID
    
    If @IsWeightUnit = 1 or @IsPackageUnit = 1 RETURN 1
    Return 0
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_IsWeightUnit] TO [IRMAClientRole]
    AS [dbo];

