SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateTitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UpdateTitle]
GO

CREATE PROCEDURE dbo.UpdateTitle
	@TitleID int,
	@TitleDesc varchar(50)
AS 
	UPDATE Title SET Title_Desc = @TitleDesc WHERE Title_ID = @TitleID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO