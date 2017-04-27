IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetItemTypes]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[GetItemTypes]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE    PROCEDURE dbo.GetItemTypes AS
BEGIN
	SELECT ItemType_ID, ItemType_Name 
	FROM ItemType
	ORDER BY ItemType_Name
END
GO 