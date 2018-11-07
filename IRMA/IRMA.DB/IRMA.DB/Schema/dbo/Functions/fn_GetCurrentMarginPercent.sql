-- =============================================
-- Author:		Hussain Hashim
-- Create date: 08/02/2007
-- Description:	Gets Margin for a unit
-- =============================================
CREATE FUNCTION [dbo].[fn_GetCurrentMarginPercent]
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
	DECLARE @MARGIN money

	-- Add the T-SQL statements to compute the return value here
    if @Price * @Multiple > 0
        select @MARGIN = 
            ((@Price / @Multiple) - (@Cost / @CasePack)) / (@Price / @Multiple)
    else
        select @MARGIN = (0.0)

	-- Return the result of the function
	RETURN @MARGIN

END