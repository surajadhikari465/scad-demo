SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertProdHierarchyLevel4]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertProdHierarchyLevel4]
GO


CREATE PROCEDURE dbo.InsertProdHierarchyLevel4
@Level4_Name varchar(50),
@ProdHierarchyLevel3_ID int
AS 

INSERT INTO ProdHierarchyLevel4 (Description, ProdHierarchyLevel3_ID)
VALUES (@Level4_Name, @ProdHierarchyLevel3_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


