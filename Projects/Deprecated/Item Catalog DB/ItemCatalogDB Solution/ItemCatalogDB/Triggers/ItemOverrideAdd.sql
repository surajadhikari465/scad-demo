IF EXISTS (SELECT * FROM sysobjects WHERE type = 'TR' AND name = 'ItemOverrideAdd')
	BEGIN
		DROP  Trigger ItemOverrideAdd
	END
GO

CREATE Trigger ItemOverrideAdd ON ItemOverride 
FOR INSERT AS 
BEGIN
    DECLARE @error_no int
    SELECT @error_no = 0

    IF @error_no = 0
    BEGIN
		-- CREATE THE INITIAL HISTORY RECORD TO TRACK THE CHANGE
		INSERT INTO ItemOverrideHistory (
			Item_Key, 
			StoreJurisdictionID,
			Item_Description, 
			Sign_Description, 
			Package_Desc1, 
			Package_Desc2, 
			Package_Unit_ID, 
			Retail_Unit_ID, 
			Vendor_Unit_ID, 
			Distribution_Unit_ID, 
			POS_Description,
			Food_Stamps, 
			Price_Required,
			Quantity_Required,
			Manufacturing_Unit_ID,
			QtyProhibit, 
			GroupList,
			Case_Discount, 
			Coupon_Multiplier,
			Misc_Transaction_Sale, 
			Misc_Transaction_Refund,
			Ice_Tare,
			Brand_ID,
			Origin_ID,
			CountryProc_ID,
			SustainabilityRankingRequired,
			SustainabilityRankingID,
			LabelType_ID,
			CostedByWeight,
			Average_Unit_Weight,
			Ingredient,
			Recall_Flag,
			LockAuth,
			Not_Available,
			Not_AvailableNote,
			FSA_Eligible,
			Product_Code,
			Unit_Price_Category
		) SELECT 
			Inserted.Item_Key, 			
			Inserted.StoreJurisdictionID,
			Inserted.Item_Description, 
			Inserted.Sign_Description, 
			Inserted.Package_Desc1, 
			Inserted.Package_Desc2, 
			Inserted.Package_Unit_ID, 
			Inserted.Retail_Unit_ID, 
			Inserted.Vendor_Unit_ID, 
			Inserted.Distribution_Unit_ID, 
			Inserted.POS_Description,
			Inserted.Food_Stamps, 
			Inserted.Price_Required,
			Inserted.Quantity_Required,
			Inserted.Manufacturing_Unit_ID,
			Inserted.QtyProhibit, 
			Inserted.GroupList,
			Inserted.Case_Discount, 
			Inserted.Coupon_Multiplier,
			Inserted.Misc_Transaction_Sale, 
			Inserted.Misc_Transaction_Refund,
			Inserted.Ice_Tare,
			Inserted.Brand_ID,
			Inserted.Origin_ID,
			Inserted.CountryProc_ID,
			Inserted.SustainabilityRankingRequired,
			Inserted.SustainabilityRankingID,
			Inserted.LabelType_ID,
			Inserted.CostedByWeight,
			Inserted.Average_Unit_Weight,
			Inserted.Ingredient,
			Inserted.Recall_Flag,
			Inserted.LockAuth,
			Inserted.Not_Available,
			Inserted.Not_AvailableNote,
			Inserted.FSA_Eligible,
			Inserted.Product_Code,
			Inserted.Unit_Price_Category
        FROM Inserted
            
		SELECT @error_no = @@ERROR
    END
              
	-- SEND DOWN PRICE BATCH DETAIL RECORDS TO ALLOW ITEM CHANGES TO BE BATCHED FOR EACH STORE THAT
	-- IS ASSIGNED TO THE JURISDICTION THAT CHANGED
    IF @error_no = 0
    BEGIN
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Store_No, Inserted.Item_Key, 2, 'ItemOverrideAdd Trigger'
        FROM Inserted
        INNER JOIN Item ON 
			Item.Item_Key = Inserted.Item_Key 
			AND (Item.Remove_Item = 0 AND Item.Deleted_Item = 0)
        INNER JOIN Store ON
			Store.StoreJurisdictionID = Inserted.StoreJurisdictionID
            AND (Store.WFM_Store = 1 OR Store.Mega_Store = 1)
        WHERE (dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key, Store.Store_No) = 0)
            
        SELECT @error_no = @@ERROR
    END
    
    IF @error_no <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @error_no), 16)
        RAISERROR ('ItemOverrideAdd Trigger failed with @@ERROR: %d', @Severity, 1, @error_no)
    END
END
GO