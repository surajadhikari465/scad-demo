SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeamName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeamName]
GO


CREATE PROCEDURE dbo.GetSubTeamName
@SubTeam_No int 
AS 

SELECT SubTeam_Name 
FROM SubTeam 
WHERE SubTeam_No = @SubTeam_No
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

