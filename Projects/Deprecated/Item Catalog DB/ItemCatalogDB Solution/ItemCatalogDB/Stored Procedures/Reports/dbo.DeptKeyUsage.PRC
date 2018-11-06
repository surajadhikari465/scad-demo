SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeptKeyUsage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeptKeyUsage]
GO


CREATE PROCEDURE dbo.DeptKeyUsage
@Store_No int,
@Zone_ID int,
@StartDate varchar(12), /*The start date and end date had to be changed to this data type because crystal requires it for dates*/
@EndDate varchar(12)
AS
BEGIN
    SET NOCOUNT ON
    
    SELECT Total.Store_Name, Total.Cashier_ID,Total.SubTeam_Name, 
           (CASE WHEN DeptKey_Sales IS NULL THEN 0 ELSE DeptKey_Sales END) AS DeptKey_Sales, 
           (CASE WHEN DeptKey_Volume IS NULL THEN 0 ELSE DeptKey_Volume END) AS DeptKey_Volume,
           Total_Sales, 
           Total_Volume
    FROM 
    	(SELECT Store.Store_Name, Cashier_ID, SubTeam_Name, 
               sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) As DeptKey_Sales,    	      
    	       SUM(Sales_Amount + Return_Amount + Markdown_Amount + Promotion_Amount) AS DeptKey_Volume
    	FROM ItemUnit (NOLOCK) right JOIN (
            Store (NOLOCK) INNER JOIN (
    	       SubTeam (NOLOCK) INNER JOIN (
    	         Time (NOLOCK) INNER JOIN (
    	            StoreSubTeam (NOLOCK) INNER JOIN (
    	             Sales_Fact (NOLOCK) INNER JOIN (
    			Item (NOLOCK) INNER JOIN ItemIdentifier (NOLOCK) ON ItemIdentifier.Item_Key = Item.Item_Key
    		          ) ON (Sales_Fact.Item_Key = Item.Item_Key)
    	           ) ON (StoreSubTeam.SubTeam_NO = Sales_Fact.SubTeam_No AND StoreSubTeam.Store_NO = Sales_Fact.Store_No) 
    	         ) ON (Time.Time_Key = Sales_Fact.Time_Key)
    	       ) ON (SubTeam.SubTeam_No = Sales_Fact.SubTeam_No)
    	     ) ON (Store.Store_No = Sales_Fact.Store_No)
            ) ON (ItemUnit.Unit_ID = Item.Retail_Unit_ID)
    	WHERE Date_Key >= @StartDate AND Date_Key <= @EndDate AND
    	      ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND ISNULL(@Zone_ID, Store.Zone_Id) = Store.Zone_Id AND (LEFT(Identifier, 2) = '99' 
    	AND RIGHT(Identifier, 2) = '00')
    	AND LEN(Identifier) = 6
    	AND Deleted_Identifier = 0
    	GROUP BY Store.Store_Name, Cashier_ID, SubTeam_Name) AS DeptKey 
    	
    FULL JOIN 

    	(SELECT Store.Store_Name, Cashier_ID, SubTeam_Name, 
    	 	SUM(Sales_Quantity) as Total_Sales, 
    	 	SUM(Sales_Amount) - SUM(Markdown_Amount) - SUM(Promotion_Amount) AS Total_Volume
    	FROM Store (NOLOCK) INNER JOIN (
    	       SubTeam (NOLOCK) INNER JOIN (
    	         Time (NOLOCK) INNER JOIN (
    	            StoreSubTeam (NOLOCK) INNER JOIN (
    	             Sales_Fact (NOLOCK) INNER JOIN Item (NOLOCK) ON (Sales_Fact.Item_Key = Item.Item_Key)
    	           ) ON (StoreSubTeam.SubTeam_NO = Sales_Fact.SubTeam_No AND StoreSubTeam.Store_NO = Sales_Fact.Store_No) 
    	         ) ON (Time.Time_Key = Sales_Fact.Time_Key)
    	       ) ON (SubTeam.SubTeam_No = Sales_Fact.SubTeam_No)
    	     ) ON (Store.Store_No = Sales_Fact.Store_No)
    	WHERE Date_Key >= @StartDate AND Date_Key <= @EndDate AND
    	      ISNULL(@Store_No, Store.Store_No) = Store.Store_No AND ISNULL(@Zone_ID, Store.Zone_Id) = Store.Zone_Id AND Sales_Account IS NULL
    	GROUP BY Store.Store_Name, Cashier_ID, SubTeam_Name
    	) Total ON (DeptKey.Store_NAME = Total.Store_NAME AND DeptKey.Cashier_ID = Total.Cashier_ID AND DeptKey.SubTeam_Name = Total.SubTeam_Name)
    ORDER BY Total.Store_Name, Total.Cashier_ID, Total.SubTeam_Name
    
    SET NOCOUNT OFF
END
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

