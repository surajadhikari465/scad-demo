SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateTitleConflicts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UpdateTitleConflicts]
GO

CREATE PROCEDURE dbo.UpdateTitleConflicts
	@TitleId int,
	@UserId int
AS 
	UPDATE Users SET Title = @TitleId WHERE User_Id = @UserId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 