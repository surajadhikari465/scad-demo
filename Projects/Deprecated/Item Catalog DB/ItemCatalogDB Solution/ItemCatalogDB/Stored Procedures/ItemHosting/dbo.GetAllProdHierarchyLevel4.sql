SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'P' AND name = 'GetAllProdHierarchyLevel4')
	BEGIN
		DROP  Procedure  dbo.GetAllProdHierarchyLevel4
	END

GO

CREATE PROCEDURE dbo.GetAllProdHierarchyLevel4
AS

SELECT ProdHierarchyLevel4_ID
      ,ProdHierarchyLevel3_ID
      ,Description
  FROM ProdHierarchyLevel4 (NOLOCK)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

