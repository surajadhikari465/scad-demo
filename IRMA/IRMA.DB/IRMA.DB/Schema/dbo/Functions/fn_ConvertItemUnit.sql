CREATE FUNCTION dbo.fn_ConvertItemUnit 
	(@Amount money, @FromUnit int, @ToUnit int)
RETURNS money
AS
BEGIN
    DECLARE @ConversionSymbol varchar(1), @ConversionFactor float
    DECLARE @result money
    
    SELECT @result = @Amount
    
    SELECT @ConversionSymbol = ConversionSymbol, @ConversionFactor = ConversionFactor 
    FROM ItemConversion WITH (nolock) 
    WHERE FromUnit_ID = @FromUnit AND ToUnit_ID = @ToUnit

    IF @ConversionSymbol IS NOT NULL
    BEGIN
        IF (@ConversionSymbol = '*') SELECT @result = @result * @ConversionFactor
        IF (@ConversionSymbol = '/') SELECT @result = @result / @ConversionFactor
        IF (@ConversionSymbol = '+') SELECT @result = @result + @ConversionFactor
        IF (@ConversionSymbol = '-') SELECT @result = @result - @ConversionFactor
    END
    ELSE SELECT @Amount = 0

    RETURN @result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertItemUnit] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertItemUnit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertItemUnit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_ConvertItemUnit] TO [IRMAReportsRole]
    AS [dbo];

