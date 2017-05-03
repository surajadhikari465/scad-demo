SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[DeleteTitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[DeleteTitle]
GO

CREATE PROCEDURE dbo.DeleteTitle
	@TitleID int
AS 
	DELETE FROM RoleConflictReason WHERE Title_Id = @TitleID
	DELETE FROM TitleDefaultPermission WHERE TitleId = @TitleID
	DELETE FROM Title WHERE Title_ID = @TitleID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO