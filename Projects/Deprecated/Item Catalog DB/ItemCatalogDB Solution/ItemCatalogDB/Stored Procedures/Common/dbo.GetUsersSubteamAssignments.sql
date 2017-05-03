 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUsersSubTeamAssignments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUsersSubTeamAssignments]
GO

CREATE PROCEDURE [dbo].[GetUsersSubTeamAssignments]
    @User_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT SubTeam.SubTeam_No, SubTeam_Name, ISNULL(Regional_Coordinator, 0) AS Regional_Coordinator
    FROM SubTeam
    INNER JOIN
        UsersSubTeam
        ON UsersSubTeam.SubTeam_No = SubTeam.SubTeam_No
    WHERE UsersSubTeam.User_ID = @User_ID
        
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
