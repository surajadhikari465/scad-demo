CREATE PROCEDURE dbo.PriceChangeEventReport
@Vendor_ID int,
@SubTeam_No int,
@Class int
AS 

BEGIN

SELECT Item.Item_Key,
	V.CompanyName,	
	IC.Category_Name,
	ItemIdentifier.Identifier, 
    Item.Item_Description,
	Item.Package_Desc2 AS UNITS, 
	ItemUnit.Unit_Abbreviation,
    SubTeam.SubTeam_Name,
    PH.Sale_Start_Date, 
	PH.Sale_End_Date,
    PriceChgTypeDesc,
    CASE WHEN dbo.fn_OnSale(PH.PriceChgTypeID) = 1 THEN PH.Sale_Multiple ELSE PH.Multiple END AS Multiple,
    CASE WHEN dbo.fn_OnSale(PH.PriceChgTypeID) = 1 THEN PH.Sale_Price ELSE PH.Price END AS Price,			
	ISNULL(dbo.fn_AvgCostHistory(Item.Item_Key, PH.Store_No, Item.SubTeam_No, PH.Sale_Start_Date), 0) AS AvgCost,
    PH.Effective_Date
    FROM PriceHistory PH (nolock)   
    INNER JOIN
        Item (nolock)
        ON Item.Item_Key = PH.Item_Key
    INNER JOIN
        ItemIdentifier (nolock)
        ON ItemIdentifier.Item_Key = Item.Item_Key AND ItemIdentifier.Default_Identifier = 1
    INNER JOIN
        SubTeam (nolock)
        ON SubTeam.SubTeam_No = Item.SubTeam_No
    INNER JOIN
        ItemUnit (nolock) 
        ON ItemUnit.Unit_ID = Item.Package_Unit_ID
    INNER JOIN
        PriceChgType (nolock)
        ON PH.PriceChgTypeID = PriceChgType.PriceChgTypeID	
	INNER JOIN
         StoreItemVendor SIV
         on SIV.Store_no = PH.Store_no and SIV.Item_Key = PH.Item_Key 
	INNER JOIN 
		Vendor V (nolock)
		ON V.Vendor_ID = SIV.Vendor_ID	
	INNER JOIN 
		 ItemCategory IC
		 on Item.Category_ID = IC.Category_ID
    WHERE Item.Deleted_Item = 0 AND
        SubTeam.SubTeam_No = ISNULL(@SubTeam_No, SubTeam.SubTeam_No) AND
        Item.Category_ID = isnull(@Class, Item.Category_ID) AND
  	    V.Vendor_ID = @Vendor_ID	
	ORDER BY Item.Item_Key, PH.Effective_Date
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PriceChangeEventReport] TO [IRMAReportsRole]
    AS [dbo];

