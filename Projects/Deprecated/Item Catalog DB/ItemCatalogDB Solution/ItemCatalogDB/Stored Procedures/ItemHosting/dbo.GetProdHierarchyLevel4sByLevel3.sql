SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetProdHierarchyLevel4sByLevel3')
	BEGIN
		DROP  Procedure  dbo.GetProdHierarchyLevel4sByLevel3
	END

GO

CREATE PROCEDURE dbo.GetProdHierarchyLevel4sByLevel3
	(@ProdHierarchyLevel3_ID as int)
AS

SELECT ProdHierarchyLevel4_ID
      ,ProdHierarchyLevel3_ID
      ,Description
  FROM ProdHierarchyLevel4 (NOLOCK)
  WHERE ProdHierarchyLevel3_ID = @ProdHierarchyLevel3_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO