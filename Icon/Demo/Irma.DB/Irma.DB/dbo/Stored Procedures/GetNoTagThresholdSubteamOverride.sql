
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-08
-- Description:	Gets the no-tag override threshold value
--				for a given subteam.
-- =============================================

CREATE PROCEDURE [dbo].[GetNoTagThresholdSubteamOverride]
	@SubteamNumber int = null
AS
BEGIN
	set nocount on

	if @SubteamNumber is null
		begin
			select nt.SubteamNumber, nt.ThresholdValueDays from NoTagThresholdSubteamOverride nt
		end
	else
		begin
			select nt.SubteamNumber, nt.ThresholdValueDays from NoTagThresholdSubteamOverride nt where nt.SubteamNumber = @SubteamNumber
		end

    
END
GO