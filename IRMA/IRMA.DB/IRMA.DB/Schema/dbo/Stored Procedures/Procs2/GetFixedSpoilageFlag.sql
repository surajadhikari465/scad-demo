CREATE PROCEDURE [dbo].[GetFixedSpoilageFlag] 
	@SubTeam_No	int
AS 
BEGIN
    
    SELECT ISNULL(FixedSpoilage, 0) AS FixedSpoilage FROM Subteam WHERE Subteam_No = @SubTeam_No

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFixedSpoilageFlag] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFixedSpoilageFlag] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFixedSpoilageFlag] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFixedSpoilageFlag] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetFixedSpoilageFlag] TO [IRMAReportsRole]
    AS [dbo];

