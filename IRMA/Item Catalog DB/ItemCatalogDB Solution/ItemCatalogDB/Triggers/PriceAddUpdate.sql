IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id('dbo.PriceAddUpdate'))
  BEGIN
    PRINT ''
    PRINT 'Dropping old version of dbo.PriceAddUpdate ...'
    DROP TRIGGER dbo.PriceAddUpdate
  END
GO 

PRINT ''
PRINT 'Creating Trigger dbo.PriceAddUpdate ...'
GO

CREATE TRIGGER dbo.PriceAddUpdate ON dbo.Price 
FOR INSERT, UPDATE
AS
BEGIN
    DECLARE @Error_No int, 	
			@iconcontrolleruserId INT

    SELECT @Error_No = 0

	SELECT @iconcontrolleruserId = User_ID FROM Users WHERE UserName = 'iconcontrolleruser'
	 
    -- NOTE:  There are some fields on the Price table, Tax Table and Restricted Hours, that when changed will fire this trigger
    -- to insert into PriceHistory, but this info is not on PriceHistory.  Even though we could condition this stored procedure to
    -- prevent the unnecessary, duplicate PriceHistory records when this info changes, it was decided that it was not worth the extra processing time this 
    -- would take since the Tax Table and Restricted Hours info rarely changes once set.
    INSERT INTO PriceHistory (Item_Key, Store_No, Multiple, Price, MSRPPrice, MSRPMultiple, PricingMethod_ID, Sale_Multiple, 
                              Sale_Price, Sale_Start_Date, Sale_End_Date, Sale_Max_Quantity, Sale_Earned_Disc1, 
                              Sale_Earned_Disc2, Sale_Earned_Disc3,  
                              User_Name, Host_Name, Effective_Date,
                              IBM_Discount, Restricted_Hours, AvgCostUpdated, POSPrice, POSSale_Price, NotAuthorizedForSale, CompFlag,
                              POSTare, POSLinkCode, LinkedItem, GrillPrint, AgeCode, VisualVerify, SrCitizenDiscount, PriceChgTypeId, ExceptionSubTeam_No, 
                              KitchenRoute_ID, Routing_Priority, Consolidate_Price_To_Prev_Item, Print_Condiment_On_Receipt, Age_Restrict,
                              CompetitivePriceTypeID, BandwidthPercentageHigh, BandwidthPercentageLow, MixMatch, Discountable, LocalItem, ItemSurcharge)
    SELECT Price.Item_Key, Price.Store_No, Price.Multiple, Price.Price, Price.MSRPPrice, Price.MSRPMultiple, Price.PricingMethod_ID, Price.Sale_Multiple, 
           Price.Sale_Price, Price.Sale_Start_Date, Price.Sale_End_Date, Price.Sale_Max_Quantity, Price.Sale_Earned_Disc1, 
           Price.Sale_Earned_Disc2, Price.Sale_Earned_Disc3, 
           Left(SUSER_NAME(), 20), Left(HOST_NAME(), 20), GETDATE(),
           Price.IBM_Discount, Price.Restricted_Hours, Price.AvgCostUpdated, Price.POSPrice, Price.POSSale_Price, Price.NotAuthorizedForSale, Price.CompFlag,
           Price.POSTare, Price.POSLinkCode, Price.LinkedItem, Price.GrillPrint, Price.AgeCode, Price.VisualVerify, Price.SrCitizenDiscount, Price.PriceChgTypeId, Price.ExceptionSubTeam_No, 
           Price.KitchenRoute_ID, Price.Routing_Priority, Price.Consolidate_Price_To_Prev_Item, Price.Print_Condiment_On_Receipt, Price.Age_Restrict,
           Price.CompetitivePriceTypeID, Price.BandwidthPercentageHigh, Price.BandwidthPercentageLow, Price.MixMatch, Price.Discountable, Price.LocalItem, Price.ItemSurcharge
    FROM Price
    INNER JOIN
        Inserted
        ON Inserted.Store_No = Price.Store_No AND Inserted.Item_Key = Price.Item_Key

    SELECT @Error_No = @@ERROR

    IF @Error_No = 0
    BEGIN
		-- Create a new PriceBatchDetail record for an Item change if the information that is updated on the Price table
		-- is Item level information, not actually Price information.  Item information is stored on the Price table when it
		-- is store specific.
        INSERT INTO PriceBatchDetail (Store_No, Item_Key, ItemChgTypeID, InsertApplication)
        SELECT Inserted.Store_No, Inserted.Item_Key, 2, 'PriceAddUpdate Trigger'
        FROM Inserted
        INNER JOIN
            Deleted
            ON Deleted.Item_Key = Inserted.Item_Key AND Deleted.Store_No = Inserted.Store_No
        INNER JOIN
            Store (nolock)
            ON Store.Store_No = Inserted.Store_No
        WHERE (WFM_Store = 1 OR Mega_Store = 1)
		AND ISNULL(INSERTED.LastScannedUserId_NonDTS, 0) <> ISNULL(@iconcontrolleruserId, 0)
        AND ((Inserted.IBM_Discount <> Deleted.IBM_Discount)
             OR (Inserted.Restricted_Hours <> Deleted.Restricted_Hours)
			 OR (Inserted.Discountable <> Deleted.Discountable)
             OR (ISNULL(Inserted.NotAuthorizedForSale,0) <> ISNULL(Deleted.NotAuthorizedForSale,0))
             OR (ISNULL(Inserted.PosTare,0) <> ISNULL(Deleted.PosTare,0))
             OR (ISNULL(Inserted.POSLinkCode,'') <> ISNULL(Deleted.POSLinkCode,''))
             OR (ISNULL(Inserted.LinkedItem,0) <> ISNULL(Deleted.LinkedItem,0))
             OR (ISNULL(Inserted.GrillPrint,0) <> ISNULL(Deleted.GrillPrint,0))
             OR (ISNULL(Inserted.AgeCode,0) <> ISNULL(Deleted.AgeCode,0))
             OR (ISNULL(Inserted.VisualVerify,0) <> ISNULL(Deleted.VisualVerify,0))
             OR (ISNULL(Inserted.SrCitizenDiscount,0) <> ISNULL(Deleted.SrCitizenDiscount,0))
             OR (ISNULL(Inserted.ExceptionSubTeam_NO, 0) <> ISNULL(Deleted.ExceptionSubTeam_No,0))
             OR (ISNULL(Inserted.KitchenRoute_ID, 0) <> ISNULL(Deleted.KitchenRoute_ID, 0))
             OR (ISNULL(Inserted.Routing_Priority, 0) <> ISNULL(Deleted.Routing_Priority, 0))
             OR (ISNULL(Inserted.Consolidate_Price_To_Prev_Item, 0) <> ISNULL(Deleted.Consolidate_Price_To_Prev_Item, 0))
             OR (ISNULL(Inserted.Print_Condiment_On_Receipt, 0) <> ISNULL(Deleted.Print_Condiment_On_Receipt, 0))
             OR (ISNULL(Inserted.Age_Restrict, 0) <> ISNULL(Deleted.Age_Restrict, 0))
             OR (ISNULL(Inserted.MixMatch, 0) <> ISNULL(Deleted.MixMatch, 0))
             OR (ISNULL(Inserted.ItemSurcharge, 0) <> ISNULL(Deleted.ItemSurcharge, 0))
            )
        AND (dbo.fn_HasPendingItemChangePriceBatchDetailRecord(Inserted.Item_Key, Inserted.Store_No) = 0)
        
        SELECT @Error_No = @@ERROR
    END

	IF @Error_No = 0
	BEGIN
		UPDATE Price
		   SET LastScannedUserId_NonDTS = NULL
		  FROM INSERTED
		 WHERE INSERTED.Item_Key = PRICE.Item_Key
		   AND INSERTED.Store_No = PRICE.Store_No
		   AND INSERTED.LastScannedUserId_NonDTS = @iconcontrolleruserId

		SELECT @Error_No = @@ERROR
	END

    IF @Error_No <> 0
    BEGIN
        ROLLBACK TRAN
        DECLARE @Severity smallint
        SELECT @Severity = ISNULL((SELECT severity FROM master.dbo.sysmessages WHERE error = @Error_No), 16)
        RAISERROR ('PriceAddUpdate trigger failed with @@ERROR: %d', @Severity, 1, @Error_No)
    END
END
GO



