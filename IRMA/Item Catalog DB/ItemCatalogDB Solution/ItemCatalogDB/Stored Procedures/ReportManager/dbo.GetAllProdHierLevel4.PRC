IF EXISTS (select * from dbo.sysobjects where id = object_id(N'[dbo].GetAllProdHierLevel4') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	BEGIN
		DROP  Procedure [dbo].GetAllProdHierLevel4
	END
GO

CREATE Procedure dbo.GetAllProdHierLevel4
AS
    SELECT   Description, ProdHierarchyLevel4_ID
    FROM     ProdHierarchyLevel4
    ORDER BY Description

GO

