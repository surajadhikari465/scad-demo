if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[fn_GetDateOnly]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[fn_GetDateOnly]
GO

CREATE FUNCTION [dbo].[fn_GetDateOnly] ( @pInputDate    DATETIME )
RETURNS DATETIME
BEGIN

    RETURN CAST(CAST(YEAR(@pInputDate) AS VARCHAR(4)) + '/' +
                CAST(MONTH(@pInputDate) AS VARCHAR(2)) + '/' +
                CAST(DAY(@pInputDate) AS VARCHAR(2)) AS DATETIME)

END
GO

