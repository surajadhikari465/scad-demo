SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetProdHierarchyLevel3sByCategory')
	BEGIN
		DROP  Procedure  dbo.GetProdHierarchyLevel3sByCategory
	END

GO

CREATE PROCEDURE dbo.GetProdHierarchyLevel3sByCategory
	(@Category_ID as int)
AS

SELECT ProdHierarchyLevel3_ID
      ,Category_ID
      ,Description
  FROM ProdHierarchyLevel3 (NOLOCK)
  WHERE Category_ID = @Category_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO