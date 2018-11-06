CREATE PROCEDURE dbo.VIMAuthorizationStatusFile 
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
    ON OBJECT::[dbo].[VIMAuthorizationStatusFile] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMAuthorizationStatusFile] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMAuthorizationStatusFile] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[VIMAuthorizationStatusFile] TO [IRMAReportsRole]
    AS [dbo];

