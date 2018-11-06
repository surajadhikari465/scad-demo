SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetPOUsersSubteam]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetPOUsersSubteam]
GO


CREATE PROCEDURE dbo.GetPOUsersSubteam

AS

BEGIN
    SET NOCOUNT ON

SELECT Users.User_ID, Users.UserName, EMail, SubTeam_No
FROM UsersSubTeam 
INNER JOIN Users ON Users.User_ID = UsersSubTeam.User_ID
WHERE Regional_Coordinator = 1

SET NOCOUNT OFF
    
    
    
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



