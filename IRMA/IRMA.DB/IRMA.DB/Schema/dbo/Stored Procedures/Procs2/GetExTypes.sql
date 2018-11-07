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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExTypes] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExTypes] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExTypes] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExTypes] TO [IRMAReportsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetExTypes] TO [IRMAAVCIRole]
    AS [dbo];

