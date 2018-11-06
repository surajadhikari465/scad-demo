SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetTitleConflicts]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetTitleConflicts]
GO

CREATE PROCEDURE dbo.GetTitleConflicts
	@TitleID int
AS 
	SELECT User_ID, Username, Fullname FROM Users WHERE Title = @TitleID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 