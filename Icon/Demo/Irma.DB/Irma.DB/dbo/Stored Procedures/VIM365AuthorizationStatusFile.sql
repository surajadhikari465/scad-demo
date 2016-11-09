
CREATE PROCEDURE [dbo].[VIM365AuthorizationStatusFile]
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


	SELECT 'P' AS Status,
          'Primary' as Description
UNION 

	SELECT 'A' AS Status,
	      'Approved' as Description
UNION
   
	SELECT 'N' AS Status,
	      'Not Approved' as Description
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIM365AuthorizationStatusFile] TO [IRMASchedJobsRole]
    AS [dbo];

