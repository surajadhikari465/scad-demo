SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
--PerpetualMovementReport 990,801
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PerpetualMovementReport]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PerpetualMovementReport]
GO


CREATE PROCEDURE [dbo].[PerpetualMovementReport]
    @Store_No int,
    @SubTeam_No int  = null,
	@Date1From as DateTime = null,
	@Date2From as DateTime = null,
	@Date3From as DateTime = null,
	@Date4From as DateTime = null,
	@Date5From as DateTime = null,
	@Date1To as DateTime = null,
	@Date2To as DateTime = null,
	@Date3To as DateTime = null,
	@Date4To as DateTime = null,
	@Date5To as DateTime = null
AS
BEGIN
    SET NOCOUNT ON

    -- Get the Item Unit ID's so we can call CostConverion
    DECLARE @ToUnit int,@Pound int
    
    SELECT @ToUnit = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Case'
    --SELECT @FromUnit = Unit_ID FROM ItemUnit WHERE Unit_Name = 'Pound'

    /*****/
    DECLARE @Today TABLE(Item_Key int, SubTeam_No int, Package_Desc1 decimal(9,4), ArrivalToday decimal(18,4))

    INSERT INTO @Today
    SELECT OrderItem.Item_Key, Transfer_To_SubTeam, VendorCostHistory.Package_Desc1,--dbo.FN_GetExePack(OrderItem.Package_Desc1, OrderItem.Package_Desc2, Item.CostedByWeight),
           SUM(dbo.fn_CostConversion(QuantityOrdered, @ToUnit, QuantityUnit, OrderItem.Package_Desc1, OrderItem.Package_Desc2, OrderItem.Package_Unit_ID))           
    FROM OrderItem (NOLOCK) 
    INNER JOIN 
        OrderHeader (NOLOCK) 
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = OrderItem.Item_Key 
	INNER JOIN VendorCostHistory (nolock)
		ON OrderItem.VendorCostHistoryID = VendorCostHistory.VendorCostHistoryID
    WHERE CloseDate IS NULL AND
          DateReceived IS NULL AND
          (Expected_Date >= DATEADD(DAY, -1, GETDATE()) AND Expected_Date <= GETDATE()) AND
          ReceiveLocation_ID = (SELECT top 1 Vendor_ID FROM Vendor (NOLOCK) WHERE Store_No = @Store_No) AND
          Return_Order = 0 AND
          Transfer_To_SubTeam = @SubTeam_No
    GROUP BY OrderItem.Item_Key, Transfer_To_SubTeam, VendorCostHistory.Package_Desc1--dbo.FN_GetExePack(OrderItem.Package_Desc1, OrderItem.Package_Desc2, Item.CostedByWeight)

    /*****/
    /*****/

    DECLARE @Future TABLE(Item_Key int, SubTeam_No int, Package_Desc1 decimal(9,4), FutureArrival decimal(18,4))

    INSERT INTO @Future
    SELECT OrderItem.Item_Key, Transfer_To_SubTeam, 
			VendorCostHistory.Package_Desc1,
           --dbo.FN_GetExePack(OrderItem.Package_Desc1, OrderItem.Package_Desc2, Item.CostedByWeight),
           SUM(dbo.fn_CostConversion(QuantityOrdered, @ToUnit, QuantityUnit, OrderItem.Package_Desc1, OrderItem.Package_Desc2, OrderItem.Package_Unit_ID))
    FROM OrderItem (NOLOCK) 
    INNER JOIN 
        OrderHeader (NOLOCK) 
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = OrderItem.Item_Key 
	INNER JOIN VendorCostHistory (nolock)
		ON OrderItem.VendorCostHistoryID = VendorCostHistory.VendorCostHistoryID
    WHERE CloseDate IS NULL AND
          DateReceived IS NULL AND
          (Expected_Date > GETDATE() AND Expected_Date <= DATEADD(DAY, 14, GETDATE())) AND  
          ReceiveLocation_ID = (SELECT top 1 Vendor_ID FROM Vendor (NOLOCK) WHERE Store_No = @Store_No) AND
          Return_Order = 0 AND
          Transfer_To_SubTeam = @SubTeam_No
    GROUP BY OrderItem.Item_Key, Transfer_To_SubTeam, VendorCostHistory.Package_Desc1--dbo.FN_GetExePack(OrderItem.Package_Desc1, OrderItem.Package_Desc2, Item.CostedByWeight) 

    /*****/
    /*****/
    DECLARE @Yesterday TABLE(Item_Key int, SubTeam_No int, Package_Desc1 decimal(9,4), ArrivalYesterday decimal(18,4))

    INSERT INTO @Yesterday
    SELECT OrderItem.Item_Key, Transfer_To_SubTeam, 
			VendorCostHistory.Package_Desc1,
           --dbo.FN_GetExePack(OrderItem.Package_Desc1, OrderItem.Package_Desc2, Item.CostedByWeight),
           SUM(dbo.fn_CostConversion(QuantityOrdered, @ToUnit, QuantityUnit, OrderItem.Package_Desc1, OrderItem.Package_Desc2, OrderItem.Package_Unit_ID))
    FROM OrderItem (NOLOCK) 
    INNER JOIN 
        OrderHeader (NOLOCK) 
        ON OrderItem.OrderHeader_ID = OrderHeader.OrderHeader_ID
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = OrderItem.Item_Key 
	INNER JOIN VendorCostHistory (nolock)
		ON OrderItem.VendorCostHistoryID = VendorCostHistory.VendorCostHistoryID
    WHERE CloseDate IS NULL AND
          DateReceived IS NULL AND
          (Expected_Date >= DATEADD(DAY, -2, GETDATE()) AND Expected_Date <=DATEADD(DAY, -1, GETDATE())) AND
          ReceiveLocation_ID = (SELECT top 1 Vendor_ID FROM Vendor (NOLOCK) WHERE Store_No = @Store_No) AND
          Return_Order = 0 AND
          Transfer_To_SubTeam = @SubTeam_No
    GROUP BY OrderItem.Item_Key, Transfer_To_SubTeam, VendorCostHistory.Package_Desc1--dbo.FN_GetExePack(OrderItem.Package_Desc1, OrderItem.Package_Desc2, Item.CostedByWeight)

    /*****/
    /*****/
    DECLARE @OnHand TABLE(Item_Key int, SubTeam_No int, Package_Desc1 decimal(9,4), OnHand decimal(18,4)) 

    INSERT INTO @OnHand
    SELECT ItemHistory.Item_Key, ItemHistory.SubTeam_No, ICH.PackSize,
            SUM(ICH.Quantity * ItemAdjustment.Adjustment_Type)
    FROM Item (nolock)
    INNER JOIN
        ItemHistory (nolock)
        ON Item.Item_Key = ItemHistory.Item_Key AND ItemHistory.Store_No = @Store_No AND ItemHistory.SubTeam_No = @SubTeam_No
    INNER JOIN
        ItemCaseHistory ICH (nolock) ON ItemHistory.ItemHistoryID = ICH.ItemHistoryID
    INNER JOIN 
        ItemAdjustment (nolock)
        ON ItemHistory.Adjustment_ID = ItemAdjustment.Adjustment_ID
    INNER JOIN
        OnHand (nolock)
        ON OnHand.Item_Key = ItemHistory.Item_Key AND OnHand.Store_No = ItemHistory.Store_No AND OnHand.SubTeam_No = ItemHistory.SubTeam_No
    WHERE dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = 0 AND Deleted_Item = 0
         AND (ItemHistory.DateStamp >= OnHand.LastReset AND ItemHistory.DateStamp <= GetDate())
    GROUP BY  ItemHistory.Item_Key, ItemHistory.SubTeam_No, ICH.PackSize
    HAVING  SUM(ICH.Quantity * ItemAdjustment.Adjustment_Type) > 0

    /*****/
    /*****/
    DECLARE @Movement TABLE(Item_Key int, SubTeam_No int, Package_Desc1 decimal(9,4), Week1 decimal(18,4), Week2 decimal(18,4), Week3 decimal(18,4), Week4 decimal(18,4),  Week5 decimal(18,4), Week_Avg decimal(18,4), Next7Days decimal(18,4), Next7to14Days decimal(18,4),Next14DaysPlus decimal(18,4))

    INSERT INTO @Movement
    SELECT Item.Item_Key, ItemHistory.SubTeam_No, ICH.PackSize,
		   CASE WHEN @Date1From IS NOT NULL AND @Date1To IS NOT NULL THEN
           SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), @Date1From, 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), @Date1To, 101) + ' 12:00:00 PM'
                      THEN ICH.Quantity 
                      ELSE 0
                    END) 
			ELSE
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, -7, GETDATE()), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), GETDATE(), 101) + ' 12:00:00 PM'
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			END
			AS Week1,
		   CASE WHEN @Date2From IS NOT NULL AND @Date2To IS NOT NULL THEN
           SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), @Date2From, 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), @Date2To, 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END) 
			ELSE
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, -14, GETDATE()), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), DATEADD(DAY, -7, GETDATE()), 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END)	
			END		
			AS Week2,
			CASE WHEN @Date3From IS NOT NULL AND @Date3To IS NOT NULL THEN
           SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), @Date3From, 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), @Date3To, 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			ELSE
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, -21, GETDATE()), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), DATEADD(DAY, -14, GETDATE()), 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			 END
			 AS Week3,
			CASE WHEN @Date4From IS NOT NULL AND @Date4To IS NOT NULL THEN
           SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), @Date4From, 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), @Date4To, 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			ELSE
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, -28, GETDATE()), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), DATEADD(DAY, -21, GETDATE()), 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			 END
			 AS Week4,
			CASE WHEN @Date5From IS NOT NULL AND @Date5To IS NOT NULL THEN
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), @Date5From, 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), @Date5To, 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END) 
			ELSE
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, -35, GETDATE()), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), DATEADD(DAY, -28, GETDATE()), 101) 
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			END
			AS Week5,
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), GETDATE(), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), DATEADD(DAY, 7, GETDATE()), 101) + ' 12:00:00 PM'
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			AS Next7Days,
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, 7, GETDATE()), 101) AND 
                         ItemHistory.DateStamp < CONVERT(varchar(12), DATEADD(DAY, 14, GETDATE()), 101) + ' 12:00:00 PM'
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			AS Next7to14Days,
			SUM(CASE WHEN ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, 14, GETDATE()), 101)
                      THEN ICH.Quantity 
                      ELSE 0
                    END)
			AS Next14DaysPlus,
           SUM(ICH.Quantity) / 5 AS Week_Avg
    FROM Item
    INNER JOIN 
        ItemHistory (NOLOCK)
        ON ItemHistory.Adjustment_ID = 6 AND Item.Item_Key = ItemHistory.Item_Key AND ItemHistory.Store_No = @Store_No AND ItemHistory.SubTeam_No = @SubTeam_No
            AND ItemHistory.DateStamp >= CONVERT(varchar(12), DATEADD(DAY, -28, GETDATE()), 101) 
            AND ItemHistory.DateStamp < CONVERT(varchar(12), GETDATE(), 101) + ' 12:00:00 PM' 
    INNER JOIN
        ItemCaseHistory ICH (NOLOCK) ON ItemHistory.ItemHistoryID = ICH.ItemHistoryID
    WHERE dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = CONVERT(bit, 0) AND Deleted_Item = CONVERT(bit, 0)
    GROUP BY Item.Item_Key, ItemHistory.SubTeam_No, ICH.PackSize   

    /*****/
    -- Main query
    /*****/
    SELECT Item.Item_Key, Item_Description, Category_Name, Identifier, 
           ISNULL(PD.Package_Desc1, Item.Package_Desc1) AS Package_Desc1,                
           ISNULL(Week1,0) as Week1, ISNULL(Week2,0) as Week2, ISNULL(Week3,0) as Week3, ISNULL(Week4,0) as Week4, ISNULL(Week5,0) as Week5, ISNULL(Week_Avg,0) as Week_Avg, ISNULL(OnHand, 0) As OnHand, ISNULL(ArrivalToday, 0) As ArrivalToday, ISNULL(FutureArrival, 0) As FutureArrival,
		    CONVERT(varchar(255), 
			CONVERT(int, ROUND(Item.Package_Desc1, 0))) + '/' + CONVERT(varchar(255), 
			CONVERT(decimal(9,2), ROUND(Item.Package_Desc2, 2))) + ' ' + IU.EDISysCode As PackageDesc,
			(CASE WHEN @Date1From is not null THEN convert(varchar,@Date1From,110) ELSE convert(varchar,getDate()- 35, 110) END) + ' - ' + (CASE WHEN @Date1To is not null THEN convert(varchar,@Date1To,110) ELSE convert(varchar,getDate()-28, 110) END) as Week1Date,
			(CASE WHEN @Date2From is not null THEN convert(varchar,@Date2From,110) ELSE convert(varchar,getDate()-28, 110) END) + ' - ' + (CASE WHEN @Date2To is not null THEN convert(varchar,@Date2To,110) ELSE convert(varchar,getDate()-21, 110) END) as Week2Date, 
			(CASE WHEN @Date3From is not null THEN convert(varchar,@Date3From,110) ELSE convert(varchar,getDate()-21, 110) END) + ' - ' + (CASE WHEN @Date3To is not null THEN convert(varchar,@Date3To,110) ELSE convert(varchar,getDate()-14, 110) END) as Week3Date, 
			(CASE WHEN @Date4From is not null THEN convert(varchar,@Date4From,110) ELSE convert(varchar,getDate()-14, 110) END) + ' - ' + (CASE WHEN @Date4To is not null THEN convert(varchar,@Date4To,110) ELSE convert(varchar,getDate()-7, 110) END) as Week4Date,
			(CASE WHEN @Date5From is not null THEN convert(varchar,@Date5From,110) ELSE convert(varchar,getDate()-7, 110) END) + ' - ' + (CASE WHEN @Date5To is not null THEN convert(varchar,@Date5To,110) ELSE convert(varchar,getDate(), 110) END) as Week5Date,
			ISNULL(Next7Days,0) AS Next7Days,ISNULL(Next7to14Days,0) AS Next7to14Days,ISNULL(Next14DaysPlus,0) AS Next14DaysPlus
    FROM Item (NOLOCK)
    LEFT JOIN
        (SELECT Item_Key, SubTeam_No, Package_Desc1 FROM @Today
         UNION
         SELECT Item_Key, SubTeam_No, Package_Desc1 FROM @Future
         UNION
         SELECT Item_Key, SubTeam_No, Package_Desc1 FROM @Yesterday
         UNION
         SELECT Item_Key, SubTeam_No, Package_Desc1 FROM @OnHand
         UNION
         SELECT Item_Key, SubTeam_No, Package_Desc1 FROM @Movement) PD
        ON Item.Item_Key = PD.Item_Key
    INNER JOIN
        ItemIdentifier (NOLOCK)
        ON Item.Item_Key = ItemIdentifier.Item_Key AND ItemIdentifier.Default_Identifier = 1
    INNER JOIN -- Limit to stuff they actually sell - this could be done in the intermediate steps, but we chose the quicker way
		ItemVendor IV 
        ON IV.Item_Key = Item.Item_Key 
        AND Deletedate IS NULL 
        AND Vendor_ID = (SELECT top 1 Vendor_ID Vendor_ID FROM Vendor (NOLOCK) WHERE Store_No = @Store_No)
    LEFT JOIN
        ItemCategory (NOLOCK) 
        ON Item.Category_ID = ItemCategory.Category_ID
    LEFT JOIN 
        ItemUnit IU
        ON Item.Package_Unit_ID = IU.Unit_ID
    LEFT JOIN
        @Today T
        ON PD.Item_Key = T.Item_Key AND PD.Package_Desc1 = T.Package_Desc1
    LEFT JOIN
        @Future F
        ON PD.Item_Key = F.Item_Key AND PD.Package_Desc1 = F.Package_Desc1
    LEFT JOIN
        @Yesterday Y
        ON PD.Item_Key = Y.Item_Key AND PD.Package_Desc1 = Y.Package_Desc1
    LEFT JOIN
        @OnHand O
        ON PD.Item_Key = O.Item_Key AND PD.Package_Desc1 = O.Package_Desc1
    LEFT JOIN
        @Movement M
        ON PD.Item_Key = M.Item_Key AND PD.Package_Desc1 = M.Package_Desc1
    WHERE ISNULL(PD.SubTeam_No, Item.SubTeam_No) = @SubTeam_No AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL) = CONVERT(bit, 0) AND Deleted_Item = CONVERT(bit, 0)

 
    SET NOCOUNT OFF
END



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO




grant exec on dbo.PerpetualMovementReport TO IRMAReportsRole, IRMAAdminRole, IRMAClientRole, IRMASchedJobsRole

go
