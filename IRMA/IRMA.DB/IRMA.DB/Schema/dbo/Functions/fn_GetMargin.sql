CREATE function [dbo].[fn_GetMargin] (
    @Price smallmoney,
    @Multiple int,
    @Cost smallmoney )
returns money --(9,8)
as
begin
    declare @MARGIN money
    
    if @Price * @Multiple > 0
        select @MARGIN = 
            100.00 *((@Price / @Multiple) - @Cost) / (@Price / @Multiple)
    else
        select @MARGIN = (0.0)
    
    return @MARGIN
end
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetMargin] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetMargin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetMargin] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[fn_GetMargin] TO [IRMASLIMRole]
    AS [dbo];

