
-- =============================================
-- Author:		Kyle Milner
-- Create date: 2015-10-09
-- Description:	Returns the subteam names & numbers of the
--				aligned subteams in a region.
-- =============================================

CREATE PROCEDURE [dbo].[GetAlignedSubteams]
	
AS
BEGIN
	set nocount on

    select
		SubTeam_Name as SubteamName, SubTeam_No as SubteamNumber
	from
		SubTeam
	where
		AlignedSubTeam = 1
END
GO
