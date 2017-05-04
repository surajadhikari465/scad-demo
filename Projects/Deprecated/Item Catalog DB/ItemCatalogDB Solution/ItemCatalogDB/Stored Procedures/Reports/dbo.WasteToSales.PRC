SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'WasteToSales') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure WasteToSales
GO


CREATE PROCEDURE dbo.WasteToSales 
@Store_No INT, 
@SubTeam_No INT, 
@ZoneID INT, 
@CategoryID INT, 
@RegionID INT, 
@Identifier VARCHAR(13), 
@FamilyCode VARCHAR(13), 
@BeginDate VARCHAR(20), 
@EndDate VARCHAR(20) 
AS

DECLARE @tmpStore TABLE(Store_No INT Primary Key) 
INSERT INTO @tmpStore
	SELECT Store.Store_No
	FROM Store
	    INNER JOIN Zone (NOLOCK) on Store.Zone_ID = Zone.Zone_ID and zone.Region_ID = ISNULL(@RegionID, Zone.Region_ID) 
	WHERE ISNULL(@ZoneID, Store.Zone_ID) = Store.Zone_ID AND
      	  ISNULL(@Store_No, Store.Store_No) = Store.Store_No 

DECLARE @tmpItem TABLE(Item_Key INT Primary Key, SubTeam_No INT, SubTeam_Name VARCHAR(100), Identifier VARCHAR(13), 
                       Brand_Name VARCHAR(25), Item_Description VARCHAR(60), Unit_Name VARCHAR(25), Weight_Unit tinyint, 
                       Package_Desc1 decimal (9,4), DELETED_iTEM BIT  ) 
Insert into @tmpItem 
    SELECT IH.Item_Key, I.SubTeam_No, ST.SubTeam_Name, null as identifier, isnull(IB.Brand_Name, '') as brand_name, I.Item_Description, 
           IU.Unit_Name, IU.Weight_Unit, I.Package_Desc1, I.Deleted_Item
    FROM ItemHistory IH (NOLOCK)
        INNER JOIN
            ItemIdentifier II (NOLOCK)
            ON II.Item_Key = IH.Item_Key and 
               II.Default_Identifier =  CASE WHEN isnull(@identifier, '') + isnull(@FamilyCode, '') = '' 
                                                                 THEN 1 
                                                                 ELSE II.Default_Identifier 
                                                             END
        INNER JOIN 
            Item I (NOLOCK)
            on I.Item_Key = IH.Item_Key
        LEFT JOIN 
            ItemBrand IB (NOLOCK)
            ON IB.Brand_ID = I.Brand_ID
        INNER JOIN
            SubTeam ST (NOLOCK)
            on ST.Subteam_no = I.SubTeam_No
        INNER JOIN
            ItemUnit IU (NOLOCK)     
            ON IU.Unit_ID = I.Retail_Unit_ID  
        INNER JOIN
            @tmpStore TS 
            on TS.Store_no = IH.Store_No
    WHERE ISNULL(@SubTeam_No, I.SubTeam_No) = I.SubTeam_No AND
 		  II.Identifier like 
			case when not(@Identifier is null) and @familyCode is null 
                    then @Identifier
			     when @Identifier is null and not(@familyCode is null) 
                    then @familycode + '%'
			     when @Identifier is null and @familyCode is null 
                    then II.Identifier
			end 		       
           AND IH.DateStamp >= @BeginDate AND IH.DateStamp < DATEADD(d,1,@EndDate)		
		       AND (IH.Adjustment_ID = 1 ) --or IH.Adjustment_ID = 3)
    GROUP BY IH.Item_Key, I.SubTeam_No, ST.SubTeam_Name, IB.Brand_Name, I.Item_Description, IU.Unit_Name, IU.Weight_Unit, I.Package_Desc1, I.Deleted_Item
    order by IH.Item_key

    update @tmpItem 
        Set Identifier = (select top 1 II.Identifier 
                          from ItemIdentifier II                 
                          where Deleted_Identifier = 0 and II.Item_Key = TI.Item_Key 
                                and II.Default_Identifier =  CASE WHEN isnull(@identifier, '') + isnull(@FamilyCode, '') = '' 
                                                                     THEN 1 
                                                                     ELSE II.Default_Identifier 
                                                                  END
                                AND II.Identifier like 
                                			case when not(@Identifier is null) and @familyCode is null 
                                                    then @Identifier
                                			     when @Identifier is null and not(@familyCode is null) 
                                                    then @familycode + '%'
                                			     when @Identifier is null and @familyCode is null 
                                                    then II.Identifier
                                			end)

    from @TmpItem TI 


DECLARE @tmpSales TABLE(Item_Key INT Primary Key, SalesAmnt numeric(9,2), SalesQnty  numeric(9,2))
INSERT INTO @TmpSales
SELECT SSBI.Item_Key, sum(SSBI.Sales_Amount) as SalesAmnt,  
       sum(dbo.Fn_ItemSalesQty(ti.Identifier, ti.Weight_Unit, SSBI.Price_Level, 
           SSBI.Sales_Quantity, SSBI.Return_Quantity, TI.Package_Desc1, SSBI.Weight)) as SalesQnty
FROM Sales_SumByItem SSBI (NOLOCK) 
    INNER JOIN 
        @tmpItem TI 
        ON TI.Item_Key = SSBI.Item_Key 
    INNER JOIN @tmpStore TS 
        ON TS.Store_No = SSBI.Store_No
WHERE SSBI.Date_Key >= @BeginDate AND SSBI.Date_Key <= DATEADD(d,1,@EndDate)	  
GROUP BY SSBI.Item_Key, SSBI.SubTeam_No

DECLARE @tmpWaste TABLE(Item_Key INT Primary Key, Waste numeric(9,4))
INSERT INTO @tmpWaste
SELECT IH.Item_Key, sum(case when IH.Quantity = 0 then IH.Weight else IH.Quantity end) as WasteQnty
FROM ItemHistory IH (NOLOCK) 
    INNER JOIN 
        @tmpItem TI 
        ON TI.Item_Key = IH.Item_Key 
    INNER JOIN
        @tmpStore TS
        on TS.Store_no = IH.Store_no
 WHERE IH.DateStamp >= @BeginDate AND IH.DateStamp < DATEADD(d,1,@EndDate)		
       AND IH.Adjustment_ID = 1 
 GROUP BY IH.Item_Key

SELECT TI.SubTeam_Name, TI.Identifier , TI.Brand_Name, TI.Item_Description, TI.Unit_Name, TW.Waste as WasteQnty,
	   TS.SalesQnty, TS.SalesAmnt,
       ISNULL(dbo.fn_AvgCostHistory(TI.Item_Key, @Store_No, TI.SubTeam_No, @EndDate), 0) AS AvgWasteCost
FROM @tmpItem TI
    INNER JOIN
        @tmpSales TS
        on TS.Item_Key = TI.Item_key
    INNER JOIN
        @tmpWaste TW
        on TW.Item_key = TI.Item_Key
Order By TI.SubTeam_Name, Cast(Identifier as bigint)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


