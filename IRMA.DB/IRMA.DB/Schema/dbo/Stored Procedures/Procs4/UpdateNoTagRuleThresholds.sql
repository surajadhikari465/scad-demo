
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-10
-- Description:	Updates the default history threshold
--				for the three no-tag rules.
-- =============================================

CREATE PROCEDURE [dbo].[UpdateNoTagRuleThresholds]
	@RuleThresholds dbo.NoTagRuleThresholdType readonly
AS
BEGIN
	set nocount on

    update
		NoTagRuleThreshold
	set
		ThresholdValueDays = rt.ThresholdValue
	from
		NoTagRuleThreshold nt
		join @RuleThresholds rt on nt.RuleName = rt.RuleName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateNoTagRuleThresholds] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateNoTagRuleThresholds] TO [IRSUser]
    AS [dbo];

