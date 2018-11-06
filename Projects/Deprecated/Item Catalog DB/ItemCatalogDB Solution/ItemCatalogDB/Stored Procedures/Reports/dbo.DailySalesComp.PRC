SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DailySalesComp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DailySalesComp]
GO

--exec DailySalesComp 10094, 6, null, 2200, '76331247937', Null, '05/01/2005', '05/30/2005', null
CREATE PROCEDURE dbo.DailySalesComp
@Store_No int,
@Zone_Id int,
@Team_No int,
@SubTeam_No int,
@Identifier varchar(13),
@FamilyCode varchar(13),
@StartDate varchar(20),
@EndDate varchar(20),
@Region_ID int
As

/*Declare @Store_No int,
@Zone_Id int,
@Team_No int,
@SubTeam_No int,
@Identifier varchar(13),
@FamilyCode varchar(13),
@StartDate varchar(20),
@EndDate varchar(20),
@Region_ID int

--Select @Store_No = 101,
--Select @Zone_Id = 1
--Select @Team_No int,
Select @SubTeam_No = 4200
--Select @Identifier varchar(13),
--Select @FamilyCode varchar(13),
Select @StartDate = '05/05/04'
Select @EndDate = '05/05/04'
Select @Region_ID =1
*/

SET NOCOUNT ON
--Create the StoreSubTeam tmp table to cut down on proccessing time
DECLARE @StoreSubTeam table(Store_no int, 
                            Team_No int, 
                            SubTeam_No int)
INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 

--Determine whether we are looking up based on family code or identifier
SELECT @Identifier = ISNULL(@identifier,@familycode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END

--declare the mid level data table
DECLARE @Data1 TABLE(DsplyID int, 
                     Identifier varchar(13), 
                     Brand_Name varchar(25), 
                     Item_Description varchar(60),
                     Unit_Name varchar(25),
                     Quantity decimal(9,2),
                     TotalSales decimal(9,2))

--declare the DsplyVals tmp table.  This will be used as a ref table
DECLARE @DsplyVals TABLE(pos int IDENTITY(1,1), 
                         Dsply_Name varchar(10),
                         DsplyID int)

--if Region is null then group by store, otherwise group by zone
if @Region_ID is null 
  Begin
     --fill the DsplyVals tmp ref table
     Insert into @DsplyVals
     Select StoreAbbr, Store_No
     From Store
     Where zone_id = @Zone_ID
     order by store_no Asc

    --Fill the mid level data table
    INSERT INTO @Data1
    SELECT Store.Store_no as DsplyID,
       ItemIdentifier.Identifier, 
       ISNULL(ItemBrand.Brand_Name,'') As Brand_Name, 
       Item_Description, 
       ISNULL(ItemUnit.Unit_Name,'') As Unit_Name, 
       

       sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) Quantity, 
	   SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) 
           + SUM(Promotion_Amount)  AS TotalPrice

    FROM Sales_SumByItem(nolock) 
    	INNER JOIN 
            @StoreSubTeam SST 
            ON (SST.SubTeam_No = Sales_SumByItem.Subteam_No 
    		    AND SST.Store_No = Sales_SumByItem.Store_No)
    	INNER JOIN 
            Store (nolock) 
            ON Store.Store_No = Sales_SumByItem.Store_No
        INNER JOIN
            Zone (nolock)
            ON Store.Zone_ID = Zone.Zone_ID
    	INNER JOIN
            Item (nolock) 
            ON Sales_SumByitem.Item_Key = Item.Item_Key
    	INNER JOIN
            ItemIdentifier (nolock)  
            ON (ItemIdentifier.Item_key = Item.Item_Key)
    		    and Default_Identifier = case when @Identifier is null then 1 else default_identifier end 
    	LEFT JOIN 
            ItemUnit (nolock) 
            ON ItemUnit.Unit_ID = Item.Retail_Unit_ID
    	LEFT JOIN
            ItemBrand (nolock)
            ON Item.Brand_ID = ItemBrand.Brand_ID
    WHERE Date_Key >= @StartDate AND Date_Key < DATEADD(day,1, @EndDate) 
          and (store.Mega_Store = 1 or WFM_Store = 1)
          AND Sales_Account IS NULL
          AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No
          AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id 
          AND ISNULL(@Region_Id, Zone.Region_Id) = Zone.Region_Id 
    	  AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier,ItemIdentifier.Identifier)
    
    GROUP BY Store.Store_no, ItemIdentifier.Identifier, ItemBrand.Brand_Name, ItemUnit.Unit_Name, 
             Item_Description, ItemUnit.Weight_Unit
    ORDER BY CAST(ItemIdentifier.Identifier As bigint), dsplyid
  END
ELSE
  BEGIN
     Insert into @DsplyVals
     Select distinct zone.Zone_Name, zone.Zone_ID
     From Zone inner join store on store.zone_id = zone.Zone_id
     Where Region_id = @Region_id 
           and (store.Mega_Store = 1 or WFM_Store = 1)
     order by zone.zone_id asc

    INSERT INTO @Data1
    SELECT Store.Zone_ID as DsplyID,
       ItemIdentifier.Identifier, 
       ISNULL(ItemBrand.Brand_Name,'') As Brand_Name, 
       Item_Description, 
       ISNULL(ItemUnit.Unit_Name,'') As Unit_Name, 
       SUM(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As Quantity, 
	   SUM(Sales_Amount) + SUM(Return_Amount) + SUM(Markdown_Amount) 
           + SUM(Promotion_Amount)  AS TotalPrice

    FROM Sales_SumByItem(nolock) 
    	INNER JOIN 
            @StoreSubTeam SST 
            ON (SST.SubTeam_No = Sales_SumByItem.Subteam_No 
    		    AND SST.Store_No = Sales_SumByItem.Store_No)
    	INNER JOIN 
            Store (nolock) 
            ON Store.Store_No = Sales_SumByItem.Store_No
        INNER JOIN
            Zone (nolock)
            ON Store.Zone_ID = Zone.Zone_ID
    	INNER JOIN
            Item (nolock) 
            ON Sales_SumByitem.Item_Key = Item.Item_Key
    	INNER JOIN
            ItemIdentifier (nolock)  
            ON (ItemIdentifier.Item_key = Item.Item_Key )
    		    --AND ItemIdentifier.Default_Identifier = 1) 
    	LEFT JOIN 
            ItemUnit (nolock) 
            ON ItemUnit.Unit_ID = Item.Retail_Unit_ID
    	LEFT JOIN
            ItemBrand (nolock)
            ON Item.Brand_ID = ItemBrand.Brand_ID
    WHERE Date_Key >= @StartDate AND Date_Key < DATEADD(day,1, @EndDate) 
          and (store.Mega_Store = 1 or WFM_Store = 1)
          AND Sales_Account IS NULL
          AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No
          AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id 
          AND ISNULL(@Region_Id, Zone.Region_Id) = Zone.Region_Id 
    	  AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier,ItemIdentifier.Identifier)
    
    GROUP BY Store.Zone_ID, ItemIdentifier.Identifier, ItemBrand.Brand_Name, ItemUnit.Unit_Name, 
             Item_Description, ItemUnit.Weight_Unit
    ORDER BY CAST(ItemIdentifier.Identifier As bigint), DsplyID
END



select Identifier,
--       max(DsplyName1) As DsplyName1,
       max(DsplyName1) As DsplyName1,
       max(DsplyName2) As DsplyName2,
       max(DsplyName3) As DsplyName3,
       max(DsplyName4) As DsplyName4,
       max(DsplyName5) As DsplyName5,
       max(DsplyName6) As DsplyName6,
       max(DsplyName7) As DsplyName7,
       max(DsplyName8) As DsplyName8,
       max(DsplyName9) As DsplyName9,
       max(DsplyName10) As DsplyName10,
       Brand_Name,
       Item_Description,
       Unit_Name,
       max(Quantity1) as Quantity1,
       max(TotalSales1) as TotalSales1,
       max(Quantity2) as Quantity2,
       max(TotalSales2) as TotalSales2,
       max(Quantity3) as Quantity3,
       max(TotalSales3) as TotalSales3,
       max(Quantity4) as Quantity4,
       max(TotalSales4) as TotalSales4,
       max(Quantity5) as Quantity5,
       max(TotalSales5) as TotalSales5,
       max(Quantity6) as Quantity6,
       max(TotalSales6) as TotalSales6,
       max(Quantity7) as Quantity7,
       max(TotalSales7) as TotalSales7,
       max(Quantity8) as Quantity8,
       max(TotalSales8) as TotalSales8,
       max(Quantity9) as Quantity9,
       max(TotalSales9) as TotalSales9,
       max(Quantity10) as Quantity10,
       max(TotalSales10) as TotalSales10       
FROM (
      Select 
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 1) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =1)
                  end) as DsplyName1,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 2) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =2)
                  end) as DsplyName2,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 3) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =3)
                  end) as DsplyName3,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 4) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =4)
                  end) as DsplyName4,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 5) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =5)
                  end) as DsplyName5,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 6) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =6)
                  end) as DsplyName6,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 7) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =7)
                  end) as DsplyName7,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 8) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =8)
                  end) as DsplyName8,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 9) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =9)
                  end) as DsplyName9,
            (case when exists (Select Dsply_Name from @DsplyVals DV where DV.Pos = 10) 
                    then (select Dsply_Name from @DsplyVals DV where DV.Pos =10)
                  end) as DsplyName10,
            Data.Identifier,
            Data.Brand_Name,
            Data.Item_Description,
            Data.Unit_Name,
            (CASE WHEN DsplyVals.Pos = 1 THEN Data.Quantity END) As Quantity1,
            (CASE WHEN DsplyVals.Pos = 1 THEN Data.TotalSales END) As TotalSales1,
    
            (CASE WHEN DsplyVals.Pos = 2 THEN Data.Quantity END) As Quantity2,
            (CASE WHEN DsplyVals.Pos = 2 THEN Data.TotalSales END) As TotalSales2,
    
            (CASE WHEN DsplyVals.Pos = 3 THEN Data.Quantity END) As Quantity3,
            (CASE WHEN DsplyVals.Pos = 3 THEN Data.TotalSales END) As TotalSales3,
    
            (CASE WHEN DsplyVals.Pos = 4 THEN Data.Quantity END) As Quantity4,
            (CASE WHEN DsplyVals.Pos = 4 THEN Data.TotalSales END) As TotalSales4,
    
            (CASE WHEN DsplyVals.Pos = 5 THEN Data.Quantity END) As Quantity5,
            (CASE WHEN DsplyVals.Pos = 5 THEN Data.TotalSales END) As TotalSales5,
    
            (CASE WHEN DsplyVals.Pos = 6 THEN Data.Quantity END) As Quantity6,
            (CASE WHEN DsplyVals.Pos = 6 THEN Data.TotalSales END) As TotalSales6,
    
            (CASE WHEN DsplyVals.Pos = 7 THEN Data.Quantity END) As Quantity7,
            (CASE WHEN DsplyVals.Pos = 7 THEN Data.TotalSales END) As TotalSales7,
    
            (CASE WHEN DsplyVals.Pos = 8 THEN Data.Quantity END) As Quantity8,
            (CASE WHEN DsplyVals.Pos = 8 THEN Data.TotalSales END) As TotalSales8,
    
            (CASE WHEN DsplyVals.Pos = 9 THEN Data.Quantity END) As Quantity9,
            (CASE WHEN DsplyVals.Pos = 9 THEN Data.TotalSales END) As TotalSales9,
    
            (CASE WHEN DsplyVals.Pos = 10 THEN Data.Quantity END) As Quantity10,
            (CASE WHEN DsplyVals.Pos = 10 THEN Data.TotalSales END) As TotalSales10
    from @data1 Data
        inner join 
            @DsplyVals DsplyVals
            on DsplyVals.DsplyID = Data.dsplyID
    --order by identifier
   ) as tmp
group by Identifier,
         Brand_Name,
         Item_Description,
         Unit_Name
order by identifier

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

