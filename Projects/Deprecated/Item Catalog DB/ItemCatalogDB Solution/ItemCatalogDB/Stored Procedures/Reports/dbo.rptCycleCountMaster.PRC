SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[rptCycleCountMaster]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[rptCycleCountMaster]
GO

CREATE PROCEDURE dbo.rptCycleCountMaster
	@MasterCountID as int = null
	,@CycleCountID as int = null
	,@Retail as bit
	,@Manufacturing as bit
AS
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SET NOCOUNT OFF

	-- Notes:
	--	11/04/05 -- Each item on the report will be limited to one line. [ItemName, default pack size, total unit count, case count, total cost].

	-- Get the current date.
	DECLARE @CurrDate smalldatetime, @EndScan datetime, @Store_No int
	SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))
    
	--  Get the EndScan Date and Store_No from the CycleCountMaster record so we can determine cost.
	SELECT @EndScan = EndScan, @Store_No = Store_No
	FROM CycleCountMaster 
	WHERE MasterCountID = @MasterCountID

	-- Get the Item Unit ID's so we can call CostConverion
	DECLARE @Case int, @Pound int

    SELECT @Case = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Case'
	SELECT @Pound = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Pound'

	-- Setup the TABLE @CD... this will store all the cost data for 1 item.
    DECLARE @CD TABLE (Item_Key int, AvgCost money, CurrPrice money, ISPrice money, VendorPackSize decimal(9,4))

	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- For each item  of each cycle count... get the Average Cost (and the Price for defaulting to Price * Cost Factor) at the end of the Fiscal Period.
	----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    INSERT INTO @CD (Item_Key, AvgCost, CurrPrice, ISPrice, VendorPackSize)
	SELECT DISTINCT
		CycleCountItems.Item_Key 
		, ISNULL(CycleCountItems.AvgCost,dbo.fn_AvgCostHistory(CycleCountItems.Item_Key, CycleCountMaster.Store_No, CycleCountMaster.SubTeam_No, CycleCountMaster.EndScan)) ---------------------AS AvgCost
		, ISNULL(dbo.fn_PriceHistoryRegPrice(CycleCountItems.Item_Key, CycleCountMaster.Store_No, CycleCountMaster.EndScan), 0)  ----------------------AS Price
       -- , CONVERT(money, ISNULL(I.EFF_PRICE,dbo.fn_PriceHistoryPrice(CycleCountItems.Item_Key, CycleCountMaster.Store_No, CycleCountMaster.EndScan)))
		, CONVERT(money, dbo.fn_PriceHistoryPrice(CycleCountItems.Item_Key, CycleCountMaster.Store_No, CycleCountMaster.EndScan))
        , dbo.fn_GetCurrentVendorPackage_Desc1(CycleCountItems.Item_Key, Store.Store_No)
    FROM 
		CycleCountMaster (NOLOCK)
	INNER JOIN
		CycleCountHeader (NOLOCK) ON CycleCountHeader.MasterCountID = CycleCountMaster.MasterCountID
	INNER JOIN
		CycleCountItems (NOLOCK) ON CycleCountItems.CycleCountID = CycleCountHeader.CycleCountID
    INNER JOIN
        Store (nolock) ON Store.Store_No = CycleCountMaster.Store_No
    INNER JOIN
		[Date] D (nolock) ON D.Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), CycleCountMaster.EndScan, 101))
    --LEFT JOIN
       -- InventoryServiceImportLoad I (nolock) 
        --ON CONVERT(int, I.PS_BU) = Store.BusinessUnit_ID 
       -- AND CONVERT(int, I.PS_PROD_SUBTEAM) = CycleCountMaster.SubTeam_No
        --AND CONVERT(int, I.SKU) = CycleCountItems.Item_Key 
        --AND I.Year = D.Year AND I.Period = D.Period
    --LEFT JOIN 
        --CycleCountHistory (NOLOCK) ON CycleCountHistory.CycleCountItemID = CycleCountItems.CycleCountItemID
    WHERE
		CycleCountMaster.MasterCountID = ISNULL(@MasterCountID, CycleCountMaster.MasterCountID)
      		---AND CycleCountHistory.PackSize IS NOT NULL 
	
	---------------------------------------------------------------------------------------------------------------------------------------------------------------
	-- Select the output and default the cost to Price * Cost Factor if there was no Average Cost or default
	---------------------------------------------------------------------------------------------------------------------------------------------------------------
	SELECT 
		-- Master Info.
		Store.Store_Name
		,MasterSubTeam.SubTeam_Name
		,Master.EndScan
		,(CASE WHEN Master.EndOfPeriod = 1 THEN 'End Of Period' ELSE 'Interim' END) AS EOP
		,(CASE WHEN Master.ClosedDate IS NULL THEN 'Open' ELSE 'Closed' END) AS MasterStatus

		-- CountInfo.
		,ISNULL(Loc.InvLoc_Name, MasterSubTeam.SubTeam_Name) AS CountName
		,(CASE WHEN Loc.InvLoc_Name IS NULL THEN 'SubTeam' ELSE 'Location' END)	AS CountType
		,Header.StartScan
		,(CASE WHEN Header.ClosedDate IS NULL THEN 'Open' ELSE 'Closed' END) AS CountStatus

		-- Item Info.	
		,ISNULL(Cat.Category_Name, 'No Category') as Category_Name
		,ItemIdentifier.Identifier
		,Item.Item_Description
		,ItemUnit.Unit_Name

		-- Item entry info.
		,(CASE WHEN ItemSubTeam.SubTeam_No = MasterSubTeam.SubTeam_No THEN 'Retail Items' ELSE 'Ingredient Items' END) AS RetailIngredient
		--, dbo.fn_GetCurrentVendorPackage_Desc1(Item.Item_Key, Store.Store_No) as VendorPackSize
		, CD.VendorPackSize
		,SUM(ISNULL(History.Count, 0)) AS Quantity
		,SUM(ISNULL(History.Weight, 0)) AS Weight
	
		-- Cost data.
		,CD.AvgCost
		,CD.AvgCost * CD.VendorPackSize * CASE WHEN CostedByWeight = 1 THEN Package_Desc2 ELSE 1 END AS AvgCaseCost
		,StoreSubTeam.CostFactor
		,CD.CurrPrice
		,CD.ISPrice
		,CD.ISPrice * ISNULL(StoreSubTeam.CostFactor, 0) As ISCost
		
		,SUM(CASE 
			WHEN ((ISNULL(History.Count,0) / CASE WHEN CD.VendorPackSize <> 0 THEN CD.VendorPackSize ELSE 1 END) + (ISNULL(History.Weight,0) / CASE WHEN CD.VendorPackSize * Package_Desc2 > 0 THEN (CD.VendorPackSize * Package_Desc2) ELSE 1 END)) = 0 THEN 
				Null
			ELSE 
				((ISNULL(History.Count,0) / CASE WHEN CD.VendorPackSize <> 0 THEN CD.VendorPackSize ELSE 1 END) + (ISNULL(History.Weight,0) / CASE WHEN CD.VendorPackSize * Package_Desc2 > 0 THEN (CD.VendorPackSize * Package_Desc2) ELSE 1 END))
			END) AS CaseCount

	    -- Total Cost.
		,SUM(ISNULL(CD.AvgCost, CD.CurrPrice * ISNULL(StoreSubTeam.CostFactor, 0))  * 
			CASE 
				WHEN History.Weight >  0 THEN
					ISNULL(History.Weight, 0)
				ELSE 
					ISNULL(History.Count,0)
				END) AS TotalCost
        ,SUM(CD.ISPrice * ISNULL(StoreSubTeam.CostFactor, 0) * 
			CASE 
				WHEN History.Weight >  0 THEN
					ISNULL(History.Weight, 0)
				ELSE 
					ISNULL(History.Count,0)
				END) AS TotalISCost
		,SUM(CD.ISPrice * 
			CASE 
				WHEN History.Weight >  0 THEN
					ISNULL(History.Weight, 0)
				ELSE 
					ISNULL(History.Count,0)
				END) as TotalRetail
	FROM 
		-- Master.
		CycleCountMaster Master (NOLOCK)
		INNER JOIN Store (NOLOCK) ON Master.Store_No = Store.Store_No
		LEFT JOIN SubTeam MasterSubTeam (NOLOCK) ON Master.SubTeam_No = MasterSubTeam.SubTeam_No

		-- Header.
		INNER JOIN CycleCountHeader Header (NOLOCK) ON Master.MasterCountID = Header.MasterCountID
		INNER JOIN CycleCountItems Items (NOLOCK) ON Header.CycleCountID = Items.CycleCountID
		LEFT JOIN InventoryLocation Loc (NOLOCK) ON Header.InvLocID = Loc.InvLoc_ID

		-- Items.
		INNER JOIN Item (NOLOCK) ON (Items.Item_Key = Item.Item_Key)
	 	INNER JOIN SubTeam ItemSubTeam (NOLOCK) ON Item.Subteam_No = ItemSubTeam.SubTeam_No
		INNER JOIN ItemIdentifier (NOLOCK) ON (ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1)
		INNER JOIN StoreSubTeam (NOLOCK) ON StoreSubTeam.Store_No = Master.Store_No AND StoreSubTeam.SubTeam_No = Item.SubTeam_No
		LEFT JOIN CycleCountHistory History(NOLOCK) ON (Items.CycleCountItemID = History.CycleCountItemID) 
		LEFT JOIN ItemUnit (NOLOCK) ON Item.Package_Unit_ID = ItemUnit.Unit_ID
		LEFT JOIN ItemCategory Cat (NOLOCK) ON Item.Category_ID = Cat.Category_ID

		-- Cost.
		LEFT JOIN @CD CD ON Item.Item_Key = CD.Item_Key  
	
	WHERE
		Master.MasterCountID = ISNULL(@MasterCountID, Header.MasterCountID)
		AND Header.CycleCountID = ISNULL(@CycleCountID, Header.CycleCountID)

		-- Retail Items and or Ingredient Items.
		AND(ItemSubTeam.SubTeam_No = CASE WHEN @Retail = 1 THEN MasterSubTeam.SubTeam_No ELSE 9999999 END
		OR ItemSubTeam.SubTeam_No <> CASE WHEN @Manufacturing = 1 THEN MasterSubTeam.SubTeam_No ELSE ItemSubTeam.SubTeam_No END)
		-- AND History.PackSize IS NOT NULL

	GROUP BY
		Store.Store_Name
		,MasterSubTeam.SubTeam_Name
		,Master.EndScan
		,Master.EndOfPeriod
		,Master.ClosedDate
		,Loc.InvLoc_Name
		,Header.StartScan
		,Header.ClosedDate
		,Cat.Category_Name
		,ItemIdentifier.Identifier
		,Item.Item_Description
		,ItemUnit.Unit_Name
		,Item.Item_Key
		,Store.Store_No
		,Item.Package_Desc1
		,Item.Package_Desc2
		,Item.Package_Unit_ID
		,ItemSubTeam.SubTeam_No
		,MasterSubTeam.SubTeam_No
		,CD.AvgCost
		,CD.VendorPackSize
		,CD.CurrPrice
		,CD.ISPrice
		,StoreSubTeam.CostFactor
        ,CostedByWeight
    UNION
    SELECT 		
        -- Master Info.
		Store.Store_Name
		,MasterSubTeam.SubTeam_Name
		,Master.EndScan
		,(CASE WHEN Master.EndOfPeriod = 1 THEN 'End Of Period' ELSE 'Interim' END) AS EOP
		,(CASE WHEN Master.ClosedDate IS NULL THEN 'Open' ELSE 'Closed' END) AS MasterStatus

		-- CountInfo.
		,MasterSubTeam.SubTeam_Name AS CountName
		,'Unknown'	AS CountType
		,NULL As StartScan
		,CASE WHEN Master.ClosedDate IS NULL THEN 'Open' ELSE 'Closed' END AS CountStatus

		-- Item Info.	
		,'No Category' as Category_Name
		,IL.UPC As Identifier
		,IL.DESCRIPTION As Item_Description
		,NULL As Unit_Name

		-- Item entry info.
		,'Retail Items' AS RetailIngredient
		,NULL AS VendorPackSize
		,SUM(CONVERT(decimal(18,4), ISNULL(IL.COUNT, 0))) AS Quantity
		,0 AS Weight
	
		-- Cost data.
		,NULL As AvgCost
		,NULL AS AvgCaseCost
		,StoreSubTeam.CostFactor
		,0 As CurrPrice
		,0 As ISPrice
		,SUM(CONVERT(money, CASE WHEN ISNULL(IL.EFF_PRICE, '') <> '' THEN IL.EFF_PRICE ELSE 0 END) * ISNULL(StoreSubTeam.CostFactor, 0)) As ISCost
		,0 AS CaseCount
		,0 AS TotalCost
        ,SUM(CONVERT(money, ISNULL(IL.EFF_PRICE_EXTENDED, 0)) * ISNULL(StoreSubTeam.CostFactor, 0)) AS TotalISCost
        , 0 as TotalRetail
    FROM CycleCountMaster Master (NOLOCK)
	INNER JOIN Store (NOLOCK) ON Master.Store_No = Store.Store_No
	INNER JOIN SubTeam MasterSubTeam (NOLOCK) ON Master.SubTeam_No = MasterSubTeam.SubTeam_No
	INNER JOIN StoreSubTeam (NOLOCK) ON StoreSubTeam.Store_No = Master.Store_No AND StoreSubTeam.SubTeam_No = Master.SubTeam_No
	INNER JOIN [Date] D (nolock) ON D.Date_Key = CONVERT(smalldatetime, CONVERT(varchar(255), Master.EndScan, 101))
    INNER JOIN InventoryServiceImportLoad IL
        ON CONVERT(int, IL.PS_BU) = Store.BusinessUnit_ID 
        AND CONVERT(int, IL.PS_PROD_SUBTEAM) = Master.SubTeam_No
        AND IL.Year = D.Year AND IL.Period = D.Period

    WHERE Master.MasterCountID = ISNULL(@MasterCountID, Master.MasterCountID)
        AND ISNULL(SKU, '0') = '0'
    GROUP BY
    	Store.Store_Name
		,MasterSubTeam.SubTeam_Name
		,Master.EndScan
        ,Master.ClosedDate
		,Master.EndOfPeriod
        ,IL.UPC
		,IL.DESCRIPTION
        ,StoreSubTeam.CostFactor
        ,IL.EFF_PRICE
        ,IL.EFF_PRICE_EXTENDED
        ,IL.COUNT
	ORDER BY 
		CountType DESC, Category_Name, Item_Description

SET NOCOUNT OFF
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

