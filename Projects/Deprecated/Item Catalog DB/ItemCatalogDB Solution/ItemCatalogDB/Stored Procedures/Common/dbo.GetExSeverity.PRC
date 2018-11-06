SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExSeverity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExSeverity]
GO
--exec GetExSeverity 4, 2

CREATE PROCEDURE dbo.GetExSeverity
@AppID int,
@RuleID int

AS
BEGIN
    SET NOCOUNT ON
    DECLARE @RuleTableName varchar(50)
    DECLARE @SQL varchar(5000)    

    SELECT @RuleTableName = (SELECT 'ExRule_' + RuleDef.RuleName
                                  FROM RuleDef
                                  WHERE AppID = @AppID
                                        AND RuleID = isnull(@RuleID, RuleID))
    if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[' + @RuleTableName + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
     begin
        select @SQL = ''
        select @SQL = @SQL + 'select distinct Severity'
        select @SQL = @SQL + ' from ' + @RuleTableName + ' '
        exec(@SQL)
     end
    
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO







