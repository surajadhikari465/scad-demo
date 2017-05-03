
CREATE FUNCTION [dbo].[fn_GetUOMConversionDivisor]
(
    @TaxJurisdictionDesc VARCHAR(30),
    @Unit_Abbreviation   VARCHAR(5),
    @ItemAttribute       VARCHAR(50)
)
RETURNS FLOAT
AS
BEGIN		
    DECLARE @Divisor FLOAT

    IF RTRIM(LTRIM(@TaxJurisdictionDesc)) <> 'NJ'
        SET @Divisor = 1
    ELSE
    IF @ItemAttribute IS NOT NULL
        IF @ItemAttribute <> 'Not Applicable'
            SET @Divisor = (SELECT UOM_ConversionDivisor
                            FROM   UOM_Conversion
                            WHERE  UOM_Abbreviation = @Unit_Abbreviation AND
                                   UOM_Conversion   = @ItemAttribute)

	RETURN @Divisor
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetUOMConversionDivisor] TO [IRMAClientRole]
    AS [dbo];

