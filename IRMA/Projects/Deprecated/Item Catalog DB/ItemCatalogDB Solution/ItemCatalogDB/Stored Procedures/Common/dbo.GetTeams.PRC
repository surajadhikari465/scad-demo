SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTeams]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTeams]
GO


CREATE PROCEDURE dbo.GetTeams
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Team_No, Team_Name
    FROM Team (NoLock)
    ORDER BY Team_Name
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


