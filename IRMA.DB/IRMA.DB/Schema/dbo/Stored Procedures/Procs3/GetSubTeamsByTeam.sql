CREATE PROCEDURE dbo.GetSubTeamsByTeam
@TeamList varchar(8000)
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT DISTINCT SubTeam_Name, SubTeam_No 
    FROM SubTeam (NOLOCK)
        INNER JOIN 
            dbo.fn_parse_list(@TeamList, '|') TeamList
            on TeamList.Key_Value = SubTeam.Team_No       
--    WHERE (Left(SubTeam_No, 1) <> 9 Or SubTeam_No = 9990) AND
--          SubTeam_No <> 4000 AND SubTeam_No <> 4010
    ORDER BY SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamsByTeam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamsByTeam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamsByTeam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetSubTeamsByTeam] TO [IRMAReportsRole]
    AS [dbo];

