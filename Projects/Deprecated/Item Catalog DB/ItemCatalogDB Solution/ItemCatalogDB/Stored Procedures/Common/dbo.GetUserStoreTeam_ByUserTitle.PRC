SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetUserStoreTeam_ByUserTitle]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetUserStoreTeam_ByUserTitle]
GO


CREATE PROCEDURE dbo.GetUserStoreTeam_ByUserTitle
	@User_ID int,
	@Title_ID int
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT USTT.Team_No, USTT.Title_ID
    FROM UserStoreTeamTitle USTT
    WHERE USTT.User_ID = @User_ID
    AND ISNULL(@Title_ID, USTT.Title_ID) = USTT.Title_ID    

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


