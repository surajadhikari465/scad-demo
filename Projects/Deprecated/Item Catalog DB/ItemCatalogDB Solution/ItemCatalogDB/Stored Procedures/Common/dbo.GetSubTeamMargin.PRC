SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetSubTeamMargin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetSubTeamMargin]
GO


CREATE PROCEDURE dbo.GetSubTeamMargin
@SubTeam_No int 
AS 

SELECT Target_Margin
FROM SubTeam
WHERE SubTeam_No = @SubTeam_No
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

