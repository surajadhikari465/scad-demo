SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateProdHierarchyLevel3]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateProdHierarchyLevel3]
GO


CREATE PROCEDURE dbo.CheckForDuplicateProdHierarchyLevel3 
    @ProdHierarchyLevel3_ID int, 
    @Description varchar(25),
    @Category_ID int 
AS 

SELECT COUNT(*) AS Level3Count 
FROM ProdHierarchyLevel3 
WHERE Description = @Description
AND Category_ID = @Category_ID
AND ProdHierarchyLevel3_ID <> @ProdHierarchyLevel3_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


