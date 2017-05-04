/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 08/04/2006 16:51:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GetLabelTypes]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[GetLabelTypes]
GO

/****** Object:  StoredProcedure [dbo].[GetLabelTypes]    Script Date: 8/04/2006 16:51:50 ******/
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE dbo.[GetLabelTypes] AS

BEGIN
    SET NOCOUNT ON

	SELECT [LabelType_ID], [LabelTypeDesc]
	FROM [LabelType]
	ORDER BY [LabelTypeDesc]
    
    SET NOCOUNT OFF
END

GO