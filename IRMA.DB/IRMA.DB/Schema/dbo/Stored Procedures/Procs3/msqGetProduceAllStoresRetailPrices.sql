/****** Object:  Stored Procedure dbo.msqGetProduceAllStoresRetailPrices    Script Date: 10/4/2005 11:27:29 AM ******/
CREATE PROCEDURE dbo.msqGetProduceAllStoresRetailPrices
AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				TFS 8755					Using the function dbo.fn_GetDiscontinueStatus instead of the Discontinue_Item field in the Item table.
**********************************************************************************************************************************************************************************************************************************/

SET NOCOUNT ON

    DECLARE @Price TABLE (
			Store_No int , 
			StoreName varchar(60),    	
			Item_Key int ,
			Category varchar(60),
			Identifier varchar(13),
			ItemDescription varchar(65),
			Pack numeric(9,4),
			Unit varchar(15),
	    	Retail smallmoney ,
		 	Multiple tinyint ,   
			Cost smallmoney, 	
    	PRIMARY KEY (Item_Key, Store_No)
    )
   
    INSERT INTO @Price
    SELECT Price.Store_No, 
		Store_Name, 
		Price.Item_Key, 
		Category_Name, 
		Identifier, 
		Item_Description, 
		Package_Desc1 AS 'Pack', 
		Unit_Name AS 'Unit',
		CASE WHEN dbo.fn_OnSale(PriceChgTypeId) = 1 THEN Sale_Price ELSE Price END AS 'Retail',
		CASE WHEN dbo.fn_OnSale(PriceChgTypeId) = 1 THEN Sale_Multiple ELSE Multiple END AS 'Multiple',
		NULL
     FROM Price (nolock)
	    INNER JOIN	Item (nolock)			ON Item.Item_Key = Price.Item_Key
		INNER JOIN	ItemIdentifier (nolock)	ON Item.Item_Key = ItemIdentifier.Item_Key 
		LEFT JOIN	ItemUnit (nolock)		ON Item.Package_Unit_ID = ItemUnit.Unit_ID
		LEFT JOIN	ItemCategory (nolock)	ON Item.Category_ID = ItemCategory.Category_ID
		INNER JOIN	Store (nolock)			ON Store.Store_No = Price.Store_No
	WHERE Store.Store_No IN (
				SELECT Store_No
				FROM Store
				WHERE Zone_ID IN (1,6,7,21)) AND 
			Item.SubTeam_No = 1700 AND 
			Deleted_Item=0 AND 
			Remove_Item=0 AND 
			dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL)=0 AND 
			Default_Identifier=1 AND 
			Retail_Sale=1



DECLARE @CurrDate smalldatetime
SELECT @CurrDate = CONVERT(smalldatetime, CONVERT(varchar(255), GETDATE(), 101))

UPDATE @Price

SET Cost = (SELECT TOP 1 UnitCost + ISNULL(UnitFreight, 0)
                       FROM dbo.fn_VendorCostAll(@CurrDate) VC
                       WHERE VC.Item_Key = P.Item_Key
											 AND VC.Vendor_ID = 4798
                       AND VC.Store_No = 101)
FROM @Price P



SELECT ARE.Identifier, ARE.ItemDescription, ARE.Pack, ARE.Unit, ARE.Cost, ARE.Multiple AS 'Mult', ARE.Retail AS 'ARE Retail',
GWI.Retail AS 'GWI Retail',
COB.Retail AS 'COB Retail',
BCF.Retail AS 'BCF Retail',
SDY.Retail AS 'SDY Retail',
PNC.Retail AS 'PNC Retail',
WPF.Retail AS 'WPF Retail',
CHP.Retail AS 'CHP Retail',
RAL.Retail AS 'RAL Retail',
DRH.Retail AS 'DRH Retail',
WIN.Retail AS 'WIN Retail',
CAR.Retail AS 'CAR Retail',
CHL.Retail AS 'CHL Retail'

FROM @Price ARE

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 106) GWI 
							ON ARE.Identifier = GWI.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 108) COB 
							ON ARE.Identifier = COB.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10094) BCF 
							ON ARE.Identifier = BCF.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10136) SDY 
							ON ARE.Identifier = SDY.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10196) PNC 
							ON ARE.Identifier = PNC.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10206) WPF 
							ON ARE.Identifier = WPF.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10008) CHP 
							ON ARE.Identifier = CHP.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10016) RAL 
							ON ARE.Identifier = RAL.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10041) DRH 
							ON ARE.Identifier = DRH.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10110) WIN 
							ON ARE.Identifier = WIN.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10132) CAR 
							ON ARE.Identifier = CAR.Identifier 

INNER JOIN (
						SELECT Identifier, Retail 
						FROM @Price 
						WHERE Store_No = 10201) CHL 
							ON ARE.Identifier = CHL.Identifier 

WHERE Store_No = 101 
ORDER BY Category, ARE.Identifier, ARE.ItemDescription, ARE.Pack 

SET NOCOUNT OFF
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetProduceAllStoresRetailPrices] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetProduceAllStoresRetailPrices] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetProduceAllStoresRetailPrices] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqGetProduceAllStoresRetailPrices] TO [IRMAReportsRole]
    AS [dbo];

