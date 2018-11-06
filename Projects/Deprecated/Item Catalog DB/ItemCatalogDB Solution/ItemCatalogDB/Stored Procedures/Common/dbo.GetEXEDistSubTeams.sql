SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetEXEDistSubTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetEXEDistSubTeams]
GO


CREATE PROCEDURE dbo.GetEXEDistSubTeams
AS 
BEGIN
    SET NOCOUNT ON
    
    SELECT 
		  SubTeam_Name, SubTeam_No 
    FROM 
		SubTeam (NOLOCK)
	WHERE EXEDistributed = 1
    ORDER BY 
		SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


 