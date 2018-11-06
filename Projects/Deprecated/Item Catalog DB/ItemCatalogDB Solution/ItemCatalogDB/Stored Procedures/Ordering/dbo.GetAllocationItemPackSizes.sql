 SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetAllocationItemPackSizes]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetAllocationItemPackSizes]
GO

CREATE PROCEDURE dbo.GetAllocationItemPackSizes
	@ItemKey int
AS 
		SELECT DISTINCT PackSize 
		FROM tmpOrdersAllocateItems 
		WHERE Item_Key = @ItemKey AND PackSize IS NOT NULL 
		ORDER BY PackSize

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 