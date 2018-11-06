SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetDistSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetDistSubTeams]
GO

CREATE PROCEDURE dbo.GetDistSubTeams  
	@RetailSubTeam_No int = NULL
AS
SELECT DISTINCT
	  S.SubTeam_Name as DistSubTeam_Name
	, DS.DistSubTeam_No
	, DS.RetailSubTeam_No
    , S2.SubTeam_Name as RetailSubTeam_Name
FROM 
	DistSubTeam DS WITH (NOLOCK)
INNER JOIN SubTeam S WITH (NOLOCK)
	ON DS.DistSubTeam_No = S.SubTeam_No
	AND ISNULL(@RetailSubTeam_No, DS.RetailSubTeam_No) = DS.RetailSubTeam_No
INNER JOIN SubTeam S2 WITH (NOLOCK) 
    ON DS.RetailSubTeam_No = S2.SubTeam_No
GROUP BY 
	  DS.DistSubTeam_No
	, S.SubTeam_Name
	, DS.RetailSubTeam_No
    , S2.SubTeam_Name
ORDER BY 
	S.SubTeam_Name
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


