SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertProdHierarchyLevel3]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertProdHierarchyLevel3]
GO


CREATE PROCEDURE dbo.InsertProdHierarchyLevel3
@Level3_Name varchar(50),
@Category_ID int
AS 

INSERT INTO ProdHierarchyLevel3 (Description, Category_ID)
VALUES (@Level3_Name, @Category_ID)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


