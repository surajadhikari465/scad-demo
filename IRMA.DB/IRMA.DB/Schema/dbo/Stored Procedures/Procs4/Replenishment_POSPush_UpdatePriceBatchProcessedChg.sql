﻿
CREATE PROCEDURE [dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedChg]
	@PriceBatchHeaderIds BatchIdsType READONLY --this is a user table type that contains just one int type column
AS

BEGIN
SET NOCOUNT ON
	    
DECLARE @RegPriceChgTypeID	int
SELECT @RegPriceChgTypeID = PriceChgTypeID
FROM	PriceChgType
WHERE	On_Sale = 0

IF OBJECT_ID('tempdb..#Grouper') IS NOT NULL
BEGIN
	DROP TABLE #Grouper
END 

IF OBJECT_ID('tempdb..#PriceBatchHeaders') IS NOT NULL
BEGIN
	DROP TABLE #PriceBatchHeaders
END 

CREATE TABLE #Grouper
(
	PriceBatchDetailID int PRIMARY KEY
	, Item_Key int
	, Store_No int
	, StartDate smalldatetime
)
	
CREATE TABLE #PriceBatchHeaders
(
	PriceBatchHeaderID INT
	, BatchId INT
)

--Put the incoming data into the temp table for better performance
INSERT INTO #PriceBatchHeaders
SELECT PriceBatchHeaderId, BatchId FROM @PriceBatchHeaderIds p where p.PriceBatchHeaderID > 0
BEGIN TRANSACTION
	
BEGIN TRY

	INSERT INTO #Grouper
	SELECT MAX(PriceBatchDetailID), Item_Key, Store_No, MAX(ISNULL(D.StartDate, H.StartDate))
	FROM PriceBatchDetail D  
	INNER JOIN PriceBatchHeader H  ON D.PriceBatchHeaderID = H.PriceBatchHeaderID
	WHERE D.PriceBatchHeaderID IN (SELECT PriceBatchHeaderID from #PriceBatchHeaders)
	GROUP BY Item_Key, Store_No

	UPDATE PriceBatchHeader 
	SET PriceBatchStatusID = 6,
	    ProcessedDate = GETDATE(),
	    POSBatchID = pbhtemp.BatchId
	FROM PriceBatchHeader pbh
	INNER JOIN #PriceBatchHeaders pbhtemp ON PBH.PriceBatchHeaderID = pbhtemp.PriceBatchHeaderID
	
	-- Note: Some of the columns used in SignQueue aren't being inserted into 
	-- PriceBatchDetail for the "off sale" batches.  These are not set until the
	-- UpdatePriceBatchPackage stored procedure is run to package the batch.  This ensures that
	-- the PBD record will contain the current Item information at the time the 
	-- batch is packaged (which could be much later in the future), not the Item information
	-- as it exists today.  
    	
	-- INSERT "off sale" rows, entries that revert to the old price at the end of the sale

	-- First, expire any unsent, autogenerated Regular records that will be superseded by these new records.
	-- Note: This should only be done if the PBD record being processed contains a price change;  if it is only 
	-- for an item change, this logic does not apply.
	UPDATE	PriceBatchDetail 
	SET		Expired = 1
	FROM	PriceBatchDetail  
	INNER JOIN PriceBatchDetail PBD_New	 ON	PBD_New.Item_Key = PriceBatchDetail.Item_Key 
		AND PBD_New.Store_No = PriceBatchDetail.Store_No
		AND PBD_New.PriceBatchHeaderID = PriceBatchDetail.PriceBatchHeaderID
	-- limit to details assigned to the batch that has just been processed
	INNER JOIN #PriceBatchHeaders pbhtemp ON PBD_new.PriceBatchHeaderID = pbhtemp.PriceBatchHeaderId
	INNER JOIN Price P ON PBD_New.Store_No = P.Store_No AND	PBD_New.Item_Key = P.Item_Key
	WHERE	PriceBatchDetail.PriceBatchHeaderId is NULL
		AND PriceBatchDetail.Expired = 0
		AND PriceBatchDetail.AutoGenerated = 1
		AND PBD_New.PriceChgTypeID IS NOT NULL -- make sure the batch that has just been processed contains a price change
		AND NOT EXISTS 
		(
			SELECT 1 
			FROM PriceBatchDetail PBD_Existing  
				WHERE PBD_New.PriceBatchDetailID != PBD_Existing.PriceBatchDetailID
				AND PBD_New.Store_No = PBD_Existing.Store_No 
				AND PBD_New.Item_Key = PBD_Existing.Item_Key
				AND PBD_New.Sale_End_Date >= PBD_Existing.StartDate 
				AND PBD_New.Sale_End_Date < PBD_Existing.Sale_End_Date
		)
	
		  
	-- next, INSERT regular price entries where there is no sale available in PBD
	INSERT INTO PriceBatchDetail (AsOfDate, AutoGenerated, Expired, Item_Key, Store_No, PriceChgTypeID, 
		StartDate, Multiple, Price, POSPrice, MSRPPrice, MSRPMultiple, PricingMethod_ID, Sale_Multiple, Sale_Price, 
		Sale_End_Date, Sale_Max_Quantity, Sale_Mix_Match, Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3,
		InsertApplication, MixMatch)
	SELECT	PBD_New.Sale_End_Date	as AsOfDate,
			1						as AutoGenerated,
			0						as Expired,
			P.Item_Key, 
			P.Store_No, 
			@RegPriceChgTypeID		as PriceChgTypeID, 
			dateadd(d, 1, PBD_New.Sale_End_Date) as StartDate,
			isnull(PBD_New.Multiple, P.Multiple), 
			isnull(PBD_New.Price, P.Price), 
			isnull(PBD_New.POSPrice, P.POSPrice),
			isnull(PBD_New.MSRPPrice, P.MSRPPrice), 
			isnull(PBD_New.MSRPMultiple, P.MSRPMultiple),
			NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL,
			'Sale Off',
			MixMatch = CASE WHEN dbo.fn_InstanceDataValue ('ResetMixMatchAfterSale', PBD_New.Store_No) = 1	
				THEN 0
			ELSE NULL END				
	FROM	PriceBatchDetail PBD_New  
	INNER JOIN Price P  ON PBD_New.Store_No = P.Store_No AND	PBD_New.Item_Key = P.Item_Key
	INNER JOIN PriceChgType PCT   -- this JOIN will limit to price changes; the inserts should not be made for just item changes
		ON PCT.PriceChgTypeID = PBD_New.PriceChgTypeID	
	WHERE	PBD_New.PriceBatchHeaderID IN (SELECT PriceBatchHeaderID FROM #PriceBatchHeaders)
		AND	NOT EXISTS (
			SELECT 1 
			FROM PriceBatchDetail PBD_Existing  
			LEFT JOIN PriceBatchHeader PBH_E  ON PBH_E.PriceBatchHeaderId = PBD_Existing.PriceBatchHeaderId
				WHERE PBD_New.PriceBatchDetailID != PBD_Existing.PriceBatchDetailID
				AND PBD_New.Store_No = PBD_Existing.Store_No 
				AND PBD_New.Item_Key = PBD_Existing.Item_Key
				AND PBD_Existing.PriceChgTypeId is not null  -- otherwise future Cost changes act as decoys
				AND dateadd(d, 1, PBD_New.Sale_End_Date) >= PBD_Existing.StartDate  -- Add one to New.Sale_End_Date because that's the start date of the inserted row
				AND PBD_New.Sale_End_Date < isnull(PBD_Existing.Sale_End_Date,dateadd(d, 1, PBD_New.Sale_End_Date))
	-- if there is a user-entered REG, it will not have been expired in prior step, and should also block a new REG for the sale-off
				AND PBD_Existing.Expired = 0
				AND PBD_Existing.EndedEarly = 0
				AND isnull(PBH_E.PriceBatchStatusId,0) < 6)
		--ENSURE THAT NEW PBH IS NOT A NEW REGULAR PRICE BATCH; AUTO-GEN PBD RECORDS SHOULD ONLY BE CREATED 
		--FOR SALES THAT ARE ENDING AND GOING BACK TO REG PRICE
		AND PCT.On_Sale = 1
	 		
-- finally, INSERT sale entries where this sale will drop down to a lower priority sale when it ends.  
	INSERT INTO PriceBatchDetail (AsOfDate, AutoGenerated, Expired, Item_Key, Store_No, PriceChgTypeID, 
		StartDate, Multiple, Price, POSPrice, MSRPPrice, MSRPMultiple, PricingMethod_ID, Sale_Multiple, Sale_Price, POSSale_Price,
		Sale_End_Date, Sale_Max_Quantity, Sale_Mix_Match, Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3,
		InsertApplication)
	SELECT	PBD_Existing.StartDate	as AsOfDate,
			1						as AutoGenerated,
			0						as Expired,
			PBD_Existing.Item_Key, 
			PBD_Existing.Store_No, 
			PBD_Existing.PriceChgTypeID, 
			--PBD_Existing.StartDate, 				
			DATEADD(d, 1, PBD_New.Sale_End_Date) as StartDate, --SET START DATE OF NEW PBD TO 1 DAY MORE THAN END DATE OF BATCH THAT WAS JUST PROCESSED
			PBD_Existing.Multiple, 
			PBD_Existing.Price, 
			PBD_Existing.POSPrice,
			PBD_Existing.MSRPPrice, 
			PBD_Existing.MSRPMultiple, 
			PBD_Existing.PricingMethod_ID, 
			PBD_Existing.Sale_Multiple, 
			PBD_Existing.Sale_Price,
			PBD_Existing.POSSale_Price,
			PBD_Existing.Sale_End_Date, 
			PBD_Existing.Sale_Max_Quantity, 
			PBD_Existing.Sale_Mix_Match, 
			PBD_Existing.Sale_Earned_Disc1, 
			PBD_Existing.Sale_Earned_Disc2, 
			PBD_Existing.Sale_Earned_Disc3,
			'Sale Off'
	FROM	#PriceBatchHeaders pbhtemp--JAMALI: ADDED THIS TO TRIM DOWN THE ROWS THAT HAVE TO BE SELECTED INITIALLY 
	INNER JOIN  PriceBatchDetail PBD_New  ON pbhtemp.PriceBatchHeaderID = PBD_New.PriceBatchHeaderId
	INNER JOIN	PriceBatchDetail PBD_Existing  
		ON	PBD_New.PriceBatchDetailID != PBD_Existing.PriceBatchDetailID
			AND	PBD_New.Item_Key	= PBD_Existing.Item_Key
			AND	PBD_New.Store_No	= PBD_Existing.Store_No
			AND DATEADD(d, 1, PBD_New.Sale_End_Date) >= PBD_Existing.StartDate 
			AND PBD_New.Sale_End_Date < PBD_Existing.Sale_End_Date				
	LEFT JOIN PriceBatchHeader PBH  ON PBH.PriceBatchHeaderID = PBD_Existing.PriceBatchHeaderID 
	WHERE 
		--SHOULD ONLY HAVE TO CREATE NEW RECORD WHEN REVERTING BACK TO AN EXISTING 
		--RECORD THAT WAS ALREADY PUSHED DOWN (HENCE THE STATUS = 6)
		ISNULL(PBH.PriceBatchStatusID, 0) = 6
		--ELIMINATE ANY PENDING BATCHES THAT SUPERSEDE PREVIOUSLY PROCESSED BATCHES  
		AND PBD_Existing.PriceBatchDetailID = (
				SELECT TOP 1 PriceBatchDetailID
						FROM PriceBatchDetail PBD_Top  ,
						PriceChgType	PCT  
						WHERE PBD_New.Store_No = PBD_Top.Store_No 
						AND PBD_New.Item_Key = PBD_Top.Item_Key
						AND DATEADD(d, 1, PBD_New.Sale_End_Date) >= PBD_Top.StartDate 
						AND PBD_New.Sale_End_Date < PBD_Top.Sale_End_Date
						AND PCT.PriceChgTypeID = PBD_Top.PriceChgTypeId				
						AND PBD_Top.Expired = 0
						ORDER BY PCT.Priority DESC, PBD_Top.StartDate DESC)
 
-- END of the "Insert Off-sale rows"  section
	
UPDATE PriceBatchDetail
	SET	Expired = 0
	FROM PriceBatchDetail  
	INNER JOIN #PriceBatchHeaders pbh  ON PriceBatchDetail.PriceBatchHeaderId = pbh.PriceBatchHeaderID
	INNER JOIN PriceChgType  ON PriceBatchDetail.PriceChgTypeId = PriceChgType.PriceChgTypeId
	WHERE PriceChgType.On_Sale = 0
	
UPDATE Price
	SET           
        --users can update REG price info when making SALE price changes; grab value from PBD
        Multiple = 	ISNULL(PBD.Multiple, Price.Multiple),
    	Price = 	ISNULL(PBD.Price, Price.Price), 
    	POSPrice = 	ISNULL(PBD.POSPrice, Price.POSPrice),
        PriceChgTypeId = PBD.PriceChgTypeId,
        MSRPPrice = ISNULL(PBD.MSRPPrice, 0),
        MSRPMultiple = ISNULL(PBD.MSRPMultiple, 1),
        PricingMethod_ID = ISNULL(PBD.PricingMethod_ID, 0),             
        --ONLY UPDATE SALE FIELDS WHEN NOT A REG PRICE PBD RECORD
        Sale_Price = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_Price      
							ELSE ISNULL(PBD.Sale_Price, 0) END,
        POSSale_Price = CASE WHEN PCT.On_Sale = 0 THEN Price.POSSale_Price      
							    ELSE ISNULL(PBD.POSSale_Price, 0) END,
        Sale_Multiple = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_Multiple      
								ELSE ISNULL(PBD.Sale_Multiple, 1) END,
        Sale_Start_Date = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_Start_Date      
								ELSE PBD.StartDate END,
        Sale_End_Date = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_End_Date      
								ELSE ISNULL(PBD.Sale_End_Date, Price.Sale_End_Date) END,
        Sale_Earned_Disc1 = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_Earned_Disc1      
									ELSE ISNULL(PBD.Sale_Earned_Disc1,0) END,
		Sale_Earned_Disc2 = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_Earned_Disc2      
									ELSE ISNULL(PBD.Sale_Earned_Disc2,0) END,									 
        Sale_Earned_Disc3 = CASE WHEN PCT.On_Sale = 0 THEN Price.Sale_Earned_Disc3      
									ELSE ISNULL(PBD.Sale_Earned_Disc3,0) END,
		-- Mix Match resets to 0 if Sale going to Reg and instance data flag is set
		MixMatch = CASE WHEN (dbo.fn_InstanceDataValue ('ResetMixMatchAfterSale', PBD.Store_No) = 1	
							AND PBD.PriceChgTypeId = @RegPriceChgTypeID
							AND Price.PriceChgTypeId <> @RegPriceChgTypeID) 
						THEN 0
					ELSE Price.MixMatch 
						END
    FROM Price  
    INNER JOIN PriceBatchDetail PBD  ON PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
    INNER JOIN #PriceBatchHeaders pbhtemp ON pbd.PriceBatchHeaderId = pbhtemp.PriceBatchHeaderId
    INNER JOIN PriceBatchHeader PBH  ON pbhtemp.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN #Grouper G
        ON PBD.PriceBatchDetailID = G.PriceBatchDetailID AND PBD.Item_Key = G.Item_Key AND PBD.Store_No = G.Store_No
    INNER JOIN PriceChgType PCT  ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
    WHERE PBD.PriceChgTypeID IS NOT NULL
        AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate 
    
--IF PRICE TABLE IS BEING UPDATED WITH A REGULAR PRICE CHANGE, THEN UPDATE THE SALE_END_DATE IF IT IS IN THE FUTURE;
--SET THE NEW SALE_END_DATE TO 1 DAY BEFORE THE START DATE OF THE REG PRICE CHANGE
    UPDATE Price SET
		-- If the regular price change start date is the same as or earlier than the ongoing sale start date, 
		-- the sale dates need to be cleared rather than updated
		Sale_Start_Date = CASE 
			WHEN DATEDIFF(d, Price.Sale_Start_Date, DATEADD(d, -1, PBD.StartDate)) <= 0 THEN NULL
			ELSE Price.Sale_Start_Date END,
		Sale_End_Date = CASE 
			WHEN DATEDIFF(d, Price.Sale_Start_Date, DATEADD(d, -1, PBD.StartDate)) <= 0 THEN NULL
			ELSE DATEADD(d, -1, PBD.StartDate) END
    FROM Price
    INNER JOIN PriceBatchDetail  PBD ON PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
	INNER JOIN #PriceBatchHeaders pbhtemp ON pbd.PriceBatchHeaderId = pbhtemp.PriceBatchHeaderId
    INNER JOIN PriceBatchHeader PBH  ON pbhtemp.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN #Grouper G
        ON PBD.PriceBatchDetailID = G.PriceBatchDetailID AND PBD.Item_Key = G.Item_Key AND PBD.Store_No = G.Store_No
    INNER JOIN PriceChgType PCT ON PCT.PriceChgTypeID = PBD.PriceChgTypeID
    WHERE PBD.PriceChgTypeID = @RegPriceChgTypeID
        AND Price.Sale_End_Date IS NOT NULL
        AND Price.Sale_End_Date > PBD.StartDate --GREATER THAN TODAY
        AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate 

    UPDATE SignQueue
    SET Sign_Description = ISNULL(PBD.Sign_Description, ''), 
        Ingredients=PBD.Ingredients, 
        Identifier= ISNULL(PBD.Identifier, ''), 
        Sold_By_Weight=ISNULL(PBD.Sold_By_Weight, 0), 
            
        --users can update REG price info when making SALE price changes; grab value from PBD
        Multiple = 	ISNULL(PBD.Multiple, Price.Multiple),
    	Price = 	ISNULL(PBD.Price, Price.Price), 
    	POSPrice = 	ISNULL(PBD.POSPrice, Price.POSPrice),
                	                     	     
        MSRPMultiple=ISNULL(PBD.MSRPMultiple, 1), 
        MSRPPrice=ISNULL(PBD.MSRPPrice, 0), 
        Case_Price=ISNULL(PBD.Case_Price, 0), 
        Sale_Multiple=ISNULL(PBD.Sale_Multiple, 1), 
        Sale_Price=ISNULL(PBD.Sale_Price, 0), 
        POSSale_Price=ISNULL(PBD.POSSale_Price, 0), 
        Sale_Start_Date=CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN PBD.StartDate ELSE SQ.Sale_Start_Date END, 
        Sale_End_Date=ISNULL(PBD.Sale_End_Date, SQ.Sale_End_Date), 
        Sale_Earned_Disc1=ISNULL(PBD.Sale_Earned_Disc1,0), 
        Sale_Earned_Disc2=ISNULL(PBD.Sale_Earned_Disc2,0), 
        Sale_Earned_Disc3=ISNULL(PBD.Sale_Earned_Disc3,0), 
        PricingMethod_ID=PBD.PricingMethod_ID, 
        SubTeam_No=PBD.SubTeam_No, 
        Origin_Name=PBD.Origin_Name, 
        Brand_Name=PBD.Brand_Name, 
        Retail_Unit_Abbr=PBD.Retail_Unit_Abbr, 
        Retail_Unit_Full=PBD.Retail_Unit_Full, 
        Package_Unit=PBD.Package_Unit, 
        Package_Desc1= ISNULL(PBD.Package_Desc1, 0), 
        Package_Desc2=ISNULL(PBD.Package_Desc2, 0), 
        Organic=PBD.Organic, 
        Vendor_Id=PBD.Vendor_Id, 
        [User_ID]=0,
        User_ID_Date=ISNULL(PBH.PrintedDate, GETDATE()), 
        ItemType_ID=PBD.ItemType_ID, 
        ScaleDesc1=PBD.ScaleDesc1, 
        ScaleDesc2=PBD.ScaleDesc2, 
        POS_Description=PBD.POS_Description, 
        Restricted_Hours=PBD.Restricted_Hours, 
        LocalItem = PBD.LocalItem,
        Quantity_Required=PBD.Quantity_Required, 
        Price_Required=PBD.Price_Required, 
        Retail_Sale=PBD.Retail_Sale, 
        --Tax_Table_A=PBD.Tax_Table_A, 
        --Tax_Table_B=PBD.Tax_Table_B, 
        --Tax_Table_C=PBD.Tax_Table_C, 
        --Tax_Table_D=PBD.Tax_Table_D, 
        Discountable=PBD.Discountable, 
        Food_Stamps=PBD.Food_Stamps, 
        IBM_Discount=PBD.IBM_Discount, 
        New_Item=CASE WHEN PBD.ItemChgTypeID = 1 THEN 1 ELSE 0 END, 
        Price_Change=CASE WHEN PBD.PriceChgTypeID IS NOT NULL THEN 1 ELSE 0 END, 
        Item_Change=CASE WHEN PBD.ItemChgTypeID = 2 THEN 1 ELSE 0 END,
        LastQueuedType = 1,
        PriceChgTypeID = isnull(PBD.PriceChgTypeID,SQ.PriceChgTypeID),
        TagTypeID = PBD.TagTypeID,
        Item_Description = PBD.Item_Description,
		Case_Discount = PBD.Case_Discount,
		Coupon_Multiplier = PBD.Coupon_Multiplier,
		Misc_Transaction_Sale = PBD.Misc_Transaction_Sale,
		Misc_Transaction_Refund = PBD.Misc_Transaction_Refund,
		Recall_Flag = PBD.Recall_Flag,
		Product_Code = PBD.Product_Code,
		Unit_Price_Category = PBD.Unit_Price_Category,
		Ice_Tare = PBD.Ice_Tare,
		NotAuthorizedForSale = PBD.NotAuthorizedForSale,
		Age_Restrict = PBD.Age_Restrict,
		Routing_Priority = PBD.Routing_Priority,
		Consolidate_Price_To_Prev_Item = PBD.Consolidate_Price_To_Prev_Item,
		Print_Condiment_On_Receipt = PBD.Print_Condiment_On_Receipt,
		KitchenRoute_ID = PBD.KitchenRoute_ID,
		ItemSurcharge = PBD.ItemSurcharge
    FROM SignQueue SQ  
    INNER JOIN PriceBatchDetail PBD  ON PBD.Item_Key = SQ.Item_Key AND PBD.Store_No = SQ.Store_No
    INNER JOIN #PriceBatchHeaders pbhtemp  ON PBD.PriceBatchHeaderId = pbhtemp.PriceBatchHeaderId
    INNER JOIN PriceBatchHeader PBH  ON pbhtemp.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN Price  ON PBD.Item_Key = Price.Item_Key AND PBD.Store_No = Price.Store_No
    INNER JOIN #Grouper G  
        ON PBD.PriceBatchDetailID = G.PriceBatchDetailID AND PBD.Item_Key = G.Item_Key AND PBD.Store_No = G.Store_No AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate 
      
--IF SIGNQUEUE TABLE IS BEING UPDATED WITH A REGULAR PRICE CHANGE, THEN UPDATE THE SALE_END_DATE IF IT IS IN THE FUTURE;
--SET THE NEW SALE_END_DATE TO 1 DAY BEFORE THE START DATE OF THE REG PRICE CHANGE
    UPDATE SignQueue SET
		-- If the regular price change start date is the same as or earlier than the ongoing sale start date, 
		-- the sale dates need to be cleared rather than updated
		Sale_Start_Date = CASE 
			WHEN DATEDIFF(d, Price.Sale_Start_Date, DATEADD(d, -1, PBD.StartDate)) <= 0 THEN NULL
			ELSE Price.Sale_Start_Date END,
		Sale_End_Date = CASE 
			WHEN DATEDIFF(d, Price.Sale_Start_Date, DATEADD(d, -1, PBD.StartDate)) <= 0 THEN NULL
			ELSE DATEADD(d, -1, PBD.StartDate) END
    FROM SignQueue SQ  
    INNER JOIN PriceBatchDetail PBD  ON PBD.Item_Key = SQ.Item_Key AND PBD.Store_No = SQ.Store_No
    INNER JOIN #PriceBatchHeaders pbhtemp  ON PBD.PriceBatchHeaderId = pbhtemp.PriceBatchHeaderId
    INNER JOIN PriceBatchHeader PBH  ON pbhtemp.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN Price  ON PBD.Store_No = Price.Store_No AND PBD.Item_Key = Price.Item_Key
    INNER JOIN #Grouper G
            ON PBD.PriceBatchDetailID = G.PriceBatchDetailID AND PBD.Item_Key = G.Item_Key AND PBD.Store_No = G.Store_No AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate 
    WHERE PBD.PriceChgTypeID = @RegPriceChgTypeID
        AND SQ.Sale_End_Date IS NOT NULL
        AND SQ.Sale_End_Date > PBD.StartDate --GREATER THAN TODAY

	INSERT INTO SignQueue (Item_Key, Store_No, Sign_Description, Ingredients, Identifier, Sold_By_Weight, Multiple, Price, POSPrice, POSSale_Price, MSRPMultiple, MSRPPrice, Case_Price, Sale_Multiple, Sale_Price, Sale_Start_Date, Sale_End_Date, Sale_Earned_Disc1, Sale_Earned_Disc2, Sale_Earned_Disc3, PricingMethod_ID, SubTeam_No, Origin_Name, Brand_Name, Retail_Unit_Abbr, Retail_Unit_Full, Package_Unit, Package_Desc1, Package_Desc2, Sign_Printed, Organic, Vendor_Id, [User_ID], User_ID_Date, ItemType_ID, ScaleDesc1, ScaleDesc2, POS_Description, Restricted_Hours, LocalItem, Quantity_Required, Price_Required, Retail_Sale, 
    --Tax_Table_A, Tax_Table_B, Tax_Table_C, Tax_Table_D, 
    Discountable, Food_Stamps, IBM_Discount, New_Item, Price_Change, Item_Change, LastQueuedType, PriceChgTypeId, TagTypeID, ItemSurcharge)
    SELECT PBD.Item_Key, PBD.Store_No,
        ISNULL(PBD.Sign_Description, NULL), 
        PBD.Ingredients, 
        ISNULL(PBD.Identifier, ''),
        ISNULL(PBD.Sold_By_Weight, 0),
            				 
		--users can update REG price info when making SALE price changes; grab value from PBD
        Multiple = 	ISNULL(PBD.Multiple, Price.Multiple),
    	Price = 	ISNULL(PBD.Price, Price.Price), 
    	POSPrice = 	ISNULL(PBD.POSPrice, Price.POSPrice),
				 
        ISNULL(PBD.POSSale_Price, 0),                
        ISNULL(PBD.MSRPMultiple, 1), 
        ISNULL(PBD.MSRPPrice, 0), 
        ISNULL(PBD.Case_Price, 0), 
        ISNULL(PBD.Sale_Multiple, 1), 
        ISNULL(PBD.Sale_Price, 0), 
        CASE WHEN dbo.fn_OnSale(PBD.PriceChgTypeID) = 1 THEN PBD.StartDate ELSE NULL END, 
        PBD.Sale_End_Date, 
        ISNULL(PBD.Sale_Earned_Disc1,0), 
        ISNULL(PBD.Sale_Earned_Disc2,0), 
        ISNULL(PBD.Sale_Earned_Disc3,0), 
        PBD.PricingMethod_ID, 
        PBD.SubTeam_No, 
        PBD.Origin_Name, 
        PBD.Brand_Name, 
        PBD.Retail_Unit_Abbr, 
        PBD.Retail_Unit_Full, 
        PBD.Package_Unit, 
        ISNULL(PBD.Package_Desc1, 0), 
        ISNULL(PBD.Package_Desc2, 0),
        1, 
        PBD.Organic, 
        PBD.Vendor_Id, 
        0,
        ISNULL(PBH.PrintedDate, GETDATE()), 
        PBD.ItemType_ID, 
        PBD.ScaleDesc1, 
        PBD.ScaleDesc2, 
        PBD.POS_Description, 
        PBD.Restricted_Hours, 
        PBD.LocalItem,
        PBD.Quantity_Required, 
        PBD.Price_Required, 
        PBD.Retail_Sale, 
        --PBD.Tax_Table_A, 
        --PBD.Tax_Table_B, 
        --PBD.Tax_Table_C, 
        --PBD.Tax_Table_D, 
        PBD.Discountable, 
        PBD.Food_Stamps, 
        PBD.IBM_Discount, 
        CASE WHEN PBD.ItemChgTypeID = 1 THEN 1 ELSE 0 END, 
        CASE WHEN PBD.PriceChgTypeID IS NOT NULL THEN 1 ELSE 0 END, 
        CASE WHEN PBD.ItemChgTypeID = 2 THEN 1 ELSE 0 END,
        1,
        PBD.PriceChgTypeId,
        PBD.TagTypeID,
        PBD.ItemSurcharge
    FROM #PriceBatchHeaders pbhtemp  
    INNER JOIN  PriceBatchDetail PBD ON pbhtemp.PriceBatchHeaderID = PBD.PriceBatchHeaderId
    INNER JOIN PriceBatchHeader PBH  ON pbhtemp.PriceBatchHeaderID = PBH.PriceBatchHeaderID
    INNER JOIN #Grouper G
            ON PBD.PriceBatchDetailID = G.PriceBatchDetailID AND PBD.Item_Key = G.Item_Key AND PBD.Store_No = G.Store_No
    INNER JOIN Price  ON PBD.Store_No = Price.Store_No AND PBD.Item_Key = Price.Item_Key
    LEFT JOIN SignQueue  SQ ON PBD.Store_No = SQ.Store_No AND PBD.Item_Key = SQ.Item_Key
    WHERE SQ.Item_Key IS NULL
		AND ISNULL(PBD.StartDate, PBH.StartDate) = G.StartDate
    
-- Update SLIM_InStoreSpecials
    DECLARE @ISSPriceTypeID AS INT
	DECLARE @ProcessedStatusId AS INT
		
    SET @ISSPriceTypeID = (SELECT PriceChgTypeID FROM PriceChgType WHERE (PriceChgTypeDesc = 'ISS'))
	SET @ProcessedStatusId = (SELECT statusid FROM slim_statustypes  WHERE status = 'Processed')
		
	UPDATE SLIM_InStoreSpecials
		SET Status = @ProcessedStatusId   -- 'Processed' status  
		FROM SLIM_InStoreSpecials  AS sis
		INNER JOIN PriceBatchDetail AS pbd  ON  sis.Item_Key = pbd.Item_Key AND sis.Store_no = pbd.Store_No AND sis.RequestId = pbd.SLIMRequestID
		INNER JOIN #PriceBatchHeaders pbhtemp  ON pbd.PriceBatchHeaderID = pbhtemp.PriceBatchHeaderID
		WHERE pbd.PriceChgTypeID = ISNULL(@ISSPriceTypeID, pbd.PriceChgTypeID)			  -- Change Type is an 'In-Store Special'		
		
	COMMIT TRANSACTION
	DROP TABLE #Grouper	
	DROP TABLE #PriceBatchHeaders	
END TRY
BEGIN CATCH
	IF @@TRANCOUNT <> 0
        ROLLBACK TRANSACTION
        
	DECLARE @ErrorMessage NVARCHAR(MAX);
	DECLARE @ErrorSeverity INT;
	DECLARE @ErrorState INT;
		
	SELECT 
		@ErrorMessage = '[Replenishment_POSPush_UpdatePriceBatchProcessedChg] failed with error: ' + ERROR_MESSAGE(),
		@ErrorSeverity = ERROR_SEVERITY(),
		@ErrorState = ERROR_STATE()

	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
    
SET NOCOUNT OFF
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedChg] TO [IRMAAdminRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedChg] TO [IRMAClientRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedChg] TO [IRMASchedJobsRole]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[Replenishment_POSPush_UpdatePriceBatchProcessedChg] TO [IRMAReportsRole]
    AS [dbo];

