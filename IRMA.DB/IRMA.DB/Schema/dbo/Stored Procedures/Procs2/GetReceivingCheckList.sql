
CREATE PROCEDURE dbo.GetReceivingCheckList
@OrderHeader_ID int, 
@Item_ID bit
AS 

/*
	Update History
	--------------------------------------------------------------------------------------
	TFS 12111
	v3.6
	03/17/2010
	Tom Lux
	1) Added join to ItemCategory and CategoryName field to results because it's needed in the report.
	2) Rewrote joins.
	3) Removed unused store-number code before main SELECT.
	--------------------------------------------------------------------------------------
	TFS 12053
	v4.0
	07/7/2010
	Kevin Jessen
	1) Added field to select and join to dbo.SustainabilityRanking table because we need the abbrv for report.
	--------------------------------------------------------------------------------------
	TFS 13667
	09/12/2013
	Min Zhao
	Added SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED.
	--------------------------------------------------------------------------------------
	TFS 15828
	02/24/2015
	Denis Ng
	Added Brand Name to the output
*/

BEGIN 

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

select 
	LineItem = convert(int,ROW_NUMBER() OVER (ORDER BY ReceivingCheckList.OrderItem_ID ASC)),
	Identifier,
	Item_Description,
	SustainabilityRankingAbbr,
	Package_Desc = convert(varchar(13),Package_Desc1) + '/' + convert(varchar(13),Package_Desc2) + ' ' + Package_Unit,
	QuantityOrdered = convert(varchar,convert(decimal,QuantityOrdered)),
	QuantityUnit = ltrim(rtrim(Unit_Name)),
	Cost = convert(varchar,convert(decimal(10,4),Cost)),
	UnitCost = convert(varchar,convert(decimal(10,4),UnitCost)),
	Shipped = convert(varchar, convert(int, ReceivingCheckList.Shipped)),
	CategoryName,
	Brand_Name
FROM
    (SELECT OrderItem.OrderItem_ID, 
          (CASE WHEN ISNULL(ItemVendor.Item_ID,'') > '' THEN ItemVendor.Item_ID ELSE Identifier END) AS Identifier, 
          Item.Item_Description, 
          SustainabilityRanking.RankingAbbr as SustainabilityRankingAbbr,
          OrderItem.QuantityOrdered, 
          OrderItem.QuantityReceived, 
          OrderItem.QuantityDiscount, 
          OrderItem.DiscountType, 
          ItemUnit.Unit_Name,
          OrderItem.Package_Desc1,
          OrderItem.Package_Desc2,
          ISNULL(pkgunit.Unit_Name, 'Unit') AS Package_Unit,
          LineItemCost As Cost, 
          OrderItem.UnitExtCost as UnitCost,
		  OrderItem.QuantityShipped as Shipped,
		  CategoryName = itemcategory.category_name,
		  Brand_Name
    FROM  
		OrderHeader (NOLOCK)
		join OrderItem (NOLOCK) ON OrderHeader.OrderHeader_ID = OrderItem.OrderHeader_ID
		join ItemUnit (NOLOCK) ON OrderItem.QuantityUnit = ItemUnit.Unit_ID
		join Item (NOLOCK) ON OrderItem.Item_Key = Item.Item_Key
		join ItemBrand (NOLOCK) ON Item.Brand_ID = ItemBrand.Brand_ID
		left outer join SustainabilityRanking (NOLOCK) ON SustainabilityRanking.ID=OrderItem.SustainabilityRankingID
		join ItemIdentifier (NOLOCK) on ItemIdentifier.Item_Key = Item.Item_Key and ItemIdentifier.Default_Identifier = 1
		left join ItemUnit pkgunit (NOLOCK) on OrderItem.Package_Unit_ID = pkgunit.Unit_ID
		left join ItemVendor (NOLOCK) on @Item_ID = 1 and ItemVendor.Item_Key = OrderItem.Item_Key and ItemVendor.Vendor_ID = OrderHeader.Vendor_ID
		left join itemcategory on item.category_id = itemcategory.category_id
    WHERE OrderHeader.OrderHeader_ID = @OrderHeader_ID 
    GROUP BY
		OrderItem.OrderItem_ID
		,ItemVendor.Item_ID
		,Identifier
		,Item_Description
		,SustainabilityRanking.RankingAbbr
		,OrderItem.QuantityOrdered
		,OrderItem.QuantityReceived
		,OrderItem.QuantityDiscount
		,OrderItem.DiscountType
		,ItemUnit.Unit_Name
		,OrderItem.Package_Desc1
		,OrderItem.Package_Desc2
		,pkgunit.Unit_Name
		,LineItemCost
		,OrderItem.UnitExtCost
		,OrderItem.QuantityShipped
		,itemcategory.category_name
		,Brand_Name
		) as ReceivingCheckList

    ORDER BY ReceivingCheckList.OrderItem_ID ASC
    
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingCheckList] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingCheckList] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingCheckList] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetReceivingCheckList] TO [IRMAReportsRole]
    AS [dbo];

