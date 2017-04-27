SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[BuggySalesByItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[BuggySalesByItem]
GO


CREATE PROCEDURE dbo.BuggySalesByItem
@identifier varchar(13),
@Store_No int,
@Zone_Id int,
@StartDate varchar(20),
@EndDate varchar(20)
AS

/*DECLARE @identifier varchar(13),
        @Store_No int,
        @Zone_Id int,
        @StartDate varchar(20),
        @EndDate varchar(20)

SELECT  @identifier = '23999900000',
        @Store_No = 101,
--        @Zone_Id = null
        @StartDate = '12/13/2004',
        @EndDate = '12/19/2004'*/


DECLARE @lItemKey int
SELECT @lItemKey = (SELECT item_key 
                   FROM itemidentifier 
                   WHERE identifier = @identifier and ItemIdentifier.Deleted_Identifier = 0)

DECLARE @BeginDateDt smalldatetime, @EndDateDt smalldatetime
SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate), @EndDateDt = CONVERT(smalldatetime, @EndDate)

SELECT ItemIdentifier.Identifier, 
    Item_Description, 
    Sales_Fact.Time_Key, 
    Sales_Fact.Transaction_No, 
    Sales_Fact.Register_No, 
    Store.Store_Name, 
    sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) Quantity,
    (SELECT SUM(Cash_Amount) + SUM(Credit_Amount) + SUM(Check_Amount) + SUM(Food_Stamp_Amount)
		 + SUM(Vendor_Coupon_Amount) + SUM(Coupon_Amount)+ SUM(GC_In_Amount) + SUM(Employee_Dis_Amount) 
		 - SUM(Change_Amount) - SUM(Tax_Table1_Amount) - SUM(Tax_Table2_Amount) - SUM(Tax_Table3_Amount) 
         - SUM(GC_Sales_Amount) 
     FROM Buggy_Fact (nolock) 
     WHERE Buggy_Fact.Time_Key = Sales_Fact.Time_Key 
           AND Buggy_Fact.Transaction_No = Sales_Fact.Transaction_No 
           AND Buggy_Fact.Register_No = Sales_Fact.Register_No 
           AND Buggy_Fact.Store_No = Sales_Fact.Store_No) AS Net_Sales 

FROM Store (nolock) 
	INNER JOIN 
        Sales_Fact (nolock) 
        ON Store.Store_No = Sales_Fact.Store_No
	INNER JOIN 
        Item (nolock) 
        ON Sales_Fact.Item_Key = Item.Item_Key
    LEFT JOIN
        ItemUnit
        on Item.Retail_Unit_ID = ItemUnit.Unit_ID
	INNER JOIN 
        ItemIdentifier (nolock) 
        ON (ItemIdentifier.Item_key = Item.Item_Key 
            AND ItemIdentifier.Default_Identifier = 1)	
WHERE Time_Key >= @BeginDateDt AND Time_Key < DATEADD(day, 1, @EndDateDt) 
      AND Sales_Fact.Item_Key = @lItemKey 
      AND ISNULL(@Store_No, Store.Store_No) = Store.Store_No 
      AND ISNULL(@Zone_Id, Store.Zone_Id) = Store.Zone_Id
GROUP BY ItemIdentifier.Identifier, Item_Description, Sales_Fact.Time_Key, 
         Sales_Fact.Transaction_No, Sales_Fact.Register_No, Store.Store_Name, Sales_Fact.Store_No

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


--exec BuggySalesByItem '23999900000', 101, null, '12/13/2004', '12/19/2004'

