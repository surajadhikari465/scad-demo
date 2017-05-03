SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'dbo.GetPLUMDeptMap') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure dbo.GetPLUMDeptMap
GO


CREATE PROCEDURE dbo.GetPLUMDeptMap

AS

BEGIN
    SET NOCOUNT ON

    SELECT SubTeam_No, ScaleDept
    FROM SubTeam (nolock)
    WHERE ScaleDept IS NOT NULL

    SET NOCOUNT OFF
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



