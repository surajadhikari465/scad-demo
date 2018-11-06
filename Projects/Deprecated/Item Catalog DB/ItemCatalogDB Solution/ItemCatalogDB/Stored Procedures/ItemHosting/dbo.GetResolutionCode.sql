IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetResolutionCode]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetResolutionCode]
GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[GetResolutionCode]
	@ResolutionCodeID AS int 	
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT *
	FROM [dbo].[ResolutionCodes]
	WHERE [ResolutionCodeID] = @ResolutionCodeID
END
GO