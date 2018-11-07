IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetNonAlignedSubteamNames]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetNonAlignedSubteamNames]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

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
