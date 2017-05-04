IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND OBJECT_ID = OBJECT_ID(N'dbo.VIM365AuthorizationStatusFile'))
	EXEC('CREATE PROCEDURE [dbo].[VIM365AuthorizationStatusFile] AS BEGIN SET NOCOUNT ON; END')
GO

ALTER PROCEDURE [dbo].[VIM365AuthorizationStatusFile]
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
