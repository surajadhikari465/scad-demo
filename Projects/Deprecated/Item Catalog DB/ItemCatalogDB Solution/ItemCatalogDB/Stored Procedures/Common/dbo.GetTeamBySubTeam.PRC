SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTeamBySubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTeamBySubTeam]
GO


CREATE PROCEDURE dbo.GetTeamBySubTeam
@SubTeam_No int,
@SubTeam_Name varchar(100) output,
@Team_No int output,
@Team_Name varchar(100) output
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @Team_No = SubTeam.Team_No, @Team_Name = Team.Team_Name, @SubTeam_Name = SubTeam.SubTeam_Name
    FROM Subteam
        inner join
            team
            on SubTeam.Team_No = Team.Team_No
    where SubTeam.SubTeam_no = @SubTeam_No

    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


