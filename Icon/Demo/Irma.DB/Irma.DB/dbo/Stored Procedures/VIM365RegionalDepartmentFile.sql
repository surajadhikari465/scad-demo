
CREATE PROCEDURE [dbo].[VIM365RegionalDepartmentFile]
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @365RegionCode varchar(2) = 'TS';

SELECT
	@365RegionCode AS REGION,
	SubTeam.Dept_No AS POS_DEPT,
	SubTeam_Name AS DEPT_NAME
FROM SubTeam (NOLOCK)



END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIM365RegionalDepartmentFile] TO [IRMASchedJobsRole]
    AS [dbo];

