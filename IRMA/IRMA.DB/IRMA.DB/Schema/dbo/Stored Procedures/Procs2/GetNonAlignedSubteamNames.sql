
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-02-08
-- Description:	Returns the subteam numbers of the
--				non-aligned subteams in a region.
-- =============================================

CREATE PROCEDURE [dbo].[GetNonAlignedSubteamNames]
	
AS
BEGIN
	
	set nocount on;

    select
		SubTeam_Name
	from
		SubTeam
	where
		AlignedSubTeam = 0

END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetNonAlignedSubteamNames] TO [IRMAClientRole]
    AS [dbo];

