	
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-08
-- Description:	Gets the history threshold for a
--				given no-tag rule.
-- =============================================

CREATE PROCEDURE [dbo].[GetNoTagRuleThreshold]
	@RuleName nvarchar(64) = null
AS
BEGIN
	set nocount on

	if @RuleName is null
		begin
			select nt.RuleName, nt.ThresholdValueDays from NoTagRuleThreshold nt
		end
	else
		begin
			select nt.RuleName, nt.ThresholdValueDays from NoTagRuleThreshold nt where nt.RuleName = @RuleName
		end    
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoTagRuleThreshold] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNoTagRuleThreshold] TO [IRSUser]
    AS [dbo];

