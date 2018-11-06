SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Scale_GetExtraText]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Scale_GetExtraText]
GO

CREATE PROCEDURE [dbo].[Scale_GetExtraText] 
	@ScaleExtraTextID as int
AS

BEGIN

	SELECT 
		Scale_ExtraText_ID, 
		[Description],
		ExtraText,
		Scale_LabelType_ID
	FROM dbo.Scale_ExtraText
		
	WHERE Scale_ExtraText_ID = @ScaleExtraTextID

END

GO

