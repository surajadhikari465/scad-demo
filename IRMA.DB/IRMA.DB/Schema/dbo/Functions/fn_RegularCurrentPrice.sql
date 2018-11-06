CREATE FUNCTION [dbo].[fn_RegularCurrentPrice]
     (@POSPrice smallmoney)
RETURNS VARCHAR(8)
AS
BEGIN
	DECLARE @Result VARCHAR(8)

    IF @POSPrice IS NULL
        SET @Result = ''
    ELSE        
    IF @POSPrice = 0
        SET @Result = ''
    ELSE        
       SET @Result = LTRIM(RTRIM(CAST(STR(@POSPrice, 8, 2) AS VARCHAR(8))))

    return @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_RegularCurrentPrice] TO [IRMAClientRole]
    AS [dbo];

