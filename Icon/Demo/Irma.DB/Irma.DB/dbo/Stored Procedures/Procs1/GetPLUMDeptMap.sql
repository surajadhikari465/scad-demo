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
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMDeptMap] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMDeptMap] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMDeptMap] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetPLUMDeptMap] TO [IRMAReportsRole]
    AS [dbo];

