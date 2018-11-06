SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetAllProdHierarchyLevel3')
	BEGIN
		DROP  Procedure  dbo.GetAllProdHierarchyLevel3
	END

GO

CREATE PROCEDURE dbo.GetAllProdHierarchyLevel3
AS

SELECT ProdHierarchyLevel3_ID
      ,Category_ID
      ,Description
  FROM ProdHierarchyLevel3 (NOLOCK)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO