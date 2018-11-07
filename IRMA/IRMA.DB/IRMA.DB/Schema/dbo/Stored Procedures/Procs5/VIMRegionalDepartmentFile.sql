
CREATE PROCEDURE dbo.VIMRegionalDepartmentFile  
AS 
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT 	--(select primaryregioncode from instancedata) AS REGION,
	(select top 1 runmode from conversion_runmode) AS REGION,
	--SUBSTRING ('00000', 1, 5 - LEN(LEFT(SubTeam_No, 3))) + CONVERT(varchar(5), (LEFT(SubTeam_No, 3))) AS POS_DEPT,
	SubTeam.Dept_No AS POS_DEPT,
        	SubTeam_Name AS DEPT_NAME
FROM SubTeam (NOLOCK)



END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRegionalDepartmentFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRegionalDepartmentFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRegionalDepartmentFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRegionalDepartmentFile] TO [IRMAReportsRole]
    AS [dbo];

