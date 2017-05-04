SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER PROCEDURE dbo.VIMItemStatusFile 
AS 
BEGIN
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	SELECT 'A' AS Status,
			'Active' as Description,
			cr.runmode AS REGION from instancedata, conversion_runmode cr

	UNION ALL
	
		SELECT 'I' AS Status,
			'Inactive' as Description,
			cr.runmode AS REGION from instancedata, conversion_runmode cr

	UNION ALL
	
		SELECT 'D' AS Status,
			'Deleted' as Description,
			cr.runmode AS REGION from instancedata, conversion_runmode cr
END 	
GO
