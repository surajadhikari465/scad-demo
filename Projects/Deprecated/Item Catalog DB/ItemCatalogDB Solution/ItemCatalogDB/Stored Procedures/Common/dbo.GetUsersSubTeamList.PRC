SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUsersSubTeamList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUsersSubTeamList]
GO

CREATE PROCEDURE dbo.GetUsersSubTeamList
    @User_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    IF EXISTS (SELECT * FROM Users WHERE User_ID = @User_ID AND SuperUser = 1)
        EXEC GetAllSubTeams
    ELSE
        SELECT SubTeam.SubTeam_No, SubTeam_Name
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


