IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_MSRP_Price') 
    DROP FUNCTION fn_MSRP_Price
GO

CREATE FUNCTION [dbo].[fn_MSRP_Price]
     (@MSRP_Price smallmoney)
RETURNS VARCHAR(8)
AS
BEGIN
	DECLARE @Result VARCHAR(8)

    IF @MSRP_Price IS NULL
        SET @Result = ''
    ELSE        
    IF @MSRP_Price = 0
        SET @Result = ''
    ELSE        
       SET @Result = LTRIM(RTRIM(CAST(STR(@MSRP_Price, 8, 2) AS VARCHAR(8))))

    return @Result
END

GO
