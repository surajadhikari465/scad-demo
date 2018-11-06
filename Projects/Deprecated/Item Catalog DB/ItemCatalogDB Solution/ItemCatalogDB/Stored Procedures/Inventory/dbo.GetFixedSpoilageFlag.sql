if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetFixedSpoilageFlag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetFixedSpoilageFlag]

GO

CREATE PROCEDURE [dbo].[GetFixedSpoilageFlag] 
	@SubTeam_No	int
AS 
BEGIN
    
    SELECT ISNULL(FixedSpoilage, 0) AS FixedSpoilage FROM Subteam WHERE Subteam_No = @SubTeam_No

END

GO 