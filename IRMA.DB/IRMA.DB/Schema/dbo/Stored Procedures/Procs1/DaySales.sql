CREATE PROCEDURE dbo.DaySales 
@Team_No int,
@SubTeam_No int,
@Store_No int,
@Zone_Id int,
@Identifier varchar(13),
@FamilyCode varchar(13),
@nReportDays smallint,
@StartDate varchar(20)
AS

DECLARE @BEGINDateDt smalldatetime
SELECT @BEGINDateDt = CONVERT(smalldatetime, @StartDate )

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

select identifier, 
       Item_Description, 
       ISNULL(Brand_Name, '') As BrandName,
       SUM(ISNULL(Day1Qnty, 0)) As Day1Qnty,
	   SUM(ISNULL(Day2Qnty, 0)) As Day2Qnty,
	   SUM(ISNULL(Day3Qnty, 0)) As Day3Qnty,
	   SUM(ISNULL(Day4Qnty, 0)) As Day4Qnty,
	   SUM(ISNULL(Day5Qnty, 0)) As Day5Qnty,
	   SUM(ISNULL(Day6Qnty, 0)) As Day6Qnty,
	   SUM(ISNULL(Day7Qnty, 0)) As Day7Qnty,
	   SUM(ISNULL(Day8Qnty, 0)) As Day8Qnty,
	   SUM(ISNULL(Day9Qnty, 0)) As Day9Qnty,
	   SUM(ISNULL(Day10Qnty, 0)) As Day10Qnty,
	   SUM(ISNULL(Day11Qnty, 0)) As Day11Qnty,
	   SUM(ISNULL(Day12Qnty, 0)) As Day12Qnty,
	   SUM(ISNULL(Day13Qnty, 0)) As Day13Qnty,
	   SUM(ISNULL(Day14Qnty, 0)) As Day14Qnty,
	   SUM(ISNULL(Day15Qnty, 0)) As Day15Qnty,
	   SUM(ISNULL(Day16Qnty, 0)) As Day16Qnty,
	   SUM(ISNULL(Day17Qnty, 0)) As Day17Qnty,
	   SUM(ISNULL(Day18Qnty, 0)) As Day18Qnty,
	   SUM(ISNULL(Day19Qnty, 0)) As Day19Qnty,
	   SUM(ISNULL(Day20Qnty, 0)) As Day20Qnty,
	   SUM(ISNULL(Day21Qnty, 0)) As Day21Qnty,
	   SUM(ISNULL(Day22Qnty, 0)) As Day22Qnty,
	   SUM(ISNULL(Day23Qnty, 0)) As Day23Qnty,
	   SUM(ISNULL(Day24Qnty, 0)) As Day24Qnty,
	   SUM(ISNULL(Day25Qnty, 0)) As Day25Qnty,
	   SUM(ISNULL(Day26Qnty, 0)) As Day26Qnty,
	   SUM(ISNULL(Day27Qnty, 0)) As Day27Qnty,
	   SUM(ISNULL(Day28Qnty, 0)) As Day28Qnty,
       SUM(ISNULL(Quantity, 0)) As Tot28DayQnty
--This is the two inner quaries
 FROM (
	   SELECT tst1.Identifier, 
              tst1.Item_Key, 
              tst1.Item_Description, 
              tst1.Brand_ID, 
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 0 THEN tst1.Quantity END) As Day1Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 1 THEN tst1.Quantity END) As Day2Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 2 THEN tst1.Quantity END) As Day3Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 3 THEN tst1.Quantity END) As Day4Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 4 THEN tst1.Quantity END) As Day5Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 5 THEN tst1.Quantity END) As Day6Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 6 THEN tst1.Quantity END) As Day7Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 7 THEN tst1.Quantity END) As Day8Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 8 THEN tst1.Quantity END) As Day9Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 9 THEN tst1.Quantity END) As Day10Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 10 THEN tst1.Quantity END) As Day11Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 11 THEN tst1.Quantity END) As Day12Qnty,
              (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 12 THEN tst1.Quantity END) As Day13Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 13 THEN tst1.Quantity END) As Day14Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 14 THEN tst1.Quantity END) As Day15Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 15 THEN tst1.Quantity END) As Day16Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 16 THEN tst1.Quantity END) As Day17Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 17 THEN tst1.Quantity END) As Day18Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 18 THEN tst1.Quantity END) As Day19Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 19 THEN tst1.Quantity END) As Day20Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 20 THEN tst1.Quantity END) As Day21Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 21 THEN tst1.Quantity END) As Day22Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 22 THEN tst1.Quantity END) As Day23Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 23 THEN tst1.Quantity END) As Day24Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 24 THEN tst1.Quantity END) As Day25Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 25 THEN tst1.Quantity END) As Day26Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 26 THEN tst1.Quantity END) As Day27Qnty,
		      (CASE WHEN DATEDIFF(day, @BEGINDateDT, tst1.DT) = 27 THEN tst1.Quantity END) As Day28Qnty,
		      tst1.Quantity		 
	   FROM (
		     SELECT ItemIdentifier.Identifier, 
                    Sales_SumByItem.Item_Key, 
                    Item.Item_Description, 
                    Item.Brand_ID,
			        Date_Key As DT,
                    sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) Quantity			        
		     FROM Store (nolock) 
			     INNER JOIN 
                    Sales_SumByItem (nolock) 
                    ON Store.Store_No = Sales_SumByItem.Store_No
			    INNER JOIN
                    @StoreSubTeam SST
                    ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No 
				        AND Sales_SumByItem.Store_No = SST.Store_No ) 
			    INNER JOIN 
                    Item (nolock) 
                    ON Sales_SumByItem.Item_Key = Item.Item_Key 
                LEFT JOIN
                    ItemUnit
                    on item.retail_Unit_ID = Itemunit.Unit_ID
			    INNER JOIN 
                    ItemIdentifier (nolock)  
                    ON (ItemIdentifier.Item_key = Item.Item_Key)
				        and Default_Identifier = case when @Identifier is null then 1 else default_identifier end
		     WHERE  Date_Key >= @BEGINDateDt AND Date_Key <= (@BEGINDateDt + @nReportDays - 1)
                    AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No 
                    AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id 
                    AND Sales_Account IS NULL   
			        AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier, ItemIdentifier.Identifier)
		     GROUP  BY Date_Key, Identifier, Item_Description, Item.Brand_ID, Sales_SumByItem.Item_Key 
            ) As tst1
      ) As tst2
    LEFT JOIN 
        ItemBrand (nolock) 
        ON tst2.Brand_ID = ItemBrand.Brand_ID
GROUP BY identifier, Item_Description, ItemBrand.Brand_Name
ORDER BY CAST(Identifier As bigint) ASC
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DaySales] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DaySales] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DaySales] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[DaySales] TO [IRMAReportsRole]
    AS [dbo];

