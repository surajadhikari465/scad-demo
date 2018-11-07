CREATE PROCEDURE dbo.GetSubTeamMargin
@SubTeam_No int 
AS 

SELECT Target_Margin
FROM SubTeam
WHERE SubTeam_No = @SubTeam_No
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamMargin] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamMargin] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamMargin] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamMargin] TO [IRMAReportsRole]
    AS [dbo];

