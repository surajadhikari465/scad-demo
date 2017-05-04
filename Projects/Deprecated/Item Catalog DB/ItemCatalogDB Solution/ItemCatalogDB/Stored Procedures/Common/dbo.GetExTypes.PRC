SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExTypes]
GO


CREATE PROCEDURE dbo.GetExTypes
@AppID int

AS
BEGIN
    SET NOCOUNT ON
    
    SELECT RuleDef.ruleID, RuleDef.RuleName
    FROM RuleDef
    where RuleDef.AppID = @AppID and RuleID >= 0



    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


