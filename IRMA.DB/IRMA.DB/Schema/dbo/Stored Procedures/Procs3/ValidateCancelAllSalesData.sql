CREATE PROCEDURE [dbo].[ValidateCancelAllSalesData] 
  (
	@StoreNoItemIdentiferData	dbo.StoreNoItemIdentiferType READONLY
  )
AS
BEGIN

CREATE TABLE #StoreNoItemIdentiferErrorTable
(
	[StoreNo] INT NULL,
	[ItemKey] INT NULL,
	[ItemIdentifier]  VARCHAR(13) NULL,
	[ErrorDetails] VARCHAR(MAX),
	[OnSale] BIT NULL,
)

INSERT INTO #StoreNoItemIdentiferErrorTable
SELECT StoreNo,
       NULL,
	   ItemIdentifier,
	   NULL,
	   0
FROM  @StoreNoItemIdentiferData
	   
	UPDATE siet
	SET [ItemKey] = ii.Item_Key
	FROM  #StoreNoItemIdentiferErrorTable siet
	JOIN  ItemIdentifier ii ON siet.[ItemIdentifier] = ii.Identifier
	WHERE  ii.Remove_Identifier = 0 
		   AND ii.Deleted_Identifier = 0

	UPDATE #StoreNoItemIdentiferErrorTable
	SET ErrorDetails ='Identifier does not exist in IRMA as entered'
	WHERE ItemKey IS NULL


	UPDATE siet
	SET ErrorDetails = 'Identifier exists but in a deleted state'
	FROM  #StoreNoItemIdentiferErrorTable siet
	JOIN  item i ON siet.[ItemKey] = i.Item_Key 
	JOIN  ItemIdentifier ii ON siet.[ItemIdentifier] = ii.Identifier AND i.Item_Key =ii.Item_Key
	WHERE ( Deleted_Item = 1
		    OR Remove_Item = 1
		    OR  ii.Remove_Identifier = 1
		    OR ii.Deleted_Identifier = 1
		  )
		   AND ErrorDetails IS NULL

	UPDATE siet
	SET ErrorDetails = 'Identifier exists in IRMA but is not authorized for store/item combination chosen in the menu'
	FROM  #StoreNoItemIdentiferErrorTable siet
	JOIN  storeitem si ON siet.[ItemKey] = si.Item_Key 
					   AND siet.StoreNo = si.Store_No
	WHERE Authorized = 0
		  AND siet.ItemKey IS NOT NULL
		  AND ErrorDetails IS NULL
	

    UPDATE siet
	SET ErrorDetails = 'Item was locked by ' +  cast(u.UserName  as varchar) +  ' on ' + cast(User_ID_Date as Varchar)
	FROM  #StoreNoItemIdentiferErrorTable siet
	JOIN  item i ON siet.[ItemKey] = i.Item_Key 
	JOIN Users u on i.User_ID = u.User_ID
	WHERE  i.user_id IS NOT NULL
		   AND ErrorDetails IS NULL

    UPDATE siet
	SET OnSale = 1
	FROM  #StoreNoItemIdentiferErrorTable siet
	JOIN  Price p ON siet.[ItemKey] = p.Item_Key  AND siet.StoreNo = p.Store_No
	INNER JOIN PriceChgType (nolock) PCT ON PCT.PriceChgTypeID = P.PriceChgTypeID
	WHERE   PCT.On_Sale = 1				
			AND P.Sale_End_Date > GetDate() 
			AND ErrorDetails IS NULL

    UPDATE siet
	SET OnSale = 1
	FROM  #StoreNoItemIdentiferErrorTable siet
	JOIN  PriceBatchDetail pbd ON siet.[ItemKey] = pbd.Item_Key  AND siet.StoreNo = pbd.Store_No
	INNER JOIN Store (nolock) ON Store.Store_No = PBD.Store_No
	INNER JOIN PriceChgType (nolock) PCT ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
	LEFT JOIN PriceBatchHeader PBH (nolock) ON PBD.PriceBatchHeaderID = PBH.PriceBatchHeaderID
	WHERE ISNULL(PBH.PriceBatchStatusID, 0) < 2 
		  AND PBD.PriceChgTypeID IS NOT NULL		  
		  AND PCT.On_Sale = 1						 
		  AND PBD.Expired = 0						 
		  AND PBD.Sale_End_Date > GetDate()	
		  AND siet.ItemKey IS NOT NULL
		  AND ErrorDetails IS NULL

   UPDATE siet
   SET ErrorDetails = 'Identifier is not currently on a TPR or has pending batches'
   FROM  #StoreNoItemIdentiferErrorTable siet
   WHERE OnSale= 0 
		 AND ErrorDetails IS NULL

  SELECT StoreNo AS Store,
         ISNULL(ItemKey,'') as ItemKey,
		 ISNULL(ErrorDetails,'') as ErrorDetails,
		 [ItemIdentifier] as Identifier,
		 s.Store_Name as StoreName
   FROM #StoreNoItemIdentiferErrorTable
   INNER JOIN Store s ON #StoreNoItemIdentiferErrorTable.StoreNo = s.Store_No

  	DROP TABLE #StoreNoItemIdentiferErrorTable
END 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ValidateCancelAllSalesData] TO [IRMAClientRole]
    AS [dbo];