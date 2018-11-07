-- =============================================
-- Author:		Hussain Hashim
-- Create date: 08/02/2007
-- Description:	Gets Markup for a unit
-- =============================================
CREATE FUNCTION [dbo].[fn_GetCurrentMarkupPercent]
(
	-- Add the parameters for the function here
    @Price smallmoney,
    @Multiple int,
    @Cost smallmoney,
	@CasePack	int 
)
RETURNS Money
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Markup money

	-- Add the T-SQL statements to compute the return value here
	-- configure this so zero cost doesn't return an error.
    if @Price * @Multiple > 0 and @Cost > 0 and @CasePack > 0 
        select @Markup = 
            (@Price / @Multiple) / (@Cost / @CasePack)
    else
        select @Markup = (0.0)

	-- Return the result of the function
	RETURN @Markup

END