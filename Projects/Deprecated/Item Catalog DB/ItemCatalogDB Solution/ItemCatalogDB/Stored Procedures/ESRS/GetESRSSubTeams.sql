if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetESRSSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetESRSSubTeams]
GO


CREATE PROCEDURE dbo.GetESRSSubTeams
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		  SubTeam_Name 
		, SubTeam_No 
		, Team_No
		, Dept_No
		, SubDept_No
    FROM 
		SubTeam (NOLOCK)
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
