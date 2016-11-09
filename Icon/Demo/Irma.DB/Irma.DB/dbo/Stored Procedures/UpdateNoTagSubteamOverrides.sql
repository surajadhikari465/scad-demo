
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-10
-- Description:	Updates the default history threshold
--				for the three no-tag rules.
-- =============================================

CREATE PROCEDURE [dbo].[UpdateNoTagSubteamOverrides]
	@SubteamOverrides dbo.NoTagSubteamOverrideType readonly
AS
BEGIN
	set nocount on

    update
		NoTagThresholdSubteamOverride
	set
		ThresholdValueDays = so.ThresholdValue
	from
		NoTagThresholdSubteamOverride nt
		join @SubteamOverrides so on nt.SubteamNumber = so.SubteamNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateNoTagSubteamOverrides] TO [IRSUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateNoTagSubteamOverrides] TO [IRMAClientRole]
    AS [dbo];

