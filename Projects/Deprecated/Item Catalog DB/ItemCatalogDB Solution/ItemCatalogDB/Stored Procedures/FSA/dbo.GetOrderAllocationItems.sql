SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

IF EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[GetOrderAllocationItems]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE [dbo].[GetOrderAllocationItems]
GO

CREATE PROCEDURE dbo.GetOrderAllocationItems
	@StoreNo int,
	@SubTeamNo int,
	@PreOrder int,
	@GroupById int,
	@MultiPackOnly bit
AS 

	DECLARE @PackCount int

	IF @MultiPackOnly = 0
		SET @PackCount = 0
	ELSE
		SET @PackCount = 1

	IF @PreOrder = -1
		SELECT @PreOrder = NULL

	IF @GroupById = 1
		BEGIN
			SELECT 
				Item_Key, Identifier 
			FROM 
				tmpOrdersAllocateItems 
			WHERE
				Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = ISNULL(@PreOrder, Pre_Order)
			GROUP BY 
				Item_Key, Identifier, Category_Name, Item_Description
			HAVING 
				COUNT(PackSize) > @PackCount		
			ORDER BY Category_Name, Item_Description
		END
	ELSE IF @GroupById = 2
		BEGIN
			SELECT 
				Item_Key, Identifier 
			FROM 
				tmpOrdersAllocateItems 
			WHERE
				Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = ISNULL(@PreOrder, Pre_Order)
			GROUP BY 
				Item_Key, Identifier, Category_Name, Item_Description 
			HAVING 
				(SUM(ISNULL(BOH, 0)) - MAX(ISNULL(SOO, 0))) > 0 AND COUNT(PackSize) > @PackCount	
			ORDER BY Category_Name, Item_Description
		END
	ELSE IF @GroupById = 3
		BEGIN
			SELECT 
				Item_Key, Identifier 
			FROM 
				tmpOrdersAllocateItems 
			WHERE
				Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = ISNULL(@PreOrder, Pre_Order)
			GROUP BY 
				Item_Key, Identifier, Category_Name, Item_Description
			HAVING (SUM(ISNULL(BOH, 0)) - MAX(ISNULL(SOO, 0))) <= 0 AND COUNT(PackSize) > @PackCount	
			ORDER BY Category_Name, Item_Description
		END
	ELSE IF @GroupById = 4
		BEGIN
			SELECT 
				Item_Key, Identifier 
			FROM 
				tmpOrdersAllocateItems 
			WHERE
				Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = ISNULL(@PreOrder, Pre_Order)
			GROUP BY 
				Item_Key, Identifier, Category_Name, Item_Description
			HAVING (SUM(ISNULL(BOH, 0)) - MAX(ISNULL(SOO, 0))) < 0 AND COUNT(PackSize) > @PackCount	
			ORDER BY Category_Name, Item_Description
		END
	ELSE IF @GroupById = 5
		BEGIN
			SELECT 
				Item_Key, Identifier 
			FROM 
				tmpOrdersAllocateItems 
			WHERE
				Store_No = @StoreNo AND SubTeam_No = @SubTeamNo AND Pre_Order = ISNULL(@PreOrder, Pre_Order)
			GROUP BY 
				Item_Key, Identifier, Category_Name, Item_Description
			HAVING (SUM(ISNULL(BOH, 0)) - MAX(ISNULL(SOO, 0))) >= 0 AND COUNT(PackSize) > @PackCount	
			ORDER BY Category_Name, Item_Description
		END		
	
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO 