SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetAllocationItems]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetAllocationItems]
GO

CREATE PROCEDURE [dbo].[GetAllocationItems]
	@ItemKey int
AS 
	SELECT
		tmp.OrderItem_ID,
		tmp.CompanyName,
		tmp.OrderHeader_ID,
		st.SubTeam_Name,
		tmp.QuantityOrdered,
		tmp.QuantityAllocated,
		tmp.Package_Desc1,
		ii.Identifier,
		i.Item_Description
	FROM 
		tmpOrdersAllocateOrderItems		tmp
	JOIN 
		ItemIdentifier					ii	ON ii.Item_Key = tmp.Item_Key
	JOIN 
		Item							i	ON i.Item_Key = tmp.Item_Key
	LEFT JOIN
		Subteam							st	ON st.SubTeam_No = tmp.Transfer_To_Subteam
	WHERE 
		tmp.Item_Key = @ItemKey
	AND 
		ii.Default_Identifier = 1
	ORDER BY CompanyName
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 