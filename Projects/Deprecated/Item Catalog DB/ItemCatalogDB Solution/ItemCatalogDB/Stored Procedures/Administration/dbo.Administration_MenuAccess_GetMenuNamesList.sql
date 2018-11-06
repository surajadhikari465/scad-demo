IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_MenuAccess_GetMenuNamesList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_MenuAccess_GetMenuNamesList]
GO

/**********************************************************************************
Author.......: Maria Younes
Date Modified: 11/06/2008
Description..: Get menu names
***********************************************************************************/

CREATE PROCEDURE [dbo].[Administration_MenuAccess_GetMenuNamesList]
	
AS

BEGIN
	
	SET NOCOUNT ON
	
	SELECT     
		MenuName,
		MenuAccessID
	FROM MenuAccess
    ORDER BY
		MenuName
		
	SET NOCOUNT OFF

END

GO





