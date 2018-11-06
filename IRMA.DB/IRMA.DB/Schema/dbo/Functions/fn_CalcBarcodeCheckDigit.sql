CREATE FUNCTION dbo.fn_CalcBarcodeCheckDigit 
	(@Barcode varchar(13))
RETURNS char(1)
AS
BEGIN
    DECLARE @MAX_BAR_CODE_LEN int, @In varchar(14), @i int, @OddNumbers int, @EvenNumbers int, @OddAndEvenSum int, @x int, @result int

    SELECT @MAX_BAR_CODE_LEN = 14

    SELECT @In = REPLICATE('0', @MAX_BAR_CODE_LEN - LEN(@Barcode)) + @Barcode

    SELECT @EvenNumbers = 0, @i = 1
    WHILE @i <= @MAX_BAR_CODE_LEN
    BEGIN
        SELECT @EvenNumbers = @EvenNumbers + SUBSTRING(@In, @i, 1)
        SELECT @i = @i + 2
    END
    
    SELECT @OddNumbers = 0, @i = 2
    WHILE @i <= @MAX_BAR_CODE_LEN
    BEGIN
        SELECT @OddNumbers = @OddNumbers + SUBSTRING(@In, @i, 1)
        SELECT @i = @i + 2
    END

    SELECT @OddNumbers = @OddNumbers * 3
    
    SELECT @OddAndEvenSum = @OddNumbers + @EvenNumbers

    SELECT @x = 10 - (@OddAndEvenSum % 10)

    IF @x < 10
        SELECT @result = @x
    ELSE
        SELECT @result = 0

    RETURN @result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CalcBarcodeCheckDigit] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CalcBarcodeCheckDigit] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CalcBarcodeCheckDigit] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_CalcBarcodeCheckDigit] TO [IRMAReportsRole]
    AS [dbo];

