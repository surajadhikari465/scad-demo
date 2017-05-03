set ANSI_NULLS ON
set QUOTED_IDENTIFIER ON
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_GetUOMConversionFactor]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_GetUOMConversionFactor]

GO

CREATE FUNCTION [dbo].[fn_GetUOMConversionFactor]
(
    @TaxJurisdictionDesc VARCHAR(30),
    @Unit_Abbreviation   VARCHAR(5),
    @ItemAttribute       VARCHAR(50)
)
RETURNS FLOAT
AS
BEGIN
    DECLARE @Factor FLOAT	

    -- Set default factor for both non-New Jersey items and for
    -- New Jersey items that do not require unit of measure conversion.

    SET @Factor = 1

    IF RTRIM(LTRIM(@TaxJurisdictionDesc)) = 'NJ' AND @ItemAttribute IS NOT NULL
        IF @ItemAttribute <> 'Not Applicable'
            SET @Factor = (SELECT UOM_ConversionFactor
                            FROM   UOM_Conversion
                            WHERE  UOM_Abbreviation = @Unit_Abbreviation AND
                                   UOM_Conversion   = @ItemAttribute)

        RETURN @Factor
END

GO
