if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetNoTagRuleThreshold]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	drop procedure dbo.GetNoTagRuleThreshold
go
	
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
go