SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUsersSubteam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUsersSubteam]
GO


CREATE PROCEDURE dbo.GetUsersSubteam
	@User_ID int,
	@SubTeam_No int,
	@Item_Key int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT SubTeam.SubTeam_No, SubTeam_Name
    FROM UsersSubTeam
    INNER JOIN
    	SubTeam ON SubTeam.SubTeam_No = UsersSubTeam.SubTeam_No
    WHERE UsersSubTeam.User_ID = @User_ID
    AND ISNULL(@SubTeam_No, UsersSubTeam.SubTeam_No) = UsersSubTeam.SubTeam_No
    AND ISNULL((SELECT SubTeam_No FROM Item WHERE Item_Key = ISNULL(@Item_Key, -1)), UsersSubTeam.SubTeam_No) = UsersSubTeam.SubTeam_No
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


