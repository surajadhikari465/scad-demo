SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetRuleDef]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetRuleDef]
GO


CREATE PROCEDURE dbo.GetRuleDef
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


    IF EXISTS (select * from dbo.sysobjects 
               where id = object_id(N'[' + @RuleTableName + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
       begin
            select @SQL = ''
            select @SQL = @SQL + 'Select RuleDef.RuleName, RuleDef.AppID, RuleDef.RuleID, RuleDef.RuleDescTemplate, ''' + @RuleTableName + ''' as RuleTableName ' --' + @RuleTableName + '.* ' + ',
            select @SQL = @SQL + 'FROM RuleDef '--, ' + @RuleTableName
            select @SQL = @SQL + ' WHERE 1 = 1 '            
            select @SQL = @SQL + '   AND RuleDef.RuleID = ' + cast(@RuleID as varchar(10))
            select @SQL = @SQL + '   AND RuleDef.AppID = ' + cast(@AppID as varchar(10))

            EXEC(@SQL)
       end

    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

