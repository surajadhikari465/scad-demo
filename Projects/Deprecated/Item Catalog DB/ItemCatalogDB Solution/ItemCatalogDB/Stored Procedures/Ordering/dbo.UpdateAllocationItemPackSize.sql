SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[UpdateAllocationItemPackSize]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[UpdateAllocationItemPackSize]
GO

CREATE PROCEDURE dbo.UpdateAllocationItemPackSize
AS 
	UPDATE tmpOrdersAllocateOrderItems SET Package_Desc1 = OOH.PackSize
	FROM 
		tmpOrdersAllocateOrderItems tmp
		INNER JOIN dbo.fn_OrderAllocItemsOnePackSizeOH() AS OOH ON OOH.Item_Key = tmp.Item_Key
	WHERE 
		(QuantityAllocated IS NULL) AND PackSize <> Package_Desc1 
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 