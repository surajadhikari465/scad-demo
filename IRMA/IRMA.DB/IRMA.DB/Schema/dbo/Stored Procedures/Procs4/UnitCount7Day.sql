CREATE PROCEDURE dbo.UnitCount7Day
@Team_No int,
@SubTeam_No int,
@Store_No int,
@StartDate varchar(20),
@Identifier varchar(13),
@familycode varchar(13)

AS


/*DECLARE @Zone_Id int
declare @Team_No int
declare @SubTeam_No int
declare @Store_No int
declare @StartDate varchar(20)
Declare @Identifier varchar(13)
Declare @familycode varchar(5)

SELECT @Team_No = 240
--SELECT @SubTeam_No = 4200
SELECT @Store_No = 101
SELECT @StartDate = '04/25/2004'
--SELECT @identifier = '3149381132'*/

SET NOCOUNT ON
    
DECLARE @BeginDateDt smalldatetime
SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate )

SELECT @Identifier = ISNULL(@identifier,@familycode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END

DECLARE @StoreSubTeam table(Store_no int, Team_No int, SubTeam_No int)
INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 

SELECT identifier, 
    Item_Description, 
    ISNULL(Brand_Name, '') As BrandName,

	SUM(ISNULL(Day1Qnty, 0)) As Day1Qnty,    
    MAX(LastSold1) As lastSold1,

	SUM(ISNULL(Day2Qnty, 0)) As Day2Qnty,
    MAX(LastSold2) As lastSold2,

	SUM(ISNULL(Day3Qnty, 0)) As Day3Qnty,
    MAX(LastSold3) As lastSold3,

	SUM(ISNULL(Day4Qnty, 0)) As Day4Qnty,
    MAX(LastSold4) As lastSold4,

	SUM(ISNULL(Day5Qnty, 0)) As Day5Qnty,
    MAX(LastSold5) As lastSold5,

	SUM(ISNULL(Day6Qnty, 0)) As Day6Qnty,
    MAX(LastSold6) As lastSold6,

	SUM(ISNULL(Day7Qnty, 0)) As Day7Qnty,
    MAX(LastSold7) As lastSold7,

    SUM(Quantity) As TotQnty,
    SUM(TotalRetail) As TotalRetail
FROM(
     SELECT tst1.Identifier, tst1.Item_Key, tst1.Item_Description, tst1.Brand_ID, 
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 0 THEN tst1.Quantity END) As Day1Qnty,     
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 0 THEN tst1.LastSold END) As LastSold1,
        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 1 THEN tst1.Quantity END) As Day2Qnty,        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 1 THEN tst1.LastSold END) As LastSold2,
        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 2 THEN tst1.Quantity END) As Day3Qnty,        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 2 THEN tst1.LastSold END) As LastSold3,
        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 3 THEN tst1.Quantity END) As Day4Qnty,        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 3 THEN tst1.LastSold END) As LastSold4,
        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 4 THEN tst1.Quantity END) As Day5Qnty,        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 4 THEN tst1.LastSold END) As LastSold5,
        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 5 THEN tst1.Quantity END) As Day6Qnty,        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 5 THEN tst1.LastSold END) As LastSold6,
        
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 6 THEN tst1.Quantity END) As Day7Qnty,
        (CASE WHEN DATEDIFF(d, @BEGINDateDT, tst1.DT) = 6 THEN tst1.LastSold END) As LastSold7,
        tst1.Quantity,
        tst1.TotalRetail
     FROM (    
            SELECT Date_Key As DT, 
                   Item.Item_Description, 
                   Item.Item_Key, 
                   ItemIdentifier.Identifier,
                   Item.Brand_ID, 
                   MAX(Sales_Fact.Time_Key) As LastSold,

                   SUM(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As Quantity, 
                SUM(Sales_Amount) - SUM(Return_Amount) - SUM(Promotion_Amount) As TotalRetail 
            FROM Item (nolock)
                INNER JOIN
                    Sales_Fact (nolock)
                    ON Sales_Fact.Item_Key = Item.Item_Key
                LEFT JOIN 
                    ItemUnit (nolock)
                    on item.Retail_Unit_ID = ItemUnit.Unit_ID
                INNER JOIN
                    @StoreSubTeam SST
                    ON (SST.SubTeam_No = Sales_Fact.SubTeam_No 
                        AND SST.Store_No = Sales_Fact.Store_No)
                INNER JOIN
                    Time (nolock)
                    ON Time.Time_Key = Sales_Fact.Time_Key
                INNER JOIN
                    ItemIdentifier (nolock)
                    ON (ItemIdentifier.Item_key = Item.Item_Key) and Default_Identifier = case when @Identifier is null then 1 else default_identifier end
            WHERE Date_Key >= @BeginDateDt AND Date_Key <=  DATEADD(day,7,@BeginDateDt)
                  AND sales_fact.Time_Key >= @BeginDateDt AND sales_Fact.Time_Key <=  DATEADD(day,7,@BeginDateDt)
                  AND sales_fact.Sales_Quantity <> 0
                  AND ISNULL(@Store_No, Sales_Fact.Store_No) = Sales_Fact.Store_No             
                  AND Sales_Account IS NULL
                  AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier,ItemIdentifier.Identifier)
            GROUP BY Date_Key, Item.Item_Description, Item.Item_Key, 
                     ItemIdentifier.Identifier, Item.Brand_Id
          ) As tst1
    ) As tst2
    LEFT JOIN 
        ItemBrand (nolock) ON tst2.Brand_ID = ItemBrand.Brand_ID
GROUP BY identifier, Item_Description, ItemBrand.Brand_Name
ORDER BY CAST(Identifier As bigint)
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnitCount7Day] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnitCount7Day] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UnitCount7Day] TO [IRMAReportsRole]
    AS [dbo];

