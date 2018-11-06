SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[AddTitle]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[AddTitle]
GO

CREATE PROCEDURE dbo.AddTitle
	@TitleDesc varchar(50)
AS 
	DECLARE @MaxId int
	SELECT @MaxId = MAX(Title_ID) + 1 FROM Title

	INSERT INTO dbo.Title (Title_ID, Title_Desc) VALUES (@MaxId, @TitleDesc)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 