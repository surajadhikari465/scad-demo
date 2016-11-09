CREATE PROCEDURE dbo.[msqProduceRetailPriceGuide]
    @Store_No int,
    @SubTeam_No int
    
AS
/**********************************************************************************************************************************************************************************************************************************
CHANGE LOG
DEV					DATE					TASK						Description
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DN					2013.01.11				TFS 8755					Using the function dbo.fn_GetDiscontinueStatus instead of the Discontinue_Item field in the Item table.
**********************************************************************************************************************************************************************************************************************************/
BEGIN
    SET NOCOUNT ON

		DECLARE @Date datetime  
		 SELECT @Date = CONVERT(datetime, CONVERT(varchar(255), DATEADD(day, 1, GETDATE()), 101))

    DECLARE @Price TABLE (
    	Item_Key int,
    	Multiple tinyint ,
    	Price smallmoney ,
    	PricingMethod_ID int ,
    	Sale_Multiple tinyint ,
    	Sale_Price smallmoney ,
    	Sale_Start_Date smalldatetime  ,
    	Sale_End_Date smalldatetime  ,
    	Sale_Max_Quantity tinyint ,
    	Sale_Earned_Disc1 tinyint ,
    	Sale_Earned_Disc2 tinyint ,
    	Sale_Earned_Disc3 tinyint ,
    	PriceChgTypeDesc varchar(20), 
    	PRIMARY KEY (Item_Key)
    )
    
    INSERT INTO @Price
    SELECT PBD.Item_Key, 
           CASE WHEN PBD.Price IS NULL THEN Price.Multiple ELSE PBD.Multiple END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Price ELSE PBD.Price END, 
           CASE WHEN PBD.Price IS NULL THEN Price.PricingMethod_ID ELSE PBD.PricingMethod_ID END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_Multiple ELSE PBD.Sale_Multiple END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_Price ELSE PBD.Sale_Price END, 
           PBD.StartDate, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_End_Date ELSE PBD.Sale_End_Date END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_Max_Quantity ELSE PBD.Sale_Max_Quantity END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_Earned_Disc1 ELSE PBD.Sale_Earned_Disc1 END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_Earned_Disc2 ELSE PBD.Sale_Earned_Disc2 END, 
           CASE WHEN PBD.Price IS NULL THEN Price.Sale_Earned_Disc3 ELSE PBD.Sale_Earned_Disc3 END,
    	   isnull(PCT.PriceChgTypeDesc,'')
    FROM PriceBatchDetail PBD (nolock)
    INNER JOIN
        Price (nolock)
        ON Price.Item_Key = PBD.Item_Key AND Price.Store_No = @Store_No
     LEFT JOIN 
		PriceChangeType PCT
		ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
    WHERE PBD.PriceBatchDetailID = (SELECT TOP 1 D.PriceBatchDetailID
                                     FROM PriceBatchDetail D
                                     LEFT JOIN
                                         PriceBatchHeader H (nolock)
                                         ON D.PriceBatchHeaderID = H.PriceBatchHeaderID
                                     WHERE ISNULL(H.PriceBatchStatusID, 0) < 6
                                         AND D.Store_No = @Store_No
                                         AND D.Item_Key = PBD.Item_Key
                                         AND D.PriceChgTypeID IS NOT NULL
                                         AND D.StartDate <= @Date
                                     ORDER BY D.StartDate DESC)    
    
    SELECT 
        Identifier, Item_Description AS 'Item Description', Package_Desc1 AS 'Pack', Unit_Name AS 'Unit', 
      	CASE WHEN dbo.fn_OnSale(Price.PriceChgTypeID) = 1 THEN Price.Sale_Multiple ELSE Price.Multiple END AS 'Mult', 
		dbo.fn_Price(Price.PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) AS 'Retail', 
		(dbo.fn_Price(Price.PriceChgTypeID, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price) * Package_Desc1) * CasePriceDiscount AS 'CaseRetail', 
		Origin_Name AS 'Origin'
    FROM Item (nolock)
    LEFT JOIN
        ItemCategory (nolock)
        ON ItemCategory.Category_ID = Item.Category_ID
    INNER JOIN 
        ItemIdentifier (nolock)
        ON ItemIdentifier.Item_Key = Item.Item_Key
    LEFT JOIN 
        ItemOrigin (nolock)
        ON ItemOrigin.Origin_ID = Item.Origin_ID
    INNER JOIN
        ItemUnit (nolock)
        ON ItemUnit.Unit_ID = Item.Package_Unit_ID
    INNER JOIN
        (SELECT Item_Key, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price, Sale_Start_Date, Sale_End_Date, Sale_Max_Quantity, Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3, isnull(PCT.PriceChgTypeDesc,'')
         FROM Price (nolock)
				LEFT JOIN PriceChangeType PCT
				ON PCT.PriceChgTypeID = Price.PriceChgTypeID
         WHERE Price.Store_No = @Store_No
             AND NOT EXISTS (SELECT * FROM @Price P WHERE P.Item_Key = Price.Item_Key)
         UNION
         SELECT Item_Key, Multiple, Price, PricingMethod_ID, Sale_Multiple, Sale_Price, Sale_Start_Date, Sale_End_Date, Sale_Max_Quantity, Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3, PriceChgTypeDesc
         FROM @Price P) Price
        ON Price.Item_Key = Item.Item_Key
    INNER JOIN
        StoreSubTeam (nolock)
        ON StoreSubTeam.Store_No = @Store_No AND StoreSubTeam.SubTeam_No = Item.SubTeam_No
    WHERE Item.SubTeam_No=@SubTeam_No 
        AND Deleted_Item=0 AND Remove_Item=0 AND dbo.fn_GetDiscontinueStatus(Item.Item_Key, NULL, NULL)=0 AND Default_Identifier=1 AND Retail_Sale=1
    ORDER BY Category_Name, Item_Description, Item.Package_Desc1

    SET NOCOUNT OFF
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqProduceRetailPriceGuide] TO [IRMASupportRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqProduceRetailPriceGuide] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqProduceRetailPriceGuide] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[msqProduceRetailPriceGuide] TO [IRMAReportsRole]
    AS [dbo];

