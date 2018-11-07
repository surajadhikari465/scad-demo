
CREATE PROCEDURE [dbo].[VIM365ItemStatusFile]
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
    ON OBJECT::[dbo].[VIM365ItemStatusFile] TO [IRMASchedJobsRole]
    AS [dbo];

