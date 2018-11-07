CREATE FUNCTION [dbo].[FN_GetExePack] 
	(@Package_Desc1 int, @Package_Desc2 Decimal(9, 4), @CostedByWeight bit)
RETURNS  Decimal(9, 4)
AS
BEGIN
    DECLARE @Result Decimal(9, 4)


    SELECT @Result = CASE WHEN @CostedByWeight = 1
                            THEN @Package_Desc1 * CASE WHEN round(@Package_Desc2, 0) = 0 THEN 1 ELSE round(@Package_Desc2, 0) END
                            ELSE @Package_Desc1
                          END

    RETURN @Result
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FN_GetExePack] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[FN_GetExePack] TO [IRMAReportsRole]
    AS [dbo];

