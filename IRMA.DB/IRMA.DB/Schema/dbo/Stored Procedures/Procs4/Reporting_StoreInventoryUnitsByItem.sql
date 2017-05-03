create Procedure [dbo].[Reporting_StoreInventoryUnitsByItem]
	(
	@Store_No INT, @OldCountDate SMALLDATETIME, @NewCountDate SMALLDATETIME, @SubTeam INT
	)
AS
   -- **************************************************************************
   -- Procedure: Reporting_StoreInventoryUnitsByItem()
   --    Author: n/a
   --      Date: n/a
   --
   -- Description:
   --
   -- Modification History:
   -- Date        Init	TFS		Comment
   -- 12/13/2010  BBB	13334	removed deprecated and unused code
   -- **************************************************************************
BEGIN

DECLARE @ReceiveLocation_ID INT, @Zone INT, @CostOption TINYINT
SET @ReceiveLocation_ID = NULL
SET @Zone = NULL
SET @CostOption = NULL

DECLARE @StoreVendorID INT
SELECT @StoreVendorID = (SELECT Vendor_ID FROM Vendor (NOLOCK) WHERE Store_No = @Store_No)

/****Get the Beginning On Hand****/
DECLARE @BOH TABLE (SubTeam_No INT, Item_Key INT, Weight DECIMAL(18,4), Units DECIMAL(18,4))

	INSERT INTO @BOH
		SELECT	Master.SubTeam_No, Items.Item_Key,
				Weight = SUM(ISNULL(History.Weight,0)),
				Units = SUM(ISNULL(History.Count,0))
		FROM CycleCountMaster Master (NOLOCK)
		INNER JOIN CycleCountHeader Header (NOLOCK)
			ON Header.MasterCountID = Master.MasterCountID
		INNER JOIN CycleCountItems Items (NOLOCK)
			ON Items.CycleCountID = Header.CycleCountID
		INNER JOIN CycleCountHistory History (NOLOCK)
			ON History.CycleCountItemID = Items.CycleCountItemID
		INNER JOIN Item I (NOLOCK)
			ON I.Item_Key = Items.Item_Key
		WHERE Master.Store_No = @Store_No
		AND (Master.EndScan >= @OldCountDate AND Master.EndScan < DATEADD(Day, 1, @OldCountDate))
		AND (Master.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		GROUP BY Master.SubTeam_No, Items.Item_Key

/****Get the Ending Physical Count****/
DECLARE @PhysicalCount TABLE (SubTeam_No INT, Item_Key INT, Weight DECIMAL(18,4), Units DECIMAL(18,4))

	INSERT INTO @PhysicalCount
		SELECT	ISNULL(Master.SubTeam_No,0), ISNULL(Items.Item_Key,0),
				Weight = SUM(ISNULL(History.Weight,0)),
				Units = SUM(ISNULL(History.Count,0))
		FROM CycleCountMaster Master (NOLOCK)
		INNER JOIN CycleCountHeader Header (NOLOCK)
			ON Header.MasterCountID = Master.MasterCountID
		INNER JOIN CycleCountItems Items (NOLOCK)
			ON Items.CycleCountID = Header.CycleCountID
		INNER JOIN CycleCountHistory History (NOLOCK)
			ON History.CycleCountItemID = Items.CycleCountItemID
		INNER JOIN Item I (NOLOCK)
			ON I.Item_Key = Items.Item_Key
		WHERE Master.Store_No = @Store_No
		AND (Master.EndScan >= @NewCountDate AND Master.EndScan < DATEADD(Day, 1, @NewCountDate))
		AND (Master.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		GROUP BY Master.SubTeam_No, Items.Item_Key

/****Combine the BOH and the Ending Physical Count to capture all items in both counts****/
DECLARE @BothCountsItems TABLE (SubTeam_No INT, Item_Key INT, BOHWeight DECIMAL(18,4), CountWeight DECIMAL(18,4),
								BOHUnits DECIMAL(18,4), CountUnits DECIMAL(18,4))
INSERT INTO @BothCountsItems
	SELECT 
		 ISNULL(B.SubTeam_No, p.SubTeam_No)
		,ISNULL(B.Item_Key, p.Item_Key) 
		,ISNULL(b.Weight, 0) , ISNULL(p.Weight,0)
		,ISNULL(b.Units, 0) , ISNULL(p.Units,0)
	FROM @BOH B
	FULL OUTER JOIN @PhysicalCount P
		ON B.Item_Key = P.Item_Key

/****Get the inbound Purchases, minus credits****/
DECLARE @Purchases TABLE (SubTeam_No INT, Item_Key INT, Units DECIMAL(18,4))

	INSERT INTO @Purchases
		SELECT OH.Transfer_To_SubTeam AS SubTeam_No, OI.Item_Key,
			Units = SUM(CASE WHEN OH.Return_Order = 0
								THEN UnitsReceived
									ELSE -(UnitsReceived)
										END)
		FROM OrderHeader OH (NOLOCK)
		INNER JOIN OrderItem OI (NOLOCK)
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE OH.ReceiveLocation_ID = @StoreVendorID
		AND (OH.CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND OH.CloseDate < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (OH.Transfer_To_SubTeam = @SubTeam OR @SubTeam IS NULL)
		GROUP BY OH.Transfer_To_SubTeam, OI.Item_Key

/****Get the Sales****/
DECLARE @Sales TABLE (SubTeam_No INT, Item_Key INT, Units DECIMAL(18,4))

	-- This method of using ItemHistory rather than Sales_SumByItem was done to improve performance.  Using Sales_SumByItem would not run for a full Quarter, so we had to switch.  There is a key
	-- difference with using ItemHistory:  with Sales_SumByItem, cases sales are converted to units using the current Pack whereas with ItemHistory, this conversion is done when the ItemHistory record
	-- is created using the Pack at the time.  So if users are comparing this report to others that use Sales_SumByItem, the numbers cam be different.  Accounting approved the ItemHistory approach
	-- for this report because the differences are not material and being able to run this report for a Quarter is important to the business.
	INSERT INTO @Sales

	SELECT IH.SubTeam_No, IH.Item_Key, SUM(IH.Quantity + IH.Weight)
	FROM ItemHistory IH (nolock)
	WHERE Adjustment_ID = 3
	AND IH.DateStamp >= DATEADD(d, 1, CONVERT(VARCHAR(10),@OldCountDate,101)) AND IH.DateStamp < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101))
	AND IH.Store_No = @Store_No
	AND IH.SubTeam_No = ISNULL(@SubTeam, IH.SubTeam_No)
	GROUP BY IH.SubTeam_No, IH.Item_Key

/****Get the credits given****/
DECLARE @Credits TABLE (SubTeam_No INT, Item_Key INT, Units DECIMAL(18,4), Credit$ MONEY)

INSERT INTO @Credits

SELECT ISNULL(@SubTeam, Transfer_SubTeam), OrderItem.Item_key,
SUM(UnitsReceived) as 'Units',
       SUM(CASE WHEN @CostOption = 1 
                THEN ISNULL(dbo.fn_AvgCostHistory(OrderItem.Item_Key, Store.Store_No, OrderHeader.Transfer_SubTeam, OrderHeader.CloseDate), 0) * UnitsReceived 
                ELSE ReceivedItemCost 
             END) AS ReceivedItemCost
FROM OrderHeader (NOLOCK) 
    INNER JOIN 
        OrderItem (NOLOCK) 
        ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
    LEFT JOIN 
        ReturnOrderList (NOLOCK) 
        ON ReturnOrderList.ReturnOrderHeader_ID = OrderItem.OrderHeader_ID
    LEFT JOIN --[SH 4/20/10] Changed to a left join so that transfers out could be included
        CreditReasons (NOLOCK) 
        ON CreditReasons.CreditReason_ID = OrderItem.CreditReason_ID
    INNER JOIN 
        Vendor ReceiveLocation (NOLOCK) 
        ON ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID 
    INNER JOIN 
        Vendor (NOLOCK) 
        ON Vendor.Vendor_ID = OrderHeader.Vendor_id
    INNER JOIN 
        Store (NOLOCK) 
        ON Store.Store_No = ReceiveLocation.Store_No
    INNER JOIN 
        Zone (NOLOCK) 
        ON Zone.Zone_ID = Store.Zone_ID
    LEFT JOIN 
        (SELECT SUM(T1.ReceivedItemCostTotal) AS ReceivedItemCostTotal, 
                  T1.Zone_ID 
         FROM (SELECT Zone.Zone_ID, 
                    (CASE WHEN @CostOption = 1 
                             THEN ISNULL(dbo.fn_AvgCostHistory(OrderItem.Item_Key, Store.Store_No, OrderHeader.Transfer_SubTeam, OrderHeader.CloseDate), 0) * UnitsReceived
                             ELSE ReceivedItemCost 
                          END) AS ReceivedItemCostTotal
                 FROM OrderHeader (NOLOCK) 
                    INNER JOIN 
                        OrderItem (NOLOCK) 
                        ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
                    INNER JOIN 
                        Vendor ReceiveLocation (NOLOCK) 
                        ON ReceiveLocation.Vendor_ID = OrderHeader.ReceiveLocation_ID 
                    INNER JOIN 
                        Vendor (NOLOCK) 
                        ON Vendor.Vendor_ID = OrderHeader.Vendor_id
                    INNER JOIN 
                        Store (NOLOCK) 
                        ON Store.Store_No = ReceiveLocation.Store_No
                    INNER JOIN 
                        Zone (NOLOCK) 
                    ON Zone.Zone_ID = Store.Zone_ID
             WHERE CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND CloseDate < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101)) 
                       AND ISNULL(@StoreVendorID, OrderHeader.Vendor_ID) = OrderHeader.Vendor_ID
                       AND Transfer_SubTeam = ISNULL(@SubTeam, Transfer_SubTeam) 
                       AND CreditReason_ID IS NULL) T1
         GROUP BY T1.Zone_ID) T2 
         ON (T2.Zone_ID = Store.Zone_ID)
WHERE CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND CloseDate < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101)) AND
      ISNULL(@StoreVendorID, OrderHeader.Vendor_ID) = OrderHeader.Vendor_ID AND
      ISNULL(@ReceiveLocation_ID, OrderHeader.ReceiveLocation_ID) = OrderHeader.ReceiveLocation_ID AND
      ISNULL(@Zone, Zone.Zone_ID) = Zone.Zone_ID
      AND Transfer_SubTeam = ISNULL(@SubTeam, Transfer_SubTeam)
GROUP BY ISNULL(@SubTeam, Transfer_SubTeam) , OrderItem.Item_Key

/****Get the Waste****/
DECLARE @Waste TABLE (SubTeam_No INT, Item_Key INT, Units DECIMAL(18,4))

	INSERT INTO @Waste
		SELECT	IH.SubTeam_No, IH.Item_Key,
				Units = 
                      SUM((ISNULL(IH.Quantity, 0) + ISNULL(IH.Weight, 0)))
		FROM ItemHistory IH (NOLOCK)
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
		WHERE IH.Adjustment_ID = 1
		AND IH.Store_No = @Store_No
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
        GROUP BY IH.SubTeam_No, IH.Item_Key 

/****Get the Positive Adjustments****/
DECLARE @PosAdj TABLE (SubTeam_No INT, Item_Key INT, Units DECIMAL(18,4))

	INSERT INTO @PosAdj
		SELECT	IH.SubTeam_No, IH.Item_Key,
				Units = 
                      SUM((ISNULL(IH.Quantity, 0) + ISNULL(IH.Weight, 0)))
		FROM ItemHistory IH (NOLOCK)
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
		WHERE IH.Adjustment_ID = 8
		AND IH.Store_No = @Store_No
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		AND (IH.Weight + IH.Quantity > 0)
        GROUP BY IH.SubTeam_No, IH.Item_Key

/****Get the Negative Adjustments****/
DECLARE @NegAdj TABLE (SubTeam_No INT, Item_Key INT, Units DECIMAL(18,4))

	INSERT INTO @NegAdj
		SELECT	IH.SubTeam_No, IH.Item_Key,
				Units = SUM((ISNULL(IH.Quantity, 0) + ISNULL(IH.Weight, 0)))*-1
		FROM ItemHistory IH (NOLOCK)
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
			AND Item.SubTeam_No = IH.SubTeam_No
		WHERE IH.Adjustment_ID = 8
		AND IH.Store_No = @Store_No
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(d, 1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		AND (IH.Weight + IH.Quantity < 0)
        GROUP BY IH.SubTeam_No, IH.Item_Key


/****** Make sure all distinct SubTeam/Item combinations are accounted for in the @BothCountsItems table for the left joins.
	Items other than in the counts can be purchased, wasted, credited etc. ******/

DECLARE @SubTeamItem TABLE (SubTeam_No INT, Item_Key INT)
INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Credits

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Purchases 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Sales 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @PosAdj 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @NegAdj 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Waste 

INSERT INTO @BothCountsItems
SELECT DISTINCT SubTeam_No, Item_Key, 0, 0, 0, 0
FROM @SubTeamItem WHERE Item_Key NOT IN  (SELECT Item_Key FROM @BothCountsItems)

/****[SH 1/6/10] Moved the following 2 avg cost temp tables to here so that theywould include all items and not just items that were counted****/

/****Get Current AvgCost****/
    DECLARE @AvgCost TABLE (SubTeam_No int, Item_Key int, AvgCost smallmoney, Package_Desc1 decimal(18,4),
							Package_Desc2 decimal(18,4), CostedByWeight bit)   
    INSERT INTO @AvgCost
    SELECT BCI.SubTeam_No, I.Item_Key, /****[SH 1/6/10] Modified to use BCI subteam intead of item so that cost was obtained for the inventorying subteam and not the item subteam****/
			ISNULL(dbo.fn_AvgCostHistory(BCI.Item_Key, @Store_No, BCI.SubTeam_No, @NewCountDate), 0)
			,Package_Desc1, Package_Desc2, I.CostedByWeight
    FROM @BothCountsItems BCI 
	INNER JOIN Item I(nolock) ON I.Item_Key = BCI.Item_Key
    WHERE (BCI.SubTeam_No = @SubTeam OR @SubTeam IS NULL)

/****Get Old AvgCost****/
    DECLARE @OldAvgCost TABLE (SubTeam_No int, Item_Key int, AvgCost smallmoney, Package_Desc1 decimal(18,4),
							Package_Desc2 decimal(18,4), CostedByWeight bit)    
    INSERT INTO @OldAvgCost
    SELECT BCI.SubTeam_No, I.Item_Key,  /****[SH 1/6/10] Modified to use BCI subteam intead of item so that cost was obtained for the inventorying subteam and not the item subteam****/
			ISNULL(dbo.fn_AvgCostHistory(BCI.Item_Key, @Store_No, BCI.SubTeam_No, @OldCountDate), 0)
			,Package_Desc1, Package_Desc2, I.CostedByWeight
    FROM @BothCountsItems BCI 
	INNER JOIN Item I(nolock) ON I.Item_Key = BCI.Item_Key
    WHERE (BCI.SubTeam_No = @SubTeam OR @SubTeam IS NULL)

/****Bring it all together****/
SELECT	 SubTeam = SubTeam_Name, Item.Item_Key
		,Identifier
		,Description = Item_Description
		,'BOH Units' = ISNULL(SUM(CASE WHEN ISNULL(BOHWeight,0) > 0
											THEN ISNULL(BOHWeight,0)
											ELSE ISNULL(BOHUnits, 0) END), 0)
		,'BOH $' =  ISNULL(SUM(OAC.AvgCost * (CASE WHEN ISNULL(BOHWeight,0) > 0
											THEN ISNULL(BOHWeight,0)
											ELSE ISNULL(BOHUnits, 0) END)),0)
		,'Net Units Purchased' = ISNULL(SUM(Purchases.Units),0)
		,'Net Units Sold' = ISNULL(SUM(Sales.Units),0)
		,'Wasted Units' = ISNULL(SUM(Waste.Units),0)
		,'Pos Adj Units' = ISNULL(SUM(PosAdj.Units),0)
		,'Neg Adj Units' = ISNULL(SUM(NegAdj.Units),0)
		,'Calc EOH Units' = (ISNULL(SUM(CASE WHEN ISNULL(BOHWeight,0) > 0/****[SH 12/30/09] Modified to account for items counted only by weight****/
											THEN ISNULL(BOHWeight,0)
											ELSE ISNULL(BOHUnits, 0) END), 0) 
								+ ISNULL(SUM(Purchases.Units),0)
								- ISNULL(SUM(Sales.Units),0)
								- ISNULL(SUM(Waste.Units),0)
								+ ISNULL(SUM(PosAdj.Units),0)
								- ISNULL(SUM(NegAdj.Units),0)
								- ISNULL(SUM(Credited.Units), 0)) --[SH 4/20/10] - Include credits in the Calc EOH Units
		,'Counted EOH Units' = ISNULL(SUM(CASE WHEN ISNULL(CountWeight,0) > 0
											THEN ISNULL(CountWeight,0)
											ELSE ISNULL(CountUnits, 0) END),0)
		,'Counted EOH $' =  ISNULL(SUM(AC.AvgCost * (CASE WHEN ISNULL(CountWeight,0) > 0
											THEN ISNULL(CountWeight,0)
											ELSE ISNULL(CountUnits, 0) END)),0)
		,'Units Credited' = ISNULL(SUM(Credited.Units),0)
		,'Credit $' = ISNULL(SUM(Credited.Credit$),0)
		,'Unknown Shrink Units' = ((ISNULL(SUM(CASE WHEN ISNULL(BOHWeight,0) > 0/****[SH 12/30/09] Modified to account for items counted only by weight****/
											THEN ISNULL(BOHWeight,0)
											ELSE ISNULL(BOHUnits, 0) END), 0) 
								+ ISNULL(SUM(Purchases.Units),0)
								- ISNULL(SUM(Sales.Units),0)
								- ISNULL(SUM(Waste.Units),0)
								+ ISNULL(SUM(PosAdj.Units),0)
								- ISNULL(SUM(NegAdj.Units),0)
								- ISNULL(SUM(Credited.Units), 0)) --[SH 4/20/10] - Include credits in the Unknown Shrink
									- ISNULL(SUM(CASE WHEN ISNULL(CountWeight,0) > 0
											THEN ISNULL(CountWeight,0)
											ELSE ISNULL(CountUnits, 0) END),0))*-1
		,'Avg Unit Cost' = ISNULL(SUM(AC.AvgCost), 0)
		,'Shrink $' = (
						(((ISNULL(SUM(CASE WHEN ISNULL(BOHWeight,0) > 0/****[SH 12/30/09] Modified to account for items counted only by weight****/
											THEN ISNULL(BOHWeight,0)
											ELSE ISNULL(BOHUnits, 0) END), 0) 
							+ ISNULL(SUM(Purchases.Units),0)
							- ISNULL(SUM(Sales.Units),0)
							- ISNULL(SUM(Waste.Units),0)
							+ ISNULL(SUM(PosAdj.Units),0)
							- ISNULL(SUM(NegAdj.Units),0)
							- ISNULL(SUM(Credited.Units),0)) --[SH 4/20/10] - Include credits in the Unknown Shrink $
								 - ISNULL(SUM(CASE WHEN ISNULL(CountWeight,0) > 0
											THEN ISNULL(CountWeight,0)
											ELSE ISNULL(CountUnits, 0) END),0))*-1)
						* ISNULL(SUM(AC.AvgCost),0))
FROM @BothCountsItems BCI
	LEFT JOIN @Purchases Purchases
		ON Purchases.Item_Key = BCI.Item_Key
		AND Purchases.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @Sales Sales
		ON Sales.Item_Key = BCI.Item_Key
		AND Sales.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @Credits Credited
		ON Credited.Item_Key = BCI.Item_Key
		AND Credited.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @Waste Waste
		ON Waste.Item_Key = BCI.Item_Key
		AND Waste.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @AvgCost AC
		ON AC.Item_Key = BCI.Item_Key
		AND AC.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @OldAvgCost OAC
		ON OAC.Item_Key = BCI.Item_Key
		AND OAC.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @PosAdj PosAdj
		ON PosAdj.Item_Key = BCI.Item_Key
		AND PosAdj.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @NegAdj NegAdj
		ON NegAdj.Item_Key = BCI.Item_Key
		AND NegAdj.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN SubTeam ST (NOLOCK)
		ON ST.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN Item (NOLOCK)
		ON Item.Item_Key = BCI.Item_Key
	LEFT JOIN ItemIdentifier II (NOLOCK)
		ON II.Item_Key = BCI.Item_Key
		AND Default_Identifier = 1
GROUP BY SubTeam_Name, Item.Item_Key, Identifier, Item_Description
ORDER BY SubTeam_Name, Identifier
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Reporting_StoreInventoryUnitsByItem] TO [IRMAReportsRole]
    AS [dbo];

