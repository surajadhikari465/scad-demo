SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserEmail]
GO



CREATE PROCEDURE dbo.GetUserEmail
@Store_No int, 
@Team_No int, 
@SubTeam_No int, 
@Title_ID int
AS

/*Declare @Store_No int, 
        @Team_No int, 
        @SubTeam_No int, 
        @Title_ID int

Select @Store_No = 106
Select @Team_No = 200
Select @SubTeam_No = 2200
Select @Title_ID = 10*/
BEGIN
    SET NOCOUNT ON

    if not(@SubTeam_No is null) 
        Select @Team_No = (Select top 1 Team_No 
                          from StoreSubTeam 
                          where StoreSubTeam.SubTeam_No = @SubTeam_No 
                                and StoreSubTeam.Store_No = isnull(@Store_No,StoreSubTeam.Store_No))
    
    select Users.Email, Users.User_ID, USTT.Store_No, USTT.Team_No, USTT.Title_ID
    from UserStoreTeamTitle USTT
        INNER JOIN
            Users (nolock)
            on USTT.User_ID = Users.User_ID
    where USTT.Store_No = isnull(@Store_No, USTT.Store_No)
          AND USTT.Team_No = isnull(@Team_No, USTT.Team_No)
          AND Title_ID = isnull(@Title_ID, USTT.Title_ID)

    SET NOCOUNT OFF
END


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


