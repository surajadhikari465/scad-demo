SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesByItemByDayCrossTab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SalesByItemByDayCrossTab]
GO


CREATE PROCEDURE dbo.SalesByItemByDayCrossTab
@Store_No int,
@Zone_Id int,
@Team_No int,
@SubTeam_No int,
@Identifier varchar(13),
@FamilyCode varchar(13),
@StartDate varchar(20),
@EndDate varchar(20)
As

SET NOCOUNT ON

SELECT @Identifier = ISNULL(@identifier,@familycode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END

DECLARE @StoreSubTeam table(Store_no int, 
                            Team_No int, 
                            SubTeam_No int)
INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 

-- Convert the dates to smalldatetime because this will run forever if not
-- The parameters are varchar because Crystal does not cooperate otherwise.

DECLARE @BeginDateDt smalldatetime, 
        @EndDateDt smalldatetime
SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate), @EndDateDt = CONVERT(smalldatetime, @EndDate)


DECLARE @FinalData table (Date_Key smalldatetime, 
                          Store_Name varchar(50), 
                          Identifier varchar(13), 
                          ItemDescription VARCHAR(60), 
                          RowDesc VARCHAR(30), 
                          TYValue INT, 
                          LYValue INT) 
INSERT INTO @FinalData 
SELECT Sales_SumByItem.Date_Key, 
       Store.Store_Name, 
       ItemIdentifier.Identifier, 
       Item.item_Description, 
       'This Year Sales' As RowDesc, 

        sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) Quantity,
       NULL
FROM Sales_SumByItem (nolock) 	
	INNER JOIN 
        Store (nolock) 
        ON Store.Store_No = Sales_SumByItem.Store_No	
	INNER JOIN 
        Item (nolock) 
        ON Sales_SumByItem.Item_Key = Item.Item_Key 
	INNER JOIN 
        ItemIdentifier (nolock) 
        ON (ItemIdentifier.Item_key = Item.Item_Key)
		    --AND ItemIdentifier.Default_Identifier = 1)
    LEFT JOIN 
        ItemUnit
        ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
	INNER JOIN 
        @StoreSubTeam SST 
        ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
		    AND SST.Store_No = Sales_SumByItem.Store_No) 	
WHERE
	Date_Key >= @BeginDateDt AND Date_Key <= @EndDateDt  
    AND Sales_Account IS NULL 
    AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No 
    AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id 
    AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier, ItemIdentifier.Identifier) 			
GROUP BY Sales_SumByItem.Date_Key, Store.Store_Name, ItemIdentifier.Identifier, 
         Item.Item_Description, ItemIdentifier.Identifier 
ORDER BY Store_Name, ItemIdentifier.Identifier, 
         Sales_SumByItem.Date_Key, RowDesc

SET NOCOUNT ON

INSERT INTO @FinalData
SELECT DATEADD(day, 364,Sales_SumByItem.Date_Key), 
       Store.Store_Name, 
       ItemIdentifier.Identifier, 
       Item.item_Description, 
       'Last Year Sales' As RowDesc,
       NULL,
	   SUM(Sales_Quantity) - SUM(Return_Quantity) 
FROM Sales_SumByItem (nolock) 	
	INNER JOIN 
        Store (nolock) 
        ON Store.Store_No = Sales_SumByItem.Store_No	
	INNER JOIN 
        Item (nolock) 
        ON Sales_SumByItem.Item_Key = Item.Item_Key 
	INNER JOIN 
        ItemIdentifier (nolock) 
        ON (ItemIdentifier.Item_key = Item.Item_Key)
		    --AND ItemIdentifier.Default_Identifier = 1)
	INNER JOIN 
        @StoreSubTeam SST 
        ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
		    AND SST.Store_No = Sales_SumByItem.Store_No) 	
WHERE
	Date_Key >= DATEADD(day, -364, @BeginDateDt) AND Date_Key <= DATEADD(day, -364, @EndDateDt)  
    AND Sales_Account IS NULL 
    AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No 
    AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id 
    AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier, ItemIdentifier.Identifier)
GROUP BY Sales_SumByItem.Date_Key, Store.Store_Name, ItemIdentifier.Identifier, 
         Item.Item_Description, ItemIdentifier.Identifier 
ORDER BY Store_Name, ItemIdentifier.Identifier, 
         Sales_SumByItem.Date_Key, RowDesc

SELECT * FROM @FinalData

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


