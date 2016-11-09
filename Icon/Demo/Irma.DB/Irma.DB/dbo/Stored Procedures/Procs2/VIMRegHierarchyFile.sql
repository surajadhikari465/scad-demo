﻿CREATE PROCEDURE dbo.VIMRegHierarchyFile
AS 
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT (select primaryregioncode from instancedata) AS REGION,  
	HIERARCHY_REF AS REG_HIER_REF,
	HIER_FULL_NAME AS HIER_FULL_NAME,
	HIER_LEVEL AS HIER_LEVEL,
	HIER_PARENT AS HIER_PARERNT,
	HIER_LVL_ID AS HIER_LVL_ID
	FROM NATHIER_CLASS

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMRegHierarchyFile] TO [IRMASchedJobsRole]
    AS [dbo];

