IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_MSRP_UnitsPerPrice') 
    DROP FUNCTION fn_MSRP_UnitsPerPrice
GO

CREATE FUNCTION [dbo].[fn_MSRP_UnitsPerPrice]
     (@MSRPMultiple tinyint)
RETURNS VARCHAR(8)
AS
BEGIN
	DECLARE @Result VARCHAR(8)

    IF @MSRPMultiple IS NULL
        SET @Result = ''
    ELSE        
    IF @MSRPMultiple = 0
        SET @Result = ''
    ELSE        
       SET @Result = LTRIM(RTRIM(CAST(@MSRPMultiple AS VARCHAR(8))))

    return @Result
END

GO