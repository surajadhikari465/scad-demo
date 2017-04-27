

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DailySales]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DailySales]
GO

--exec dailysales 10094, 6, null, 2200, '76331247937', Null, '05/01/2005', '05/30/2005', null

CREATE PROCEDURE dbo.DailySales
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

DECLARE @StoreSubTeam table(Store_no int, 
                            Team_No int, 
                            SubTeam_No int)
INSERT INTO @StoreSubTeam
SELECT Store_No, Team_No, SubTeam_No
FROM StoreSubTeam
WHERE ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No)= StoreSubTeam.SubTeam_No
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 

SELECT @Identifier = ISNULL(@identifier,@familycode) + CASE WHEN @familycode IS NULL THEN '' ELSE '%' END

SELECT Store.Store_Name, 
       ItemIdentifier.Identifier, 
       ISNULL(ItemBrand.Brand_Name,'') As Brand_Name, 
       Item_Description, 
       ISNULL(ItemUnit.Unit_Name,'') As Unit_Name, 
       Item_Description, 
       SUM(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight))as Quanty,
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

GROUP BY ItemIdentifier.Identifier, ItemBrand.Brand_Name, ItemUnit.Unit_Name, 
         Item_Description, Store.Store_name, ItemUnit.Weight_Unit
ORDER BY CAST(ItemIdentifier.Identifier As bigint)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

