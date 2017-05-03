 IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetSustainabilityRankings')
	BEGIN
		DROP  Procedure  dbo.GetSustainabilityRankings
	END

GO

CREATE Procedure dbo.GetSustainabilityRankings

AS
SELECT ID, RankingDescription FROM SustainabilityRanking (nolock) ORDER BY ID

GO
