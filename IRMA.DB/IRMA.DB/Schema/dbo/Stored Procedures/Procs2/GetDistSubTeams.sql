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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistSubTeams] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistSubTeams] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistSubTeams] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistSubTeams] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetDistSubTeams] TO [IRMAExcelRole]
    AS [dbo];

