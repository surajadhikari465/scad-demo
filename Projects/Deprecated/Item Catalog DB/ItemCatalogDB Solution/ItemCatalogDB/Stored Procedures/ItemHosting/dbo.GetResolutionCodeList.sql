IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetResolutionCodeList]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetResolutionCodeList]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetResolutionCodeList]
@ActiveOnly bit
AS 
BEGIN
	SET NOCOUNT ON

	IF (@ActiveOnly = 1)
		BEGIN
			SELECT * FROM ResolutionCodes WHERE Active = 1
		END
	ELSE
		BEGIN
			SELECT * FROM ResolutionCodes
		END			
	
	SET NOCOUNT OFF 
END
GO
