CREATE PROCEDURE dbo.VIMItemStatusFile 
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
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMItemStatusFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMItemStatusFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMItemStatusFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMItemStatusFile] TO [IRMAReportsRole]
    AS [dbo];

