SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CheckForDuplicateProdHierarchyLevel4]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CheckForDuplicateProdHierarchyLevel4]
GO


CREATE PROCEDURE dbo.CheckForDuplicateProdHierarchyLevel4 
    @ProdHierarchyLevel4_ID int, 
    @Description varchar(25),
    @ProdHierarchyLevel3_ID int 
AS 

SELECT COUNT(*) AS Level4Count 
FROM ProdHierarchyLevel4 
WHERE Description = @Description
AND ProdHierarchyLevel3_ID = @ProdHierarchyLevel3_ID
AND ProdHierarchyLevel4_ID <> @ProdHierarchyLevel4_ID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


