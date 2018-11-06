CREATE Procedure dbo.rptInventoryCasesBySubteam

	(
		@Warehouse_ID INT, @OldCountDate SMALLDATETIME, @NewCountDate SMALLDATETIME, @SubTeam INT
	)


AS



DECLARE @Case INT, @Pound INT, @Unit INT               
	SELECT @Case = Unit_ID FROM ItemUnit WHERE EDISysCode = 'CA'
    SELECT @Unit = Unit_ID FROM ItemUnit WHERE EDISysCode = 'UN'
    SELECT @Pound = Unit_ID FROM ItemUnit WHERE EDISysCode = 'LB'
	


DECLARE @ReceiveLocation_ID int, @Zone int,@CostOption tinyint, @Store_No INT
SET @ReceiveLocation_ID = NULL
SET @Zone = NULL
SET @CostOption = NULL
SELECT @Store_No = (SELECT Store_No FROM Vendor(nolock) WHERE Vendor_ID = @Warehouse_ID)
print @Store_No

--Get the Beginning On Hand
DECLARE @BOH TABLE (SubTeam_No INT, Item_Key INT, Weight DECIMAL(18,4), Units DECIMAL(18,4), Cases DECIMAL(18,4))
INSERT INTO @BOH
		SELECT	Master.SubTeam_No, Items.Item_Key,
				Weight = SUM(ISNULL(History.Weight,0)),
				Units = SUM(ISNULL(History.Count,0)),
				Cases = SUM(NULLIF(dbo.fn_CostConversion(
		                                  ISNULL(History.Count,0) + ISNULL(History.Weight,0)
		                                  ,@Case  --to Case
		                                  ,CASE WHEN I.CostedByWeight = 1 THEN @Pound ELSE @Unit END --FROM Unit Or Pound
                                          ,History.PackSize -- PackSize already accounts for Package_Desc1 * Package_Desc2 if the item is costed by weight
                                          ,1
		                                  ,I.Package_Unit_ID),0))
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

--select * from @BOH



--Get the Ending Physical Count
DECLARE @PhysicalCount TABLE (SubTeam_No INT, Item_Key INT, Weight DECIMAL(18,4), Units DECIMAL(18,4), Cases DECIMAL(18,4))

	INSERT INTO @PhysicalCount
		SELECT	ISNULL(Master.SubTeam_No,0), ISNULL(Items.Item_Key,0),
				Weight = SUM(ISNULL(History.Weight,0)),
				Units = SUM(ISNULL(History.Count,0)),
				Cases = SUM(NULLIF(dbo.fn_CostConversion(
		                                  ISNULL(History.Count,0) + ISNULL(History.Weight,0)
		                                  ,@Case  --to Case
		                                  ,CASE WHEN I.CostedByWeight = 1 THEN @Pound ELSE @Unit END --FROM Unit Or Pound
                                          ,History.PackSize -- PackSize already accounts for Package_Desc1 * Package_Desc2 if the item is costed by weight
                                          ,1
		                                  ,I.Package_Unit_ID),0))
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

--select * from @PhysicalCount

--Combine the BOH and the Ending Physical Count to capture all items in both counts
DECLARE @BothCountsItems TABLE (SubTeam_No INT, Item_Key INT, BOHCases DECIMAL(18,4), CountCases DECIMAL(18,4),
								BOHWeight DECIMAL(18,4), CountWeight DECIMAL(18,4), BOHUnits DECIMAL(18,4), CountUnits DECIMAL(18,4))
INSERT INTO @BothCountsItems
	SELECT 
		 ISNULL(B.SubTeam_No, p.SubTeam_No)
		,ISNULL(B.Item_Key, p.Item_Key) 
		,ISNULL(b.Cases, 0) , ISNULL(p.Cases,0) 
		,ISNULL(b.Weight, 0) , ISNULL(p.Weight,0)
		,ISNULL(b.Units, 0) , ISNULL(p.Units,0)
	FROM @BOH B
	FULL OUTER JOIN @PhysicalCount P
		ON B.Item_Key = P.Item_Key


--select * from @BothCountsItemsg 

 --Get DC AvgCost
  DECLARE @AvgCost TABLE (SubTeam_No int, Item_Key int, AvgCost smallmoney, Package_Desc1 decimal(18,4),
							Package_Desc2 decimal(18,4), CostedByWeight bit)
    
   INSERT INTO @AvgCost
    SELECT  I.SubTeam_No, I.Item_Key, 
			ISNULL(dbo.fn_AvgCostHistory(BCI.Item_Key, @Store_No, BCI.SubTeam_No, @NewCountDate), 0)
			,(select distinct vca.Package_Desc1 from dbo.fn_VendorCostAll(@NewCountDate) vca
		 where VCA.Vendor_id=@Warehouse_ID and VCA.Item_key=BCI.Item_Key )Package_Desc1
		    ,Package_Desc2, I.CostedByWeight
    FROM @BothCountsItems BCI 
	JOIN Item I(nolock) ON I.Item_Key = BCI.Item_Key
    WHERE (BCI.SubTeam_No = @SubTeam OR @SubTeam IS NULL)

--select * from @AvgCost

-- Get Old AvgCost
   DECLARE @OldAvgCost TABLE (SubTeam_No int, Item_Key int, AvgCost smallmoney, Package_Desc1 decimal(18,4),
							Package_Desc2 decimal(18,4), CostedByWeight bit)
    
    INSERT INTO @OldAvgCost
   SELECT  I.SubTeam_No, I.Item_Key, 
			ISNULL(dbo.fn_AvgCostHistory(BCI.Item_Key, @Store_No, BCI.SubTeam_No, @OldCountDate), 0)
			,(select distinct vca.Package_Desc1 from dbo.fn_VendorCostAll(@NewCountDate) vca
		 where  VCA.Vendor_id=@Warehouse_ID and VCA.Item_key=BCI.Item_Key)Package_Desc1
, Package_Desc2, I.CostedByWeight
    FROM @BothCountsItems BCI 
	JOIN Item I(nolock) ON I.Item_Key = BCI.Item_Key
	WHERE (BCI.SubTeam_No = @SubTeam OR @SubTeam IS NULL)

--select * from @OldAvgCost

DECLARE @Purchases TABLE (SubTeam_No INT, Item_Key INT, Cases DECIMAL(18,4))

	INSERT INTO @Purchases
		SELECT OH.Transfer_To_SubTeam AS SubTeam_No, OI.Item_Key,
			Cases = SUM(CASE WHEN OH.Return_Order = 0
								THEN QuantityReceived
									ELSE -(QuantityReceived)
										END)
		FROM OrderHeader OH (NOLOCK)
		INNER JOIN OrderItem OI (NOLOCK)
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE OH.ReceiveLocation_ID = @Warehouse_ID
		AND (OH.CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND OH.CloseDate < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (OH.Transfer_To_SubTeam = @SubTeam OR @SubTeam IS NULL)
		GROUP BY OH.Transfer_To_SubTeam, OI.Item_Key

--select * from @Purchases

--Get the outbound (from DC) Shipped, minus credits given by DC
DECLARE @Shipped TABLE (SubTeam_No INT, Item_Key INT, Cases DECIMAL(18,4))

	INSERT INTO @Shipped
		SELECT OH.Transfer_SubTeam AS SubTeam_No, OI.Item_Key,
			Cases = SUM(CASE WHEN OH.Return_Order = 0
								THEN QuantityReceived
									ELSE -(QuantityReceived)
										END)
		FROM OrderHeader OH (NOLOCK)
		INNER JOIN OrderItem OI (NOLOCK)
			ON OI.OrderHeader_ID = OH.OrderHeader_ID
		WHERE OH.Vendor_ID = @Warehouse_ID
		AND (OH.CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND OH.CloseDate < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (OH.Transfer_SubTeam = @SubTeam OR @SubTeam IS NULL)
		GROUP BY OH.Transfer_SubTeam, OI.Item_Key

--select * from @Shipped



DECLARE @Credits TABLE (SubTeam_No INT, Item_Key INT, Cases DECIMAL(18,4), Credit$ MONEY)

INSERT INTO @Credits

SELECT ISNULL(@SubTeam, Transfer_SubTeam), OrderItem.Item_key,
SUM(QuantityReceived) as 'Cases',
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
    INNER JOIN 
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
             WHERE CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND CloseDate < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)) 
                       AND ISNULL(@Warehouse_ID, OrderHeader.Vendor_ID) = OrderHeader.Vendor_ID
                       AND Transfer_SubTeam = ISNULL(@SubTeam, Transfer_SubTeam) 
                       AND CreditReason_ID IS NULL) T1
         GROUP BY T1.Zone_ID) T2 
         ON (T2.Zone_ID = Store.Zone_ID)
WHERE CloseDate > CONVERT(VARCHAR(10),@OldCountDate,101) AND CloseDate < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)) AND
      ISNULL(@Warehouse_ID, OrderHeader.Vendor_ID) = OrderHeader.Vendor_ID AND
      ISNULL(@ReceiveLocation_ID, OrderHeader.ReceiveLocation_ID) = OrderHeader.ReceiveLocation_ID AND
      ISNULL(@Zone, Zone.Zone_ID) = Zone.Zone_ID
      AND Transfer_SubTeam = ISNULL(@SubTeam, Transfer_SubTeam)
GROUP BY ISNULL(@SubTeam, Transfer_SubTeam) , OrderItem.Item_Key

--select * from @Credits


--Get the Waste
DECLARE @WastewithPck TABLE (SubTeam_No INT,Item_Key INT,Package_Desc1 DECIMAL(18,4))
INSERT INTO @WastewithPck
		SELECT	IH.SubTeam_No, IH.Item_Key,
				
		(select distinct vca.Package_Desc1 from dbo.fn_VendorCostAll(@NewCountDate) vca
		 where  VCA.Vendor_id=@Warehouse_ID and VCA.Item_key=IH.Item_Key)Package_Desc1
		FROM ItemHistory IH (NOLOCK)
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
		WHERE IH.Adjustment_ID = 1
		AND IH.Store_No = (SELECT Store_No FROM Vendor (NOLOCK) WHERE Vendor_ID = @Warehouse_ID)
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
        GROUP BY IH.SubTeam_No, IH.Item_Key 


DECLARE @Waste TABLE (SubTeam_No INT,Item_Key INT,Cases DECIMAL(18,4))
	INSERT INTO @Waste
		SELECT	IH.SubTeam_No, IH.Item_Key,
				Cases = 
                      SUM(dbo.fn_CostConversion(ISNULL(IH.Quantity, 0) + ISNULL(IH.Weight, 0),
                                            @Case,
                                            CASE WHEN Item.CostedByWeight = 1 THEN @pound ELSE @Unit END,
                                            W.Package_desc1,
                                            Item.Package_Desc2,
                                            Item.Package_Unit_ID
                                           ))
		FROM ItemHistory IH (NOLOCK)
		Inner join @WastewithPck W
			on W.Item_key=IH.Item_Key
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
		WHERE IH.Adjustment_ID = 1
		AND IH.Store_No = (SELECT Store_No FROM Vendor (NOLOCK) WHERE Vendor_ID = @Warehouse_ID)
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
        GROUP BY IH.SubTeam_No, IH.Item_Key 

--select * from @Waste

--Get the Positive Adjustments
DECLARE @PosAdjwithPck TABLE (SubTeam_No INT, Item_Key INT, Package_Desc1 DECIMAL(18,4))
INSERT INTO @PosAdjwithPck
		SELECT	IH.SubTeam_No, IH.Item_Key,
			(select distinct vca.Package_Desc1 from dbo.fn_VendorCostAll(@NewCountDate) vca
		 where  VCA.Vendor_id=@Warehouse_ID and VCA.Item_key=IH.Item_Key)Package_Desc1	
		FROM ItemHistory IH (NOLOCK)
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
		WHERE IH.Adjustment_ID = 8
		AND IH.Store_No = (SELECT Store_No FROM Vendor (NOLOCK) WHERE Vendor_ID = @Warehouse_ID)
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		AND (IH.Weight + IH.Quantity > 0)
        GROUP BY IH.SubTeam_No, IH.Item_Key
--select * from @PosAdjwithPck
DECLARE @PosAdj TABLE (SubTeam_No INT, Item_Key INT, Cases DECIMAL(18,4))
	INSERT INTO @PosAdj
		SELECT	IH.SubTeam_No, IH.Item_Key,
				Cases = 
                      SUM(dbo.fn_CostConversion(ISNULL(IH.Quantity, 0) + ISNULL(IH.Weight, 0),
                                            @Case,
                                            CASE WHEN Item.CostedByWeight = 1 THEN @pound ELSE @Unit END,
                                            P.Package_Desc1,
                                            Item.Package_Desc2,
                                            Item.Package_Unit_ID
                                           ))
		FROM ItemHistory IH (NOLOCK)
		Inner join @PosAdjwithPck P
			on P.Item_key=IH.Item_Key
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
		WHERE IH.Adjustment_ID = 8
		AND IH.Store_No = (SELECT Store_No FROM Vendor (NOLOCK) WHERE Vendor_ID = @Warehouse_ID)
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		AND (IH.Weight + IH.Quantity > 0)
        GROUP BY IH.SubTeam_No, IH.Item_Key

--select * from @PosAdj

--Get the Negative Adjustments
DECLARE @NegAdj TABLE (SubTeam_No INT, Item_Key INT, Cases DECIMAL(18,4))
DECLARE @NegAdjWithPck TABLE (SubTeam_No INT, Item_Key INT, Package_desc1 DECIMAL(18,4))


	INSERT INTO @NegAdjWithPck
		SELECT	 IH.SubTeam_No, IH.Item_Key,
			(select distinct vca.Package_Desc1 from dbo.fn_VendorCostAll(@NewCountDate) vca
		 where  VCA.Vendor_id=@Warehouse_ID and VCA.Item_key=IH.Item_Key)Package_Desc1	
		FROM ItemHistory IH (NOLOCK)
		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
			AND Item.SubTeam_No = IH.SubTeam_No
		WHERE IH.Adjustment_ID = 8
		AND IH.Store_No = (SELECT Store_No FROM Vendor (NOLOCK) WHERE Vendor_ID = @Warehouse_ID)
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		AND (IH.Weight + IH.Quantity < 0)
        GROUP BY IH.SubTeam_No, IH.Item_Key
--select * from @NegAdjWithPck


	INSERT INTO @NegAdj
		SELECT	 IH.SubTeam_No, IH.Item_Key,
				Cases = SUM(dbo.fn_CostConversion(ISNULL(IH.Quantity, 0) + ISNULL(IH.Weight, 0),
                            @Case,
                            CASE WHEN Item.CostedByWeight = 1 THEN @pound ELSE @Unit END,
                            N.Package_Desc1,
                            Item.Package_Desc2,
                            Item.Package_Unit_ID
                           ))*-1
		FROM ItemHistory IH (NOLOCK)


		inner join @NegAdjWithPck N
		on N.Item_key=IH.Item_Key

		INNER JOIN Item (NOLOCK)
			ON Item.Item_Key = IH.Item_Key
			AND Item.SubTeam_No = IH.SubTeam_No
		WHERE IH.Adjustment_ID = 8
		AND IH.Store_No = (SELECT Store_No FROM Vendor (NOLOCK) WHERE Vendor_ID = @Warehouse_ID)
		AND (DateStamp > CONVERT(VARCHAR(10),@OldCountDate,101) AND DateStamp < DATEADD(D,1,CONVERT(VARCHAR(10),@NewCountDate,101)))
		AND (IH.SubTeam_No = @SubTeam OR @SubTeam IS NULL)
		AND (IH.Weight + IH.Quantity < 0)
        GROUP BY IH.SubTeam_No, IH.Item_Key

--select * from @NegAdj


DECLARE @SubTeamItem table (SubTeam_No int, Item_Key int)
INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Credits

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Purchases 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Shipped 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @PosAdj 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @NegAdj 

INSERT INTO @SubTeamItem 
SELECT SubTeam_No, Item_Key FROM @Waste 

--select * from @SubTeamItem

INSERT INTO @BothCountsItems
SELECT DISTINCT SubTeam_No, Item_Key, 0, 0, 0, 0, 0, 0
FROM @SubTeamItem WHERE Item_Key NOT IN  (SELECT Item_Key FROM @BothCountsItems)

--select * from @BothCountsItems

--Save off line item details for subteam summary
DECLARE @Summary table (SubTeam_No int, Item_Key int,Item_Description varchar(60),UPC varchar(13), BOHCases decimal(18,4), BOHDollars decimal(18,4), NetPurchasedCases decimal(18,4), 
							NetCasesShipped decimal(18,4), ShrinkCases decimal(18,4), PosAdj decimal(18,4), NegAdj decimal(18,4), CalcEOHCases decimal(18,4),  
							CountedEOHCases decimal(18,4), CountedEOHDollars decimal(18,4), CreditCases decimal(18,4), CreditDollars decimal(18,4),UnknownShrinkCases decimal(18,4), AvgCaseCost smallmoney,
							Shrink$ money)

INSERT INTO @Summary
SELECT	 SubTeam = BCI.SubTeam_No
		,BCI.Item_Key
		,Item.Item_description
		,II.identifier as UPC
		,'BOH Cases' = ISNULL(SUM(BOHCases), 0)
		,'BOH $' =  ISNULL(SUM(OAC.AvgCost * (CASE WHEN ISNULL(BOHWeight,0) > 0
											THEN ISNULL(BOHWeight,0)
											ELSE ISNULL(BOHUnits, 0) END)),0)
		,'Net Cases Purchased' = ISNULL(SUM(Purchases.Cases),0)
		,'Net Cases Shipped' = ISNULL(SUM(Shipped.Cases),0)
		,'Shrink Cases' = ISNULL(SUM(Waste.Cases),0)
		,'Pos Adj Cases' = ISNULL(SUM(PosAdj.Cases),0)
		,'Neg Adj Cases' = ISNULL(SUM(NegAdj.Cases),0)
		,'Calc EOH Cases' = (ISNULL(SUM(BOHCases), 0) 
								+ ISNULL(SUM(Purchases.Cases),0)
								- ISNULL(SUM(Shipped.Cases),0)
								- ISNULL(SUM(Waste.Cases),0)
								+ ISNULL(SUM(PosAdj.Cases),0)
								- ISNULL(SUM(NegAdj.Cases),0))
		,'Counted EOH Cases' = ISNULL(SUM(CountCases),0)
		,'Counted EOH $' =  ISNULL(SUM(AC.AvgCost * (CASE WHEN ISNULL(CountWeight,0) > 0
											THEN ISNULL(CountWeight,0)
											ELSE ISNULL(CountUnits, 0) END)),0)
		,'Cases Credited' = ISNULL(SUM(Credited.Cases),0)
		,'Credit $' = ISNULL(SUM(Credited.Credit$),0)
		,'Unknown Shrink Cases' = ((ISNULL(SUM(BOHCases), 0) 
								+ ISNULL(SUM(Purchases.Cases),0)
								- ISNULL(SUM(Shipped.Cases),0)
								- ISNULL(SUM(Waste.Cases),0)
								+ ISNULL(SUM(PosAdj.Cases),0)
								- ISNULL(SUM(NegAdj.Cases),0)) - ISNULL(SUM(CountCases),0))*-1
		,'Avg Case Cost' = ISNULL(SUM(AC.AvgCost * AC.Package_Desc1 * BCI.BOHCases *
							CASE WHEN AC.CostedByWeight = 1 THEN AC.Package_Desc2 ELSE 1 END ), 0)
		,'Shrink $' = (
						(((ISNULL(SUM(BOHCases), 0) 
							+ ISNULL(SUM(Purchases.Cases),0)
							- ISNULL(SUM(Shipped.Cases),0)
							- ISNULL(SUM(Waste.Cases),0)
							+ ISNULL(SUM(PosAdj.Cases),0)
							- ISNULL(SUM(NegAdj.Cases),0)) - ISNULL(SUM(CountCases),0))*-1)
						* ISNULL(SUM(AC.AvgCost * AC.Package_Desc1 * BCI.BOHCases *
							CASE WHEN AC.CostedByWeight = 1 THEN AC.Package_Desc2 ELSE 1 END ),0))
FROM @BothCountsItems BCI
	LEFT JOIN @Purchases Purchases
		ON Purchases.Item_Key = BCI.Item_Key
		AND Purchases.SubTeam_No = BCI.SubTeam_No
	LEFT JOIN @Shipped Shipped
		ON Shipped.Item_Key = BCI.Item_Key
		AND Shipped.SubTeam_No = BCI.SubTeam_No
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

GROUP BY BCI.SubTeam_No, BCI.Item_Key,Item.Item_description,II.Identifier,AC.Package_Desc1, AC.Package_Desc2, AC.CostedByWeight,
PosAdj.Cases, NegAdj.Cases,BCI.BOHCases

--select * from @Summary

SELECT	SubTeam = SubTeam_Name
		,'BOH Cases' = ISNULL(SUM(BOHCases),0)
		,'BOH $' = ISNULL(SUM(BOHDollars), 0)
		,'Net Cases Purchased' = ISNULL(SUM(NetPurchasedCases),0)
		,'Net Cases Shipped' = ISNULL(SUM(NetCasesShipped),0)
		,'Wasted Cases' = ISNULL(SUM(ShrinkCases),0)
		,'PosAdj Cases' = ISNULL(SUM(PosAdj), 0)
		,'NegAdj Cases' = ISNULL(SUM(NegAdj), 0)
		,'Calc EOH Cases' = ISNULL(SUM(CalcEOHCases), 0)
		,'Counted EOH Cases' = ISNULL(SUM(CountedEOHCases),0)
		,'Counted EOH $' = ISNULL(SUM(CountedEOHDollars), 0)
		,'Cases Credited' =  ISNULL(SUM(CreditCases), 0)  
		,'Credit $' = ISNULL(SUM(CreditDollars), 0) 
		,'Unknown Shrink Cases' = ISNULL(SUM(UnknownShrinkCases),0)
		,'Avg Case Cost' = convert(decimal(9,2),ISNULL(SUM(AvgCaseCost), 0)/IsNull(Nullif(SUM(BOHCases), 0), '1'))--ISNULL(SUM(BOHCases),0)
		,'Shrink $' = convert(decimal(9,2),ISNULL(SUM(Shrink$), 0)/IsNull(Nullif(SUM(BOHCases), 0), '1'))--ISNULL(SUM(BOHCases),0)
	
FROM @Summary S
LEFT JOIN @Purchases Purchases
	ON Purchases.Item_Key = S.Item_Key
	AND Purchases.SubTeam_No = S.SubTeam_No
LEFT JOIN @Shipped Shipped
	ON Shipped.Item_Key = S.Item_Key
	AND Shipped.SubTeam_No = S.SubTeam_No
LEFT JOIN @Waste Waste
	ON Waste.Item_Key = S.Item_Key
	AND Waste.SubTeam_No = S.SubTeam_No
LEFT JOIN @AvgCost AC
	ON AC.Item_Key = S.Item_Key
	AND AC.SubTeam_No = S.SubTeam_No
LEFT JOIN @AvgCost OAC
	ON OAC.Item_Key = S.Item_Key
	AND OAC.SubTeam_No = S.SubTeam_No
INNER JOIN SubTeam ST (NOLOCK)
	ON ST.SubTeam_No = S.SubTeam_No
GROUP BY SubTeam_Name
ORDER BY SubTeam_Name



SET ANSI_NULLS OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptInventoryCasesBySubteam] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptInventoryCasesBySubteam] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptInventoryCasesBySubteam] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[rptInventoryCasesBySubteam] TO [IRMAReportsRole]
    AS [dbo];

