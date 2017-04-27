SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SalesPercentage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SalesPercentage]
GO
--EXEC SalesPercentage 10094, NULL, NULL, 1000, NULL, NULL, '7/4/2005', '7/31/2005'
CREATE PROCEDURE dbo.SalesPercentage
@Store_No int,
@Zone_Id int,
@Team_No int,
@SubTeam_No int,
@Identifier varchar(13),
@FamilyCode varchar(13),
@StartDate smalldatetime,
@EndDate smalldatetime
As

/*DECLARE @Store_No int
DECLARE @Zone_Id int
DECLARE @Team_No int
DECLARE @SubTeam_No int
DECLARE @Identifier varchar(13)
DECLARE @FamilyCode varchar(13)
DECLARE @StartDate smalldatetime
DECLARE @EndDate smalldatetime

SELECT @StartDate  = cast('04/20/2004' as smalldatetime)
SELECT @EndDate  = cast('04/26/2004' as smalldatetime)
exec SalesPercentage 101, null, null, 4200, null, null, @StartDate, @EndDate 


SELECT @Store_No = 101
--SELECT @Zone_Id = 1
--SELECT @Team_No =  
SELECT @SubTeam_No = 2100
--LECT @Identifier = '2345960'
--LECT @FamilyCode = '34'
SELECT @StartDate  = cast('04/20/2004' as smalldatetime)
SELECT @EndDate  = cast('04/26/2004' as smalldatetime)*/

SET NOCOUNT ON

DECLARE @StoreSubTeam table(Store_no int, 
                            Team_No int, 
                            SubTeam_No int)

INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No

SELECT @identifier = ISNULL(@identifier,@FamilyCode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END

SELECT Date_Key, 
       Store.Store_no, 
       SUM(ISNULL(Sales_Amount,0)) + SUM(ISNULL(Return_Amount,0)) + SUM(ISNULL(Markdown_Amount,0)) + SUM(ISNULL(Promotion_Amount,0)) As DeptSales,        
	   SUM(ISNULL(Sales_Amount,0)) + SUM(ISNULL(Return_Amount,0)) + SUM(ISNULL(Markdown_Amount,0)) + SUM(ISNULL(Promotion_Amount,0)) As TotalSales
FROM Sales_SumByItem (nolock) 
    INNER JOIN 
        Store (nolock)  
        ON Store.Store_No = Sales_SumByItem.Store_No
    INNER JOIN 
        @StoreSubTeam SST
        ON (Sales_SumByItem.SubTeam_No = SST.SubTeam_No
            AND Sales_SumByItem.Store_No = SST.Store_No)     
    INNER JOIN 
        Item (nolock) 
        ON Sales_SumByItem.Item_Key = Item.Item_Key
    INNER JOIN 
        ItemIdentifier (nolock) 
        ON (item.Item_Key = ItemIdentifier.Item_key
            AND ItemIdentifier.default_identifier = case when @identifier = '' then 1 else ItemIdentifier.default_identifier end)
WHERE Date_Key >= @StartDate AND Date_Key <= @EndDate 
    AND Sales_Account IS NULL 
    AND	ISNULL(@Store_No, Store.Store_No) = Store.Store_No
    AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id
    AND ItemIdentifier.Identifier LIKE ISNULL(@Identifier, ItemIdentifier.identifier)
GROUP BY Date_Key, Store.Store_no

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


