SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetTitlePermissions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetTitlePermissions]
GO

CREATE PROCEDURE dbo.GetTitlePermissions
	@TitleID int
AS 
	SELECT * FROM TitleDefaultPermission WHERE TitleId = @TitleID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 