CREATE Procedure dbo.GetSustainabilityRankings

AS
SELECT ID, RankingDescription FROM SustainabilityRanking (nolock) ORDER BY ID
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMASLIMRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSustainabilityRankings] TO [IRMAPromoRole]
    AS [dbo];

