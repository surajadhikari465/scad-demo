CREATE FUNCTION [dbo].[fn_GetDateOnly] ( @pInputDate    DATETIME )
RETURNS DATETIME
BEGIN

    RETURN CAST(CAST(YEAR(@pInputDate) AS VARCHAR(4)) + '/' +
                CAST(MONTH(@pInputDate) AS VARCHAR(2)) + '/' +
                CAST(DAY(@pInputDate) AS VARCHAR(2)) AS DATETIME)

END