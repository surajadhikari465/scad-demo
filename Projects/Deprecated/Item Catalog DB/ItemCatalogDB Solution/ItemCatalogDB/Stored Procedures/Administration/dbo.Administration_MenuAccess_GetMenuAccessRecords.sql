IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_MenuAccess_GetMenuAccessRecords]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[Administration_MenuAccess_GetMenuAccessRecords]
GO

/**********************************************************************************
Author.......: Maria Younes
Date Modified: 11/06/2008
Description..: Get menu access records
***********************************************************************************/

CREATE PROCEDURE [dbo].[Administration_MenuAccess_GetMenuAccessRecords]
AS

BEGIN
	
	SET NOCOUNT ON
	
	SELECT     
		MenuAccessID, MenuName, Visible
	FROM 
		MenuAccess
	ORDER BY
		MenuName
	
	SET NOCOUNT OFF
	    
END

GO