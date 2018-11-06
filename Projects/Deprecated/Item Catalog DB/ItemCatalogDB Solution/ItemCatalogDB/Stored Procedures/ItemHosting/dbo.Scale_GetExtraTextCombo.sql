SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_GetExtraTextCombo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_GetExtraTextCombo]
GO

CREATE PROCEDURE [dbo].[Scale_GetExtraTextCombo] 

AS

BEGIN

	SELECT 
		Scale_ExtraText_ID, 
		[Description]
	FROM dbo.Scale_ExtraText

END

GO

