SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

ALTER PROCEDURE dbo.VIMAuthorizationStatusFile 
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
