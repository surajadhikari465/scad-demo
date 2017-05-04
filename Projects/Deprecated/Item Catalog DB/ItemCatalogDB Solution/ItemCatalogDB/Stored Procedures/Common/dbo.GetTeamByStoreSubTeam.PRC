SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTeamByStoreSubTeam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTeamByStoreSubTeam]
GO


CREATE PROCEDURE dbo.GetTeamByStoreSubTeam
@Store_No int,
@SubTeam_No int,
@SubTeam_Name varchar(100) output,
@Team_No int output,
@Team_Name varchar(100) output
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT @Team_No = StoreSubTeam.Team_No, @Team_Name = Team.Team_Name, @SubTeam_Name = SubTeam.SubTeam_Name
    FROM StoreSubTeam
        inner join
            team
            on StoreSubTeam.Team_No = Team.Team_No
        inner join
            Subteam
            on StoreSubteam.SubTeam_No = SubTeam.Subteam_No
    where StoreSubTeam.Store_No = @Store_No and StoreSubTeam.SubTeam_no = @SubTeam_No

    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


