SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UnitCountTotalAllStores_CrossTab]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UnitCountTotalAllStores_CrossTab]
GO


CREATE PROCEDURE dbo.UnitCountTotalAllStores_CrossTab
@Region_ID int,
@Zone_ID int,
@Store_No int,
@Team_No int,
@SubTeam_No int,
@StartDate varchar(20),
@Identifier varchar(13),
@FamilyCode varchar(13)

AS

DECLARE @BeginDateDt smalldatetime


SELECT @BeginDateDt = CONVERT(smalldatetime, @StartDate)

SELECT CAST(ItemIdentifier.Identifier As bigInt) Identifier, 
       Item.Item_Description, 
       Brand_Name, 
       Store.Store_Name,  
       zone.Zone_Name, 
       sum(dbo.Fn_ItemSalesQty(ItemIdentifier.Identifier, ItemUnit.Weight_Unit, Price_Level, 
                               Sales_Quantity, Return_Quantity, Package_Desc1, Weight)) as Qnty
FROM Item (nolock)		
	INNER JOIN 
        ItemIdentifier (nolock) 
        ON ItemIdentifier.Item_key = Item.Item_Key
            and Default_Identifier = case when @Identifier is null then 1 else default_identifier end
	INNER JOIN 
        Sales_SumByItem (nolock) 
        ON Item.Item_key = Sales_SumByItem.Item_Key 
	INNER JOIN 
        StoreSubTeam (nolock) 
        ON (StoreSubTeam.SubTeam_No = Sales_SumByItem.SubTeam_No 
            AND Sales_SumByItem.Store_No = StoreSubTeam.Store_No)     
	INNER JOIN 
        Store (nolock) 
        ON StoreSubTeam.Store_No = Store.Store_No
	INNER JOIN 
        Zone (nolock) 
        ON Store.Zone_ID = Zone.Zone_ID 
	INNER JOIN 
        Region (nolock)
        ON Zone.Region_ID = Region.Region_ID 
	LEFT JOIN 
        ItemBrand (nolock) 
        ON Item.Brand_ID = ItemBrand.Brand_id
    LEFT JOIN
        ItemUnit
        ON Item.Retail_Unit_ID = ItemUnit.Unit_ID
WHERE Sales_SumByItem.Date_Key >= @BeginDateDt AND Sales_SumByItem.Date_Key <= @BeginDateDt + 27 
      AND ISNULL(@Region_ID, Region.Region_ID) = Region.Region_ID 
      AND ISNULL(@Zone_ID , Zone.Zone_ID) = Zone.Zone_ID 
      AND ISNULL(@Store_No, StoreSubTeam.Store_No) = StoreSubTeam.Store_No 
      AND ISNULL(@SubTeam_No, StoreSubTeam.SubTeam_No) = StoreSubTeam.SubTeam_No 
      AND Item.Sales_Account IS NULL 
      AND ISNULL(@Team_No, StoreSubTeam.Team_No) = StoreSubTeam.Team_No
	  AND ItemIdentifier.Identifier LIKE CASE WHEN NOT(@Identifier IS NULL) and @FamilyCode IS NULL 
                                                THEN @Identifier
		                                      WHEN @Identifier IS NULL AND NOT(@familyCode IS NULL) 
                                                THEN @familycode + '%'
		                                      WHEN @Identifier IS NULL AND @familyCode IS NULL 
                                                THEN ItemIdentifier.Identifier
		                                      END
GROUP BY ItemIdentifier.Identifier, Item.Item_Description, 
         ItemBrand.Brand_Name, Zone.Zone_Name, Store.Store_Name  
ORDER BY CAST(ItemIdentifier.Identifier As bigint), Zone.Zone_Name, Store.Store_Name       


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


