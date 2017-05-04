if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateNoTagRuleThresholds]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.UpdateNoTagRuleThresholds
go

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