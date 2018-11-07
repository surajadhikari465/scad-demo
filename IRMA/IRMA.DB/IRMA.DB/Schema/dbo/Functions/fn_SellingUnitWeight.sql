CREATE FUNCTION [dbo].[fn_SellingUnitWeight]
    (@Package_Desc1 DECIMAL(9,4),
     @Package_Desc2 DECIMAL(9,4))
RETURNS VARCHAR(8)
AS
BEGIN
	DECLARE @Result     VARCHAR(8)
	DECLARE @TempResult DECIMAL(9,4)

    IF @Package_Desc1 IS NULL OR @Package_Desc2 IS NULL
        SET @Result = ''
    ELSE
    BEGIN
        SET @TempResult = @Package_Desc1 * @Package_Desc2
        SET @Result     = LTRIM(RTRIM(CAST(STR(@TempResult, 8, 2) AS VARCHAR(8))))
    END
    
    return @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_SellingUnitWeight] TO [IRMAClientRole]
    AS [dbo];

