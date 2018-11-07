SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER PROCEDURE dbo.VIMRegionalDepartmentFile  
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
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


